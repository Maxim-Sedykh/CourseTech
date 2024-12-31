using AutoFixture;
using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{
    public class AddRoleForUserTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;
        private readonly IFixture _autoFixture;

        public AddRoleForUserTests(RoleServiceFixture fixture)
        {
            _fixture = fixture;
            _autoFixture = new Fixture();
        }

        [Fact]
        public async Task AddRoleForUserAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var dto = _autoFixture.Create<UserRoleDto>();
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default))
                .ReturnsAsync((User)null);

            // Act
            var result = await _fixture.RoleService.AddRoleForUserAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserNotFound, result.Error.Code);
        }

        [Fact]
        public async Task AddRoleForUserAsync_RoleNotFound_ReturnsFailure()
        {
            // Arrange
            var dto = _autoFixture.Create<UserRoleDto>();
            var user = new User { Login = dto.Login, Roles = new List<Role>() };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default))
                .ReturnsAsync(user);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), default))
                .ReturnsAsync((Role)null);

            // Act
            var result = await _fixture.RoleService.AddRoleForUserAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);
        }

        [Fact]
        public async Task AddRoleForUserAsync_UserAlreadyHasRole_ReturnsFailure()
        {
            // Arrange
            var dto = _autoFixture.Create<UserRoleDto>();
            var user = new User
            {
                Login = dto.Login,
                Roles = new List<Role> { new Role { Name = dto.RoleName } }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default))
                .ReturnsAsync(user);

            // Act
            var result = await _fixture.RoleService.AddRoleForUserAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserAlreadyExistThisRole, result.Error.Code);
        }

        [Fact]
        public async Task AddRoleForUserAsync_SuccessfullyAddsRole_ReturnsSuccess()
        {
            // Arrange
            var dto = _autoFixture.Create<UserRoleDto>();
            var user = new User { Login = dto.Login, Roles = new List<Role>() };
            var role = new Role { Id = 1, Name = dto.RoleName };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default))
                .ReturnsAsync(user);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), default))
                .ReturnsAsync(role);

            // Act
            var result = await _fixture.RoleService.AddRoleForUserAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(dto.Login, result.Data.Login);
            Assert.Equal(role.Name, result.Data.RoleName);

            // Verify that the CreateUserRoleCommand was sent
            _fixture.MediatorMock.Verify(m => m.Send(It.Is<CreateUserRoleCommand>(cmd => cmd.RoleId == role.Id && cmd.UserId == user.Id), default), Times.Once);
        }
    }
}
