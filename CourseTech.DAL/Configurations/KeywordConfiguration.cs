﻿using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Word).HasMaxLength(50).IsRequired();
    }
}
