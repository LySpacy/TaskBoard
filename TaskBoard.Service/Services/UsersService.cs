using CSharpFunctionalExtensions;
using System.Data;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepositoty _usersRepositoty;
        private readonly IBansRepositoty _bansRepositoty;

        public UsersService(IUsersRepositoty usersRepositoty, IBansRepositoty bansRepositoty)
        {
            _usersRepositoty = usersRepositoty;
            _bansRepositoty = bansRepositoty;
        }
        public async Task<Result<UserModel>> GetUserById(Guid id)
        {
            try
            {
                var user = await _usersRepositoty.GetById(id);

                if (user == null) 
                {
                    return Result.Failure<UserModel>($"[GetUserById]: User not found");
                }
                return Result.Success<UserModel>(user);
            }
            catch (Exception ex)
            {
                return Result.Failure<UserModel>($"[GetUserById]: {ex.Message}");
            }
        }

        public async Task<Result<UserModel>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _usersRepositoty.GetByEmail(email);

                if (user == null)
                {
                    return Result.Failure<UserModel>($"Пользователь {email} не найден");
                }

                return Result.Success<UserModel>(user);
            }
            catch (Exception ex)
            {
                return Result.Failure<UserModel>($"[GetUserByEmail]: {ex.Message}");
            }
        }

        public async Task<Result<List<UserModel>>> GetAll()
        {
            try
            {
                var users = await _usersRepositoty.Get();

                return Result.Success<List<UserModel>>(users);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<UserModel>>($"[GetAll]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateUser(UserModel user)
        {
            try
            {
                await _usersRepositoty.Update(user.Id, user.Name, user.PasswordHash,user.Email, user.Role);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[UpdateUser]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteUser(Guid id)
        {
            try
            {
                await _usersRepositoty.Delete(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteUser]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CreateUser(UserModel user)
        {
            try
            {
                await _usersRepositoty.Add(user);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteUser]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateUserRole(Guid id, UserRole role)
        {
            try
            {
                await _usersRepositoty.UpdateRole(id, role);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[UpdateUserRole]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> BanUser(Guid id, string cause)
        {
            try
            {
                await _bansRepositoty.Baned(id, cause);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[BanUser]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UnbanUser(Guid id)
        {
            try
            {
                await _bansRepositoty.UnBaned(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[UnbanUser]: {ex.Message}");
            }
        }

        public async Task<Result<List<BanModel>>> GetBanUser()
        {
            try
            {
                var users = await _bansRepositoty.GetBanList();

                return Result.Success<List<BanModel>>(users);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<BanModel>>($"[GetBanUser]: {ex.Message}");
            }
        }
    }
}
