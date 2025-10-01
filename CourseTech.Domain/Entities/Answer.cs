using CourseTech.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities
{
    public class Answer : IEntityId<long>, ICreatable
    {
        public long Id { get; set; }

        public long SessionId { get; set; }
        public long QuestionId { get; set; }

        public string AudioFileUrl { get; set; }

        public string TranscribedText { get; set; }

        public Session Session { get; set; }

        public Question Question { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
