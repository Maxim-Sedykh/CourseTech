using CourseTech.Domain.Dto.Answer;
using CourseTech.Domain.Dto.Category;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Session;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain;

namespace CourseTech.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserRepository _userRepository;

        public SessionService(
            ISessionRepository sessionRepository,
            IQuestionRepository questionRepository,
            ICategoryRepository categoryRepository,
            IAnswerRepository answerRepository,
            IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<SessionDto>> StartSessionAsync(SessionConfigDto config, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result<SessionDto>.Failure("Пользователь не найден");

            var category = await _categoryRepository.GetByIdAsync(config.CategoryId);
            if (category == null)
                return Result<SessionDto>.Failure("Категория не найдена");

            var session = new Session
            {
                UserId = userId,
                CategoryId = config.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            await _sessionRepository.CreateAsync(session);

            var sessionDto = await MapToSessionDto(session, user, category);

            return Result.Success(sessionDto);
        }

        public async Task<Result<QuestionDto>> GetNextQuestionAsync(long sessionId, Guid userId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
                return Result<QuestionDto>.Failure("Сессия не найдена");

            if (session.FinishedAt.HasValue)
                return Result<QuestionDto>.Failure("Сессия уже завершена");

            // Получаем ID уже отвеченных вопросов в этой сессии
            var answeredQuestionIds = await _answerRepository.GetQuestionIdsBySessionIdAsync(sessionId);

            var question = await _questionRepository.GetRandomAsync(
                session.CategoryId,
                null, // difficulty можно брать из сессии, если добавить это поле
                [.. answeredQuestionIds.Select(id => (int)id)]);

            if (question == null)
                return Result<QuestionDto>.Failure("Нет доступных вопросов для этой сессии");

            var category = await _categoryRepository.GetByIdAsync(question.CategoryId);

            var questionDto = MapToQuestionDto(question, category);

            return Result.Success(questionDto);
        }

        public async Task<Result<SessionDto>> FinishSessionAsync(long sessionId, Guid userId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
                return Result<SessionDto>.Failure("Сессия не найдена");

            if (session.FinishedAt.HasValue)
                return Result<SessionDto>.Failure("Сессия уже завершена");

            session.FinishedAt = DateTime.UtcNow;
            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);
            var category = await _categoryRepository.GetByIdAsync(session.CategoryId);
            var sessionDto = await MapToSessionDto(session, user, category);

            return Result.Success(sessionDto);
        }

        public async Task<Result<SessionDto>> GetSessionByIdAsync(long sessionId, Guid userId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
                return Result<SessionDto>.Failure("Сессия не найдена");

            var user = await _userRepository.GetByIdAsync(userId);
            var category = await _categoryRepository.GetByIdAsync(session.CategoryId);
            var sessionDto = await MapToSessionDto(session, user, category);

            return Result.Success(sessionDto);
        }

        private async Task<SessionDto> MapToSessionDto(Session session, User user, Category category)
        {
            var answers = await _answerRepository.GetBySessionIdAsync(session.Id);

            return new SessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                CategoryId = session.CategoryId,
                Difficulty = "Middle", // Можно добавить поле в Session entity
                CreatedAt = session.CreatedAt,
                FinishedAt = session.FinishedAt,
                Category = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IconUrl = category.IconUrl,
                    CreatedAt = category.CreatedAt,
                    QuestionsCount = 0
                },
                Answers = answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    SessionId = a.SessionId,
                    QuestionId = (int)a.QuestionId,
                    AudioFileUrl = a.AudioFileUrl,
                    TranscribedText = a.TranscribedText,
                    CreatedAt = a.CreatedAt
                }).ToList(),
                User = new UserDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    Roles = new List<string> { user.Role.ToString() },
                    IsExamCompleted = false,
                    IsEditAble = true
                }
            };
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
                    QuestionsCount = 0
                },
                CreatedAt = question.CreatedAt
            };
        }
    }
}
