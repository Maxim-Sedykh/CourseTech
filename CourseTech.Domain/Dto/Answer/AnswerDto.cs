using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Session;

namespace CourseTech.Domain.Dto.Answer
{
    public class AnswerDto
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public int QuestionId { get; set; }
        public string AudioFileUrl { get; set; }
        public string TranscribedText { get; set; }
        public DateTime CreatedAt { get; set; }
        public QuestionDto Question { get; set; }
        public SessionDto Session { get; set; }
        public AnswerAnalysisDto Analysis { get; set; }
    }
}
