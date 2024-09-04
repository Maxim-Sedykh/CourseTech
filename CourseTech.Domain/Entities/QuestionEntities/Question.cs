using CourseTech.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities
{
    public class Question : IEntityId<int>, IAuditable
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int LessonId { get; set; }

        public string DisplayQuestion { get; set; }

        public Lesson Lesson { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
