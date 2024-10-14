using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Enum;

public enum ErrorCodes
{
    UserNotFound = 0,
    UserAlreadyExists = 2,
    UserAnalysNotFound = 3,

    UserProfileNotFound = 10,

    LessonNotFound = 20,
    LessonsNotFound = 21,
    InvalidLessonType = 22,

    LessonRecordsNotFound = 30,

    ReviewNotFound = 40,
    ReviewsNotFound = 41,

    RoleNotFound = 50,
    UserAlreadyExistThisRole = 51,
    RolesNotFound = 52,
    RoleAlreadyExist = 53,

    QuestionsNotFound = 60,
    LessonQuestionsNotFound = 61,
    LessonTestQuestionsNotFound = 62,
    LessonOpenQuestionsNotFound = 63,

    TestQuestionsCorrectVariantsNotFound = 70,
    OpenQuestionsAnswerVariantsNotFound = 71,

    InvalidClientRequest = 100,
    PasswordIsWrong = 101,
    PasswordNotEqualsPasswordConfirm = 102,

    RedisTransactionFailed = 102,

    InternalServerError = 500
}
