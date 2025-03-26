namespace CourseTech.Domain.Dto.Role;

/// <summary>
/// Модель данных для отображения роли
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public class RoleDto
{
    public long Id { get; set; }

    public string RoleName { get; set; }
}
