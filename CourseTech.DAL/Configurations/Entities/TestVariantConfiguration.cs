﻿using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Вариант ответа для тестового вопроса" (настройка таблицы в БД)
/// </summary>
public class TestVariantConfiguration : IEntityTypeConfiguration<TestVariant>
{
    public void Configure(EntityTypeBuilder<TestVariant> builder)
    {
        builder.Property(tv => tv.Id).ValueGeneratedOnAdd();

        builder.Property(tv => tv.Content).IsRequired().HasMaxLength(ValidationConstraints.TestVariantContentMaximumLength);
        builder.Property(tv => tv.IsCorrect).HasDefaultValue(false);

        builder.HasIndex(tv => new { tv.TestQuestionId, tv.VariantNumber }).IsUnique();

        builder.HasOne(tv => tv.TestQuestion)
            .WithMany(q => q.TestVariants)
            .HasForeignKey(tv => tv.TestQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
