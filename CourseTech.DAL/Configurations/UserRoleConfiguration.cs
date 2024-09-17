using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(new List<UserRole>()
        {
            new UserRole()
            {
                UserId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                RoleId = 2,
            },
            new UserRole()
            {
                UserId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                RoleId = 1,
            },
            new UserRole()
            {
                UserId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                RoleId = 3,
            },
            new UserRole()
            {
                UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                RoleId = 1,
            }
        });
    }
}
