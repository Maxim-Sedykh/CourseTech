using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CourseTech.Application.Services;

public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper) : IReviewService
{
    public async Task<BaseResult> CreateReviewAsync(CreateReviewDto dto, Guid userId)
    {
        var user = await unitOfWork.Users.GetAll()
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
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

        return BaseResult.Success();
    }

    public async Task<BaseResult> DeleteReview(long reviewId)
    {
        var review = await unitOfWork.Reviews.GetAll()
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        if (review is null)
        {
            return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
        }

        unitOfWork.Reviews.Remove(review);

        await unitOfWork.SaveChangesAsync();

        return BaseResult.Success();
    }

    public async Task<CollectionResult<ReviewDto>> GetReviewsAsync()
    {
        var reviews = await unitOfWork.Reviews.GetAll()
                .Select(x => mapper.Map<ReviewDto>(x))
                .ToArrayAsync();

        if (!reviews.Any())
        {
            return CollectionResult<ReviewDto>.Failure((int)ErrorCodes.ReviewsNotFound, ErrorMessage.ReviewsNotFound);
        }

        return CollectionResult<ReviewDto>.Success(reviews);
    }

    public async Task<CollectionResult<ReviewDto>> GetUserReviews(Guid userId)
    {
        var reviews = await unitOfWork.Reviews.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => mapper.Map<ReviewDto>(x))
                .ToArrayAsync();

        if (!reviews.Any())
        {
            return CollectionResult<ReviewDto>.Failure((int)ErrorCodes.ReviewsNotFound, ErrorMessage.ReviewsNotFound);
        }

        return CollectionResult<ReviewDto>.Success(reviews);
    }
}
