using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.OpenQuestionAnswer
{
    public class OpenQuestionAnswerDto
    {
        public string OpenQuestionCorrectAnswer { get; set; }

        public int QuestionId { get; set; }
    }
}
