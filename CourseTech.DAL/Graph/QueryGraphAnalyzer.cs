﻿using CourseTech.Domain.Constants;
using CourseTech.Domain.Interfaces.Graph;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Graph
{
    public class QueryGraphAnalyzer: IQueryGraphAnalyzer
    {
        // To Do этот код никак вроде больше нельзя оптимизировать, графы здесь плохо используются, нужно улучшить алгоритм с графом
        public void CalculateUserQueryScore(string sqlQuery, List<string> questionKeywords, out float grade, out List<string> remarks)
        {
            grade = 0;
            remarks = new List<string>();

            if (string.IsNullOrEmpty(sqlQuery) || questionKeywords == null || questionKeywords.Count == 0)
                return;

            var graph = CreateGraph(questionKeywords);
            var gradePerCategory = (float)Math.Round(QuestionGradeConstants.PracticalQuestionGrade / (questionKeywords.Count + 2), 2);

            var words = sqlQuery.Split(UserQueryAnalyzeRemarks.SqlQuerySplitters, StringSplitOptions.RemoveEmptyEntries);
            var keywordIndexes = new List<int>();

            foreach (var edge in graph.Edges)
            {
                keywordIndexes.Add(sqlQuery.IndexOf(edge.Source));
                keywordIndexes.Add(sqlQuery.IndexOf(edge.Target));
            }

            keywordIndexes.AddRange(graph.Vertices.Select(keyword => sqlQuery.IndexOf(keyword)));

            if (keywordIndexes.SequenceEqual(keywordIndexes.OrderBy(x => x)))
            {
                grade += gradePerCategory;
            }
            else
            {
                remarks.Add(UserQueryAnalyzeRemarks.KeywordsIncorrectOrderRemark);
            }

            foreach (var keyword in graph.Vertices)
            {
                if (words.Contains(keyword))
                {
                    grade += gradePerCategory;
                }
                else
                {
                    remarks.Add(string.Format(UserQueryAnalyzeRemarks.MissingKeywordRemark, keyword.ToUpper()));
                }
            }

            grade = (float)Math.Round(grade, 2);
        }


        private AdjacencyGraph<string, Edge<string>> CreateGraph(List<string> keywords)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();

            graph.AddVertexRange(keywords);

            for (int i = 0; i < keywords.Count - 1; i++)
            {
                graph.AddEdge(new Edge<string>(keywords[i], keywords[i + 1]));
            }
            return graph;
        }
    }
}