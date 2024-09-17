using CourseTech.Domain.Entities.QuestionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Number).IsUnique();

        builder.ToTable(x => x.HasCheckConstraint("CK_Question_Number", "Number BETWEEN 0 AND 100"));

        builder.Property(q => q.LessonId).IsRequired();
        builder.Property(q => q.DisplayQuestion).IsRequired().HasMaxLength(500);

        builder.HasOne(q => q.Lesson)
            .WithMany(l => l.Questions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
