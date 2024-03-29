namespace TaskBoard.API.Contracts.Sprint
{
    public record CreateSprintRequest(
       Guid ProjectId,
       string Title,
       string Description,
       DateTime DateEnd,
       string Comment
       );
}
