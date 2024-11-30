using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    /// <summary>
    /// Модель для отображения пользователю списка вопросов в практической части.
    /// </summary>
    public interface IQuestionDto
    {
        /// <summary>
        /// Идентификатор вопроса.
        /// </summary>
        int Id { get; set; }
        
        /// <summary>
        /// Номер вопроса.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Вопрос, который отображается пользователю.
        /// </summary>
        string DisplayQuestion { get; set; }
    }
}
