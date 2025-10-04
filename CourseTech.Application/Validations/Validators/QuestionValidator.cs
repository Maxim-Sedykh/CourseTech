using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Validations.Validators;

public class QuestionValidator : IQuestionValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateCorrectAnswersOnNull(IEnumerable<TestVariantDto> correctTestVariants, IEnumerable<OpenQuestionAnswerDto> openQuestionAnswers)
    {
        if (!correctTestVariants.Any())
        {
            return BaseResult.Failure((int)ErrorCode.TestQuestionsCorrectVariantsNotFound,
                ErrorMessage.TestQuestionsCorrectVariantsNotFound);
        }

        if (!openQuestionAnswers.Any())
        {
            return BaseResult.Failure((int)ErrorCode.OpenQuestionsAnswerVariantsNotFound,
                ErrorMessage.OpenQuestionsAnswerVariantsNotFound);
        }

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public BaseResult ValidateUserLessonOnNull(UserProfile userProfile, Section lesson)
    {
        if (userProfile == null)
        {
            return BaseResult.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        if (lesson == null)
        {
            return BaseResult.Failure((int)ErrorCode.LessonNotFound, ErrorMessage.LessonNotFound);
        }

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public BaseResult ValidateQuestions(List<ICheckQuestionDto> lessonQuestions,
                                        int userAnswersCount,
                                        LessonTypes currentLessonType)
    {
        if (lessonQuestions.Count != userAnswersCount
            || !lessonQuestions.OfType<TestQuestionCheckingDto>().Any() || !lessonQuestions.OfType<OpenQuestionCheckingDto>().Any())
        {
            return BaseResult.Failure((int)ErrorCode.LessonQuestionsNotFound,
                ErrorMessage.LessonQuestionsNotFound);
        }

        //if (currentLessonType != LessonTypes.Common && !lessonQuestions.OfType<PracticalQuestionCheckingDto>().Any())
        //{
        //    return BaseResult.Failure((int)ErrorCodes.InvalidLessonType,
        //        ErrorMessage.InvalidLessonType);
        //}

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public BaseResult ValidateLessonQuestions(Section lesson, IEnumerable<IQuestionDto> questions)
    {
        if (lesson is null)
        {
            return DataResult<LessonPracticeDto>.Failure((int)ErrorCode.LessonNotFound, ErrorMessage.LessonNotFound);
        }

        return BaseResult.Success();
    }
}
