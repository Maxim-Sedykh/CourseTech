using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<BaseResult> ChangeUserSubscriptionAsync(ChangeSubscriptionDto dto, Guid userId);
    }
}
