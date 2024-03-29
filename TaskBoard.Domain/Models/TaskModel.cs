using CSharpFunctionalExtensions;
using TaskBoard.Domain.Enum;

namespace TaskBoard.Domain.Models
{
    public class TaskModel
    {
        public const int MAX_TITLE_LENGTH = 120;
        private TaskModel(Guid sprintId, Guid id, string title, string description, string comment)
        {
            SprintId = sprintId;
            Id = id;
            Title = title;
            Description = description;
            Comment = comment;
        }
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Comment { get; private set; } = string.Empty;
        public StatusTask Status { get; private set; } = StatusTask.Expectation;
        public Guid SprintId { get; private set; } = Guid.Empty;
        public SprintModel? Sprint{ get; private set; }
        public List<FileModel> Files { get; private set; } = [];

        public void UpdateStatus(StatusTask newStatus) => Status = newStatus;
        public static Result<TaskModel> Create(Guid sprintId, Guid id, string title, string description, string comment)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Result.Failure<TaskModel>($"'{nameof(title)}' cannot be null or empty");
            }

            if (title.Length > MAX_TITLE_LENGTH)
            {
                return Result.Failure<TaskModel>($"'{nameof(title)}' cannot be longer than {MAX_TITLE_LENGTH} characters");
            }

            if (string.IsNullOrEmpty(description))
            {
                return Result.Failure<TaskModel>($"'{nameof(description)}'cannot be null or empty");
            }

            var task = new TaskModel(sprintId, id, title, description, comment);

            return Result.Success<TaskModel>(task);
        }
    }
}
