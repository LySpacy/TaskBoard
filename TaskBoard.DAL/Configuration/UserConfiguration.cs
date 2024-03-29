using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.DAL.Model;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Extensions;
using TaskBoard.Domain.Helpers;

namespace TaskBoard.DAL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.
                HasMany(u => u.Sprints)
                .WithOne(s => s.User)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasData(new UserEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "FirstAdmin",
                    PasswordHash = PasswordHelper.GetHashPassword("admin123"),
                    Email = "spacyworktesting@yandex.ru",
                    Role = UserRole.Administator
                }); 


        }
    }


}
