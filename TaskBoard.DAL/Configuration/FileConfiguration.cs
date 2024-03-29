using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.DAL.Model;

namespace TaskBoard.DAL.Configuration
{
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.
                HasOne(t => t.Sprint)
                .WithMany(s => s.Files)
                .HasForeignKey(t => t.SprintId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.
                HasOne(f => f.Task)
                .WithMany(t => t.Files)
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
