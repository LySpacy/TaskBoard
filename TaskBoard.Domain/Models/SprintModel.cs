using CSharpFunctionalExtensions;

namespace TaskBoard.Domain.Models
{
    public class SprintModel
    {
        public const int MAX_TITLE_LENGTH = 120;
        private List<FileModel> _files = new();
        private SprintModel(Guid projectId, Guid id, string title, string description, DateTime dateEnd, string comment, Guid? userId)
        {
            ProjectId = projectId;
            Id = id;
            Title = title;
            Description = description;
            DateEnd = dateEnd;
            Comment = comment;
            UserId = userId;
        }
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Comment { get; private set; } = string.Empty;
        public Guid ProjectId { get; private set; } = Guid.Empty;
        public DateTime DateStart { get; private set; } = DateTime.Now;
        public DateTime DateEnd { get; private set; }
        public List<TaskModel> Tasks { get; private set; } = [];
        public Guid? UserId { get; private set; } = Guid.Empty;
        public UserModel? User { get; private set; } = null;
        public IReadOnlyCollection<FileModel> Files => _files;

        public void AddFile(List<FileModel> files) => _files.AddRange(files);
        public static Result<SprintModel> Create(Guid projectId, Guid id, string title, string description, DateTime dateEnd, string comment, Guid? userId)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Result.Failure<SprintModel>($"'{nameof(title)}' cannot be null or empty");
            }

            if (title.Length > MAX_TITLE_LENGTH)
            {
                return Result.Failure<SprintModel>($"'{nameof(title)}' cannot be longer than {MAX_TITLE_LENGTH} characters");
            }

            if (string.IsNullOrEmpty(description))
            {
                return Result.Failure<SprintModel>($"'{nameof(description)}'cannot be null or empty");
            }

            var sprint = new SprintModel(projectId, id, title, description, dateEnd, comment, userId);

            return Result.Success<SprintModel>(sprint);
        }
    }
}
