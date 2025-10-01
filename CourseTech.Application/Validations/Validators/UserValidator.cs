using CourseTech.Application.Resources;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Validations.Validators;

public class UserValidator : IUserValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateDeletingUser(UserProfile userProfile, User user)
    {
        if (user is null)
        {
            return BaseResult.Failure((int)ErrorCode.UserNotFound, ErrorMessage.UserNotFound);
        }

        if (userProfile is null)
        {
            return BaseResult.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        return BaseResult.Success();
    }
}
