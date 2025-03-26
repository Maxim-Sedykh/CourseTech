namespace CourseTech.Domain.Constants.Validation;

/// <summary>
/// Класс для хранения сообщений об ошибке в валидаторах.
/// </summary>
public static class ValidationErrorMessages
{
    public const string ReviewTextNotEmptyMessage = "Введите отзыв";
    public const string LessonNameNotEmptyMessage = "Название урока должно быть задано";
    public const string DateOfBirthNotEmptyMessage = "Введите дату рождения";
    public const string ValidDateOfBirthMessage = "Неверный формат даты рождения";
    public const string PasswordConfirmNotEmptyMessage = "Подтвердите пароль";
    public const string PasswordConfirmNotEqualPasswordMessage = "Пароли не совпадают";
    public const string LoginNotEmptyMessage = "Введите логин";
    public const string PasswordNotEmptyMessage = "Введите пароль";
    public const string UserNameNotEmptyMessage = "Введите имя";
    public const string SurnameNotEmptyMessage = "Введите фамилию";
    public const string RoleNameNotEmptyMessage = "Введите название роли";
    public const string LessonMarkupNotEmpty = "Введите разметку лекции";

    public static string GetLoginMinimumLengthMessage() =>
        $"Логин должен быть не менее {ValidationConstraints.LoginMinimumLength} символов";

    public static string GetLoginMaximumLengthMessage() =>
        $"Логин должен быть не более {ValidationConstraints.LoginMaximumLength} символов";

    public static string GetPasswordMinimumLengthMessage() =>
        $"Пароль должен быть не менее {ValidationConstraints.PasswordMinimumLength} символов";

    public static string GetLessonNameMinimumLengthMessage() =>
        $"Название урока должно быть не менее {ValidationConstraints.LessonNameMinimumLength} символов";

    public static string GetLessonNameMaximumLengthMessage() =>
        $"Название урока должно быть не более {ValidationConstraints.LessonNameMaximumLength} символов";

    public static string GetUserNameMaximumLengthMessage() =>
        $"Длина имени должна быть меньше {ValidationConstraints.UserNameMaximumLength} символов";

    public static string GetSurnameMaximumLengthMessage() =>
        $"Длина фамилии должна быть меньше {ValidationConstraints.SurnameMaximumLength} символов";

    public static string GetRoleNameMinimumLengthMessage() =>
        $"Длина роли должна быть больше {ValidationConstraints.RoleNameMinimumLength} символов";

    public static string GetRoleNameMaximumLengthMessage() =>
        $"Длина роли должна быть меньше {ValidationConstraints.RoleNameMaximumLength} символов";

    public static string GetReviewTextMaximumLengthMessage() =>
        $"Длина отзыва должна быть меньше {ValidationConstraints.ReviewTextMaximumLength} символов";
}
