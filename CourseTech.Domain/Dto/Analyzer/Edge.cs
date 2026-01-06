namespace CourseTech.Domain.Dto.Analyzer;

/// <summary>
/// Модель для ребра графа
/// </summary>
public record Edge
{
    /// <summary>
    /// Номер вершины откуда идёт ребро
    /// </summary>
    public int From { get; set; }

    /// <summary>
    /// Номер вершины откуда куда ребро
    /// </summary>
    public int To { get; set; }
}
