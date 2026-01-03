using System.Text.Json.Serialization;

namespace CourseTech.Domain.Dto.Analyzer;

public class ChatGptAnalysResponseDto
{
    [JsonPropertyName("UserGrade")]
    public float UserGrade { get; set; }

    [JsonPropertyName("UserQueryAnalys")]
    public string UserQueryAnalys { get; set; }

    [JsonPropertyName("Vertexes")]
    public Vertex[] Vertexes { get; set; }

    [JsonPropertyName("Edges")]
    public Edge[] Edges { get; set; }
}

public class Vertex
{
    public int Number { get; set; }
    public string Name { get; set; }
}

public class Edge
{
    public int From { get; set; }

    public int To { get; set; }
}
