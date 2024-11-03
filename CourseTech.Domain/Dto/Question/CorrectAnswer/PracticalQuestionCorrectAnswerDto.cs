using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.Pass
{
    /// <summary>
    /// Модель данных для отображения правильного ответа на вопрос практического типа.
    /// </summary>
    public class PracticalQuestionCorrectAnswerDto : ICorrectAnswerDto
    {
        public int Id { get; set; }

        public string CorrectAnswer { get; set; }

        public float QuestionUserGrade { get; set; }

        public DataTable QueryResult { get; set; }

        public List<string> Remarks { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
