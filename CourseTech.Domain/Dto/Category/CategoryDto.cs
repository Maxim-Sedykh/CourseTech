namespace CourseTech.Domain.Dto.Category;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int QuestionsCount { get; set; }
}
