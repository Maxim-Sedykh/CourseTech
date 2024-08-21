using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Enum;

public enum ErrorCodes
{
    UserNotFound = 0,
    UserAlreadyExists = 1,
    UserAnalysNotFound = 2,

    LessonNotFound = 10,

    LessonRecordsNotFound = 20,

    TasksDataNotFound = 30,

    ReviewNotFound = 40,
    ReviewsNotFound = 41,

    InternalServerError = 500
}
