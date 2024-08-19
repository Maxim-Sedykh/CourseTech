using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.User
{
    public class CreateUserDto
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(4, ErrorMessage = "Длина логина должна быть больше четырёх символов")]
        [MaxLength(12, ErrorMessage = "Длина логина должна быть меньше двенадцати символов")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Длина пароля должна быть больше пяти символов")]
        [MaxLength(20, ErrorMessage = "Длина пароля должна быть меньше двадцати символов")]
        public string Password { get; set; }

        [Display(Name = "Роль")]
        [Required(ErrorMessage = "Выберите роль")]
        public Role Role { get; set; }
    }
}
