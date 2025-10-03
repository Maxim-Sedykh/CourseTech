using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IAnswerService
    {
        Task<AttemptResult> ProcessAttemptAsync(Guid sessionId, Guid userId, IFormFile audioFile, Guid questionId);
        Task<AttemptAnalysis> GetAttemptAnalysisAsync(Guid attemptId, Guid userId);
        Task<List<Attempt>> GetUserAttemptsAsync(Guid userId, int page, int pageSize);
    }
}
