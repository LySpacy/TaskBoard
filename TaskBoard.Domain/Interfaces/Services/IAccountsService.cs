using CSharpFunctionalExtensions;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface IAccountsService
    {
        Task<Result<string>> Login(string email, string password);
        Task<Result<bool>> Register(string userName, string password, string email);
        Task<Result<bool>> ValidationPassword(Guid id, string password);
        Task<Result<bool>> ResetPassword(Guid id, string password);
    }
}
