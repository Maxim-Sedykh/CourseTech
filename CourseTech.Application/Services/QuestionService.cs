using CourseTech.Domain;
using CourseTech.Domain.Dto.Category;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;

namespace CourseTech.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public QuestionService(
            IQuestionRepository questionRepository,
            ICategoryRepository categoryRepository)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<QuestionDto>> GetRandomQuestionAsync(QuestionFilterDto filter)
        {
            var category = await _categoryRepository.GetByIdAsync(filter.CategoryId);
            if (category == null)
                return Result<QuestionDto>.Failure("Категория не найдена");

            var question = await _questionRepository.GetRandomAsync(
                filter.CategoryId,
                filter.Difficulty,
                filter.ExcludedQuestionIds);

            if (question == null)
                return Result<QuestionDto>.Failure("Вопросы по заданным критериям не найдены");

            var questionDto = MapToQuestionDto(question, category);
            return Result.Success(questionDto);
        }

        public async Task<Result<QuestionDto>> GetQuestionByIdAsync(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
                return Result<QuestionDto>.Failure("Вопрос не найден");

            var category = await _categoryRepository.GetByIdAsync(question.CategoryId);
            var questionDto = MapToQuestionDto(question, category);

            return Result.Success(questionDto);
        }

        public async Task<CollectionResult<QuestionDto>> GetQuestionsByCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return CollectionResult<QuestionDto>.Failure("Категория не найдена");

            var questions = await _questionRepository.GetByCategoryIdAsync(categoryId);
            var questionDtos = questions.Select(q => MapToQuestionDto(q, category)).ToList();

            return CollectionResult<QuestionDto>.Success(questionDtos);
        }

        private QuestionDto MapToQuestionDto(Question question, Category category)
        {
            return new QuestionDto
            {
                Id = question.Id,
                CategoryId = question.CategoryId,
                Title = question.Title,
                Difficulty = question.Difficulty ?? "Middle",
                ExampleAnswer = question.ExampleAnswer,
                KeyPoints = question.KeyPoints?.Split(',').ToList() ?? new List<string>(),
                Category = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IconUrl = category.IconUrl,
                    CreatedAt = category.CreatedAt,
                    QuestionsCount = 0 // Можно рассчитать при необходимости
                },
                CreatedAt = question.CreatedAt
            };
        }
    }
}
