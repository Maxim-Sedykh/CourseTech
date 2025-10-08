using CourseTech.Domain.Dto.Subscription;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Получение всех подписок.
        /// </summary>
        Task<CollectionResult<SubscriptionDto>> GetSubscriptionsAsync();

        /// <summary>
        /// Изменение подписки пользователя.
        /// </summary>
        Task<Result> ChangeUserSubscriptionAsync(ChangeSubscriptionDto dto, Guid userId);
    }
}
