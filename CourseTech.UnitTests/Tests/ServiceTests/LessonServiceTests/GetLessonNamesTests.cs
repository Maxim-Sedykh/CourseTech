using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.LessonServiceTests
{
    public class GetLessonNamesTests : IClassFixture<LessonServiceFixture>
    {
        private readonly LessonServiceFixture _fixture = new();

        [Fact]
        public async Task GetLessonNamesAsync_LessonsNotExists_ReturnsFailure()
        {
            // Arrange
            _fixture.CacheServiceMock.Setup(c => c.GetOrAddToCache(It.Is<string>(s => s == CacheKeys.LessonNames),
                It.IsAny<Func<Task<IEnumerable<LessonNameDto>>>>())).ReturnsAsync([]);

            // Act
            var result = await _fixture.LessonService.GetLessonNamesAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.Error.Code == (int)ErrorCodes.LessonsNotFound);
        }

        [Fact]
        public async Task GetLessonNamesAsync_LessonsExists_ReturnsSuccess()
        {
            // Arrange
            LessonNameDto[] lessonNames = [new LessonNameDto() { Name = "Название урока" }];
            _fixture.CacheServiceMock.Setup(c => c.GetOrAddToCache(It.IsAny<string>(),
                It.IsAny<Func<Task<LessonNameDto[]>>>())).ReturnsAsync(lessonNames);

            // Act
            var result = await _fixture.LessonService.GetLessonNamesAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(lessonNames, result.Data.ToArray());
        }
    }
}
