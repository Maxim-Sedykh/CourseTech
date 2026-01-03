using CourseTech.Application.Resources;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question.Factory
{
    /// <summary>
    /// Фабрика для создания стратегии проверки ответа пользователя
    /// </summary>
    public class AnswerCheckingStrategyFactory : IAnswerCheckingStrategyFactory
    {
        private readonly IEnumerable<IAnswerCheckingStrategy> _strategies;

        public AnswerCheckingStrategyFactory()
        {
        }

        /// <summary>
        /// Создать стратегию
        /// </summary>
        /// <param name="userAnswerType">Тип ответа пользователя</param>
        /// <returns>Стратегию</returns>
        public IAnswerCheckingStrategy CreateAnswerCheckingStrategy(Type userAnswerType)
        {
            var strategy = _strategies.FirstOrDefault(s => s.UserAnswerType == userAnswerType);

            return strategy ?? throw new ArgumentException(ErrorMessage.InvalidQuestionType);
        }
    }
}
