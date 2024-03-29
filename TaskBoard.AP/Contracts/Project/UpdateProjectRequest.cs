using System.ComponentModel.DataAnnotations;
using TaskBoard.Domain.Models;

namespace TaskBoard.API.Contracts.Project
{
    public record UpdateProjectRequest(
        Guid Id,
        string Title,
        string Description
        );
}
