using CourseTech.Domain.Dto.Role;
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
using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Entities;
using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Result;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{
    public class DeleteRoleForUserTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;

        public DeleteRoleForUserTests(RoleServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteRoleForUserAsync_ValidRole_DeletesRoleAndReturnsSuccessResult()
        {
            // Arrange
            var dto = new DeleteUserRoleDto("testUser", 1);

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
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default)) 
                .ReturnsAsync(user);

            _fixture.RoleValidatorMock
                .Setup(v => v.ValidateRoleForUser(user, user.Roles.First()))
                .Returns(BaseResult.Success());

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserRoleByIdsQuery>(), default))
                .ReturnsAsync(userRole);

            // Act
            var result = await _fixture.RoleService.DeleteRoleForUserAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.Login, result.Data.Login);
            Assert.Equal("Admin", result.Data.RoleName);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleForUserAsync_InvalidRole_ReturnsFailureResult()
        {
            // Arrange
            var dto = new DeleteUserRoleDto("testUser", 1);

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
                .Setup(m => m.Send(It.IsAny<GetUserWithRolesByLoginQuery>(), default))
                .ReturnsAsync(user);

            _fixture.RoleValidatorMock
                .Setup(v => v.ValidateRoleForUser(user, null))
                .Returns(BaseResult.Failure((int)ErrorCodes.RoleNotFound, "Role not found"));

            // Act
            var result = await _fixture.RoleService.DeleteRoleForUserAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.Error.Code);
            Assert.Equal("Role not found", result.Error.Message);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteUserRoleCommand>(), default), Times.Never);
        }
    }
}
