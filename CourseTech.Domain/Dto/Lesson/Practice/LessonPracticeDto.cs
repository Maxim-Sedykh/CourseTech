﻿using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson.Test
{
    public class LessonPracticeDto
    {
        public int LessonId { get; set; }

        public LessonType LessonType { get; set; }

        public List<IQuestionDto> Questions { get; set; }
    }
}