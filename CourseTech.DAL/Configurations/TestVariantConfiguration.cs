using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class TestVariantConfiguration : IEntityTypeConfiguration<TestVariant>
{
    public void Configure(EntityTypeBuilder<TestVariant> builder)
    {
        builder.Property(tv => tv.Id).ValueGeneratedOnAdd();

        builder.Property(tv => tv.Content).IsRequired().HasMaxLength(500);
        builder.Property(tv => tv.IsCorrect).HasDefaultValue(false);

        builder.HasIndex(tv => new { tv.TestQuestionId, tv.VariantNumber }).IsUnique();

        builder.HasOne(tv => tv.TestQuestion)
            .WithMany(q => q.TestVariants)
            .HasForeignKey(tv => tv.TestQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
