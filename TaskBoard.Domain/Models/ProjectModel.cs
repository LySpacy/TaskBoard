using CSharpFunctionalExtensions;
using System.IO;

namespace TaskBoard.Domain.Models
{
    public class ProjectModel
    {
        public const int MAX_TITLE_LENGTH = 100;
        private ProjectModel(Guid id, string title, string description, List<SprintModel> sprints)
        {
            Id = id; 
            Title = title; 
            Description = description; 
            Sprints = sprints;
        }
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public List<SprintModel> Sprints { get; private set; } = [];

        public static Result<ProjectModel> Create(Guid id, string title, string description, List<SprintModel> sprints)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Result.Failure<ProjectModel>($"'{nameof(title)}' cannot be null or empty");
            }

            if (title.Length > MAX_TITLE_LENGTH)
            {
                return Result.Failure<ProjectModel>($"'{nameof(title)}' cannot be longer than {MAX_TITLE_LENGTH} characters");
            }

            if (string.IsNullOrEmpty(description)) 
            {
                return Result.Failure<ProjectModel>($"'{nameof(description)}'cannot be null or empty");
            }

            var project = new ProjectModel(id, title, description, sprints);

            return Result.Success<ProjectModel>(project);
        }

    }
}
