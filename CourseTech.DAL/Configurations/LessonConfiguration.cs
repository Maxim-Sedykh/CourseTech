using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(l => l.Name).HasMaxLength(75).IsRequired();
        builder.Property(l => l.LectureMarkup).HasMaxLength(10000).IsRequired(false);
    }
}
