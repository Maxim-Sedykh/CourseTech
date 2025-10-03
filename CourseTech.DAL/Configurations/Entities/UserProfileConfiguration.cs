using CourseTech.Domain.Entities.UserRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Профиль пользователя" (настройка таблицы в БД)
/// </summary>
public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.Id);
        builder.Property(up => up.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(up => up.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(up => up.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(up => up.AvatarUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(up => up.Age)
            .IsRequired();

        builder.Property(up => up.DateOfBirth)
            .IsRequired();

        builder.Property(up => up.UserId)
            .IsRequired();

        builder.Property(up => up.CreatedAt)
            .IsRequired();

        builder.Property(up => up.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(up => up.UserId)
            .IsUnique();

        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
