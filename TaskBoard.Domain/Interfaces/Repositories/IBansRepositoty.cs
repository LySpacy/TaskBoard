using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface IBansRepositoty
    {
        Task UnBaned(Guid id);
        Task Baned(Guid id, string cause);
        Task<List<BanModel>> GetBanList();
        Task<BanModel> GetBanUserById(Guid id);
    }
}
