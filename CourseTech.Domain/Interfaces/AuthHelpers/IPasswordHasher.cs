using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string enteredPassword, string passwordHash);
    }
}
