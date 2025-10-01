using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Запись прохождения урока пользователем" (настройка таблицы в БД)
/// </summary>
public class LessonRecordConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(lr => lr.Id).ValueGeneratedOnAdd();

        builder.Property(lr => lr.Mark)
            .HasDefaultValue(0);

        builder.HasOne(lr => lr.User)
            .WithMany(u => u.LessonRecords)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lr => lr.Lesson)
            .WithMany(l => l.LessonRecords)
            .HasForeignKey(r => r.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
