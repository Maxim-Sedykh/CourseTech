using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Statistic
{
    public class UserStatisticsDto
    {
        public int TotalAnswers { get; set; }
        public double AverageScore { get; set; }
        public int SessionsCompleted { get; set; }
        public string BestCategory { get; set; }
        public int TotalPracticeTime { get; set; }
        public DateTime? LastActivity { get; set; }
    }
}
