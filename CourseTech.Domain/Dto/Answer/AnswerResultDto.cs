using CourseTech.Domain.Dto.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Answer
{
    public class AnswerResultDto
    {
        public AnswerDto Answer { get; set; }
        public AnswerAnalysisDto Analysis { get; set; }
    }
}
