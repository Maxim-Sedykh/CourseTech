using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities
{
    /// <summary>
    /// Ответ на вопрос открытого типа.
    /// </summary>
    public class OpenQuestionAnswer : IEntityId<long>
    {
        public long Id { get; set; }

        /// <summary>
        /// Текст ответа.
        /// </summary>
        public string AnswerText { get; set; }

        /// <summary>
        /// Вопрос открытого типа.
        /// </summary>
        public OpenQuestion OpenQuestion { get; set; }

        /// <summary>
        /// Идентификатор вопроса открытого типа, которому принадлежит данный ответ.
        /// </summary>
        public int OpenQuestionId { get; set; }
    }
}
