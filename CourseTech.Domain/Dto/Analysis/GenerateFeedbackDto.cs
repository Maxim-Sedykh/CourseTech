using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Analysis
{
    public class GenerateFeedbackDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> KeyPoints { get; set; }
    }
}
