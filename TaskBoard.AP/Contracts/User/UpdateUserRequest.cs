using TaskBoard.Domain.Enum;

namespace TaskBoard.API.Contracts.User
{
    public record UpdateUserRequest(
        Guid Id,
        string Name,
        string Password,
        string NewPassword,
        string Email,
        UserRole Role
        );
}
