using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Answer
{
    public class ProcessAnswerDto
    {
        public long SessionId { get; set; }
        public int QuestionId { get; set; }
        public IFormFile AudioFile { get; set; }
    }
}
