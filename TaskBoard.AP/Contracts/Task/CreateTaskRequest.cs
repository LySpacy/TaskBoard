using TaskBoard.Domain.Enum;

namespace TaskBoard.API.Contracts.Task
{
    public record CreateTaskRequest(
       Guid SprintId,
       string Title,
       string Description,
       StatusTask Status,
       string Comment
       );
}
