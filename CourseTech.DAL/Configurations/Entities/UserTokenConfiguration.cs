using CourseTech.Domain.Entities.UserRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Refresh - токен пользователя" (настройка таблицы в БД)
/// </summary>
public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.RefreshToken).IsRequired();
        builder.Property(x => x.RefreshTokenExpireTime).IsRequired();

        builder.Property(ut => ut.RefreshToken)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(ut => ut.UserId)
            .IsRequired();

        builder.HasIndex(ut => ut.UserId)
            .IsUnique();

        builder.HasIndex(ut => ut.RefreshToken);

        builder.HasOne(ut => ut.User)
            .WithOne(u => u.UserToken)
            .HasForeignKey<UserToken>(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
