using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseTech.Domain.Dto.Auth
{
    /// <summary>
    /// DTO для авторизации пользователя
    /// </summary>
    public class LoginUserDto
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
