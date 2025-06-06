﻿using CourseTech.Application.CQRS.Commands.ReviewCommands;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Application.CQRS.Queries.Entities.ReviewQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class ReviewService(
    ICacheService cacheService,
    IMediator mediator,
    IUnitOfWork unitOfWork,
    ILogger logger) : IReviewService
{
    /// <inheritdoc/>
    public async Task<BaseResult> CreateReviewAsync(CreateReviewDto dto, Guid userId)
    {
        var userProfile = await mediator.Send(new GetProfileByUserIdQuery(userId));

        if (userProfile is null)
        {
            return BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        using (var transaction = await unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await mediator.Send(new CreateReviewCommand(dto.ReviewText, userId));
                await mediator.Send(new UpdateProfileReviewsCountCommand(userProfile));

                await cacheService.RemoveAsync(CacheKeys.Reviews);

                await unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);

                await transaction.RollbackAsync();

                return BaseResult.Failure((int)ErrorCodes.CreateReviewFailed, ErrorMessage.CreateReviewFailed);
            }
        }

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public async Task<BaseResult> DeleteReview(long reviewId)
    {
        var review = await mediator.Send(new GetReviewByIdQuery(reviewId));

        if (review == null)
        {
            return BaseResult.Failure((int)ErrorCodes.ReviewNotFound, ErrorMessage.ReviewNotFound);
        }

        await mediator.Send(new DeleteReviewCommand(review));

        await cacheService.RemoveAsync(CacheKeys.Reviews);

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public async Task<CollectionResult<ReviewDto>> GetReviewsAsync()
    {
        var reviews = await cacheService.GetOrAddToCache(
            CacheKeys.Reviews,
            async () => await mediator.Send(new GetReviewDtosQuery()));

        if (!reviews.Any())
        {
            return CollectionResult<ReviewDto>.Failure((int)ErrorCodes.ReviewsNotFound, ErrorMessage.ReviewsNotFound);
        }

        return CollectionResult<ReviewDto>.Success(reviews);
    }

    /// <inheritdoc/>
    public async Task<CollectionResult<ReviewDto>> GetUserReviews(Guid userId)
    {
        var reviews = await mediator.Send(new GetUserReviewDtosQuery(userId));

        if (!reviews.Any())
        {
            return CollectionResult<ReviewDto>.Failure((int)ErrorCodes.ReviewsNotFound, ErrorMessage.ReviewsNotFound);
        }

        return CollectionResult<ReviewDto>.Success(reviews);
    }
}
