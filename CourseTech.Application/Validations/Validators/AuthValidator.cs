using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Validations.Validators;

public class AuthValidator(IPasswordHasher passwordHasher) : IAuthValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateLogin(User user, string enteredPassword)
    {
        if (user == null)
        {
            return BaseResult.Failure((int)ErrorCode.UserNotFound, ErrorMessage.UserNotFound);
        }

        bool verified = passwordHasher.Verify(enteredPassword, passwordHash: user.Password);

        if (!verified)
        {
            return BaseResult.Failure((int)ErrorCode.PasswordIsWrong, ErrorMessage.PasswordIsWrong);
        }

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public BaseResult ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm)
    {
        if (enteredPassword != enteredPasswordConfirm)
        {
            return DataResult<UserDto>.Failure((int)ErrorCode.PasswordNotEqualsPasswordConfirm, ErrorMessage.PasswordNotEqualsPasswordConfirm);
        }

        if (user != null)
        {
            return DataResult<UserDto>.Failure((int)ErrorCode.UserAlreadyExists, ErrorMessage.UserAlreadyExists);
        }

        return BaseResult.Success();
    }
}
