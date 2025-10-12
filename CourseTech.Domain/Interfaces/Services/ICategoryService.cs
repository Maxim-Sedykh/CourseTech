using CourseTech.Domain.Dto.Category;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с категориями.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Получение всех категорий.
    /// </summary>
    Task<Result<CategoryDto[]>> GetCategoriesAsync();

    /// <summary>
    /// Получение категории по идентификатору.
    /// </summary>
    Task<Result<CategoryDto>> GetCategoryByIdAsync(int categoryId);
}
