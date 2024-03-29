using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface ISprintsRepository
    {
        Task Add(SprintModel sprintModel);
        Task<List<SprintModel>> GetByProject(Guid projectId);
        Task<SprintModel> GetById(Guid id);
        Task Update(Guid id, string title, string description, string comment, DateTime dateEnd);
        Task Delete(Guid id);
        Task SetUser(Guid id, Guid userId);
        Task RemoveUser(Guid id);
    }

}
