using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Ключевые слова запросов для вопросов практического типа.
/// </summary>
public class Keyword : IEntityId<int>
{
    public int Id { get; set; }
    
    /// <summary>
    /// Ключевое слово.
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// Практические вопросы, в ответе на которых используются данное ключевое слово.
    /// </summary>
    public List<PracticalQuestion> PracticalQuestions { get; set; }
}
