using TaskBoard.DAL.Model;


namespace TaskBoard.DAL.Entity
{
    public class BanEntity
    {
        public int Id { get; set; } 
        public Guid UserId { get; set; }
        public DateTime DateBan { get; set; } = DateTime.Now;
        public string Сause { get; set; } = string.Empty;
        public UserEntity? User { get; set; }
    }
}
