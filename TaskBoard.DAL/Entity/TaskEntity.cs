using TaskBoard.Domain.Enum;

namespace TaskBoard.DAL.Model
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public Guid SprintId { get; set; }
        public SprintEntity? Sprint { get; set; }
        public List<FileEntity> Files { get; set; } = [];
        public StatusTask Status { get; set; } = StatusTask.Expectation;
    }
}
