using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.Pass
{
    /// <summary>
    /// Модель данных для отображения правильного ответа на вопрос открытого типа.
    /// </summary>
    public class OpenQuestionCorrectAnswerDto : ICorrectAnswerDto
    {
        public int Id { get; set; }

        public string CorrectAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
