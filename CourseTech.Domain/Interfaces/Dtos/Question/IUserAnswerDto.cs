using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    /// <summary>
    /// Интерфейс для передачи коллекции ответов пользователя на сервер.
    /// </summary>
    public interface IUserAnswerDto
    {
        /// <summary>
        /// Идентификатор вопроса
        /// </summary>
        int QuestionId { get; set; }

        string QuestionType { get; set; }
    }
}
