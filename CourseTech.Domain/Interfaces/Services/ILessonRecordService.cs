using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с записями о прохождении какого-либо урока определённым пользователем.
/// </summary>
public interface ILessonRecordService
{
    /// <summary>
    /// Получение записей о прохождении уроков пользователем по индентификатору пользователя.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<CollectionResult<LessonRecordDto>> GetUserLessonRecordsAsync(Guid userId);
}
