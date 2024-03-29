using CSharpFunctionalExtensions;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface IProjectsService
    {
        Task<Result<IEnumerable<ProjectModel>>> GetProjects();
        Task<Result<bool>> CreateProject(ProjectModel project);
        Task<Result<ProjectModel>> GetProject(Guid id);
        Task<Result<bool>> UpdateProject(ProjectModel project);
        Task<Result<bool>> DeleteProject(Guid id);
        Task<Result<ProjectModel>> GetProjectWithUserSprints(Guid id, Guid userId);
    }
}
