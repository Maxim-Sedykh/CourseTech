namespace CourseTech.Domain.Settings;

public class JwtSettings
{
    public const string DefaultSection = "Jwt";

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public string JwtKey { get; set; }

    public string Lifetime { get; set; }

    public string RefreshTokenValidityInDays { get; set; }
}
