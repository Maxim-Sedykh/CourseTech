using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    /// <summary>
    /// Вопрос открытого типа.
    /// </summary>
    public class OpenQuestion : Question
    {
        /// <summary>
        /// Возможные варианты ответа на вопрос.
        /// </summary>
        public List<OpenQuestionAnswer> AnswerVariants {  get; set; }
    }
}
