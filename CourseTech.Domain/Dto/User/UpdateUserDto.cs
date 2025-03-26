namespace CourseTech.Domain.Dto.User;

/// <summary>
/// Модель данных для обновления пользователя.
/// </summary>
public class UpdateUserDto
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string UserName { get; set; }

    public string Surname { get; set; }

    public bool IsEditAble { get; set; }
}
