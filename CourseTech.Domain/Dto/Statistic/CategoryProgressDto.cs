using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Statistic
{
    public class CategoryProgressDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double AverageScore { get; set; }
        public int AnswersCount { get; set; }
        public int Improvement { get; set; } // в процентах
    }
}
