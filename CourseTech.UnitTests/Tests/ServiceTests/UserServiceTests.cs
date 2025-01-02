using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Application.Queries.Dtos.UserDtoQueries;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Application.Queries.Entities.UserTokenQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Data;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests
{
    public class UserServiceTests : IClassFixture<UserServiceFixture>
    {
        private readonly UserServiceFixture _fixture;

        public UserServiceTests(UserServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUpdateUserDtoByUserIdQuery>(), default))
                .ReturnsAsync((UpdateUserDto)null);

            // Act
            var result = await _fixture.UserService.GetUserByIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserNotFound, result.Error.Code);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnSuccess_WhenUserIsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new UpdateUserDto { Id = userId, Login = "Test User" };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUpdateUserDtoByUserIdQuery>(), default))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _fixture.UserService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedUser, result.Data);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnSuccess_WhenUsersAreFetchedFromCache()
        {
            // Arrange
            var userDtos = new List<UserDto> { new UserDto { Id = new Guid(), Login = "User1" } };
            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<List<UserDto>>>>()))
                .ReturnsAsync(userDtos);

            // Act
            var result = await _fixture.UserService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(userDtos, result.Data);
            _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(CacheKeys.Users, It.IsAny<Func<Task<List<UserDto>>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserDataAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Id = new Guid() };
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithProfileByUserIdQuery>(), default))
                .ReturnsAsync((User)null);

            // Act
            var result = await _fixture.UserService.UpdateUserDataAsync(updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserNotFound, result.Error.Code);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserWithProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserCommand>(), default), Times.Never);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Users), Times.Never);
        }

        [Fact]
        public async Task UpdateUserDataAsync_ShouldUpdateUserAndReturnSuccess_WhenUserFound()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Id = new Guid(), Login = "testuser" };
            var user = new User { Id = updateUserDto.Id, Login = updateUserDto.Login };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithProfileByUserIdQuery>(), default))
                .ReturnsAsync(user);

            // Act
            var result = await _fixture.UserService.UpdateUserDataAsync(updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(updateUserDto, result.Data);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserWithProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Users), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFailure_WhenUserProfileValidationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var userProfile = new UserProfile { UserId = userId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default))
                .ReturnsAsync(user);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(userProfile);

            // Act
            var result = await _fixture.UserService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(1001, result.Error.Code);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserCommand>(), default), Times.Never);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserProfileCommand>(), default), Times.Never);
            _fixture.UnitOfWorkMock.Verify(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser_WhenValidationSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var userProfile = new UserProfile { UserId = userId };
            var userToken = new UserToken { UserId = userId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default))
                .ReturnsAsync(user);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(userProfile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserTokenByUserIdQuery>(), default))
                .ReturnsAsync(userToken);

            _fixture.UnitOfWorkMock
                .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            // Act
            var result = await _fixture.UserService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserCommand>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserProfileCommand>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserTokenCommand>(), default), Times.Once);

            _fixture.UnitOfWorkMock.Verify(u => u.BeginTransactionAsync(IsolationLevel.RepeatableRead), Times.Once);
            _fixture.UnitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
            _fixture.CacheServiceMock.Verify(c => c.RemoveAsync(CacheKeys.Users), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRollbackTransaction_WhenExceptionOccurs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var userProfile = new UserProfile { UserId = userId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default))
                .ReturnsAsync(user);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(userProfile);

            _fixture.UnitOfWorkMock
                .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
                .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _fixture.UserService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserCommand>(), default), Times.Once);
        }
    }
}
