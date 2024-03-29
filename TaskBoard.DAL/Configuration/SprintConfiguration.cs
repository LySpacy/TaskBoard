using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.DAL.Model;

namespace TaskBoard.DAL.Configuration
{
    public class SprintConfiguration : IEntityTypeConfiguration<SprintEntity>
    {
        public void Configure(EntityTypeBuilder<SprintEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.
                HasOne(s => s.Project)
                .WithMany(p => p.Sprints)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.
                HasMany(s => s.Tasks)
                .WithOne(t => t.Sprint)
                .HasForeignKey(t => t.SprintId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.
                HasOne(s => s.User)
                .WithMany(u => u.Sprints)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(s => s.Files)
                .WithOne(f => f.Sprint)
                .HasForeignKey(f => f.SprintId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
