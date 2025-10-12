namespace CourseTech.Domain.Dto.Answer;

public class AnswerFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? CategoryId { get; set; }
}
