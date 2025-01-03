﻿using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Application.Queries.Entities.ReviewQueries;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests
{
    public class ReviewServiceTests
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
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(userProfile);

            var mockTransaction = new Mock<IDbContextTransaction>();
            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(default))
                .ReturnsAsync(mockTransaction.Object);

            // Настройка мока для CommitAsync
            mockTransaction.Setup(t => t.CommitAsync(default)).Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.ReviewService.CreateReviewAsync(dto, userId);

            // Assert
            Assert.True(result.IsSuccess);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<CreateReviewCommand>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateProfileReviewsCountCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Reviews), Times.Once);
            _fixture.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateReviewAsync_ExceptionDuringCreation_RollsBackTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateReviewDto { ReviewText = "Great product!" };
            var userProfile = new UserProfile();

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(userProfile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<CreateReviewCommand>(), default))
                .ThrowsAsync(new Exception("Error creating review")); // Имитируем ошибку

            // Act
            var result = await _fixture.ReviewService.CreateReviewAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            _fixture.LoggerMock.Verify(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            _fixture.UnitOfWorkMock.Verify(u => u.BeginTransactionAsync(default), Times.Once);
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
            var reviewDtos = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, ReviewText = "Great product!" },
                new ReviewDto { Id = 2, ReviewText = "Not bad." }
            };

            // Настройка мока для кэша
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<ReviewDto>>>>()))
                .ReturnsAsync(reviewDtos);

            // Act
            var result = await _fixture.ReviewService.GetReviewsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(reviewDtos.Count, result.Data.Count());
            _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(CacheKeys.Reviews, It.IsAny<Func<Task<IEnumerable<ReviewDto>>>>()), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetReviewDtosQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task GetReviewsAsync_NoReviews_ReturnsFailure()
        {
            // Arrange
            var emptyReviews = new List<ReviewDto>();

            // Настройка мока для кэша
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<ReviewDto>>>>()))
                .ReturnsAsync(emptyReviews);

            // Act
            var result = await _fixture.ReviewService.GetReviewsAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.ReviewsNotFound, result.Error.Code);
            _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(CacheKeys.Reviews, It.IsAny<Func<Task<IEnumerable<ReviewDto>>>>()), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetReviewDtosQuery>(), default), Times.Never);
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
