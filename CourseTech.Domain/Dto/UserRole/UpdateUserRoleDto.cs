namespace CourseTech.Domain.Dto.UserRole
{
    /// <summary>
    /// Модель данных для обновления роли у пользователя на другую роль.
    /// </summary>
    /// <param name="Login"></param>
    /// <param name="FromRoleId"></param>
    /// <param name="ToRoleId"></param>
    public record UpdateUserRoleDto(
            string Login,
            long FromRoleId,
            long ToRoleId
        );
}
