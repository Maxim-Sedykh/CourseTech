using CourseTech.Domain;
using CourseTech.Domain.Dto.Subscription;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

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
            var subscriptions = await _subscriptionRepository.GetAll().ToListAsync();
            var subscriptionDtos = subscriptions.Select(MapToSubscriptionDto).ToList();

            return CollectionResult<SubscriptionDto>.Success(subscriptionDtos);
        }

        public async Task<Result> ChangeUserSubscriptionAsync(ChangeSubscriptionDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result.Failure("Пользователь не найден");

            var newSubscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId);
            if (newSubscription == null)
                return Result.Failure("Подписка не найдена");

            if (user.SubscriptionId == dto.SubscriptionId)
                return Result.Failure("У вас уже активна эта подписка");

            user.SubscriptionId = dto.SubscriptionId;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Result.Success();
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
