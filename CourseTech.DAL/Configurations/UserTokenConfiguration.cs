using CourseTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.DAL.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.RefreshToken).IsRequired();
        builder.Property(x => x.RefreshTokenExpireTime).IsRequired();

        builder.HasData(new List<UserToken>()
        {
            new UserToken()
            {
                Id = 1,
                RefreshToken = "jbodfiujbINOIU3O4$",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
                UserId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
            },
            new UserToken()
            {
                Id = 2,
                RefreshToken = "hgiroej[giertjivfs",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
                UserId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            },
            new UserToken()
            {
                Id = 3,
                RefreshToken = "reatbyt42t423hgerf",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
                UserId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
            }
        });
    }
}
