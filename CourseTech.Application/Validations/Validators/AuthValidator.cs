using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.Validators
{
    public class AuthValidator(IPasswordHasher passwordHasher) : IAuthValidator
    {
        public BaseResult ValidateLogin(User user, string enteredPassword)
        {
            if (user == null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            bool verified = passwordHasher.Verify(enteredPassword, passwordHash: user.Password);

            if (!verified)
            {
                return BaseResult.Failure((int)ErrorCodes.PasswordIsWrong, ErrorMessage.PasswordIsWrong);
            }

            return BaseResult.Success();
        }
    }
}
