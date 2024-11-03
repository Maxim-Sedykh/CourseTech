using CourseTech.Domain.Interfaces.Dtos.Validation;
using CourseTech.Domain.Interfaces.Validators;

namespace CourseTech.Domain.Dto.Role
{
    /// <summary>
    /// Модель данных для создания роли.
    /// </summary>
    /// <param name="Name"></param>
    public class CreateRoleDto : IRoleNameValidation
    {
        public string RoleName { get; set; }
    }
}
