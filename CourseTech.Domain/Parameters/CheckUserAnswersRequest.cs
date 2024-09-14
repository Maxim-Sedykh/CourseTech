using Azure;
using CourseTech.Domain.Dto.Keyword;
using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Parameters
{
    public class CheckUserAnswersRequest
    {
        public List<IUserAnswerDto> UserAnswers { get; set; }

        public List<TestVariantDto> CorrectTestsVariants { get; set; }

        public List<OpenQuestionAnswerDto> OpenQuestionsAnswerVariants { get; set; }

        public List<PracticalQuestionCheckingDto> CheckPracticalQuestionDtos { get; set; }

        public List<KeywordDto> PracticalQuestionsKeywords { get; set; }

        public void Deconstruct(out List<IUserAnswerDto> userAnswers,
                           out List<TestVariantDto> correctTestsVariants,
                           out List<OpenQuestionAnswerDto> openQuestionsAnswerVariants,
                           out List<PracticalQuestionCheckingDto> checkPracticalQuestionDtos,
                           out List<KeywordDto> practicalQuestionsKeywords)
        {
            userAnswers = UserAnswers;
            correctTestsVariants = CorrectTestsVariants;
            openQuestionsAnswerVariants = OpenQuestionsAnswerVariants;
            checkPracticalQuestionDtos = CheckPracticalQuestionDtos;
            practicalQuestionsKeywords = PracticalQuestionsKeywords;
        }
    }
}
