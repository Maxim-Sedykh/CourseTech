using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities;

/// <summary>
/// Конфигурация сущности "Профиль пользователя" (настройка таблицы в БД)
/// </summary>
public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(up => up.Id).ValueGeneratedOnAdd();

        builder.Property(up => up.Name).HasMaxLength(ValidationConstraints.UserNameMaximumLength).IsRequired();
        builder.Property(up => up.Surname).HasMaxLength(ValidationConstraints.SurnameMaximumLength).IsRequired();
        builder.Property(up => up.Age).IsRequired();
        builder.Property(up => up.DateOfBirth).IsRequired();
        builder.Property(up => up.LessonsCompleted).IsRequired().HasDefaultValue(0);
        builder.Property(up => up.CountOfReviews).IsRequired().HasDefaultValue(0);

        builder.Property(x => x.IsEditAble).HasDefaultValue(true);
        builder.Property(x => x.IsExamCompleted).HasDefaultValue(false);

        builder.Property(x => x.Analys).HasDefaultValue(AnalysParts.NotReceivedYet);
        builder.Property(x => x.CurrentGrade)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnType("float")
            .HasPrecision(3, 2);

        builder.Property(x => x.LessonsCompleted).IsRequired().HasDefaultValue(0);

    }
}
