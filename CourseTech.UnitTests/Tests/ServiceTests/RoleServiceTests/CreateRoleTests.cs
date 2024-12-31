using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{

    public class CreateRoleServiceTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;

        public CreateRoleServiceTests(RoleServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateRoleAsync_RoleExists_ReturnsFailureResult()
        {
            // Arrange
            var createRoleDto = new CreateRoleDto { RoleName = "Admin" };
            var existingRole = new Role { Name = "Admin" };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), default))
                .ReturnsAsync(existingRole);

            // Act
            var result = await _fixture.RoleService.CreateRoleAsync(createRoleDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);
        }

        [Fact]
        public async Task CreateRoleAsync_RoleDoesNotExist_CreatesRoleAndReturnsSuccessResult()
        {
            // Arrange
            var createRoleDto = new CreateRoleDto { RoleName = "User" };
            Role createdRole = null;

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), default))
                .ReturnsAsync((Role)null); // Роль не существует

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<CreateRoleCommand>(), default))
                .Returns(Task.CompletedTask); // Успешно создаем роль

            _fixture.MapperMock
                .Setup(m => m.Map<RoleDto>(It.IsAny<RoleDto>()))
                .Returns((RoleDto r) => r); // Для простоты, просто возвращаем тот же объект

            // Act
            var result = await _fixture.RoleService.CreateRoleAsync(createRoleDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<CreateRoleCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(m => m.RemoveAsync(CacheKeys.Roles), Times.Once);
        }
    }
}
