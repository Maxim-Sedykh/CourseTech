using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class UserToken : IEntityId<long>
{
    public long Id { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpireTime { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
