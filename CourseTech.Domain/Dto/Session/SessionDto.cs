using CourseTech.Domain.Dto.Answer;
using CourseTech.Domain.Dto.Category;
using CourseTech.Domain.Dto.User;

namespace CourseTech.Domain.Dto.Session
{
    public class SessionDto
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long CategoryId { get; set; }
        public string Difficulty { get; set; } // Добавил сложность
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public CategoryDto Category { get; set; }
        public List<AnswerDto> Answers { get; set; }
        public UserDto User { get; set; }
    }
}
