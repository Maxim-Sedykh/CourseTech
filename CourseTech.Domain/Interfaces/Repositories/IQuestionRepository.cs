using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Repositories
{
    public interface IQuestionRepository : IBaseRepository<Question, int>
    {
    }
}
