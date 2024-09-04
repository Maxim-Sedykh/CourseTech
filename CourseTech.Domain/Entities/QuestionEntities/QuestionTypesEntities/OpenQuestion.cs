using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    public class OpenQuestion : Question
    {
        public string Notation { get; set; }

        public List<OpenQuestionAnswerVariant> AnswerVariants {  get; set; }
    }
}
