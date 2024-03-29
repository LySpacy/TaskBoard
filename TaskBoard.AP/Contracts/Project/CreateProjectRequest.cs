using System.ComponentModel.DataAnnotations;
using TaskBoard.Domain.Models;

namespace TaskBoard.API.Contracts.Project
{
    public record CreateProjectRequest(
        string Title,
        string Description
        );
}
