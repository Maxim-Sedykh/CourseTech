using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.Get
{
    public class OpenQuestionDto : IQuestionDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string DisplayQuestion { get; set; }

        public string Notation { get; set; }
    }
}
