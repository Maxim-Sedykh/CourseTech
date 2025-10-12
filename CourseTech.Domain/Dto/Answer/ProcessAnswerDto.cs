using Microsoft.AspNetCore.Http;

namespace CourseTech.Domain.Dto.Answer;

public class ProcessAnswerDto
{
    public long SessionId { get; set; }
    public int QuestionId { get; set; }
    public IFormFile AudioFile { get; set; }
}
