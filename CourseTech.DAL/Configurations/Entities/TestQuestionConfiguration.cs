﻿using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Entities.QuestionEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Вопрос тестового типа" (настройка таблицы в БД)
/// </summary>
public class TestQuestionConfiguration : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
        builder.HasBaseType<BaseQuestion>();

        builder.HasMany(q => q.TestVariants)
            .WithOne(tv => tv.TestQuestion)
            .HasForeignKey(tv => tv.TestQuestionId);
    }
}