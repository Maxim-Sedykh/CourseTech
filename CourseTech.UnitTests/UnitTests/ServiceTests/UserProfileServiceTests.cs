using AutoFixture;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Tests.Configurations.Fixture;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CourseTech.Tests.UnitTests.ServiceTests
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
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<UserProfileDto>>>()))
                .ReturnsAsync(userProfileDto);

            // Act
            var result = await _fixture.UserProfileService.GetUserProfileAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(userProfileDto, result.Data);
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnFailure_WhenProfileDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Настраиваем медиатор для возврата null, что имитирует отсутствие профиля
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<UserProfileDto>>>()))
                .ReturnsAsync((UserProfileDto)null);

            // Act
            var result = await _fixture.UserProfileService.GetUserProfileAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnSuccess_WhenProfileExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UpdateUserProfileDto()
            {
                UserName = "Test",
                Surname = "Test",
                DateOfBirth = DateTime.UtcNow,
            };
            var existingProfile = new UserProfile()
            {
                Id = 1,
                UserId = userId,
                Name = "NameToUpdate",
                Surname = "SurNameToUpdate",
                DateOfBirth = DateTime.UtcNow.AddDays(1),
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProfile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var redisTransactionMock = new Mock<ITransaction>();
            redisTransactionMock.Setup(t => t.ExecuteAsync(default)).ReturnsAsync(true);
            _fixture.RedisDatabaseMock.Setup(db => db.CreateTransaction(default)).Returns(redisTransactionMock.Object);

            _fixture.CacheServiceMock.Setup(cs => cs.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _fixture.CacheServiceMock.Setup(cs => cs.SetObjectAsync(It.IsAny<string>(), It.IsAny<UserProfileDto>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenProfileDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = _autoFixture.Create<UpdateUserProfileDto>();

            // Настраиваем медиатор для возврата null, что имитирует отсутствие профиля
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfile)null);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldReturnFailure_WhenRedisTransactionFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UpdateUserProfileDto()
            {
                UserName = "Test",
                Surname = "Test",
                DateOfBirth = DateTime.UtcNow,
            };
            var existingProfile = new UserProfile()
            {
                Id = 1,
                UserId = userId,
                Name = "NameToUpdate",
                Surname = "SurNameToUpdate",
                DateOfBirth = DateTime.UtcNow.AddDays(1),
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(existingProfile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserProfileCommand>(), default))
                .Returns(Task.CompletedTask);

            var redisTransactionMock = new Mock<ITransaction>();
            redisTransactionMock.Setup(t => t.ExecuteAsync(default)).ReturnsAsync(false);
            _fixture.RedisDatabaseMock.Setup(db => db.CreateTransaction(default)).Returns(redisTransactionMock.Object);

            // Act
            var result = await _fixture.UserProfileService.UpdateUserProfileAsync(updateDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RedisTransactionFailed, result.Error.Code);
        }
    }
}
