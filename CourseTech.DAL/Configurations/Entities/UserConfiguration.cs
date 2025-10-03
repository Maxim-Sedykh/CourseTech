using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.UserRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Пользователь" (настройка таблицы в БД)
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Login)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.SubscriptionId)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.UserToken)
            .WithOne(ut => ut.User)
            .HasForeignKey<UserToken>(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Subscription)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<Session>()
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
