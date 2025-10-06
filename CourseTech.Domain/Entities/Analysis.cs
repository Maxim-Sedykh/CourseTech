using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities
{
    public class Analysis : IEntityId<long>, ICreatable
    {
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public int CompletenessScore { get; set; }
        public int ClarityScore { get; set; }
        public int TechnicalAccuracyScore { get; set; }
        public int OverallScore { get; set; }
        public string Strengths { get; set; } // JSON или разделитель
        public string Weaknesses { get; set; } // JSON или разделитель
        public string DetailedFeedback { get; set; }
        public string SuggestedAnswer { get; set; }
        public DateTime CreatedAt { get; set; }

        public Answer Answer { get; set; }
    }
}
