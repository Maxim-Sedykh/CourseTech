using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IStatisticService
    {
        Task<UserStatistics> GetUserStatisticsAsync(Guid userId);
        Task<List<CategoryProgress>> GetCategoryProgressAsync(Guid userId);
        Task<SessionSummary> GetSessionSummaryAsync(Guid sessionId, Guid userId);
    }
}
