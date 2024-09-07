using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question
{
    public class CheckPracticalQuestionDto
    {
        public int Id { get; set; }

        public string CorrectQueryCode { get; set; }

        public List<string> PracticalQuestionKeywords { get; set; }
    }
}
