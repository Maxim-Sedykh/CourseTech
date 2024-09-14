using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Helpers
{
    public interface IQuestionAnswerChecker
    {
        List<ICorrectAnswerDto> CheckUserAnswers(List<ICheckQuestionDto> checkQuestions, List<IUserAnswerDto> userAnswers, out float userGrade);
    }
}
