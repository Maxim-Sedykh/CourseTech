using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using Xunit;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Application.Queries.Dtos.ReviewDtoQueries;

namespace CourseTech.UnitTests.Tests.ServiceTests.ReviewServiceTests
{
    public class GetReviewsTests : IClassFixture<ReviewServiceFixture>
    {
        private readonly ReviewServiceFixture _fixture;

        public GetReviewsTests(ReviewServiceFixture fixture)
        {
            _fixture = fixture;
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
    }
}
