using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface IProjectsRepository
    {
        Task<List<ProjectModel>> Get();
        Task<ProjectModel> GetById(Guid id);
        Task<List<ProjectModel>> GetByPage(int page, int pageSize);
        Task Add(ProjectModel projectModel);
        Task Update(Guid id, string title, string description);
        Task Delete(Guid id);
        Task<ProjectModel> GetByIdWithUserSprints(Guid id, Guid userId);
    }
}
