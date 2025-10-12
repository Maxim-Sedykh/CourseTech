using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.User;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User;

/// <summary>
/// Управление пользователями системы (только для администраторов и модераторов)
/// </summary>
[AllowRoles(Role.Admin, Role.Moderator)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly UpdateUserValidator _updateUserValidator;

    public UserController(IUserService userService, UpdateUserValidator updateUserValidator)
    {
        _userService = userService;
        _updateUserValidator = updateUserValidator;
    }

    /// <summary>
    /// Получение списка всех пользователей
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/users
    ///     Headers: Authorization: Bearer {token}
    ///     
    /// Требуемые роли: Admin, Moderator
    /// </remarks>
    /// <returns>Список пользователей системы</returns>
    /// <response code="200">Успешное получение списка пользователей</response>
    /// <response code="403">Недостаточно прав</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserDto[]>> GetUsersAsync()
    {
        var result = await _userService.GetUsersAsync();

        return HandleDataResult(result);
    }

    /// <summary>
    /// Удаление пользователя по идентификатору
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/users/12345678-1234-1234-1234-123456789abc
    ///     Headers: Authorization: Bearer {token}
    ///     
    /// Требуемые роли: Admin
    /// </remarks>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Пользователь успешно удален</response>
    /// <response code="400">Ошибка при удалении пользователя</response>
    /// <response code="403">Недостаточно прав</response>
    [AllowRoles(Role.Admin)]
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteUserAsync(Guid userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        return HandleResult(result);
    }

    /// <summary>
    /// Получение пользователя по идентификатору
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/users/12345678-1234-1234-1234-123456789abc
    ///     Headers: Authorization: Bearer {token}
    ///     
    /// Требуемые роли: Admin, Moderator
    /// </remarks>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Данные пользователя</returns>
    /// <response code="200">Успешное получение пользователя</response>
    /// <response code="400">Пользователь не найден</response>
    /// <response code="403">Недостаточно прав</response>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UpdateUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UpdateUserDto>> GetUserByIdAsync(Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Обновление данных пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     PUT /api/v1/users
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "id": "12345678-1234-1234-1234-123456789abc",
    ///         "login": "new_username",
    ///         "userName": "Новое имя",
    ///         "surname": "Новая фамилия",
    ///         "isEditAble": true
    ///     }
    ///     
    /// Требуемые роли: Admin, Moderator
    /// </remarks>
    /// <param name="dto">Данные для обновления пользователя</param>
    /// <returns>Обновленные данные пользователя</returns>
    /// <response code="200">Данные пользователя успешно обновлены</response>
    /// <response code="400">Ошибки валидации или обновления</response>
    /// <response code="403">Недостаточно прав</response>
    [HttpPut]
    [ProducesResponseType(typeof(UpdateUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UpdateUserDto>> UpdateUserAsync([FromBody] UpdateUserDto dto)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(string.Join(Environment.NewLine, errors));
        }

        var result = await _userService.UpdateUserDataAsync(dto);

        return HandleDataResult(result);
    }
}
