using CourseTech.Domain;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;

namespace CourseTech.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
        }

        public async Task<CollectionResult<SubscriptionDto>> GetSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _subscriptionRepository.GetAllAsync();
                var subscriptionDtos = subscriptions.Select(MapToSubscriptionDto).ToList();

                return CollectionResult<SubscriptionDto>.Success(subscriptionDtos);
            }
            catch (Exception ex)
            {
                return CollectionResult<SubscriptionDto>.Failure($"Ошибка при получении подписок: {ex.Message}");
            }
        }

        public async Task<Result> ChangeUserSubscriptionAsync(ChangeSubscriptionDto dto, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return Result.Failure("Пользователь не найден");

                var newSubscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId);
                if (newSubscription == null)
                    return Result.Failure("Подписка не найдена");

                // Проверяем, не пытается ли пользователь перейти на ту же подписку
                if (user.SubscriptionId == dto.SubscriptionId)
                    return Result.Failure("У вас уже активна эта подписка");

                // Здесь можно добавить логику проверки платежей и т.д.
                user.SubscriptionId = dto.SubscriptionId;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Ошибка при изменении подписки: {ex.Message}");
            }
        }

        private SubscriptionDto MapToSubscriptionDto(Subscription subscription)
        {
            return new SubscriptionDto
            {
                Id = subscription.Id,
                Name = subscription.Name,
                MaxQuestionsPerDay = subscription.MaxQuestionsPerDay,
                Price = GetSubscriptionPrice(subscription.Name),
                Description = GetSubscriptionDescription(subscription.Name),
                Features = GetSubscriptionFeatures(subscription.Name)
            };
        }

        private decimal GetSubscriptionPrice(string subscriptionName)
        {
            return subscriptionName.ToLower() switch
            {
                "premium" => 29.99m,
                "pro" => 14.99m,
                "free" => 0m,
                _ => 0m
            };
        }

        private string GetSubscriptionDescription(string subscriptionName)
        {
            return subscriptionName.ToLower() switch
            {
                "premium" => "Полный доступ ко всем функциям платформы",
                "pro" => "Расширенные возможности для активной подготовки",
                "free" => "Базовые функции для начала подготовки",
                _ => "Описание недоступно"
            };
        }

        private List<string> GetSubscriptionFeatures(string subscriptionName)
        {
            var baseFeatures = new List<string>
            {
                "Доступ к базе вопросов",
                "Голосовые ответы",
                "Базовый анализ ответов"
            };

            return subscriptionName.ToLower() switch
            {
                "premium" => baseFeatures.Concat(new[]
                {
                "Неограниченное количество вопросов в день",
                "Расширенный AI-анализ",
                "Персональные рекомендации",
                "Приоритетная поддержка",
                "Экспорт результатов"
            }).ToList(),
                "pro" => baseFeatures.Concat(new[]
                {
                "До 50 вопросов в день",
                "Расширенный AI-анализ",
                "Статистика прогресса"
            }).ToList(),
                "free" => baseFeatures.Concat(new[]
                {
                "До 5 вопросов в день",
                "Базовый анализ ответов"
            }).ToList(),
                _ => baseFeatures
            };
        }
    }
}
