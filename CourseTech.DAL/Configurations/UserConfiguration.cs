﻿using CourseTech.Domain.Entities;
using CourseTech.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.Login).HasMaxLength(18).IsRequired();

        builder.HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasPrincipalKey<User>(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UserToken)
                .WithOne(x => x.User)
                .HasPrincipalKey<User>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>(
            x => x.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
            x => x.HasOne<User>().WithMany().HasForeignKey(x => x.UserId));
    }
}
