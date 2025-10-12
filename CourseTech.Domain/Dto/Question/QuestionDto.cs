using CourseTech.Domain.Dto.Category;

namespace CourseTech.Domain.Dto.Question;

public class QuestionDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Difficulty { get; set; } // Добавил сложность
    public string ExampleAnswer { get; set; } // Добавил пример ответа
    public List<string> KeyPoints { get; set; } // Добавил ключевые пункты
    public CategoryDto Category { get; set; }
    public DateTime CreatedAt { get; set; }
}
