using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    public interface IQuestionDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string DisplayQuestion { get; set; }
    }
}
