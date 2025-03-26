namespace CourseTech.Domain.Dto.Keyword;

/// <summary>
/// Модель данных для ключевого слова запроса.
/// </summary>
public class KeywordDto
{
    public int QuestionId { get; set; }

    public string Keyword { get; set; }
}
