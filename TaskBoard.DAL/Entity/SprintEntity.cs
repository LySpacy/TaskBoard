namespace TaskBoard.DAL.Model
{
    public class SprintEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime DateStart { get; set; } = DateTime.Now;
        public DateTime DateEnd { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectEntity? Project { get; set; }
        public List<TaskEntity> Tasks { get; set; } = [];
        public Guid? UserId { get; set; } = null;
        public UserEntity? User { get; set; }
        public List<FileEntity> Files { get; set; } = [];
    }
}
