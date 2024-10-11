using CourseTech.Domain.Dto.Role;
using System.ComponentModel.DataAnnotations;

namespace CourseTech.Domain.Dto.User
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsEditAble { get; set; }
    }
}
