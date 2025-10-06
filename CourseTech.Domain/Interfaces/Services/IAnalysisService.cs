using CourseTech.Domain.Dto.Analysis;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IAnalysisService
    {
        /// <summary>
        /// Анализ ответа пользователя.
        /// </summary>
        Task<Result<AnswerAnalysisDto>> AnalyzeAnswerAsync(AnalyzeAnswerDto dto);

        /// <summary>
        /// Генерация фидбэка на основе ответа.
        /// </summary>
        Task<Result<string>> GenerateFeedbackAsync(GenerateFeedbackDto dto);
    }
}
