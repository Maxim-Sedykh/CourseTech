using Asp.Versioning;
using CourseTech.Domain.Dto.Analysis;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// AI-анализ ответов и генерация фидбэка
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AnalysisController : BaseApiController
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    /// <summary>
    /// AI-анализ ответа пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/analysis/analyze-answer
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "answerId": 67890,
    ///         "question": "В чем разница между abstract class и interface в C#?",
    ///         "answer": "Абстрактный класс может содержать реализацию методов...",
    ///         "keyPoints": ["Наследование", "Реализация методов", "Множественное наследование"],
    ///         "exampleAnswer": "Абстрактный класс может содержать конструкторы, поля, реализованные методы..."
    ///     }
    /// </remarks>
    /// <param name="dto">Данные для анализа ответа</param>
    /// <returns>AI-анализ ответа</returns>
    /// <response code="200">Успешный анализ ответа</response>
    /// <response code="400">Ошибка при анализе ответа</response>
    [HttpPost("analyze-answer")]
    [ProducesResponseType(typeof(AnswerAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnswerAnalysisDto>> AnalyzeAnswer([FromBody] AnalyzeAnswerDto dto)
    {
        var result = await _analysisService.AnalyzeAnswerAsync(dto);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Генерация фидбэка на основе ответа
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/analysis/generate-feedback
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "question": "Что такое SOLID принципы?",
    ///         "answer": "SOLID - это принципы ООП...",
    ///         "keyPoints": ["Single Responsibility", "Open/Closed", "Liskov Substitution"]
    ///     }
    /// </remarks>
    /// <param name="dto">Данные для генерации фидбэка</param>
    /// <returns>Сгенерированный фидбэк</returns>
    /// <response code="200">Успешная генерация фидбэка</response>
    /// <response code="400">Ошибка при генерации фидбэка</response>
    [HttpPost("generate-feedback")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> GenerateFeedback([FromBody] GenerateFeedbackDto dto)
    {
        var result = await _analysisService.GenerateFeedbackAsync(dto);

        return HandleDataResult(result);
    }
}