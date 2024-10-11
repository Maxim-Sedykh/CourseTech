using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Databases
{
    public interface ISqlHelper
    {
        DataTable ExecuteQuery(string sqlQuery);
    }
}
