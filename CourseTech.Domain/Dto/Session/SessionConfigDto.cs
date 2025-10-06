using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Session
{
    public class SessionConfigDto
    {
        public int CategoryId { get; set; }
        public string Difficulty { get; set; } = "Middle";
    }
}
