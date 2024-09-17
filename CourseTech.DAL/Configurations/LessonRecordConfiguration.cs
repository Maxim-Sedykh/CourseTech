using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class LessonRecordConfiguration : IEntityTypeConfiguration<LessonRecord>
{
    public void Configure(EntityTypeBuilder<LessonRecord> builder)
    {
        builder.Property(lr => lr.Id).ValueGeneratedOnAdd();

        builder.Property(lr => lr.Mark)
            .HasDefaultValue(0)
            .HasPrecision(2);

        builder.HasOne(lr => lr.User)
            .WithMany(u => u.LessonRecords)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lr => lr.Lesson)
            .WithMany(l => l.LessonRecords)
            .HasForeignKey(r => r.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
