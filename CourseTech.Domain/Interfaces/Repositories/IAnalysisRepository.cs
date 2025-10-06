using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Repositories
{
    public interface IAnalysisRepository : IBaseRepository<Analysis, long>
    {
        Task<Analysis> GetByAnswerIdAsync(long answerId);
        Task<List<Analysis>> GetByAnswerIdsAsync(List<long> answerIds);
    }
}
