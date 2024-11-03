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
        public int Id { get; set; }
        
        /// <summary>
        /// Номер вопроса.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Вопрос, который отображается пользователю.
        /// </summary>
        public string DisplayQuestion { get; set; }
    }
}
