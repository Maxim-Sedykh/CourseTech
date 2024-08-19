using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson
{
    public class LessonPassDto
    {
        public int LessonId { get; set; }

        public LessonType LessonType { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }
}

