using AutoFixture;
using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Application.CQRS.Queries.Entities.RoleQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Result;
using CourseTech.Tests.Configurations.Fixture;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Data;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests
{
    public class RoleServiceTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;
        private readonly IFixture _autoFixture;

        public RoleServiceTests(RoleServiceFixture fixture)
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
            var dto = new UserRoleDto("test", "admin");
            {
                
            };
            var user = new User { Login = dto.Login, Roles = [new Role { Name = "wrontRole" }] };

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
            var dto = new UserRoleDto("test", "Admin");
            var user = new User { Login = dto.Login, Roles = [new Role() { Id = 1, Name = "User" }] };
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

        [Fact]
        public async Task GetAllRoles_WhenRolesExist_ReturnsSuccessResult()
        {
            // Arrange
            var roles = new[]
            {
                new RoleDto { Id = 1, RoleName = "Admin" },
                new RoleDto { Id = 2, RoleName = "User" }
            };

            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<RoleDto[]>>>()))
                .ReturnsAsync(roles);

            // Act
            var result = await _fixture.RoleService.GetAllRoles();

            var resultData = result.Data.ToList();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(roles.Length, resultData.Count);
            Assert.Equal("Admin", resultData[0].RoleName);
            Assert.Equal("User", resultData[1].RoleName);
        }

        [Fact]
        public async Task GetAllRoles_WhenNoRolesExist_ReturnsFailureResult()
        {
            // Arrange
            var roles = new RoleDto[0]; // Пустой массив ролей

            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<RoleDto[]>>>()))
                .ReturnsAsync(roles);

            // Act
            var result = await _fixture.RoleService.GetAllRoles();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RolesNotFound, result.Error.Code);
        }

        [Fact]
        public async Task DeleteRoleAsync_RoleExists_DeletesRoleAndReturnsSuccessResult()
        {
            // Arrange
            int roleId = 1;
            var existingRole = new Role() { Id = roleId, Name = "Admin" };

             _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRole);

            _fixture.MapperMock
                .Setup(m => m.Map<RoleDto>(It.IsAny<Role>()))
                .Returns(new RoleDto() { Id = roleId });

            // Act
            var result = await _fixture.RoleService.DeleteRoleAsync(roleId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(roleId, result.Data.Id);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _fixture.CacheServiceMock.Verify(m => m.RemoveAsync(CacheKeys.Roles), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleAsync_RoleDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            long roleId = 1;

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role)null);

            // Act
            var result = await _fixture.RoleService.DeleteRoleAsync(roleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);
        }

        [Fact]
        public async Task DeleteRoleForUserAsync_WhenValidationPassed_DeletesRoleAndReturnsSuccessResult()
        {
            // Arrange
            var dto = new DeleteUserRoleDto("testUser", 2);

            var roleToDelete = new Role()
            {
                Id = 2,
                Name = "Admin"
            };

            var user = new User
            {
                Id = new Guid(),
                Login = dto.Login,
                Roles =
                [
                    new Role() {
                        Id = 1,
                        Name = "User"
                    },
                    roleToDelete
                ]
            };

            var userRole = new UserRole
            {
                RoleId = roleToDelete.Id,
                UserId = user.Id
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _fixture.RoleValidatorMock
                .Setup(v => v.ValidateRoleForUser(It.IsAny<User>(), It.IsAny<Role>()))
                .Returns(BaseResult.Success());

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserRoleByIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);

            // Act
            var result = await _fixture.RoleService.DeleteRoleForUserAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.Login, result.Data.Login);
            Assert.Equal("Admin", result.Data.RoleName);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleForUserAsync_ValidationFailed_ReturnsFailureResult()
        {
            // Arrange
            var dto = new DeleteUserRoleDto("testUser", 2);

            var roleToDelete = new Role()
            {
                Id = 2,
                Name = "Admin"
            };

            var user = new User
            {
                Id = new Guid(),
                Login = dto.Login,
                Roles =
                [
                    new Role() {
                        Id = 1,
                        Name = "User"
                    },
                    roleToDelete
                ]
            };

            var userRole = new UserRole
            {
                RoleId = roleToDelete.Id,
                UserId = user.Id
            };

            var errorMessage = "Role not found";

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _fixture.RoleValidatorMock
                .Setup(v => v.ValidateRoleForUser(It.IsAny<User>(), It.IsAny<Role[]>()))
                .Returns(BaseResult.Failure((int)ErrorCodes.RoleNotFound, errorMessage));

            // Act
            var result = await _fixture.RoleService.DeleteRoleForUserAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);
            Assert.Equal(errorMessage, result.Error.Message);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), It.IsAny<CancellationToken>()), Times.Never);
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
        public async Task CreateRoleAsync_WhenRoleDoesNotExist_CreatesRoleAndReturnsSuccessResult()
        {
            // Arrange
            var createRoleDto = new CreateRoleDto { RoleName = "User" };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByNameQuery>(), default))
                .ReturnsAsync((Role)null); // Роль не существует

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<CreateRoleCommand>(), default))
                .Returns(Task.CompletedTask); // Успешно создаем роль

            _fixture.MapperMock
                .Setup(m => m.Map<RoleDto>(It.IsAny<Role>()))
                .Returns(new RoleDto()); // Для простоты, просто возвращаем тот же объект

            // Act
            var result = await _fixture.RoleService.CreateRoleAsync(createRoleDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task UpdateRoleForUserAsync_WhenValidationSucceeds_UpdatesRoleSuccessfully()
        {
            // Arrange
            var updateDto = new UpdateUserRoleDto("user1", 1, 2);
            var user = new User { Id = new Guid(), Login = "user1", Roles = [new Role() { Id = 1, Name = "OldRole" }] };
            var role = new Role { Id = 1, Name = "OldRole" };
            var newRole = new Role { Id = 2, Name = "NewRole" };
            var userRole = new UserRole() { UserId = user.Id, RoleId = role.Id };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRole);
            _fixture.RoleValidatorMock.Setup(v => v.ValidateRoleForUser(It.IsAny<User>(), It.IsAny<Role[]>()))
                .Returns(BaseResult.Success());
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserRoleByIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userRole);
            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<CreateUserRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.RoleService.UpdateRoleForUserAsync(updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("user1", result.Data.Login);
            Assert.Equal("NewRole", result.Data.RoleName);
        }

        [Fact]
        public async Task UpdateRoleForUserAsync_WhenExceptionOccurs_RollsBackTransaction()
        {
            // Arrange
            var user = new User { Id = new Guid(), Login = "user1", Roles = [new Role() { Id = 1, Name = "OldRole" }] };

            var updateDto = new UpdateUserRoleDto("user1", 1, 2);
            var role = new Role { Id = 1, Name = "OldRole" };
            var newRole = new Role { Id = 2, Name = "NewRole" };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newRole);
            _fixture.RoleValidatorMock.Setup(v => v.ValidateRoleForUser(It.IsAny<User>(), It.IsAny<Role[]>()))
                .Returns(BaseResult.Success());

            var transactionMock = new Mock<IDbContextTransaction>();

            _fixture.UnitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(transactionMock.Object);

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserRoleByIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserRole { UserId = user.Id, RoleId = role.Id });

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Database error")); // Симулируем исключение

            // Act
            var result = await _fixture.RoleService.UpdateRoleForUserAsync(updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once); // Подтверждение того что был вызван Rollback транзакции
        }

        [Fact]
        public async Task UpdateRoleAsync_WhenRoleExists_UpdatesRoleSuccessfully()
        {
            // Arrange
            var roleDto = new RoleDto { Id = 1, RoleName = "UpdatedRole" };
            var existingRole = new Role { Id = 1, Name = "OldRole" };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRole);

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _fixture.CacheServiceMock.Setup(c => c.RemoveAsync(CacheKeys.Roles))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _fixture.RoleService.UpdateRoleAsync(roleDto);

            // Assert
            Assert.True(result.IsSuccess);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRoleAsync_WhenRoleDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            var roleDto = new RoleDto { Id = 1, RoleName = "InvalidRole" };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role)null);

            // Act
            var result = await _fixture.RoleService.UpdateRoleAsync(roleDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
