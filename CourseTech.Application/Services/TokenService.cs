using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CourseTech.Application.Services;

public class TokenService(IMediator mediator, IOptions<JwtSettings> options, IUserRepository userRepository) : ITokenService
{
    private readonly IMediator _mediator = mediator;
    private readonly string _jwtKey = options.Value.JwtKey;
    private readonly string _issuer = options.Value.Issuer;
    private readonly string _audience = options.Value.Audience;

    /// <inheritdoc/>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddDays(10), credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    /// <inheritdoc/>
    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }

    /// <inheritdoc/>
    public List<Claim> GetClaimsFromUser(User user)
    {
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role.ToString()),
            };

            return claims;
        }

        throw new ArgumentNullException(nameof(user));
    }

    /// <inheritdoc/>
    public async Task<Result<TokenDto>> RefreshToken(TokenDto dto)
    {
        string accessToken = dto.AccessToken;
        string refreshToken = dto.RefreshToken;

        var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
        var login = claimsPrincipal.Identity?.Name;

        var user = await userRepository.GetByLoginAsync(login);

        if (user == null || user.UserToken.RefreshToken != refreshToken ||
            user.UserToken.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            return Result<TokenDto>.Failure("Invalid client request");
        }

        var newClaims = GetClaimsFromUser(user);

        var newAccessToken = GenerateAccessToken(newClaims);
        var newRefreshToken = GenerateRefreshToken();

        await _mediator.Send(new UpdateUserTokenCommand(user.UserToken, newRefreshToken));

        return Result.Success(new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    /// <summary>
    /// Получение ClaimsPrincipal из исчезающего токена
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
            ValidateLifetime = true,
            ValidAudience = _audience,
            ValidIssuer = _issuer
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }
}
