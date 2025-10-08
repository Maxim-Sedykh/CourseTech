using CourseTech.Domain;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Validators;

namespace CourseTech.Application.Validations.Validators;

public class UserValidator : IUserValidator
{
    /// <inheritdoc/>
    public Result ValidateDeletingUser(UserProfile userProfile, User user)
    {
        if (user is null)
        {
            return Result.Failure(string.Empty);
        }

        if (userProfile is null)
        {
            return Result.Failure(string.Empty);
        }

        return Result.Success();
    }
}
