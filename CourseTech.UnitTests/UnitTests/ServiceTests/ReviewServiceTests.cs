using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Application.Queries.Entities.ReviewQueries;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Tests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Data;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests
{
    public class ReviewServiceTests : IClassFixture<ReviewServiceFixture>
    {
        private readonly ReviewServiceFixture _fixture;

        public ReviewServiceTests(ReviewServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateReviewAsync_UserProfileNotFound_ReturnsFailure()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateReviewDto { ReviewText = "Great product!" };
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync((UserProfile)null); // Имитируем отсутствие профиля

            // Act
            var result = await _fixture.ReviewService.CreateReviewAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
        }

        [Fact]
        public async Task CreateReviewAsync_SuccessfulCreation_CommitsTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateReviewDto { ReviewText = "Great product!" };
            var userProfile = new UserProfile(); // Создаем объект профиля пользователя

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userProfile);

            var mockTransaction = new Mock<IDbContextTransaction>();
            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(mockTransaction.Object);

            // Настройка мока для CommitAsync
            mockTransaction.Setup(t => t.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.ReviewService.CreateReviewAsync(dto, userId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateReviewAsync_ExceptionDuringCreation_RollsBackTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateReviewDto { ReviewText = "Great product!" };
            var userProfile = new UserProfile();

            var mockTransaction = new Mock<IDbContextTransaction>();
            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(mockTransaction.Object);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userProfile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<CreateReviewCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error creating review")); // Имитируем ошибку

            // Act
            var result = await _fixture.ReviewService.CreateReviewAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.CreateReviewFailed, result.Error.Code);
        }

        [Fact]
        public async Task DeleteReview_ReviewExists_Success()
        {
            // Arrange
            long reviewId = 1;
            var review = new Review();

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetReviewByIdQuery>(), default))
                .ReturnsAsync(review);

            // Act
            var result = await _fixture.ReviewService.DeleteReview(reviewId);

            // Assert
            Assert.True(result.IsSuccess);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteReviewCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Reviews), Times.Once);
        }

        [Fact]
        public async Task DeleteReview_ReviewNotFound_ReturnsFailure()
        {
            // Arrange
            long reviewId = 1;

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetReviewByIdQuery>(), default))
                .ReturnsAsync((Review)null);

            // Act
            var result = await _fixture.ReviewService.DeleteReview(reviewId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.ReviewNotFound, result.Error.Code);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteReviewCommand>(), default), Times.Never);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Reviews), Times.Never);
        }

        [Fact]
        public async Task GetReviewsAsync_ReviewsExist_ReturnsSuccess()
        {
            // Arrange
            var reviewDtos = new ReviewDto[]
            {
                new() { Id = 1, ReviewText = "Great product!" },
                new() { Id = 2, ReviewText = "Not bad." }
            };

            // Настройка мока для кэша
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<ReviewDto[]>>>()))
                .ReturnsAsync(reviewDtos);

            // Act
            var result = await _fixture.ReviewService.GetReviewsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(reviewDtos.Length, result.Data.Count());
            _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(CacheKeys.Reviews, It.IsAny<Func<Task<ReviewDto[]>>>()), Times.Once);
        }

        [Fact]
        public async Task GetReviewsAsync_NoReviews_ReturnsFailure()
        {
            // Arrange
            ReviewDto[] emptyReviews = [];

            // Настройка мока для кэша
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<ReviewDto[]>>>()))
                .ReturnsAsync(emptyReviews);

            // Act
            var result = await _fixture.ReviewService.GetReviewsAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.ReviewsNotFound, result.Error.Code);
        }

        [Fact]
        public async Task GetUserReviews_ShouldReturnSuccess_WhenReviewsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reviewDtos = new ReviewDto[] {
                new() { ReviewText = "test1" },
                new() { ReviewText = "test2" }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserReviewDtosQuery>(), default))
                .ReturnsAsync(reviewDtos);

            // Act
            var result = await _fixture.ReviewService.GetUserReviews(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(reviewDtos.Length, result.Data.Count());
        }

        [Fact]
        public async Task GetUserReviews_ShouldReturnFailure_WhenReviewsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reviewDtos = Array.Empty<ReviewDto>();

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserReviewDtosQuery>(), default))
                .ReturnsAsync(reviewDtos);

            // Act
            var result = await _fixture.ReviewService.GetUserReviews(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.ReviewsNotFound, result.Error.Code);
        }
    }
}
