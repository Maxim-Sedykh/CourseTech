using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с категориями.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Получение всех категорий.
        /// </summary>
        Task<CollectionResult<CategoryDto>> GetCategoriesAsync();

        /// <summary>
        /// Получение категории по идентификатору.
        /// </summary>
        Task<DataResult<CategoryDto>> GetCategoryByIdAsync(int categoryId);
    }
}
