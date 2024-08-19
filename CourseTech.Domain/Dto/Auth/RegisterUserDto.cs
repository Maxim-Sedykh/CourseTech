using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace CourseTech.Domain.Dto.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(4, ErrorMessage = "Длина логина должна быть больше четырёх символов")]
        [MaxLength(12, ErrorMessage = "Длина логина должна быть меньше двенадцати символов")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Длина пароля должна быть больше пяти символов")]
        [MaxLength(20, ErrorMessage = "Длина пароля должна быть меньше двадцати символов")]
        public string Password { get; set; }
    }
}