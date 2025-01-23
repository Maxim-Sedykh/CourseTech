using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Analyzer
{
    public class ChatGptAnalysResponseDto
    {
        public string UserQueryAnalys { get; set; }

        public float UserQueryGrade { get; set; }
    }
}
