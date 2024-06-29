using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class TestVariantConfiguration : IEntityTypeConfiguration<TestVariant>
{
    public void Configure(EntityTypeBuilder<TestVariant> builder)
    {
        builder.Property(tv => tv.Id).ValueGeneratedOnAdd();

        builder.Property(tv => tv.Content).HasMaxLength(500).IsRequired();

        builder.HasOne(tv => tv.Question)
            .WithMany(q => q.TestVariants)
            .HasForeignKey(tv => tv.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
