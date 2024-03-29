namespace TaskBoard.API.Contracts.User
{
    public record RegisterUserRequest(
        string Name,
        string Password,
        string Email
        );
}
