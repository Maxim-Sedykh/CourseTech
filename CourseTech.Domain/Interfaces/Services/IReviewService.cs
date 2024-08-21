using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с отзывами
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Получение всех отзывов
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<ReviewDto>> GetReviewsAsync();

        /// <summary>
        /// Создание отзыва
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> CreateReviewAsync(CreateReviewDto dto, Guid userId);

        /// <summary>
        /// Удаление отзыва по идентификатору
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteReview(long reviewId);

        /// <summary>
        /// Получение отзывов определённого пользователя по его идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CollectionResult<ReviewDto>> GetUserReviews(Guid userId);
    }
}
