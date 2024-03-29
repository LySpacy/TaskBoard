namespace TaskBoard.DAL.Model
{
    public class FileEntity
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public Guid? TaskId { get; set; } 
        public TaskEntity? Task { get; set; }
        public Guid? SprintId { get; set; }
        public SprintEntity? Sprint { get; set; }
        


    }
}
