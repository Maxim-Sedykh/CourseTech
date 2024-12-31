using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{
    public class DeleteRoleTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;

        public DeleteRoleTests(RoleServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteRoleAsync_RoleExists_DeletesRoleAndReturnsSuccessResult()
        {
            // Arrange
            int roleId = 1;
            var existingRole = new Role() { Id = roleId, Name = "Admin" };

            Moq.Language.Flow.IReturnsResult<MediatR.IMediator> returnsResult = _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), default))
                .ReturnsAsync(existingRole); // Роль существует

            // Act
            var result = await _fixture.RoleService.DeleteRoleAsync(roleId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(roleId, result.Data.Id);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteRoleCommand>(), default), Times.Once);
            _fixture.CacheServiceMock.Verify(m => m.RemoveAsync(CacheKeys.Roles), Times.Once);
        }

        [Fact]
        public async Task DeleteRoleAsync_RoleDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            long roleId = 1;

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoleByIdQuery>(), default))
                .ReturnsAsync((Role)null); // Роль не существует

            // Act
            var result = await _fixture.RoleService.DeleteRoleAsync(roleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.RoleNotFound, result.Error.Code);

            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<DeleteRoleCommand>(), default), Times.Never);
            _fixture.CacheServiceMock.Verify(m => m.RemoveAsync(CacheKeys.Roles), Times.Never);
        }
    }
}
