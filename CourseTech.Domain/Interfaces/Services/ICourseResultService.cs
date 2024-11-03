using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для получения данных о результате прохождения пользователем курса
    /// и его анализа прохождения курса.
    /// </summary>
    public interface ICourseResultService
    {
        /// <summary>
        /// Получение результата прохождения пользователем курса по идентификатору пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<CourseResultDto>> GetCourseResultAsync(Guid userId);

        /// <summary>
        /// Получение анализа прохождения курса пользователем по его идентификатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UserAnalysDto>> GetUserAnalys(Guid userId);
    }
}
