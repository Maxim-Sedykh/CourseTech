using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Application.CQRS.Commands.UserCommand;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Entities.RoleQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserTokenQueries;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Result;
using CourseTech.Tests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Data;
using System.Security.Claims;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests
{
    public class AuthServiceTests : IClassFixture<AuthServiceFixture>
    {
        private readonly AuthServiceFixture _fixture;

        public AuthServiceTests(AuthServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Login_ShouldReturnSuccess_ReturnsTokenDto()
        {
            // Arrange
            var loginDto = new LoginUserDto { Login = "testuser", Password = "password123" };
            var user = new User { Id = new Guid(), Login = "testuser" }; // mock user from database

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _fixture.AuthValidatorMock.Setup(v => v.ValidateLogin(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(BaseResult.Success());

            _fixture.TokenServiceMock.Setup(t => t.GetClaimsFromUser(It.IsAny<User>()))
                .Returns(new List<Claim> { new Claim(ClaimTypes.Name, "testuser") });

            _fixture.TokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>()))
                .Returns("access_token");

            _fixture.TokenServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns("refresh_token");

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserTokenByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserToken)null); // no existing token

            // Act
            var result = await _fixture.AuthService.Login(loginDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("access_token", result.Data.AccessToken);
            Assert.Equal("refresh_token", result.Data.RefreshToken);
        }

        [Fact]
        public async Task Login_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var loginDto = new LoginUserDto { Login = "testuser", Password = "wrongpassword" };
            var user = new User { Id = new Guid(), Login = "testuser", Password = "anotherpassword" };

            var errorMessage = "Invalid password";

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _fixture.AuthValidatorMock.Setup(v => v.ValidateLogin(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(BaseResult.Failure((int)ErrorCodes.PasswordIsWrong, errorMessage));

            // Act
            var result = await _fixture.AuthService.Login(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.PasswordIsWrong, result.Error.Code);
            Assert.Equal(errorMessage, result.Error.Message);
        }

        [Fact]
        public async Task Register_ShouldReturnSuccess_ReturnsUserDto()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                Login = "newuser",
                Password = "password123",
                PasswordConfirm = "password123",
                UserName = "New",
                Surname = "User",
                DateOfBirth = DateTime.Now
            };

            var newUser = new User { Id = new Guid(), Login = "newuser" };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            _fixture.AuthValidatorMock.Setup(v => v.ValidateRegister(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(BaseResult.Success());

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newUser);

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<CreateUserProfileCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Role { Id = 1, Name = "User" });

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<CreateUserRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _fixture.CacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            _fixture.MapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto());

            // Act
            var result = await _fixture.AuthService.Register(registerDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
        }
    }
}
