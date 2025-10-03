using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IAnalysisService
    {
        Task<AiAnalysis> AnalyzeAttemptAsync(Attempt attempt);
        Task<string> GenerateFeedbackAsync(string question, string answer, List<string> keyPoints);
    }
}
