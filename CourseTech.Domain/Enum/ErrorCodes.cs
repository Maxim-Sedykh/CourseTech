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

    LessonRecordsNotFound = 30,

    TasksDataNotFound = 40,

    ReviewNotFound = 50,
    ReviewsNotFound = 51,

    InvalidClientRequest = 61,
    PasswordIsWrong = 62,
    PasswordNotEqualsPasswordConfirm = 63,

    RoleNotFound = 71,

    QuestionsNotFound = 80,

    LessonQuestionsNotFound = 91,

    InternalServerError = 500
}
