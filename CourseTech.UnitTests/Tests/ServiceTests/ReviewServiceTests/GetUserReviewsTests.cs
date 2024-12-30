using CourseTech.Application.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.ReviewServiceTests
{
    public class GetUserReviewsTests : IClassFixture<ReviewServiceFixture>
    {
        private readonly ReviewServiceFixture _fixture;

        public GetUserReviewsTests(ReviewServiceFixture fixture)
        {
            _fixture = fixture;
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
