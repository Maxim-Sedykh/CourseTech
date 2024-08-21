using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseTech.Application.Services
{
    public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper) : IReviewService
    {
        public async Task<BaseResult> CreateReviewAsync(CreateReviewDto dto, Guid userId)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return new BaseResult()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var review = new Review()
                    {
                        UserId = user.Id,
                        ReviewText = dto.ReviewText
                    };

                    user.UserProfile.CountOfReviews++;

                    await unitOfWork.Reviews.CreateAsync(review);
                    unitOfWork.Users.Update(user);

                    await unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return new BaseResult();
        }

        public async Task<BaseResult> DeleteReview(long reviewId)
        {
            var review = await unitOfWork.Reviews.GetAll()
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review is null)
            {
                return new BaseResult()
                {
                    ErrorCode = (int)ErrorCodes.ReviewNotFound,
                    ErrorMessage = ErrorMessage.ReviewNotFound
                };
            }

            unitOfWork.Reviews.Remove(review);

            await unitOfWork.SaveChangesAsync();

            return new BaseResult();
        }

        public async Task<CollectionResult<ReviewDto>> GetReviewsAsync()
        {
            var reviews = await unitOfWork.Reviews.GetAll()
                    .Select(x => mapper.Map<ReviewDto>(x))
                    .ToArrayAsync();

            if (reviews is null)
            {
                return new CollectionResult<ReviewDto>()
                {
                    ErrorCode = (int)ErrorCodes.ReviewsNotFound,
                    ErrorMessage = ErrorMessage.ReviewsNotFound
                };
            }

            return new CollectionResult<ReviewDto>
            {
                Data = reviews,
                Count = reviews.Length
            };
        }

        public async Task<CollectionResult<ReviewDto>> GetUserReviews(Guid userId)
        {
            var reviews = await unitOfWork.Reviews.GetAll()
                    .Where(x => x.UserId == userId)
                    .Select(x => mapper.Map<ReviewDto>(x))
                    .ToArrayAsync();

            if (reviews is null)
            {
                return new CollectionResult<ReviewDto>()
                {
                    ErrorCode = (int)ErrorCodes.ReviewsNotFound,
                    ErrorMessage = ErrorMessage.ReviewsNotFound
                };
            }

            return new CollectionResult<ReviewDto>
            {
                Data = reviews,
                Count = reviews.Length
            };
        }
    }
}
