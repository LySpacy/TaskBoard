using CSharpFunctionalExtensions;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task<Result<List<UserModel>>> GetAll();
        Task<Result<bool>> UpdateUser(UserModel user);
        Task<Result<bool>> UpdateUserRole(Guid id, UserRole role);
        Task<Result<bool>> DeleteUser(Guid id);
        Task<Result<bool>> CreateUser(UserModel user);
        Task<Result<UserModel>> GetUserById(Guid id);
        Task<Result<UserModel>> GetUserByEmail(string email);
        Task<Result<bool>> BanUser(Guid id, string cause);
        Task<Result<bool>> UnbanUser(Guid id);
        Task<Result<List<BanModel>>> GetBanUser();
    }
}
