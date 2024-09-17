using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Configurations
{
    public class PracticalQuestionConfiguration : IEntityTypeConfiguration<PracticalQuestion>
    {
        public void Configure(EntityTypeBuilder<PracticalQuestion> builder)
        {
            builder.HasBaseType<Question>();

            builder.Property(q => q.CorrectQueryCode).IsRequired();

            builder.HasMany(q => q.QueryWords)
                .WithOne(qw => qw.PracticalQuestion)
                .HasForeignKey(qw => qw.PracticalQuestionId);
        }
    }
}
