using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.UserProfile
{
    public class LessonRecordDto
    {
        public string LessonName { get; set; }

        public float LessonMark { get; set; }

        public DateTime DateOfReceiving { get; set; }
    }
}
