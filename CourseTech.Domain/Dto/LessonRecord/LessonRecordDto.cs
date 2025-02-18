using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.LessonRecord
{
    /// <summary>
    /// Модель данных для отображения записей о прохождении уроков.
    /// </summary>
    public class LessonRecordDto
    {
        public string LessonName { get; set; }

        public float Mark { get; set; }

        public string CreatedAt { get; set; }
    }
}
