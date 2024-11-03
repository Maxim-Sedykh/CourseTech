using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Enum;

/// <summary>
/// Статус коды для ошибок
/// </summary>
public enum ErrorCodes
{
    // Статус коды для пользователя
    UserNotFound = 0,
    UserAlreadyExists = 2,
    UserAnalysNotFound = 3,

    // Статус коды для профиля пользователя
    UserProfileNotFound = 10,

    // Статус коды для урока
    LessonNotFound = 20,
    LessonsNotFound = 21,
    InvalidLessonType = 22,

    //Статус коды для записей по прохождению уроков
    LessonRecordsNotFound = 30,

    //Статус коды для отзывов
    ReviewNotFound = 40,
    ReviewsNotFound = 41,

    //Статус коды для ролей
    RoleNotFound = 50,
    UserAlreadyExistThisRole = 51,
    RolesNotFound = 52,
    RoleAlreadyExist = 53,

    //Статус коды для вопросов
    QuestionsNotFound = 60,
    LessonQuestionsNotFound = 61,
    LessonTestQuestionsNotFound = 62,
    LessonOpenQuestionsNotFound = 63,
    AnswerCheckError = 64,

    //Статус коды для ответов на вопросы
    TestQuestionsCorrectVariantsNotFound = 70,
    OpenQuestionsAnswerVariantsNotFound = 71,

    //Статус коды для аутентификации и авторизации
    InvalidClientRequest = 100,
    PasswordIsWrong = 101,
    PasswordNotEqualsPasswordConfirm = 102,

    //Статус коды для транзакций
    RedisTransactionFailed = 110,
    RegistrationFailed = 111,
    DeleteUserFailed = 112,
    UpdateRoleForUserFailed = 113,
    CreateReviewFailed = 114,
    PassLessonFailed = 115,

    //Исключительная ситуация
    InternalServerError = 500
}
