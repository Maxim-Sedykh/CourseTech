using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

/// <summary>
/// Валидатор для получения результата прохождения курса.
/// </summary>
public interface ICourseResultValidator
{
    /// <summary>
    /// Валидирование пользователя и уроков, для получения результата прохождения курса пользователем
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="lessonCount"></param>
    /// <returns></returns>
    BaseResult ValidateUserCourseResult(UserProfile profile, int lessonCount);
}
