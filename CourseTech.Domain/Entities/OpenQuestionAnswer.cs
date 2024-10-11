using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities
{
    public class OpenQuestionAnswer : IEntityId<long>
    {
        public long Id { get; set; }

        public string AnswerText { get; set; }

        public OpenQuestion OpenQuestion { get; set; }

        public int OpenQuestionId { get; set; }
    }
}
