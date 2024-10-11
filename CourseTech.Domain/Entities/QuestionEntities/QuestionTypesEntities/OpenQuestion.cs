using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    public class OpenQuestion : Question
    {
        //To Do как проверять ответы более оптимизированно
        public List<OpenQuestionAnswer> AnswerVariants {  get; set; }
    }
}
