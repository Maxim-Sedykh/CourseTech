using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Statistic
{
    public class SessionSummaryDto
    {
        public long SessionId { get; set; }
        public string CategoryName { get; set; }
        public double AverageScore { get; set; }
        public int TotalAnswers { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
