using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasData(new UserProfile
        {
            Id = 1,
            UserId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
            CreatedAt = DateTime.UtcNow,
        },
        new UserProfile
        {
            Id = 2,
            UserId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            CreatedAt = DateTime.UtcNow,
        },
        new UserProfile
        {
            Id = 3,
            UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
            CreatedAt = DateTime.UtcNow,
        });

        builder.Property(up => up.Id).ValueGeneratedOnAdd();

        builder.Property(up => up.Name).HasMaxLength(50).IsRequired(false);
        builder.Property(up => up.Surname).HasMaxLength(50).IsRequired(false);

        builder.Property(x => x.IsEditAble).HasDefaultValue(true);
        builder.Property(x => x.IsReviewLeft).HasDefaultValue(false);
        builder.Property(x => x.IsExamCompleted).HasDefaultValue(false);

        builder.Property(x => x.Analys).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.CurrentGrade).IsRequired().HasDefaultValue(0)
            .HasColumnType("float")
            .HasPrecision(2, 2);
        builder.Property(x => x.LessonsCompleted).IsRequired().HasDefaultValue(0);

    }
}
