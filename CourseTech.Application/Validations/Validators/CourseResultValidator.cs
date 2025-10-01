using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using Serilog;

namespace CourseTech.Application.Validations.Validators;

public class CourseResultValidator(ILogger logger) : ICourseResultValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateUserCourseResult(UserProfile profile, int lessonCount)
    {
        if (profile is null)
        {
            return DataResult<CourseResultDto>.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        if (lessonCount == 0)
        {
            logger.Error(ErrorMessage.LessonsNotFound);

            return DataResult<CourseResultDto>.Failure((int)ErrorCode.LessonsNotFound, ErrorMessage.LessonsNotFound);
        }

        return BaseResult.Success();
    }
}
