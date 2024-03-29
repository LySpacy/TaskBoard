using Azure.Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.API.Contracts.User;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.API.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _usersService.GetAll();

            if (users.IsSuccess)
            {
                return View(users.Value);
            }

            return View(users.Error);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _usersService.GetUserById(id);

            if (user.IsSuccess)
            {
                return View(user.Value);
            }

            return View(user.Error);
        }


        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public IActionResult CreateUser(CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(userRequest);
            }

            var user = UserModel.Create(
                Guid.Empty,
                userRequest.Name,
                userRequest.Password,
                userRequest.Email);


            if (user.IsFailure)
            {
                return BadRequest(user.Error);
            }

            user.Value.UpdateRole(userRequest.Role);

            _usersService.CreateUser(user.Value);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateRole(Guid id)
        {
            var user = await _usersService.GetUserById(id);

            if (user.IsSuccess)
            {
                var userRequest = new UpdateUserRequest(
                    user.Value.Id,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    user.Value.Role);

                return View(userRequest);
            }
            return RedirectToAction("GetAll");

        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UpdateRole(UpdateUserRequest userRequest)
        {
            
            var userUpdate = await _usersService.UpdateUserRole(userRequest.Id, userRequest.Role);

            if (userUpdate.IsSuccess)
            {
                return RedirectToAction("GetAll", userRequest.Id);
            }

            return View(userRequest);
        }


        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> BanedUser(Guid id)
        {
            var user = await _usersService.GetUserById(id);

            if (user.Value.Email != "spacyworktesting@yandex.ru")
            {
                var banUser = new BanUserRequest(id, string.Empty);
                return View(banUser);
            }

            return BadRequest("Запрещенно");
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> BanedUser(BanUserRequest request)
        {

            var userDelete = await _usersService.BanUser(request.Id, request.Cause);

            if (userDelete.IsSuccess)
            {
                var user = await _usersService.GetUserById(request.Id);

                var message = new IdentityMessage()
                {
                    Destination = user.Value.Email,
                    Subject = "Сообщение о блокировке",
                    Body = $"Ваш аккаунт в сервисе 'TaskBoard' был заблокирован по причине: {request.Cause}"
                };
                EmailService emailService = new EmailService();
                await emailService.SendAsync(message);
                return View("GetAll");
            }

            return BadRequest("Пользователь не найден");


        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> UnBanedUser(Guid id)
        {

            var userDelete = await _usersService.UnbanUser(id);

            if (userDelete.IsSuccess)
            {
                var user = await _usersService.GetUserById(id);

                var message = new IdentityMessage()
                {
                    Destination = user.Value.Email,
                    Subject = "Сообщение о разблокировке",
                    Body = $"{user.Value.Name} Ваш аккаунт в сервисе 'TaskBoard' был разблокирован"
                };
                EmailService emailService = new EmailService();
                await emailService.SendAsync(message);

                return RedirectToAction("GetAll");
            }

            return BadRequest("Пользователь не найден");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> GetBlackList()
        {

            var blackList = await _usersService.GetBanUser();

            if (blackList.IsSuccess)
            {
                return View(blackList.Value);
            }

            return BadRequest($"{blackList.Error}");
        }

    }
}
