using CourseTech.Application.Resources;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.Validators
{
    public class RoleValidator : IRoleValidator
    {
        public BaseResult ValidateRoleForUser(User user, params Role[] roles)
        {
            if (user == null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            foreach (Role role in roles)
            {
                if (role == null)
                {
                    return BaseResult.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
                }
            }

            return BaseResult.Success();
        }
    }
}
