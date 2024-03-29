using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.DAL.Model;

namespace TaskBoard.DAL.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.
                HasOne(t => t.Sprint)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.SprintId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.
                HasMany(t => t.Files)
                .WithOne(f => f.Task)
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
