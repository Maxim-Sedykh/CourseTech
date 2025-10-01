using AutoFixture;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Enum;
using Xunit;
using Moq;
using CourseTech.Domain.Result;
using CourseTech.Tests.Configurations.Fixture;
using CourseTech.Domain.Dto.CourseResult;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Application.CQRS.Queries.Dtos.LessonRecordDtoQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Entities.UserRelated;

namespace CourseTech.Tests.UnitTests.ServiceTests;

public class CourseResultServiceTests : IClassFixture<CourseResultServiceFixture>
{
    private readonly CourseResultServiceFixture _fixture;
    private readonly IFixture _autoFixture;

    public CourseResultServiceTests(CourseResultServiceFixture fixture)
    {
        _autoFixture = new Fixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCourseResultAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new UserProfile()
        {
            Id = 1,
            UserId = userId,
            Name = "Test",
        };
        var lessonsCount = 0;

        var errorMessage = "Userprofile not found";

        _fixture.CourseResultValidatorMock
            .Setup(v => v.ValidateUserCourseResult(It.IsAny<UserProfile>(), It.IsAny<int>()))
            .Returns(BaseResult.Failure((int)ErrorCode.UserProfileNotFound, errorMessage));

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
            .ReturnsAsync(profile);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetLessonsCountQuery>(), default))
            .ReturnsAsync(lessonsCount);

        // Act
        var result = await _fixture.CourseResultService.GetCourseResultAsync(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.UserProfileNotFound, result.Error.Code);
        Assert.Equal(errorMessage, result.Error.Message);
    }

    [Fact]
    public async Task GetCourseResultAsync_ShouldReturnSuccess_WhenValidationPasses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new UserProfile()
        {
            Id = 1,
            UserId = userId,
            Name = "Test",
        };
        var lessonsCount = 10;
        var userLessonRecords = _autoFixture.Create<LessonRecordDto[]>();

        // Мокаем запросы и валидатор
        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
            .ReturnsAsync(profile);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetLessonsCountQuery>(), default))
            .ReturnsAsync(lessonsCount);

        _fixture.CourseResultValidatorMock
            .Setup(v => v.ValidateUserCourseResult(It.IsAny<UserProfile>(), lessonsCount))
            .Returns(BaseResult.Success());

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<GetLessonRecordDtosByUserIdQuery>(), default))
            .ReturnsAsync(userLessonRecords);

        _fixture.MediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateCompletedCourseUserProfileCommand>(), default))
            .Returns(Task.CompletedTask);

        _fixture.MapperMock.Setup(m => m.Map<CourseResultDto>(It.IsAny<UserProfile>()))
            .Returns(new CourseResultDto());

        // Act
        var result = await _fixture.CourseResultService.GetCourseResultAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetUserAnalys_ShouldReturnFailure_WhenAnalysNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _fixture.CacheServiceMock
            .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<UserAnalysDto>>>()))
            .ReturnsAsync((UserAnalysDto)null);

        // Act
        var result = await _fixture.CourseResultService.GetUserAnalys(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCode.UserAnalysNotFound, result.Error.Code);
    }

    [Fact]
    public async Task GetUserAnalys_ShouldReturnSuccess_WhenAnalysFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userAnalys = _autoFixture.Create<UserAnalysDto>();

        _fixture.CacheServiceMock
            .Setup(c => c.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<UserAnalysDto>>>()))
            .ReturnsAsync(userAnalys);

        // Act
        var result = await _fixture.CourseResultService.GetUserAnalys(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userAnalys, result.Data);
    }
}
