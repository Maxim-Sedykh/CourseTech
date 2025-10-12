using Asp.Versioning;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User;

/// <summary>
/// Управление профилями пользователей
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class UserProfileController : BaseApiController
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    /// <summary>
    /// Получение профиля пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/userprofile?userId=12345678-1234-1234-1234-123456789abc
    ///     Headers: Authorization: Bearer {token}
    ///     
    /// Можно получить только свой профиль, если вы не администратор
    /// </remarks>
    /// <param name="userId">Идентификатор пользователя (опционально, по умолчанию - текущий пользователь)</param>
    /// <returns>Профиль пользователя</returns>
    /// <response code="200">Успешное получение профиля</response>
    /// <response code="400">Профиль не найден</response>
    /// <response code="403">Недостаточно прав для просмотра чужого профиля</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync([FromQuery] string userId = null)
    {
        var targetUserId = string.IsNullOrEmpty(userId) ? AuthorizedUserId : new Guid(userId);

        // Проверка прав доступа (можно смотреть только свой профиль, если не админ)
        if (targetUserId != AuthorizedUserId && !User.IsInRole("Admin"))
        {
            return Forbid("Недостаточно прав для просмотра чужого профиля");
        }

        var result = await _userProfileService.GetUserProfileAsync(targetUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Обновление профиля текущего пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     PUT /api/v1/userprofile
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "userName": "Новое имя",
    ///         "surname": "Новая фамилия", 
    ///         "dateOfBirth": "1990-01-01"
    ///     }
    /// </remarks>
    /// <param name="dto">Данные для обновления профиля</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Профиль успешно обновлен</response>
    /// <response code="400">Ошибка при обновлении профиля</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateUserProfileAsync([FromBody] UpdateUserProfileDto dto)
    {
        var result = await _userProfileService.UpdateUserProfileAsync(dto, AuthorizedUserId);

        return HandleResult(result);
    }
}
