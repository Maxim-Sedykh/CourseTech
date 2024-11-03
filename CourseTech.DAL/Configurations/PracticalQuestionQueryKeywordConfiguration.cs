using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

/// <summary>
/// Конфигурация сущности "Ключевые слова корректных запросов для практического типа заданий" (настройка таблицы в БД)
/// </summary>
public class PracticalQuestionQueryKeywordConfiguration : IEntityTypeConfiguration<PracticalQuestionQueryKeyword>
{
    public void Configure(EntityTypeBuilder<PracticalQuestionQueryKeyword> builder)
    {
        builder.HasIndex(x => new { x.PracticalQuestionId, x.Number }).IsUnique();
    }
}
