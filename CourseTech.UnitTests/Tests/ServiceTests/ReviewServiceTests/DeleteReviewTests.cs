using CourseTech.Domain.Entities;
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
using CourseTech.Application.Queries.Entities.ReviewQueries;
using CourseTech.Application.Commands.Reviews;
using CourseTech.Domain.Constants.Cache;

namespace CourseTech.UnitTests.Tests.ServiceTests.ReviewServiceTests
{
    public class DeleteReviewTests : IClassFixture<ReviewServiceFixture>
    {
        private readonly ReviewServiceFixture _fixture;

        public DeleteReviewTests(ReviewServiceFixture fixture)
        {
            _fixture = fixture;
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
    }
}
