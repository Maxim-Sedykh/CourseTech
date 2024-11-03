using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    /// <summary>
    /// Вопрос практического типа.
    /// </summary>
    public class PracticalQuestion : Question
    {
        /// <summary>
        /// Ключевые слова, из которых состоит правильный запрос.
        /// </summary>
        public List<Keyword> Keywords { get; set; }

        /// <summary>
        /// Правильный запрос. Ответ на практический вопрос.
        /// Один из вариантов ответа, который даёт правильный результат.
        /// </summary>
        public string CorrectQueryCode { get; set; }
    }
}
