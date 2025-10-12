using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Auth;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User;

/// <summary>
/// Управление аутентификацией и авторизацией пользователей
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly LoginUserValidator _loginUserValidator;
    private readonly RegisterUserValidator _registerUserValidator;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        LoginUserValidator loginUserValidator,
        RegisterUserValidator registerUserValidator)
    {
        _authService = authService;
        _tokenService = tokenService;
        _loginUserValidator = loginUserValidator;
        _registerUserValidator = registerUserValidator;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/auth/register
    ///     {
    ///         "email": "user@example.com",
    ///         "login": "username",
    ///         "password": "Password123!",
    ///         "firstName": "Иван",
    ///         "lastName": "Иванов",
    ///         "dateOfBirth": "1990-01-01"
    ///     }
    /// </remarks>
    /// <param name="dto">Данные для регистрации пользователя</param>
    /// <returns>Зарегистрированный пользователь</returns>
    /// <response code="200">Успешная регистрация</response>
    /// <response code="400">Ошибки валидации или пользователь уже существует</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto dto)
    {
        var validationResult = await _registerUserValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(string.Join(Environment.NewLine, errors));
        }

        var result = await _authService.Register(dto);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/auth/login
    ///     {
    ///         "login": "username",
    ///         "password": "Password123!"
    ///     }
    /// </remarks>
    /// <param name="dto">Учетные данные пользователя</param>
    /// <returns>Токены аутентификации</returns>
    /// <response code="200">Успешный вход в систему</response>
    /// <response code="400">Ошибки валидации</response>
    /// <response code="401">Неверные учетные данные</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto dto)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(string.Join(Environment.NewLine, errors));
        }

        var result = await _authService.Login(dto);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Выход из системы
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/auth/logout
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешный выход из системы</response>
    /// <response code="400">Ошибка при выходе из системы</response>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var result = await _authService.LogoutAsync(AuthorizedUserId);
        return HandleResult(result);
    }

    /// <summary>
    /// Обновление JWT токена
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/auth/refresh-token
    ///     {
    ///         "accessToken": "old_token",
    ///         "refreshToken": "refresh_token",
    ///         "expiresAt": "2024-01-15T12:00:00Z"
    ///     }
    /// </remarks>
    /// <param name="dto">Токены для обновления</param>
    /// <returns>Новые токены аутентификации</returns>
    /// <response code="200">Токены успешно обновлены</response>
    /// <response code="400">Неверные токены</response>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] TokenDto dto)
    {
        var result = await _tokenService.RefreshToken(dto);

        return HandleDataResult(result);
    }
}