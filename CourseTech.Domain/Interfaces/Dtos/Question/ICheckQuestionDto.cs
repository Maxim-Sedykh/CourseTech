using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    /// <summary>
    /// Интерфейс для модели, в которой указаны данные для проверки правильности вопроса.
    /// </summary>
    public interface ICheckQuestionDto 
    {
        /// <summary>
        /// Идентификатор вопроса.
        /// </summary>
        int QuestionId { get; set; }
    }
}
