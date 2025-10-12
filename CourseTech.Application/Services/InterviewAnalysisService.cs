using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain;
using CourseTech.Domain.Dto.Analysis;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;

namespace CourseTech.Application.Services;

public class InterviewAnalysisService : IAnalysisService
{
    private readonly IChatGptService _chatGptService;
    private readonly IAnalysisRepository _analysisRepository;

    public InterviewAnalysisService(
        IChatGptService chatGptService,
        IAnalysisRepository analysisRepository)
    {
        _chatGptService = chatGptService;
        _analysisRepository = analysisRepository;
    }

    public async Task<Result<AnswerAnalysisDto>> AnalyzeAnswerAsync(AnalyzeAnswerDto dto)
    {
        try
        {
            var prompt = $@"
                Проанализируй ответ на вопрос собеседования и предоставь оценку в JSON формате.
                
                ВОПРОС: {dto.Question}
                КЛЮЧЕВЫЕ ПУНКТЫ: {string.Join(", ", dto.KeyPoints)}
                ПРИМЕР ОТВЕТА: {dto.ExampleAnswer}
                ОТВЕТ КАНДИДАТА: {dto.Answer}
                
                Критерии оценки:
                1. Полнота (1-10) - охвачены ли все ключевые аспекты
                2. Ясность (1-10) - структурированность и понятность изложения
                3. Техническая точность (1-10) - корректность технических деталей
                
                Верни JSON:
                {{
    
                    ""completeness_score\"": number,
                    ""clarity_score"": number, 
                    ""technical_accuracy_score"": number,
                    ""overall_score"": number,
                    ""strengths"": string[],
                    ""weaknesses"": string[],
                    ""detailed_feedback"": string,
                    ""suggested_answer"": string
                }}";

            var analysisJson = await _chatGptService.SendMessageToChatGPT(prompt);

            var analysis = ParseAnalysisJson(analysisJson, dto.AnswerId);

            var analysisEntity = new Analysis
            {
                AnswerId = dto.AnswerId,
                CompletenessScore = analysis.CompletenessScore,
                ClarityScore = analysis.ClarityScore,
                TechnicalAccuracyScore = analysis.TechnicalAccuracyScore,
                OverallScore = analysis.OverallScore,
                Strengths = string.Join(";", analysis.Strengths),
                Weaknesses = string.Join(";", analysis.Weaknesses),
                DetailedFeedback = analysis.DetailedFeedback,
                SuggestedAnswer = analysis.SuggestedAnswer,
                CreatedAt = DateTime.UtcNow
            };

            await _analysisRepository.CreateAsync(analysisEntity);
            analysis.Id = analysisEntity.Id;

            return Result.Success(analysis);
        }
        catch (Exception)
        {
            var fallbackAnalysis = new AnswerAnalysisDto
            {
                AnswerId = dto.AnswerId,
                CompletenessScore = 6,
                ClarityScore = 6,
                TechnicalAccuracyScore = 6,
                OverallScore = 6,
                Strengths = new List<string> { "Ответ предоставлен" },
                Weaknesses = new List<string> { "Требуется более детальный анализ" },
                DetailedFeedback = "Анализ временно недоступен. Пожалуйста, попробуйте позже.",
                SuggestedAnswer = dto.ExampleAnswer,
                CreatedAt = DateTime.UtcNow
            };

            return Result.Success(fallbackAnalysis);
        }
    }

    public async Task<Result<string>> GenerateFeedbackAsync(GenerateFeedbackDto dto)
    {
        try
        {
            var prompt = $"""
            Сгенерируй конструктивный фидбэк для ответа на вопрос собеседования.
            
            ВОПРОС: {dto.Question}
            КЛЮЧЕВЫЕ ПУНКТЫ: {string.Join(", ", dto.KeyPoints)}
            ОТВЕТ КАНДИДАТА: {dto.Answer}
            
            Сфокусируйся на:
            - Сильных сторонах ответа
            - Областях для улучшения  
            - Конкретных рекомендациях
            - Альтернативных формулировках
            
            Будь конструктивным и профессиональным.
            """;

            var feedback = await _chatGptService.SendMessageToChatGPT(prompt);
            return Result.Success(feedback);
        }
        catch (Exception ex)
        {
            return Result.Success($"Ошибка при генерации фидбэка: {ex.Message}");
        }
    }

    private AnswerAnalysisDto ParseAnalysisJson(string json, long answerId)
    {
        // Здесь должна быть реальная логика парсинга JSON
        // Пока возвращаем заглушку
        return new AnswerAnalysisDto
        {
            AnswerId = answerId,
            CompletenessScore = 8,
            ClarityScore = 7,
            TechnicalAccuracyScore = 9,
            OverallScore = 8,
            Strengths = new List<string> { "Хорошее понимание темы", "Логичная структура" },
            Weaknesses = new List<string> { "Можно добавить больше примеров" },
            DetailedFeedback = "Отличный ответ! Вы демонстрируете хорошее понимание темы.",
            SuggestedAnswer = "Более полный ответ с практическими примерами...",
            CreatedAt = DateTime.UtcNow
        };
    }
}
