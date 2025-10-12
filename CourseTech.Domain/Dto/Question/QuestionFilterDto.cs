namespace CourseTech.Domain.Dto.Question;

public class QuestionFilterDto
{
    public int CategoryId { get; set; }
    public string Difficulty { get; set; }
    public List<int> ExcludedQuestionIds { get; set; } = new();
}
