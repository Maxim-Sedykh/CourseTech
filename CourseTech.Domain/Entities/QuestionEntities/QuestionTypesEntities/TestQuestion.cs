using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    /// <summary>
    /// Вопрос тестового типа.
    /// </summary>
    public class TestQuestion : Question
    {
        /// <summary>
        /// Варианты ответа на тестовый вопрос.
        /// </summary>
        public List<TestVariant> TestVariants { get; set; }
    }
}
