namespace CourseTech.Domain.Dto.Analysis
{
    public class AnalyzeAnswerDto
    {
        public long AnswerId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> KeyPoints { get; set; }
        public string ExampleAnswer { get; set; }
    }
}
