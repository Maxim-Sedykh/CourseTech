using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Helpers
{
    public interface IQuestionAnswerChecker
    {
        ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariantDto, ref float userGrade);

        ICorrectAnswerDto CheckOpenQuestionAnswer(OpenQuestionUserAnswerDto userAnswer, List<string> openQuestionAnswerVariants, ref float userGrade);

        ICorrectAnswerDto CheckPracticalQuestionAnswer(PracticalQuestionUserAnswerDto userAnswer, string rightQueryCode, List<string> questionKeywords, out float questionGrade);
    }
}
