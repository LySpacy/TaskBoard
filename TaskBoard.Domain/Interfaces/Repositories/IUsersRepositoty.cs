using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface IUsersRepositoty
    {
        Task<List<UserModel>> Get();
        Task<UserModel> GetById(Guid id);
        Task<UserModel> GetByEmail(string email);
        Task Delete(Guid id);
        Task Update(Guid id, string login, string password, string email, UserRole role);
        Task UpdateRole(Guid id, UserRole role);
        Task Add(UserModel userModel);
    }
}
