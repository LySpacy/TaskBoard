using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskBoard.DAL.Entity;
using TaskBoard.DAL.Model;

namespace TaskBoard.DAL
{
    public class TaskBoardDbContext : DbContext
    {
        public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<SprintEntity> Sprints { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<BanEntity> BlackList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.InitializeConfiguration();
        }
    }
}
