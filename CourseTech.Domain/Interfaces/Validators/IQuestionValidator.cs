using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

/// <summary>
/// Валидатор практической части урока ( вопросов ).
/// </summary>
public interface IQuestionValidator
{
    /// <summary>
    /// Валидация профиля и урока на null.
    /// </summary>
    /// <param name="userProfile"></param>
    /// <param name="lesson"></param>
    /// <returns></returns>
    BaseResult ValidateUserLessonOnNull(UserProfile userProfile, Section lesson);

    /// <summary>
    /// Валидация правильных моделей тестовых вариантов, и открытых вопросов на null.
    /// </summary>
    /// <param name="correctTestVariants"></param>
    /// <param name="openQuestionAnswers"></param>
    /// <returns></returns>
    BaseResult ValidateCorrectAnswersOnNull(IEnumerable<TestVariantDto> correctTestVariants, IEnumerable<OpenQuestionAnswerDto> openQuestionAnswers);

    /// <summary>
    /// Валидация получения вопросов из БД, для прохождения практической части урока
    /// </summary>
    /// <param name="lessonQuestions"></param>
    /// <param name="userAnswersCount"></param>
    /// <param name="lessonType"></param>
    /// <returns></returns>
    BaseResult ValidateQuestions(List<ICheckQuestionDto> lessonQuestions, int userAnswersCount, LessonTypes lessonType);

    /// <summary>
    /// Валидация получения вопросов определённого урока
    /// </summary>
    /// <param name="lesson"></param>
    /// <param name="questions"></param>
    /// <returns></returns>
    BaseResult ValidateLessonQuestions(Section lesson, IEnumerable<IQuestionDto> questions);
}
