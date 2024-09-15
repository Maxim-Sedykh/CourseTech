using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities
{
    public class OpenQuestionAnswer : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }

        public string OpenQuestionCorrectAnswer { get; set; }

        public OpenQuestion OpenQuestion { get; set; }

        public int QuestionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
