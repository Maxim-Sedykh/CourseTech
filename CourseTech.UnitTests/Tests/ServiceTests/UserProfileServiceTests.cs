using AutoFixture;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CourseTech.UnitTests.Tests.ServiceTests
{
    public class UserProfileServiceTests : IClassFixture<UserProfileServiceFixture>
    {
        private readonly UserProfileServiceFixture _fixture;
        private readonly IFixture _autoFixture;

        public UserProfileServiceTests(UserProfileServiceFixture fixture)
        {
            _fixture = fixture;
            _autoFixture = new Fixture();
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnSuccess_WhenProfileExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userProfileDto = new UserProfileDto { UserId = userId, Name = "Test User" };

            // Настраиваем медиатор для возврата профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserProfileDtoByUserIdQuery>(), default))
                .ReturnsAsync(userProfileDto);

            // Act
            var result = await _fixture.UserProfileService.GetUserProfileAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(userProfileDto, result.Data);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserProfileDtoByUserIdQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnFailure_WhenProfileDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Настраиваем медиатор для возврата null, что имитирует отсутствие профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserProfileDtoByUserIdQuery>(), default))
                .ReturnsAsync((UserProfileDto)null);

            // Act
            var result = await _fixture.UserProfileService.GetUserProfileAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserProfileDtoByUserIdQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnSuccess_WhenProfileExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = _autoFixture.Create<UpdateUserProfileDto>();
            var existingProfile = _autoFixture.Create<UserProfileDto>();

            // Настраиваем медиатор для возврата существующего профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(existingProfile);

            // Настраиваем медиатор для успешного обновления профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), default))
                .Returns(Task.CompletedTask);

            // Настраиваем Redis для успешного выполнения транзакции
            var redisTransactionMock = new Mock<ITransaction>();
            redisTransactionMock.Setup(t => t.ExecuteAsync()).ReturnsAsync(true);
            _fixture.RedisDatabaseMock.Setup(db => db.CreateTransaction()).Returns(redisTransactionMock.Object);

            // Настраиваем кэширование
            _fixture.CacheServiceMock.Setup(cs => cs.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _fixture.CacheServiceMock.Setup(cs => cs.SetObjectAsync(It.IsAny<string>(), It.IsAny<UserProfileDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(cs => cs.RemoveAsync(It.IsAny<string>()), Times.Once);
            _fixture.CacheServiceMock.Verify(cs => cs.SetObjectAsync(It.IsAny<string>(), existingProfile), Times.Once);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenProfileDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = _autoFixture.Create<UpdateUserProfileDto>();

            // Настраиваем медиатор для возврата null, что имитирует отсутствие профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync((UserProfileDto)null);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.ErrorCode);
            Assert.Equal(ErrorMessage.UserProfileNotFound, result.ErrorMessage);

            // Проверяем, что обновление не было вызвано
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenRedisTransactionFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = _autoFixture.Create<UpdateUserProfileDto>();
            var existingProfile = _autoFixture.Create<UserProfileDto>();
            // Настраиваем медиатор для возврата существующего профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(existingProfile);

            // Настраиваем медиатор для успешного обновления профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), default))
                .Returns(Task.CompletedTask);

            // Настраиваем Redis для неуспешного выполнения транзакции
            var redisTransactionMock = new Mock<ITransaction>();
            redisTransactionMock.Setup(t => t.ExecuteAsync()).ReturnsAsync(false);
            _fixture.RedisDatabaseMock.Setup(db => db.CreateTransaction()).Returns(redisTransactionMock.Object);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RedisTransactionFailed, result.ErrorCode);
        }
    }
}
