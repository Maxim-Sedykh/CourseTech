using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.Info;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using Serilog;

namespace CourseTech.Application.Validations.Validators;

public class LessonValidator(ILogger logger) : ILessonValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateLessonsForUser(UserProfile profile, IEnumerable<LessonDto> lessons)
    {
        if (profile == null)
        {
            return DataResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        if (!lessons.Any())
        {
            logger.Error(ErrorMessage.LessonsNotFound);

            return DataResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
        }

        return BaseResult.Success();
    }
}
