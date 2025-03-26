namespace CourseTech.Domain.Dto.UserProfile;

/// <summary>
/// Модель данных для обновления пользователем своих учётных данных в профиле.
/// </summary>
public class UpdateUserProfileDto
{
    public string UserName { get; set; }

    public string Surname { get; set; }

    public DateTime DateOfBirth { get; set; }
}
