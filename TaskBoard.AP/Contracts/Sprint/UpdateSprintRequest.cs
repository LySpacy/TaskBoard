namespace TaskBoard.API.Contracts.Sprint
{
    public record UpdateSprintRequest(
       Guid Id,
       Guid ProjectId,
       string Title,
       string Description,
       DateTime DateEnd,
       string Comment
       );
}
