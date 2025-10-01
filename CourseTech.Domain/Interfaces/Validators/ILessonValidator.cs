using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

/// <summary>
/// Валидатор для сервиса работы с уроками.
/// </summary>
public interface ILessonValidator
{
    /// <summary>
    /// Валидация получения информации об уроках для пользователя
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="lessons"></param>
    /// <returns></returns>
    BaseResult ValidateLessonsForUser(UserProfile profile, IEnumerable<LessonDto> lessons);
}
