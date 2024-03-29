using System.ComponentModel.DataAnnotations;
using TaskBoard.Domain.Enum;

namespace TaskBoard.API.Contracts.User
{
    public record CreateUserRequest(
        string Name,
        string Password,
        string Email,
        UserRole Role
        );
}
