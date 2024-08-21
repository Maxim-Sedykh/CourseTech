using CourseTech.Domain.Enum;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson
{
    /// <summary>
    /// DTO для передачи данных об уроке
    /// С разметкой для лекции
    /// </summary>
    public class LessonLectureDto
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public LessonType LessonType { get; set; }

        public HtmlString LessonMarkup { get; set; }
    }
}
