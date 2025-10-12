using CourseTech.Domain;
using CourseTech.Domain.Dto.Statistic;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services;

public class InterviewStatisticService : IStatisticService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAnalysisRepository _answerAnalysisRepository;

    public InterviewStatisticService(
        IAnswerRepository answerRepository,
        ISessionRepository sessionRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        IAnalysisRepository answerAnalysisRepository)
    {
        _answerRepository = answerRepository;
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _answerAnalysisRepository = answerAnalysisRepository;
    }

    public async Task<Result<UserStatisticsDto>> GetUserStatisticsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<UserStatisticsDto>.Failure("Пользователь не найден");

        // Получаем все ответы пользователя с анализами
        var userAnswers = await _answerRepository.GetByUserIdAsync(userId);
        var analyses = await _answerAnalysisRepository.GetByAnswerIdsAsync(
            userAnswers.Select(a => a.Id).ToList());

        // Получаем сессии пользователя
        var userSessions = await _sessionRepository.GetByUserIdAsync(userId);
        var completedSessions = userSessions.Where(s => s.FinishedAt.HasValue).ToList();

        // Рассчитываем статистику
        var statistics = new UserStatisticsDto
        {
            TotalAnswers = userAnswers.Count,
            AverageScore = analyses.Any() ? analyses.Average(a => a.OverallScore) : 0,
            SessionsCompleted = completedSessions.Count,
            BestCategory = await GetBestCategoryAsync(userId, analyses),
            TotalPracticeTime = CalculateTotalPracticeTime(completedSessions),
            LastActivity = userAnswers.Any() ? userAnswers.Max(a => a.CreatedAt) : null
        };

        return Result.Success(statistics);
    }

    public async Task<Result<CategoryProgressDto[]>> GetCategoryProgressAsync(Guid userId)
    {
        var categories = await _categoryRepository.GetAll().ToListAsync();
        var progressList = new List<CategoryProgressDto>();

        foreach (var category in categories)
        {
            // Получаем ответы по категории
            var categoryAnswers = await _answerRepository.GetByUserIdAndCategoryAsync(userId, category.Id);
            if (!categoryAnswers.Any())
                continue;

            // Получаем анализы для этих ответов
            var analyses = await _answerAnalysisRepository.GetByAnswerIdsAsync(
                categoryAnswers.Select(a => a.Id).ToList());

            var progress = new CategoryProgressDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                AverageScore = analyses.Any() ? analyses.Average(a => a.OverallScore) : 0,
                AnswersCount = categoryAnswers.Count,
                Improvement = CalculateImprovement(categoryAnswers, analyses)
            };

            progressList.Add(progress);
        }

        return Result<CategoryProgressDto>.Success(progressList.ToArray());
    }

    public async Task<Result<SessionSummaryDto>> GetSessionSummaryAsync(long sessionId, Guid userId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null || session.UserId != userId)
            return Result<SessionSummaryDto>.Failure("Сессия не найдена");

        var category = await _categoryRepository.GetByIdAsync(session.CategoryId);
        var answers = await _answerRepository.GetBySessionIdAsync(sessionId);
        var analyses = await _answerAnalysisRepository.GetByAnswerIdsAsync(
            answers.Select(a => a.Id).ToList());

        var summary = new SessionSummaryDto
        {
            SessionId = sessionId,
            CategoryName = category?.Name ?? "Неизвестная категория",
            AverageScore = analyses.Any() ? analyses.Average(a => a.OverallScore) : 0,
            TotalAnswers = answers.Count,
            Duration = session.FinishedAt.HasValue ?
                session.FinishedAt.Value - session.CreatedAt : TimeSpan.Zero,
            CompletedAt = session.FinishedAt ?? DateTime.UtcNow
        };

        return Result.Success(summary);
    }

    private async Task<string> GetBestCategoryAsync(Guid userId, List<Analysis> analyses)
    {
        if (!analyses.Any())
            return "Нет данных";

        // Группируем анализы по категориям и находим категорию с наивысшим средним баллом
        var categoryScores = new Dictionary<int, (double Score, int Count)>();

        foreach (var analysis in analyses)
        {
            var answer = await _answerRepository.GetByIdAsync(analysis.AnswerId);
            if (answer == null) continue;

            var session = await _sessionRepository.GetByIdAsync(answer.SessionId);
            if (session == null) continue;

            if (!categoryScores.ContainsKey(session.CategoryId))
            {
                categoryScores[session.CategoryId] = (0, 0);
            }

            var (currentScore, currentCount) = categoryScores[session.CategoryId];
            categoryScores[session.CategoryId] = (currentScore + analysis.OverallScore, currentCount + 1);
        }

        var bestCategory = categoryScores
            .OrderByDescending(x => x.Value.Score / x.Value.Count)
            .FirstOrDefault();

        if (bestCategory.Key == 0)
            return "Нет данных";

        var category = await _categoryRepository.GetByIdAsync(bestCategory.Key);
        return category?.Name ?? "Неизвестная категория";
    }

    private int CalculateTotalPracticeTime(List<Session> sessions)
    {
        var totalMinutes = 0;
        foreach (var session in sessions.Where(s => s.FinishedAt.HasValue))
        {
            totalMinutes += (int)(session.FinishedAt.Value - session.CreatedAt).TotalMinutes;
        }
        return totalMinutes;
    }

    private int CalculateImprovement(List<Answer> answers, List<Analysis> analyses)
    {
        if (answers.Count < 2 || !analyses.Any())
            return 0;

        // Сортируем ответы по дате
        var sortedAnswers = answers.OrderBy(a => a.CreatedAt).ToList();

        // Берем первые 25% и последние 25% ответов для сравнения
        var quarterCount = Math.Max(1, sortedAnswers.Count / 4);

        var earlyAnswers = sortedAnswers.Take(quarterCount).ToList();
        var recentAnswers = sortedAnswers.TakeLast(quarterCount).ToList();

        var earlyAnalyses = analyses.Where(a => earlyAnswers.Any(ea => ea.Id == a.AnswerId)).ToList();
        var recentAnalyses = analyses.Where(a => recentAnswers.Any(ra => ra.Id == a.AnswerId)).ToList();

        if (!earlyAnalyses.Any() || !recentAnalyses.Any())
            return 0;

        var earlyAverage = earlyAnalyses.Average(a => a.OverallScore);
        var recentAverage = recentAnalyses.Average(a => a.OverallScore);

        if (earlyAverage == 0) return 100; // Если начинал с нуля

        var improvement = (int)((recentAverage - earlyAverage) / earlyAverage * 100);
        return Math.Max(0, improvement);
    }
}
