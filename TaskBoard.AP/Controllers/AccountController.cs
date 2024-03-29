using Microsoft.AspNetCore.Mvc;
using TaskBoard.API.Contracts.User;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using TaskBoard.Service.Interfice;
using Microsoft.AspNet.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace TaskBoard.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAccountsService _accountsService;
        private readonly IJwtProvider _jwtProvider;
        public AccountController(IAccountsService userService, IUsersService usersService, IJwtProvider jwtProvider)
        {
            _accountsService = userService;
            _usersService = usersService;
            _jwtProvider = jwtProvider;
        }

        [HttpGet("/Account/Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var userRegister = await _accountsService.Register(request.Name, request.Password, request.Email);

                if (userRegister.IsSuccess)
                {
                    var user = await _usersService.GetUserByEmail(request.Email);

                    await Authenticate(user.Value);

                    return RedirectToAction("GetAll", "Projects");
                }
                else
                {
                    ModelState.AddModelError("", userRegister.Error);
                }
            }
            return View(request);
        }

        [HttpGet("/Account/Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var userLogin = await _accountsService.Login(request.Email, request.Password);

                if (userLogin.IsSuccess)
                {
                    var user = await _usersService.GetUserByEmail(request.Email);

                    await Authenticate(user.Value); // аутентификация

                    return RedirectToAction("GetAll", "Projects");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(request);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = await _usersService.GetUserByEmail(request.Email);

            if (user.IsFailure)
            {
                ModelState.AddModelError(string.Empty, user.Error);
                return View(request);
            }

            var code = _jwtProvider.GenerateToken(user.Value);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { id = user.Value.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var message = new IdentityMessage()
            {
                Destination = request.Email,
                Subject = "Reset Password",
                Body = $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>"
            };
            EmailService emailService = new EmailService();
            await emailService.SendAsync(message);
            return View("ForgotPasswordConfirmation");

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _usersService.GetUserByEmail(model.Email);
            if (user.IsFailure)
            {
                return View("ResetPasswordConfirmation");
            }
            
            var result = await _accountsService.ResetPassword(user.Value.Id, model.Password);
            if (result.IsSuccess)
            {
                return View("ResetPasswordConfirmation");
            }

            ModelState.AddModelError(string.Empty, result.Error);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Сonfigure(Guid id)
        {
            var user = await _usersService.GetUserById(id);

            if (user.IsSuccess)
            {
                return View(user.Value);
            }

            return View(user.Error);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateData(Guid id)
        {
            var userIdFromToken = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);

            if (id != userIdFromToken)
            {
                return Forbid();
            }

            var currentUser = await _usersService.GetUserById(id);
            if (currentUser.IsFailure)
            {
                return NotFound();
            }
            var user = currentUser.Value;

            var request = new UpdateUserRequest(
                user.Id,
                user.Name,
                string.Empty,
                string.Empty,
                user.Email,
                user.Role);


            return View(request);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateData(UpdateUserRequest request)
        {
            var userIdFromToken = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);

            if (request.Id != userIdFromToken)
            {
                return Forbid();
            }


            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var isPasswordValid = await _accountsService.ValidationPassword(request.Id, request.Password);

            if (!isPasswordValid.Value)
            {
                ModelState.AddModelError("Password", "Invalid password.");
                return View(request);
            }

            var newPassword = request.Password;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                newPassword = request.NewPassword;
            }

            var updateResult = UserModel.Create(request.Id, request.Name, newPassword, request.Email);

            if (!updateResult.IsSuccess)
            {
                ModelState.AddModelError("", "Failed to update user data.");
                return View(request);
            }

            await _usersService.UpdateUser(updateResult.Value);

            return RedirectToAction("Сonfigure", "Account", new { id = request.Id });
        }
        private async Task Authenticate(UserModel user)
        {
            var token = _jwtProvider.GenerateToken(user);

            HttpContext.Response.Cookies.Append("jwtToken", token);
        }


    }
}
