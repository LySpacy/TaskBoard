using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface ITasksRepositoty
    {
        Task Add(TaskModel taskModel);
        Task<List<TaskModel>> GetBySprint(Guid sprintId);
        Task<TaskModel> GetById(Guid id);
        Task Delete(Guid id);
        Task Update(TaskModel taskModel);
    }

}
