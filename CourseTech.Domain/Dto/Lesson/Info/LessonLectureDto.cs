﻿using CourseTech.Domain.Enum;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson.LessonInfo
{
    /// <summary>
    /// Модель данных для передачи данных об уроке.
    /// С разметкой для лекции.
    /// </summary>
    public class LessonLectureDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LessonTypes LessonType { get; set; }

        public string LectureMarkup { get; set; }
    }
}
