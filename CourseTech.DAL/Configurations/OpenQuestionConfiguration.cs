using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations
{
    public class OpenQuestionConfiguration : IEntityTypeConfiguration<OpenQuestion>
    {
        public void Configure(EntityTypeBuilder<OpenQuestion> builder)
        {
            builder.HasBaseType<Question>();

            builder.HasMany(q => q.AnswerVariants)
                .WithOne(av => av.OpenQuestion)
                .HasForeignKey(av => av.OpenQuestionId);
        }
    }
}
