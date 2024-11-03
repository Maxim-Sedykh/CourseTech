using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseTech.Domain.Dto.UserProfile
{
    /// <summary>
    /// Модель данных для отображения профиля пользователю.
    /// </summary>
    public class UserProfileDto
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public Guid UserId { get; set; }

        public float CurrentGrade { get; set; }

        public bool IsExamCompleted { get; set; }

        public int LessonsCompleted { get; set; }

        public bool IsEditAble { get; set; }
    }
}
