using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.CheckQuestions
{
    public class PracticalQuestionCheckingDto : ICheckQuestionDto
    {
        public string CorrectQueryCode { get; set; }

        public List<string> PracticalQuestionKeywords { get; set; }
    }
}
