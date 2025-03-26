namespace CourseTech.Domain.Dto.User;

/// <summary>
/// Модель данных для отображения пользователя в AdminDashboard.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public List<string> Roles { get; set; }

    public bool IsExamCompleted { get; set; }

    public bool IsEditAble { get; set; }
}
