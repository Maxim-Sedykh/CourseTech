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
            return Result.Error(string.Empty);
        }

        bool verified = passwordHasher.Verify(enteredPassword, passwordHash: user.Password);

        if (!verified)
        {
            return Result.Error(string.Empty);
        }

        return Result.Ok();
    }

    /// <inheritdoc/>
    public Result ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm)
    {
        if (user != null)
        {
            return Result.Error("User already exists");
        }

        if (enteredPassword != enteredPasswordConfirm)
        {
            return Result.Error("Password not equals password confirm");
        }

        return Result.Ok();
    }
}
