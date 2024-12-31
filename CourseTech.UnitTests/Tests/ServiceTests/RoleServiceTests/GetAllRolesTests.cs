using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using Xunit;
using Moq;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{
    public class GetAllRolesTests : IClassFixture<RoleServiceFixture>
    {
        private readonly RoleServiceFixture _fixture;

        public GetAllRolesTests(RoleServiceFixture fixture)
        {
            _fixture = fixture;
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
    }
}
