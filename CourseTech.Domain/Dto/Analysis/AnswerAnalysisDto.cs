namespace CourseTech.Domain.Dto.Analysis;

public class AnswerAnalysisDto
{
    public long Id { get; set; }
    public long AnswerId { get; set; }
    public int CompletenessScore { get; set; }
    public int ClarityScore { get; set; }
    public int TechnicalAccuracyScore { get; set; }
    public int OverallScore { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> Weaknesses { get; set; }
    public string DetailedFeedback { get; set; }
    public string SuggestedAnswer { get; set; }
    public DateTime CreatedAt { get; set; }
}
