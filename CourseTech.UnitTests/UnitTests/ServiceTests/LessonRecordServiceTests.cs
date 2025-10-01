using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Enum;
using CourseTech.Tests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests;

public class LessonRecordServiceTests : IClassFixture<LessonRecordServiceFixture>
{
    private readonly LessonRecordServiceFixture _fixture;

    public LessonRecordServiceTests(LessonRecordServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetUserLessonRecordsAsync_ShouldReturnSuccess_WhenRecordsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var lessonRecords = new LessonRecordDto[]
        {
            new LessonRecordDto { LessonName = "test1" },
            new LessonRecordDto { LessonName = "test2" }
        };

        _fixture.CacheServiceMock
            .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonRecordDto[]>>>()))
            .ReturnsAsync(lessonRecords);

        // Act
        var result = await _fixture.LessonRecordService.GetUserLessonRecordsAsync(userId);

        // Assert
        _fixture.CacheServiceMock.Verify(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonRecordDto[]>>>()));
        Assert.True(result.IsSuccess);
        Assert.Equal(lessonRecords.Length, result.Data.ToList().Count);
    }

    [Fact]
    public async Task GetUserLessonRecordsAsync_ShouldReturnFailure_WhenNoRecordsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        LessonRecordDto[] emptyRecords = [];

        _fixture.CacheServiceMock
            .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonRecordDto[]>>>()))
            .ReturnsAsync(emptyRecords);

        // Act
        var result = await _fixture.LessonRecordService.GetUserLessonRecordsAsync(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.LessonRecordsNotFound, result.Error.Code);
    }

}
