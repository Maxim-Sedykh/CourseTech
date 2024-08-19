using System.ComponentModel.DataAnnotations;

namespace CourseTech.Domain.Dto.User
{
    public class UpdateUserDto
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [MinLength(4, ErrorMessage = "Длина логина должна быть больше четырёх символов")]
        [MaxLength(12, ErrorMessage = "Длина логина должна быть меньше двенадцати символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public Role Role { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Может редактировать свои данные")]
        public bool IsEditAble { get; set; }

        public Dictionary<int, string> UserRoles { get; set; }
    }
}
