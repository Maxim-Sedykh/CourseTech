using Asp.Versioning;
using CourseTech.Domain.Dto.Analysis;
using CourseTech.Domain.Dto.Answer;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Управление ответами пользователей на вопросы собеседований
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AnswersController : BaseApiController
{
    private readonly IAnswerService _answerService;

    public AnswersController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    /// <summary>
    /// Отправка голосового ответа на вопрос
    /// </summary>
    /// <remarks>
    /// Пример запроса (multipart/form-data):
    /// 
    ///     POST /api/v1/answers/sessions/12345/answers
    ///     Headers: 
    ///         Authorization: Bearer {token}
    ///         Content-Type: multipart/form-data
    ///     
    ///     Form data:
    ///     - sessionId: 12345
    ///     - questionId: 67
    ///     - audioFile: [бинарные данные аудиофайла]
    ///     
    /// Поддерживаемые форматы: WAV, MP3, M4A, WEBM (макс. 50MB)
    /// </remarks>
    /// <param name="sessionId">Идентификатор сессии</param>
    /// <param name="dto">Данные ответа с аудиофайлом</param>
    /// <returns>Результат обработки ответа с анализом</returns>
    /// <response code="200">Ответ успешно обработан</response>
    /// <response code="400">Ошибка при обработке ответа</response>
    [HttpPost("sessions/{sessionId:long}/answers")]
    [ProducesResponseType(typeof(AnswerResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnswerResultDto>> SubmitAnswer(long sessionId, [FromForm] ProcessAnswerDto dto)
    {
        dto.SessionId = sessionId;
        var result = await _answerService.ProcessAnswerAsync(dto, AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение AI-анализа ответа
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/answers/67890/analysis
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="answerId">Идентификатор ответа</param>
    /// <returns>AI-анализ ответа</returns>
    /// <response code="200">Успешное получение анализа</response>
    /// <response code="400">Анализ не найден</response>
    [HttpGet("{answerId:long}/analysis")]
    [ProducesResponseType(typeof(AnswerAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnswerAnalysisDto>> GetAnswerAnalysis(long answerId)
    {
        var result = await _answerService.GetAnswerAnalysisAsync(answerId, AuthorizedUserId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение ответов текущего пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/answers/my-answers?page=1&amp;pageSize=10&amp;categoryId=1
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="filter">Фильтр и пагинация</param>
    /// <returns>Список ответов пользователя</returns>
    /// <response code="200">Успешное получение ответов</response>
    [HttpGet("my-answers")]
    [ProducesResponseType(typeof(List<AnswerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AnswerDto>>> GetUserAnswers([FromQuery] AnswerFilterDto filter)
    {
        var result = await _answerService.GetUserAnswersAsync(AuthorizedUserId, filter);

        return HandleDataResult(result);
    }
}