namespace CourseTech.Domain.Interfaces.Services.Question
{
    /// <summary>
    /// Интерфейс для фабрики по созданию стратегии для проверки ответа пользователя
    /// </summary>
    public interface IAnswerCheckingStrategyFactory
    {
        /// <summary>
        /// Создать стратегию для проверки ответа пользователя в зависимости от типа его ответа
        /// </summary>
        /// <param name="userAnswerType">Тип ответа пользователя</param>
        /// <returns></returns>
        IAnswerCheckingStrategy CreateAnswerCheckingStrategy(Type userAnswerType);
    }
}
