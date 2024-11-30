using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    /// <summary>
    /// Конфигурация сущности "Урок" (настройка таблицы в БД)
    /// </summary>
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(l => l.Name).HasMaxLength(ValidationConstraints.LessonNameMaximumLength).IsRequired();
        builder.Property(l => l.LectureMarkup).HasMaxLength(ValidationConstraints.LectureMarkupMaximumLength).IsRequired(false);
    }
}
