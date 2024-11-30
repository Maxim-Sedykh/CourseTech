using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities.QuestionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Вопрос" (настройка таблицы в БД).
/// Используется подход TPH ( Таблица на всю иерархию ).
/// Наследуемые классы TestQuestion, OpenQuestion, PracticalQuestion.
/// </summary>
public class QuestionConfiguration : IEntityTypeConfiguration<BaseQuestion>
{
    public void Configure(EntityTypeBuilder<BaseQuestion> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => new { x.Id, x.Number }).IsUnique();

        var lessonNumberPropertyName = nameof(BaseQuestion.Number);

        builder.ToTable(x => x.HasCheckConstraint($"CK_{x.Name}{lessonNumberPropertyName}",
            $"{lessonNumberPropertyName} BETWEEN {ValidationConstraints.QuestionNumberMinValue} AND {ValidationConstraints.QuestionNumberMaxValue}"));

        builder.Property(q => q.LessonId).IsRequired();
        builder.Property(q => q.DisplayQuestion).IsRequired().HasMaxLength(ValidationConstraints.QuestionDisplayQuestionMaximumLength);

        builder.HasOne(q => q.Lesson)
            .WithMany(l => l.Questions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
