using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IAnalysisService
    {
        /// <summary>
        /// Анализ ответа пользователя.
        /// </summary>
        Task<DataResult<AnswerAnalysisDto>> AnalyzeAnswerAsync(AnalyzeAnswerDto dto);

        /// <summary>
        /// Генерация фидбэка на основе ответа.
        /// </summary>
        Task<DataResult<string>> GenerateFeedbackAsync(GenerateFeedbackDto dto);
    }
}
