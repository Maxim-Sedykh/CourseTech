﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.TestVariant
{
    /// <summary>
    /// Модель данных для отображения вариантов для тестового вопроса.
    /// </summary>
    public class TestVariantDto
    {
        public int QuestionId { get; set; }

        public string Content { get; set; }

        public int VariantNumber { get; set; }
    }
}
