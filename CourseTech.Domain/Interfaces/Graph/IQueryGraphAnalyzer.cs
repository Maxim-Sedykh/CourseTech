using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Graph
{
    /// <summary>
    /// Сервис для работы с графами принятия решений ( Для оценки запроса пользователя, на его корректность ).
    /// </summary>
    public interface IQueryGraphAnalyzer
    {
        /// <summary>
        /// Анализ запроса пользователя.
        /// </summary>
        /// <param name="sqlQuery">Запрос, который был введён пользователем.</param>
        /// <param name="questionKeywords">Ключевые слова в корректном запросе.</param>
        /// <param name="grade">Оценка за практическое задание.</param>
        /// <param name="remarks">Замечания для пользователя, что он сделал не так.</param>
        void CalculateUserQueryScore(string sqlQuery, List<string> questionKeywords, out float grade, out List<string> remarks);
    }
}
