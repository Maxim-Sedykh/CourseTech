using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations
{
    /// <summary>
    /// Конфигурация сущности "Ответы на вопрос открытого типа" (настройка таблицы в БД)
    /// </summary>
    public class OpenQuestionAnswerConfiguration : IEntityTypeConfiguration<OpenQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<OpenQuestionAnswer> builder)
        {
            builder.Property(av => av.Id).ValueGeneratedOnAdd();

            builder.Property(av => av.OpenQuestionId).IsRequired();
            builder.Property(av => av.AnswerText).IsRequired().HasMaxLength(ValidationConstraints.OpenQuestionAnswerMaximumLength);

            builder.HasOne(av => av.OpenQuestion)
                .WithMany(q => q.AnswerVariants)
                .HasForeignKey(av => av.OpenQuestionId);
        }
    }
}
