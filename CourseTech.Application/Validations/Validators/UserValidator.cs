using CourseTech.Domain;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Validators;

namespace CourseTech.Application.Validations.Validators;

public class UserValidator : IUserValidator
{
    /// <inheritdoc/>
    public Result ValidateDeletingUser(User user)
    {
        if (user is null)
        {
            return Result.Failure(string.Empty);
        }

        if (user.UserProfile is null)
        {
            return Result.Failure(string.Empty);
        }

        return Result.Success();
    }
}
