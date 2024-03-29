using TaskBoard.Domain.Enum;

namespace TaskBoard.API.Contracts.Task
{
    public record UpdateTaskRequest(
       Guid Id,
       Guid SprintId,
       string Title,
       string Description,
       StatusTask Status,
       string Comment
       );
}
