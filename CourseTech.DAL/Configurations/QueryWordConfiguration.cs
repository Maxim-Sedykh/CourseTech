using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class QueryWordConfiguration : IEntityTypeConfiguration<QueryWord>
{
    public void Configure(EntityTypeBuilder<QueryWord> builder)
    {
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.HasOne(q => q.Keyword)
            .WithMany(k => k.QueryWords)
            .HasForeignKey(q => q.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
