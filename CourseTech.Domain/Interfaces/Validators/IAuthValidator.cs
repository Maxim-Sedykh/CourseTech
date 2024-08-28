using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Validators
{
    public interface IAuthValidator
    {
        BaseResult ValidateLogin(User user, string enteredPassword);
    }
}
