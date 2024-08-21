using CourseTech.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson
{
    /// <summary>
    /// DTO для передачи данных об уроке
    /// Без разметки для лекции
    /// </summary>
    public class LessonDto
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public LessonType LessonType { get; set; }
    }
}
