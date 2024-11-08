using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;

namespace CourseTech.DAL.Configurations;

/// <summary>
/// Конфигурация сущности "Ключевые слова корректных запросов для практического типа заданий" (настройка таблицы в БД)
/// </summary>
public class PracticalQuestionQueryKeywordConfiguration : IEntityTypeConfiguration<PracticalQuestionQueryKeyword>
{
    public void Configure(EntityTypeBuilder<PracticalQuestionQueryKeyword> builder)
    {
        builder.HasKey(x => new { x.Number, x.KeywordId, x.PracticalQuestionId });

        builder.HasOne(x => x.Keyword)
            .WithMany(x => x.PracticalQuestionQueryKeywords)
            .HasForeignKey(x => x.KeywordId);

        builder.HasOne(x => x.PracticalQuestion)
            .WithMany(x => x.PracticalQuestionQueryKeywords)
            .HasForeignKey(x => x.PracticalQuestionId);
    }
}
