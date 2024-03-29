using CSharpFunctionalExtensions;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface ITasksService
    {
        Task<Result<TaskModel>> GetTask(Guid id);
        Task<Result<bool>> CreateTask(TaskModel taskModel);
        Task<Result<bool>> DeleteTask(Guid id);
        Task<Result<bool>> UpdateTask(TaskModel taskModel);
    }
}
