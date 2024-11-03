using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.Get
{
    /// <summary>
    /// Модель данных для отображения вопроса практического типа.
    /// </summary>
    public class PracticalQuestionDto : IQuestionDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string DisplayQuestion { get; set; }
    }
}
