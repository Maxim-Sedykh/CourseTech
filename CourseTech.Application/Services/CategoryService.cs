using CourseTech.Domain;
using CourseTech.Domain.Dto.Category;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IQuestionRepository questionRepository)
        {
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Result<List<CategoryDto>>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAll().ToArrayAsync();
            var categoryDtos = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var questionsCount = await _questionRepository.GetCountByCategoryIdAsync(category.Id);
                categoryDtos.Add(MapToCategoryDto(category, questionsCount));
            }

            return Result.Success(categoryDtos);
        }

        public async Task<Result<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result<CategoryDto>.Failure("Category is not found");

            var questionsCount = await _questionRepository.GetCountByCategoryIdAsync(categoryId);
            var categoryDto = MapToCategoryDto(category, questionsCount);

            return Result.Success(categoryDto);
        }

        private CategoryDto MapToCategoryDto(Category category, int questionsCount)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IconUrl = category.IconUrl,
                CreatedAt = category.CreatedAt,
                QuestionsCount = questionsCount
            };
        }
    }
}
