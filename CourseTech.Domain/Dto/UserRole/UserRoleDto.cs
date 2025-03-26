namespace CourseTech.Domain.Dto.UserRole;

/// <summary>
/// Модель данных для отображения роли пользователя.
/// </summary>
/// <param name="Login"></param>
/// <param name="RoleName"></param>
public record UserRoleDto(
        string Login,
        string RoleName
    );
