using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Configurations
{
    public class OpenQuestionAnswerConfiguration : IEntityTypeConfiguration<OpenQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<OpenQuestionAnswer> builder)
        {
            builder.Property(av => av.Id).ValueGeneratedOnAdd();

            builder.Property(av => av.OpenQuestionId).IsRequired();
            builder.Property(av => av.AnswerText).IsRequired().HasMaxLength(500);

            builder.HasOne(av => av.OpenQuestion)
                .WithMany(q => q.AnswerVariants)
                .HasForeignKey(av => av.OpenQuestionId);
        }
    }
}
