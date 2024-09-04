using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson.Practice
{
    public class PracticeUserAnswersDto
    {
        public int LessonId { get; set; }

        public List<IUserAnswerDto> UserAnswerDtos { get; set; }
    }
}
