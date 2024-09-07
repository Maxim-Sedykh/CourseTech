using CourseTech.Domain.Dto.Keyword;
using CourseTech.Domain.Dto.Question;
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

        public List<OpenQuestionAnswerVariant> OpenQuestionsAnswerVariants { get; set; }

        public List<CheckPracticalQuestionDto> CheckPracticalQuestionDtos { get; set; }

        public List<KeywordDto> PracticalQuestionsKeywords { get; set; }
    }
}
