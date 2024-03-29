namespace TaskBoard.DAL.Model
{
    public class ProjectEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<SprintEntity> Sprints { get; set; } = [];
    }
}
