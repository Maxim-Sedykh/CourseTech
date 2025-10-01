using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CourseTech.Domain.Entities;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Вопрос тестового типа" (настройка таблицы в БД)
/// </summary>
public class TestQuestionConfiguration : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
        builder.HasBaseType<Question>();

        builder.HasMany(q => q.TestVariants)
            .WithOne(tv => tv.TestQuestion)
            .HasForeignKey(tv => tv.TestQuestionId);
    }
}
