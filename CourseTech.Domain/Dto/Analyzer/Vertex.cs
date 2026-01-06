namespace CourseTech.Domain.Dto.Analyzer;

/// <summary>
/// Вершина
/// </summary>
public record Vertex
{
    /// <summary>
    /// Номер вершины
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Название вершины
    /// </summary>
    public string Name { get; set; }
}
