using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using CourseTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository)
        {
            _jwtKey = options.Value.JwtKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _userRepository = userRepository;
        }

        /// <inheritdoc/>
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentials);
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
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

            return claims;
        }

        /// <inheritdoc/>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
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

        /// <inheritdoc/>
        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
        {
            string accessToken = dto.AccessToken;
            string refreshToken = dto.RefreshToken;

            var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
            var userName = claimsPrincipal.Identity?.Name;

            var user = await _userRepository.GetAll()
                .Include(x => x.UserToken)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == userName);

            if (user == null || user.UserToken.RefreshToken != refreshToken ||
                user.UserToken.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                return BaseResult<TokenDto>.Failure((int)ErrorCodes.InvalidClientRequest, ErrorMessage.InvalidClientRequest);
            }

            var newClaims = GetClaimsFromUser(user);

            var newAccessToken = GenerateAccessToken(newClaims);
            var newRefreshToken = GenerateRefreshToken();

            user.UserToken.RefreshToken = newRefreshToken;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return BaseResult<TokenDto>.Success(new TokenDto() 
            { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken, 
            });
        }
    }
}
