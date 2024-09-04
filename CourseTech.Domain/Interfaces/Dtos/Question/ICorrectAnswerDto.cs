using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Dtos.Question
{
    public interface ICorrectAnswerDto
    {
        public int Id { get; set; }

        public string CorrectAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
