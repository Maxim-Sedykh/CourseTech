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

    LessonRecordsNotFound = 30,

    TasksDataNotFound = 40,

    ReviewNotFound = 50,
    ReviewsNotFound = 51,

    InternalServerError = 500
}
