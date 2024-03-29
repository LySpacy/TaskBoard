using CSharpFunctionalExtensions;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface ISprintsService
    {
        Task<Result<bool>> CreateSprint(SprintModel sprintModel);
        Task<Result<bool>> DeleteSprint(Guid id);
        Task<Result<bool>> UpdateSprint(SprintModel sprintModel);
        Task<Result<SprintModel>> GetSprint(Guid id);
        Task<Result<bool>> SetUser(Guid id, UserModel user);
        Task<Result<bool>> RemoveUser(Guid id);
    }
}
