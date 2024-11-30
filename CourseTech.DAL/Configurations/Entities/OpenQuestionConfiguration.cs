﻿using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Вопрос открытого типа" (настройка таблицы в БД)
/// </summary>
public class OpenQuestionConfiguration : IEntityTypeConfiguration<OpenQuestion>
{
    public void Configure(EntityTypeBuilder<OpenQuestion> builder)
    {
        builder.HasBaseType<BaseQuestion>();

        builder.HasMany(q => q.AnswerVariants)
            .WithOne(av => av.OpenQuestion)
            .HasForeignKey(av => av.OpenQuestionId);
    }
}
