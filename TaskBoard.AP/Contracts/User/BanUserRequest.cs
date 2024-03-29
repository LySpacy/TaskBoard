using System.ComponentModel.DataAnnotations;
using TaskBoard.Domain.Enum;

namespace TaskBoard.API.Contracts.User
{
    public record BanUserRequest(
        Guid Id,
        string Cause
        );
}
