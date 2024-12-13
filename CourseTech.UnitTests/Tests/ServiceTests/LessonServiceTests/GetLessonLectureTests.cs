using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.LessonServiceTests
{
    public class GetLessonLectureTests : IClassFixture<LessonServiceFixture>
    {
        private readonly LessonServiceFixture _fixture = new();

        [Fact]
        public async Task GetLessonLectureAsync_LessonExists_ReturnsSuccessResult()
        {
            // Arrange
            var lessonId = 1;
            var expectedLesson = new LessonLectureDto { Id = lessonId };
            _fixture.CacheServiceMock.Setup(cs => cs.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonLectureDto>>>()))
                .ReturnsAsync(expectedLesson);

            // Act
            var result = await _fixture.LessonService.GetLessonLectureAsync(lessonId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedLesson, result.Data);
        }

        [Fact]
        public async Task GetLessonLectureAsync_LessonDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            var lessonId = 1;
            _fixture.CacheServiceMock.Setup(cs => cs.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonLectureDto?>>>()))
                .ReturnsAsync(null as LessonLectureDto);

            // Act
            var result = await _fixture.LessonService.GetLessonLectureAsync(lessonId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
        }
    }
}
