using Asp.Versioning;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Session;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Управление сессиями собеседований
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class SessionsController : BaseApiController
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    /// <summary>
    /// Начало новой сессии собеседования
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/sessions/start
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "categoryId": 1,
    ///         "difficulty": "Middle"
    ///     }
    /// </remarks>
    /// <param name="config">Конфигурация сессии</param>
    /// <returns>Созданная сессия</returns>
    /// <response code="200">Сессия успешно создана</response>
    /// <response code="400">Ошибка при создании сессии</response>
    [HttpPost("start")]
    [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionDto>> StartSession([FromBody] SessionConfigDto config)
    {
        var result = await _sessionService.StartSessionAsync(config, AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение сессии по идентификатору
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/sessions/12345
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="sessionId">Идентификатор сессии</param>
    /// <returns>Сессия собеседования</returns>
    /// <response code="200">Успешное получение сессии</response>
    /// <response code="400">Сессия не найдена</response>
    [HttpGet("{sessionId:long}")]
    [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionDto>> GetSessionById(long sessionId)
    {
        var result = await _sessionService.GetSessionByIdAsync(sessionId, AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение следующего вопроса для сессии
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/sessions/12345/next-question
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="sessionId">Идентификатор сессии</param>
    /// <returns>Следующий вопрос для ответа</returns>
    /// <response code="200">Успешное получение вопроса</response>
    /// <response code="400">Сессия не найдена или нет доступных вопросов</response>
    [HttpGet("{sessionId:long}/next-question")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto>> GetNextQuestion(long sessionId)
    {
        var result = await _sessionService.GetNextQuestionAsync(sessionId, AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Завершение сессии собеседования
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/sessions/12345/finish
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="sessionId">Идентификатор сессии</param>
    /// <returns>Завершенная сессия</returns>
    /// <response code="200">Сессия успешно завершена</response>
    /// <response code="400">Ошибка при завершении сессии</response>
    [HttpPost("{sessionId:long}/finish")]
    [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionDto>> FinishSession(long sessionId)
    {
        var result = await _sessionService.FinishSessionAsync(sessionId, AuthorizedUserId);

        return HandleResult(result);
    }
}
