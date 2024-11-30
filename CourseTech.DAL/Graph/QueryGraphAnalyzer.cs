using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Interfaces.Graph;
using QuickGraph;

namespace CourseTech.DAL.Graph;

public class QueryGraphAnalyzer : IQueryGraphAnalyzer
{
    /// <inheritdoc/>
    public void CalculateUserQueryScore(string sqlQuery, List<string> questionKeywords, out float grade, out List<string> remarks)
    {
        grade = 0;
        remarks = new List<string>();

        if (string.IsNullOrEmpty(sqlQuery) || questionKeywords == null || questionKeywords.Count == 0)
            return;

        var decisionGraph = CreateDecisionGraph(questionKeywords);
        var gradePerCategory = (float)Math.Round((decimal)102 / (questionKeywords.Count + 2), 2);

        var words = sqlQuery.Split(QueryAnalyzeRemarks.SqlQuerySplitters, StringSplitOptions.RemoveEmptyEntries);
        var keywordPresence = new Dictionary<string, bool>();

        foreach (var keyword in questionKeywords)
        {
            keywordPresence[keyword] = words.Contains(keyword);
        }

        // Проверка правильного порядка ключевых слов
        if (IsCorrectOrder(decisionGraph, keywordPresence))
        {
            grade += gradePerCategory;
        }
        else
        {
            remarks.Add(QueryAnalyzeRemarks.KeywordsIncorrectOrderRemark);
        }

        // Проверка наличия всех ключевых слов
        foreach (var keyword in questionKeywords)
        {
            if (keywordPresence[keyword])
            {
                grade += gradePerCategory;
            }
            else
            {
                remarks.Add(string.Format(QueryAnalyzeRemarks.MissingKeywordRemark, keyword.ToUpper()));
            }
        }

        grade = (float)Math.Round(grade, 2);
    }

    /// <summary>
    /// Создаёт граф принятия решений для ключевых слов с использованием GraphX
    /// </summary>
    /// <param name="keywords"></param>
    /// <returns></returns>
    private BidirectionalGraph<string, Edge<string>> CreateDecisionGraph(List<string> keywords)
    {
        var graph = new BidirectionalGraph<string, Edge<string>>();

        graph.AddVertexRange(keywords);

        for (int i = 0; i < keywords.Count - 1; i++)
        {
            graph.AddEdge(new Edge<string>(keywords[i], keywords[i + 1]));
        }

        return graph;
    }

    /// <summary>
    /// Проверяет, соответствует ли порядок ключевых слов в запросе заданному графу
    /// </summary>
    private bool IsCorrectOrder(BidirectionalGraph<string, Edge<string>> graph, Dictionary<string, bool> keywordPresence)
    {
        var visited = new HashSet<string>();
        var currentNode = graph.Vertices.FirstOrDefault(v => keywordPresence[v]);

        while (currentNode != null && visited.Count < graph.VertexCount)
        {
            if (!keywordPresence[currentNode])
                return false;

            visited.Add(currentNode);
            var nextEdges = graph.OutEdges(currentNode).Where(e => keywordPresence[e.Target]).ToList();

            if (nextEdges.Count == 0)
                break;

            currentNode = nextEdges.First().Target; // Переход к следующему ключевому слову
        }

        return visited.Count == graph.VertexCount && visited.All(v => keywordPresence[v]);
    }
}
