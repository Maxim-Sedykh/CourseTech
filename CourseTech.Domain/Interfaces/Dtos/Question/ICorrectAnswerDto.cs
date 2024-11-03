using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    /// <summary>
    /// Интерфейс для моделей, которые передают правильные ответы.
    /// </summary>
    public interface ICorrectAnswerDto
    {
       /// <summary>
       /// Идентификатор вопроса.
       /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Правильный ответ.
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Правильный ли ответ у пользователя?
        /// </summary>
        public bool AnswerCorrectness { get; set; }
    }
}
