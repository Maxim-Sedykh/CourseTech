using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.DisplayQuestion).HasMaxLength(750).IsRequired();
        builder.Property(r => r.Answer).HasMaxLength(500).IsRequired();

        builder.HasOne(q => q.Lesson)
            .WithMany(l => l.Questions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
