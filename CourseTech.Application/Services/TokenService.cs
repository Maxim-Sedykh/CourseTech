using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CourseTech.Application.Services;

public class TokenService(IMediator mediator, IOptions<JwtSettings> options) : ITokenService
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
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), ErrorMessage.UserNotFound);
        }

        if (user.Roles == null)
        {
            throw new ArgumentNullException(nameof(user.Roles), ErrorMessage.UserRolesNotFound);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

        return claims;
    }

    /// <inheritdoc/>
    public async Task<DataResult<TokenDto>> RefreshToken(TokenDto dto)
    {
        string accessToken = dto.AccessToken;
        string refreshToken = dto.RefreshToken;

        var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
        var login = claimsPrincipal.Identity?.Name;

        var user = await _mediator.Send(new GetUserWithTokenAndRolesByLoginQuery(login));

        if (user == null || user.UserToken.RefreshToken != refreshToken ||
            user.UserToken.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            return DataResult<TokenDto>.Failure((int)ErrorCodes.InvalidClientRequest, ErrorMessage.InvalidClientRequest);
        }

        var newClaims = GetClaimsFromUser(user);

        var newAccessToken = GenerateAccessToken(newClaims);
        var newRefreshToken = GenerateRefreshToken();

        await _mediator.Send(new UpdateUserTokenCommand(user.UserToken, newRefreshToken));

        return DataResult<TokenDto>.Success(new TokenDto()
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
            throw new SecurityTokenException(ErrorMessage.InvalidToken);
        }

        return claimsPrincipal;
    }
}
