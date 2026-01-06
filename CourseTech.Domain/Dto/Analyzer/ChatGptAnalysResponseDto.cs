using System.Text.Json.Serialization;

namespace CourseTech.Domain.Dto.Analyzer;

/// <summary>
/// Модель с ответом от ChatGPT API по поводу анализа ответа пользователя
/// </summary>
public class ChatGptAnalysResponseDto
{
    /// <summary>
    /// Оценка которую ИИ дал на ответ пользователя
    /// </summary>
    [JsonPropertyName("UserGrade")]
    public float UserGrade { get; set; }

    /// <summary>
    /// Анализ запроса пользователя
    /// </summary>
    [JsonPropertyName("UserQueryAnalys")]
    public string UserQueryAnalys { get; set; }

    /// <summary>
    /// Вершины графа
    /// </summary>
    [JsonPropertyName("Vertexes")]
    public Vertex[] Vertexes { get; set; }

    /// <summary>
    /// Рёбра графа
    /// </summary>
    [JsonPropertyName("Edges")]
    public Edge[] Edges { get; set; }

    /// <summary>
    /// Правильно ли пользователь ответил на вопрос
    /// </summary>
    [JsonPropertyName("AnswerCorrectness")]
    public bool AnswerCorrectness { get; set; }
}
