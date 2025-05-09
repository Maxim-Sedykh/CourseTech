﻿namespace CourseTech.Domain.Settings;

/// <summary>
/// Настройки Jwt-токена
/// </summary>
public class JwtSettings
{
    public const string DefaultSection = "Jwt";

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public string JwtKey { get; set; }

    public string Lifetime { get; set; }

    public int RefreshTokenValidityInDays { get; set; }
}
