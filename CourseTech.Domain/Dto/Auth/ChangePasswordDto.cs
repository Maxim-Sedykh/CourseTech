using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Auth
{
    /// <summary>
    /// Модель данных для смены пароля пользователя.
    /// </summary>
    public class ChangePasswordDto
    {
        public string NewPassword { get; set; }
    }
}
