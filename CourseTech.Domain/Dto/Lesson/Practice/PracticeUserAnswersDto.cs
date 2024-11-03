using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson.Practice
{
    /// <summary>
    /// Модель данных для отправки ответов пользователя на практическую часть на сервер.
    /// </summary>
    public class PracticeUserAnswersDto
    {
        public int LessonId { get; set; }

        public List<IUserAnswerDto> UserAnswerDtos { get; set; }
    }
}
