using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Validations.Validators
{
    public class QuestionValidator : IQuestionValidator
    {
        public BaseResult ValidateCorrectAnswersOnNull(IEnumerable<TestVariantDto> correctTestVariants, IEnumerable<OpenQuestionAnswerDto> openQuestionAnswers)
        {
            if (!correctTestVariants.Any())
            {
                return BaseResult.Failure((int)ErrorCodes.TestQuestionsCorrectVariantsNotFound,
                    ErrorMessage.TestQuestionsCorrectVariantsNotFound);
            }

            if (!openQuestionAnswers.Any())
            {
                return BaseResult.Failure((int)ErrorCodes.OpenQuestionsAnswerVariantsNotFound,
                    ErrorMessage.OpenQuestionsAnswerVariantsNotFound);
            }

            return BaseResult.Success();
        }

        public BaseResult ValidateUserLessonOnNull(UserProfile userProfile, Lesson lesson)
        {
            if (userProfile == null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            if (lesson == null)
            {
                return BaseResult.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            return BaseResult.Success();
        }

        public BaseResult ValidateQuestions(List<ICheckQuestionDto> lessonQuestions,
                                            int userAnswersCount,
                                            LessonType currentLessonType)
        {
            if (!lessonQuestions.Any() || lessonQuestions.Count != userAnswersCount
                || !lessonQuestions.OfType<TestQuestionCheckingDto>().Any() || !lessonQuestions.OfType<OpenQuestionCheckingDto>().Any())
            {
                return BaseResult.Failure((int)ErrorCodes.LessonQuestionsNotFound,
                    ErrorMessage.LessonQuestionsNotFound);
            }

            if (currentLessonType != LessonType.Common && !lessonQuestions.OfType<PracticalQuestionCheckingDto>().Any())
            {
                return BaseResult.Failure((int)ErrorCodes.InvalidLessonType,
                    ErrorMessage.InvalidLessonType);
            }

            return BaseResult.Success();
        }
    }
}
