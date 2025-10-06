using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question
{
    public class QuestionFilterDto
    {
        public int CategoryId { get; set; }
        public string Difficulty { get; set; }
        public List<int> ExcludedQuestionIds { get; set; } = new();
    }
}
