namespace CourseTech.Domain.Constants.Route;

/// <summary>
/// Константы для указания понятного роута в контроллерах.
/// </summary>
public static class RouteConstants
{
    #region Аутентификация

    public const string Register = "register";

    public const string Login = "login";

    #endregion

    #region Токен

    public const string RefreshToken = "refresh-token";

    #endregion

    #region Роли

    public const string CreateRole = "create-role";

    public const string UpdateRole = "update-role";

    public const string DeleteRoleById = "delete-role/{id}";

    public const string AddRoleForUser = "add-role-for-user";

    public const string DeleteRoleForUser = "delete-role-for-user";

    public const string UpdateRoleForUser = "update-role-for-user";

    public const string GetAllRoles = "get-all-roles";

    #endregion

    #region Результат курса

    public const string GetUserAnalys = "get-user-analys";

    public const string GetCouserResult = "get-lessons-for-user";

    #endregion

    #region Урок

    public const string UpdateLessonLecture = "update-lesson-lecture";

    public const string GetLessonLecture = "get-lesson-lecture/{lessonId}";

    public const string GetLessonNames = "lesson-names";

    public const string GetLessonsForUser = "get-lessons-for-user";

    #endregion

    #region Записи прохождения уроков

    public const string GetLessonsRecords = "get-lesson-records";

    #endregion

    #region Вопрос

    public const string GetLessonQuestions = "get-lesson-questions";

    public const string PassLessonQuestions = "pass-lesson-questions";

    #endregion

    #region Отзыв

    public const string CreateReview = "create-review";

    public const string DeleteReview = "delete-review/{id}";

    public const string GetReviews = "get-reviews";

    public const string GetUserReviews = "get-user-reviews/{userId}";

    #endregion

    #region Пользователь

    public const string GetUsers = "get-users";

    public const string DeleteUser = "delete-user/{userId}";

    public const string GetUserById = "get-user/{userId}";

    public const string UpdateUser = "update-user";

    #endregion

    #region Профиль пользователя

    public const string GetUserProfile = "get-user-profile";

    public const string UpdateUserProfile = "update-user-profile";

    #endregion
}
