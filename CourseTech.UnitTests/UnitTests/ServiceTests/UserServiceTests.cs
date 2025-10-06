using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Enum;
using CourseTech.Tests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Data;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests;

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
        Assert.Equal((int)ErrorCode.UserNotFound, result.Error.Code);
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
    public async Task GetUsersAsync_ShouldReturnSuccess_WhenUsersNotEmpty()
    {
        // Arrange
        var userDtos = new UserDto[] { new UserDto { Id = new Guid(), Login = "User1" } };

        _fixture.CacheServiceMock
            .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<UserDto[]>>>()))
            .ReturnsAsync(userDtos);

        // Act
        var result = await _fixture.UserService.GetUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(userDtos, result.Data);
        _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(CacheKeys.Users, It.IsAny<Func<Task<UserDto[]>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserDataAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto { Id = new Guid() };
        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserWithProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _fixture.UserService.UpdateUserDataAsync(updateUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.UserNotFound, result.Error.Code);
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
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userProfile);

        _fixture.UserValidatorMock.Setup(v => v.ValidateDeletingUser(It.IsAny<UserProfile>(), It.IsAny<User>()))
            .Returns(Result.Failure((int)ErrorCode.UserProfileNotFound, "User profile not found"));

        // Act
        var result = await _fixture.UserService.DeleteUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.UserProfileNotFound, result.Error.Code);
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
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userProfile);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserTokenByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userToken);

        _fixture.UnitOfWorkMock
            .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _fixture.UserValidatorMock.Setup(v => v.ValidateDeletingUser(It.IsAny<UserProfile>(), It.IsAny<User>()))
            .Returns(Result.Success());

        // Act
        var result = await _fixture.UserService.DeleteUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldRollbackTransaction_WhenExceptionOccurs()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userProfile = new UserProfile { UserId = userId };

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userProfile);

        _fixture.UnitOfWorkMock
            .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
            .ReturnsAsync(Mock.Of<IDbContextTransaction>());

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"));

        _fixture.UserValidatorMock.Setup(v => v.ValidateDeletingUser(It.IsAny<UserProfile>(), It.IsAny<User>()))
            .Returns(Result.Success());

        // Act
        var result = await _fixture.UserService.DeleteUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.DeleteUserFailed, result.Error.Code);
    }
}
