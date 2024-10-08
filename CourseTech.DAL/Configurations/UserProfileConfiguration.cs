﻿using Azure;
using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(up => up.Id).ValueGeneratedOnAdd();

        builder.Property(up => up.Name).HasMaxLength(50).IsRequired();
        builder.Property(up => up.Surname).HasMaxLength(50).IsRequired();
        builder.Property(up => up.Age).IsRequired();
        builder.Property(up => up.DateOfBirth).IsRequired();
        builder.Property(up => up.LessonsCompleted).IsRequired().HasDefaultValue(0);
        builder.Property(up => up.CountOfReviews).IsRequired().HasDefaultValue(0);

        builder.Property(x => x.IsEditAble).HasDefaultValue(true);
        builder.Property(x => x.IsExamCompleted).HasDefaultValue(false);

        builder.Property(x => x.Analys).IsRequired(false);
        builder.Property(x => x.CurrentGrade)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnType("float")
            .HasPrecision(3, 2);

        builder.Property(x => x.LessonsCompleted).IsRequired().HasDefaultValue(0);

    }
}
