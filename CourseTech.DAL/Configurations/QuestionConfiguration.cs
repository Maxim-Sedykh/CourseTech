using CourseTech.Domain.Entities.QuestionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

/// <summary>
/// Конфигурация сущности "Вопрос" (настройка таблицы в БД).
/// Используется подход TPH ( Таблица на всю иерархию ).
/// Наследуемые классы TestQuestion, OpenQuestion, PracticalQuestion.
/// </summary>
public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => new { x.Id, x.Number }).IsUnique();

        builder.ToTable(x => x.HasCheckConstraint("CK_Question_Number", "Number BETWEEN 0 AND 100"));

        builder.Property(q => q.LessonId).IsRequired();
        builder.Property(q => q.DisplayQuestion).IsRequired().HasMaxLength(500);

        builder.HasOne(q => q.Lesson)
            .WithMany(l => l.Questions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
