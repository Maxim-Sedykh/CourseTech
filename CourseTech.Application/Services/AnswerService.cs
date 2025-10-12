using CourseTech.Domain;
using CourseTech.Domain.Dto.Analysis;
using CourseTech.Domain.Dto.Answer;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Session;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;

namespace CourseTech.Application.Services;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnalysisService _analysisService;
    //private readonly IFileStorageService _fileStorageService;
    //private readonly ISpeechToTextService _speechToTextService;

    public AnswerService(
        IAnswerRepository answerRepository,
        ISessionRepository sessionRepository,
        IQuestionRepository questionRepository,
        IAnalysisService analysisService)/*,*/
        //IFileStorageService fileStorageService,
        //ISpeechToTextService speechToTextService)
    {
        _answerRepository = answerRepository;
        _sessionRepository = sessionRepository;
        _questionRepository = questionRepository;
        _analysisService = analysisService;
        //_fileStorageService = fileStorageService;
        //_speechToTextService = speechToTextService;
    }

    public async Task<Result<AnswerResultDto>> ProcessAnswerAsync(ProcessAnswerDto dto, Guid userId)
    {
        var session = await _sessionRepository.GetByIdAsync(dto.SessionId);
        if (session == null || session.UserId != userId)
            return Result<AnswerResultDto>.Failure("Session is not found");

        if (session.FinishedAt.HasValue)
            return Result<AnswerResultDto>.Failure("Session is already completed");

        var question = await _questionRepository.GetByIdAsync(dto.QuestionId);
        if (question == null)
            return Result<AnswerResultDto>.Failure("Question is not found");


        //var audioFileUrl = await _fileStorageService.SaveAudioFileAsync(dto.AudioFile);

        //var transcribedText = await _speechToTextService.TranscribeAsync(dto.AudioFile);

        var answer = new Answer
        {
            SessionId = dto.SessionId,
            QuestionId = dto.QuestionId,
            AudioFileUrl = string.Empty,
            TranscribedText = string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        await _answerRepository.CreateAsync(answer);

        // Анализ ответа (можно запустить в фоне)
        var analysisResult = await _analysisService.AnalyzeAnswerAsync(new AnalyzeAnswerDto
        {
            AnswerId = answer.Id,
            Question = question.Title,
            Answer = string.Empty,
            KeyPoints = question.KeyPoints?.Split(',').ToList() ?? new List<string>(),
            ExampleAnswer = question.ExampleAnswer
        });

        var answerDto = MapToAnswerDto(answer, question, session);
        var resultDto = new AnswerResultDto
        {
            Answer = answerDto,
            Analysis = analysisResult.IsSuccess ? analysisResult.Data : null
        };

        return Result.Success(resultDto);
    }

    public async Task<Result<AnswerAnalysisDto>> GetAnswerAnalysisAsync(long answerId, Guid userId)
    {
        var answer = await _answerRepository.GetByIdAsync(answerId);
        if (answer == null)
            return Result<AnswerAnalysisDto>.Failure(string.Empty); // TODO заменить все стринг эмпти на нормальные сообщения на английском.

        var session = await _sessionRepository.GetByIdAsync(answer.SessionId);
        if (session == null || session.UserId != userId)
            return Result<AnswerAnalysisDto>.Failure(string.Empty);

        // Здесь можно получить анализ из базы, если он сохраняется
        // Пока возвращаем заглушку
        var analysis = new AnswerAnalysisDto
        {
            Id = 1, // Временное значение
            AnswerId = answerId,
            CompletenessScore = 8,
            ClarityScore = 7,
            TechnicalAccuracyScore = 9,
            OverallScore = 8,
            Strengths = new List<string> { "Хорошее понимание темы", "Структурированный ответ" },
            Weaknesses = new List<string> { "Можно добавить больше примеров" },
            DetailedFeedback = "Отличный ответ! Вы хорошо разбираетесь в теме, но можно добавить больше практических примеров.",
            SuggestedAnswer = "Более полный ответ с примерами...",
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(analysis);
    }

    public async Task<Result<List<AnswerDto>>> GetUserAnswersAsync(Guid userId, AnswerFilterDto filter)
    {
        var answers = await _answerRepository.GetByUserIdAsync(userId, filter.Page, filter.PageSize, filter.CategoryId);
        var answerDtos = new List<AnswerDto>();

        foreach (var answer in answers)
        {
            var session = await _sessionRepository.GetByIdAsync(answer.SessionId);
            var question = await _questionRepository.GetByIdAsync((int)answer.QuestionId);

            answerDtos.Add(MapToAnswerDto(answer, question, session));
        }

        return Result.Success(answerDtos);
    }

    private AnswerDto MapToAnswerDto(Answer answer, Question question, Session session)
    {
        return new AnswerDto
        {
            Id = answer.Id,
            SessionId = answer.SessionId,
            QuestionId = (int)answer.QuestionId,
            AudioFileUrl = answer.AudioFileUrl,
            TranscribedText = answer.TranscribedText,
            CreatedAt = answer.CreatedAt,
            Question = question != null ? new QuestionDto
            {
                Id = question.Id,
                CategoryId = question.CategoryId,
                Title = question.Title,
                Difficulty = question.Difficulty ?? "Middle",
                ExampleAnswer = question.ExampleAnswer,
                KeyPoints = question.KeyPoints?.Split(',').ToList() ?? [],
                CreatedAt = question.CreatedAt
            } : null,
            Session = session != null ? new SessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                CategoryId = session.CategoryId,
                CreatedAt = session.CreatedAt,
                FinishedAt = session.FinishedAt
            } : null
        };
    }
}
