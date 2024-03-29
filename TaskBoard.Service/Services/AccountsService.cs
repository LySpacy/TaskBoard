using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskBoard.Domain.Helpers;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;
using TaskBoard.Service.Interfice;

namespace TaskBoard.Service.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IUsersRepositoty _usersRepositoty;
        private readonly IJwtProvider _jwtProvider;

        public AccountsService(IUsersRepositoty usersRepositoty, IJwtProvider jwtProvider)
        {
            _usersRepositoty = usersRepositoty;
            _jwtProvider = jwtProvider;
        }


        public async Task<Result<bool>> Register(string userName, string password, string email)
        {
            try
            {
                var userByEmail = await _usersRepositoty.GetByEmail(email);

                if (userByEmail == null)
                {
                    var user = UserModel.Create(
                        Guid.Empty,
                        userName,
                        password,
                        email);

                    if (user.IsSuccess)
                    {
                        await _usersRepositoty.Add(user.Value);

                        return Result.Success(true);
                    }
                }   

                return Result.Failure<bool>("Пользователь с таким Email уже зарегистрирован");

            }
            catch (Exception ex) 
            {
                return Result.Failure<bool>($"Ошибка регистрации: {ex.Message}");
            }


        }

        public async Task<Result<string>> Login(string email, string password)
        {
            try
            {
                var user = await _usersRepositoty.GetByEmail(email);

                if (user == null)
                {

                    return Result.Failure<string>($"Пользователь с '{email}' не найден");
                }

                var result = PasswordHelper.Verify(password, user.PasswordHash);

                if (result == false)
                {
                    return Result.Failure<string>("Неверный пароль");
                }

                var token = _jwtProvider.GenerateToken(user);

                return Result.Success<string>(token);

            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Ошибка регистрации: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ValidationPassword(Guid id, string password)
        {
            try
            {
                var user = await _usersRepositoty.GetById(id);

                if (user == null)
                {
                    return Result.Failure<bool>("Пользователь не найден");
                }

                return Result.Success(PasswordHelper.Verify(password, user.PasswordHash));

            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"Ошибка регистрации: {ex.Message}");
            }


        }

        public async Task<Result<bool>> ResetPassword(Guid id, string password)
        {
            try
            {
                var user = await _usersRepositoty.GetById(id);

                if (user == null)
                {
                    return Result.Failure<bool>("Пользователь не найден");
                }

                var passwordHash = PasswordHelper.GetHashPassword(password);
                await _usersRepositoty.Update(id, user.Name, passwordHash, user.Email, user.Role);

                return Result.Success(true);

            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"Ошибка регистрации: {ex.Message}");
            }


        }
    }
}
