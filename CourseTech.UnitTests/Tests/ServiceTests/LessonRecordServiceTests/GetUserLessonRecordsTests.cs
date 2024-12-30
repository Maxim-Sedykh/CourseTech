using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.LessonRecordServiceTests
{
    public class LessonRecordServiceTests : IClassFixture<LessonRecordServiceFixture>
    {
        private readonly LessonRecordServiceFixture _fixture;

        public LessonRecordServiceTests(LessonRecordServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetUserLessonRecordsAsync_ReturnsSuccess_WhenRecordsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var lessonRecords = new List<LessonRecordDto>
            {
                new LessonRecordDto { LessonName = "test1" },
                new LessonRecordDto { LessonName = "test2" }
            };

            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<List<LessonRecordDto>>>>()))
                .ReturnsAsync(lessonRecords);

            // Act
            var result = await _fixture.LessonRecordService.GetUserLessonRecordsAsync(userId);

            // Assert
            _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonRecordDto[]>>>()));
            Assert.True(result.IsSuccess);
            Assert.Equal(lessonRecords.Count, result.Data.ToList().Count);
        }

        [Fact]
        public async Task GetUserLessonRecordsAsync_ReturnsFailure_WhenNoRecordsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emptyRecords = new List<LessonRecordDto>();

            _fixture.CacheServiceMock
                .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<List<LessonRecordDto>>>>()))
                .ReturnsAsync(emptyRecords);

            // Act
            var result = await _fixture.LessonRecordService.GetUserLessonRecordsAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonRecordsNotFound, result.Error.Code);
        }

    }
}
