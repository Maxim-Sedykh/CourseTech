using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.CheckQuestions
{
    public class OpenQuestionCheckingDto : ICheckQuestionDto
    {
        public List<string> OpenQuestionsAnswers { get; set; }
    }
}
