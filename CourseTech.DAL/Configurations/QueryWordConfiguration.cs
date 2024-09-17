using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class QueryWordConfiguration : IEntityTypeConfiguration<QueryWord>
{
    public void Configure(EntityTypeBuilder<QueryWord> builder)
    {
        builder.Property(qw => qw.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Number).IsUnique();

        builder.HasOne(qw => qw.Keyword)
            .WithMany(k => k.QueryWords)
            .HasForeignKey(qw => qw.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
