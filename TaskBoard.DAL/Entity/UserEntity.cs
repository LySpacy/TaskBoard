using TaskBoard.DAL.Entity;
using TaskBoard.Domain.Enum;

namespace TaskBoard.DAL.Model
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<SprintEntity> Sprints { get; set; } = [];
        public UserRole Role { get; set; } = UserRole.User;

        public BanEntity? Ban { get; set; }

    }
}
