using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CourseTech.DAL.Views;

namespace CourseTech.DAL.Configurations.Views;

public class QuestionTypeGradeCongifuration : IEntityTypeConfiguration<QuestionTypeGrade>
{
    /// <summary>
    /// Конфигурация сущности "Урок" (настройка таблицы в БД)
    /// </summary>
    public void Configure(EntityTypeBuilder<QuestionTypeGrade> builder)
    {
        builder.HasNoKey();
        builder.ToView("View_QuestionTypeGrade");
    }
}
