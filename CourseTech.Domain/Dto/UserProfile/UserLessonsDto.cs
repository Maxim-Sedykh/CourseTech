using CourseTech.Domain.Dto.Lesson.LessonInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.UserProfile
{
    public class UserLessonsDto
    {
        public int LessonsCompleted { get; set; }

        public List<LessonDto> LessonNames { get; set; }
    }
}
