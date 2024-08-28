using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Validators
{
    public interface IRoleValidator
    {
        BaseResult ValidateRoleForUser(User user, params Role[] roles);
    }
}
