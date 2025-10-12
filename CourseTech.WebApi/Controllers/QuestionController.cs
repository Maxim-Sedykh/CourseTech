using Asp.Versioning;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Управление вопросами для собеседований
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class QuestionsController : BaseApiController
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    /// <summary>
    /// Получение случайного вопроса по критериям
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/questions/random
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "categoryId": 1,
    ///         "difficulty": "Middle",
    ///         "excludedQuestionIds": [5, 10, 15]
    ///     }
    /// </remarks>
    /// <param name="filter">Фильтр для выбора вопроса</param>
    /// <returns>Случайный вопрос</returns>
    /// <response code="200">Успешное получение вопроса</response>
    /// <response code="400">Вопросы по заданным критериям не найдены</response>
    [HttpPost("random")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto>> GetRandomQuestion([FromBody] QuestionFilterDto filter)
    {
        var result = await _questionService.GetRandomQuestionAsync(filter);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение вопроса по идентификатору
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/questions/123
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="questionId">Идентификатор вопроса</param>
    /// <returns>Вопрос для собеседования</returns>
    /// <response code="200">Успешное получение вопроса</response>
    /// <response code="400">Вопрос не найден</response>
    [HttpGet("{questionId:int}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(int questionId)
    {
        var result = await _questionService.GetQuestionByIdAsync(questionId);

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение всех вопросов по категории
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/questions/category/1
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <returns>Список вопросов категории</returns>
    /// <response code="200">Успешное получение вопросов</response>
    /// <response code="400">Категория не найдена</response>
    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(typeof(List<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto[]>> GetQuestionsByCategory(int categoryId)
    {
        var result = await _questionService.GetQuestionsByCategoryAsync(categoryId);

        return HandleDataResult(result);
    }
}
