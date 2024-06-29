using CourseTech.Domain.Entities;
using CourseTech.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(new User
        {
            Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
            Login = "MainAdmin",
            Password = HashPasswordHelper.HashPassword("admin"),
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            Login = "MaximkaCommonUser",
            Password = HashPasswordHelper.HashPassword("1234567"),
            CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
            Login = "Sashka02",
            Password = HashPasswordHelper.HashPassword("13131313"),
            CreatedAt = DateTime.UtcNow
        });

        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.Login).HasMaxLength(18).IsRequired();

        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasPrincipalKey<User>(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UserToken)
                .WithOne(x => x.User)
                .HasPrincipalKey<User>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>(
                x => x.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
                x => x.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
            );
    }
}
