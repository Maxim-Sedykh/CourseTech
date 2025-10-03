using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations.Entities
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.CategoryId)
                .IsRequired();

            builder.Property(s => s.FinishedAt)
                .IsRequired(false);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Category)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Answers)
                .WithOne(a => a.Session)
                .HasForeignKey(a => a.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
