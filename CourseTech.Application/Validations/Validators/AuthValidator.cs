using CourseTech.Domain;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Validators;

namespace CourseTech.Application.Validations.Validators;

public class AuthValidator(IPasswordHasher passwordHasher) : IAuthValidator
{
    /// <inheritdoc/>
    public Result ValidateLogin(User user, string enteredPassword)
    {
        if (user == null)
        {
            return Result.Failure(string.Empty);
        }

        bool verified = passwordHasher.Verify(enteredPassword, passwordHash: user.Password);

        if (!verified)
        {
            return Result.Failure(string.Empty);
        }

        return Result.Success();
    }

    /// <inheritdoc/>
    public Result ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm)
    {
        if (user != null)
        {
            return Result.Failure("User already exists");
        }

        if (enteredPassword != enteredPasswordConfirm)
        {
            return Result.Failure("Password not equals password confirm");
        }

        return Result.Success();
    }
}
