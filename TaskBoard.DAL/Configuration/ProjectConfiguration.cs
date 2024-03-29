using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.DAL.Model;

namespace TaskBoard.DAL.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.
                HasMany(p => p.Sprints)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
