using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.ReviewServiceTests
{
    public class CreateReviewTests : IClassFixture<ReviewServiceFixture>
    {
        private readonly ReviewServiceFixture _fixture;

        public CreateReviewTests(ReviewServiceFixture fixture)
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
    }
}
