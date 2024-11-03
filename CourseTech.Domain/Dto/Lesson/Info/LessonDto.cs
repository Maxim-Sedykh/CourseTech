using CourseTech.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson.LessonInfo
{
    /// <summary>
    /// Модель данных для передачи данных об уроке.
    /// Без разметки для лекции.
    /// </summary>
    public class LessonDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LessonTypes LessonType { get; set; }
    }
}
