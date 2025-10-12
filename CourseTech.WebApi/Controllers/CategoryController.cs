using Asp.Versioning;
using CourseTech.Domain.Dto.Category;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Управление категориями вопросов для собеседований
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Получение всех категорий вопросов
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/categories
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <returns>Список всех категорий</returns>
    /// <response code="200">Успешное получение категорий</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryDto[]>> GetCategories()
    {
        var result = await _categoryService.GetCategoriesAsync();

        return HandleDataResult(result);
    }

    /// <summary>
    /// Получение категории по идентификатору
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/categories/1
    ///     Headers: Authorization: Bearer {token}
    /// </remarks>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <returns>Категория вопросов</returns>
    /// <response code="200">Успешное получение категории</response>
    /// <response code="400">Категория не найдена</response>
    [HttpGet("{categoryId:int}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int categoryId)
    {
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        return HandleDataResult(result);
    }
}