using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;
using System.Security.Claims;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с токенами
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерация Access-токена
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Генерация Refresh-токена
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Получение ClaimsPrincipal из исчезающего токена
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

        /// <summary>
        /// Обновление токена пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);

        /// <summary>
        /// Получение основных клаймов из пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<Claim> GetClaimsFromUser(User user);
    }
}
