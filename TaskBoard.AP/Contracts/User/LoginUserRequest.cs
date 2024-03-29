namespace TaskBoard.API.Contracts.User
{
    public record LoginUserRequest(
        string Password,
        string Email
        );
}
