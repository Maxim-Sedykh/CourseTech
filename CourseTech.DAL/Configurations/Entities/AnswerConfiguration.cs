using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(a => a.SessionId)
                .IsRequired();

            builder.Property(a => a.QuestionId)
                .IsRequired();

            builder.Property(a => a.AudioFileUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.TranscribedText)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired(false);

            // Relationships
            builder.HasOne(a => a.Session)
                .WithMany(s => s.Answers)
                .HasForeignKey(a => a.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
