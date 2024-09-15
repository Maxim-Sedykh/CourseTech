using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    public class PracticalQuestion : Question
    {
        public List<QueryWord> QueryWords { get; set; }

        public string CorrectQueryCode { get; set; }
    }
}
