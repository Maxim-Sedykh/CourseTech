using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

/// <summary>
/// Конфигурация сущности "Отзыв" (настройка таблицы в БД)
/// </summary>
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.HasData(new Review()
        {
            Id = 1,
            UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
            ReviewText = "kakakaka",
            CreatedAt = DateTime.UtcNow,
        });

        builder.Property(r => r.ReviewText).HasMaxLength(1000).IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
