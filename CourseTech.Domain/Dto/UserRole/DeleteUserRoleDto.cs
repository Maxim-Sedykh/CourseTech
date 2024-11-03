namespace CourseTech.Domain.Dto.UserRole
{
    /// <summary>
    /// Модель данных для удаления определённой роли у пользователя.
    /// </summary>
    /// <param name="Login"></param>
    /// <param name="RoleId"></param>
    public record DeleteUserRoleDto(
            string Login,
            long RoleId
        );
}
