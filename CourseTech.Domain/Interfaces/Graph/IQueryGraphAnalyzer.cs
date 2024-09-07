using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Graph
{
    public interface IQueryGraphAnalyzer
    {
        void CalculateUserQueryScore(string sqlQuery, List<string> questionKeywords, out float grade, out List<string> remarks);
    }
}
