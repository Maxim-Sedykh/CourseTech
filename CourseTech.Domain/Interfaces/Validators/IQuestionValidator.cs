using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Validators
{
    public interface IQuestionValidator
    {
        BaseResult ValidateUserLessonOnNull(UserProfile userProfile, Lesson lesson);

        BaseResult ValidateCorrectAnswersOnNull(IEnumerable<TestVariantDto> correctTestVariants, IEnumerable<OpenQuestionAnswerDto> openQuestionAnswers);

        BaseResult ValidateQuestions(List<ICheckQuestionDto> lessonQuestions, int userAnswersCount, LessonTypes lessonType);
    }
}
