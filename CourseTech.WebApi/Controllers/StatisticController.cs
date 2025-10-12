using Asp.Versioning;
using CourseTech.Domain.Dto.Statistic;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Получение статистики и прогресса пользователя
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class StatisticsController : BaseApiController
{
    private readonly IStatisticService _statisticService;

    public StatisticsController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }

    /// <summary>
    /// Получение общей статистики текущего пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/statistics/my-statistics
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <returns>Статистика пользователя</returns>
    /// <response code="200">Успешное получение статистики</response>
    [HttpGet("my-statistics")]
    [ProducesResponseType(typeof(UserStatisticsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserStatisticsDto>> GetMyStatistics()
    {
        var result = await _statisticService.GetUserStatisticsAsync(AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение прогресса по категориям
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/statistics/my-progress
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <returns>Прогресс по категориям</returns>
    /// <response code="200">Успешное получение прогресса</response>
    [HttpGet("my-progress")]
    [ProducesResponseType(typeof(List<CategoryProgressDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryProgressDto[]>> GetMyProgress()
    {
        var result = await _statisticService.GetCategoryProgressAsync(AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение сводки по сессии
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/statistics/sessions/12345/summary
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="sessionId">Идентификатор сессии</param>
    /// <returns>Сводка по сессии</returns>
    /// <response code="200">Успешное получение сводки</response>
    /// <response code="400">Сессия не найдена</response>
    [HttpGet("sessions/{sessionId:long}/summary")]
    [ProducesResponseType(typeof(SessionSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionSummaryDto>> GetSessionSummary(long sessionId)
    {
        var result = await _statisticService.GetSessionSummaryAsync(sessionId, AuthorizedUserId);

        return HandleDataResult(result);
    }
}