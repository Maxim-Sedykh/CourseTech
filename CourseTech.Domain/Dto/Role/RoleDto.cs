using CourseTech.Domain.Interfaces.Dtos.Validation;

namespace CourseTech.Domain.Dto.Role
{
    /// <summary>
    /// Модель данных для отображения роли
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    public class RoleDto : IRoleNameValidation
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
    }
}
