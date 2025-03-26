using CourseTech.Application.CQRS.Commands.LessonCommands;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Application.CQRS.Queries.Entities.LessonQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Result;
using CourseTech.Tests.Configurations.Fixture;
using Moq;
using Xunit;

namespace CourseTech.Tests.UnitTests.ServiceTests;

public class LessonServiceTests : IClassFixture<LessonServiceFixture>
{
    private readonly LessonServiceFixture _fixture = new();

    [Fact]
    public async Task GetLessonLectureAsync_ShouldReturnSuccess_LessonExists()
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
    public async Task GetLessonLectureAsync_ShouldReturnFailure_LessonDoesNotExist()
    {
        // Arrange
        var lessonId = 1;
        _fixture.CacheServiceMock.Setup(cs => cs.GetOrAddToCache(It.IsAny<string>(), It.IsAny<Func<Task<LessonLectureDto>>>()))
            .ReturnsAsync(null as LessonLectureDto);

        // Act
        var result = await _fixture.LessonService.GetLessonLectureAsync(lessonId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
    }

    [Fact]
    public async Task GetLessonNamesAsync_ShouldReturnFailure_LessonsNotExists()
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
    public async Task GetLessonNamesAsync_ShouldReturnSuccess_LessonsExists()
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

    [Fact]
    public async Task GetLessonsForUserAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new UserProfile { UserId = userId };
        var lessons = new List<LessonDto>
        {
            new LessonDto { Name = "Math" },
            new LessonDto { Name = "Science" }
        };

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
            .ReturnsAsync(profile);
        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonDtosQuery>(), default))
            .ReturnsAsync(lessons);

        var validationResult = BaseResult.Success();
        _fixture.LessonValidatorMock.Setup(v => v.ValidateLessonsForUser(profile, lessons))
            .Returns(validationResult);

        // Act
        var result = await _fixture.LessonService.GetLessonsForUserAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(lessons, result.Data.LessonNames);
        _fixture.MediatorMock.Verify(x => x.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _fixture.MediatorMock.Verify(x => x.Send(It.IsAny<GetLessonDtosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(profile.LessonsCompleted, result.Data.LessonsCompleted);
    }

    [Fact]
    public async Task GetLessonsForUserAsync_ShouldReturnFailure_InvalidUserProfile()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var lessons = new List<LessonDto>
        {
            new LessonDto { Name = "LessonName#1" },
            new LessonDto { Name = "LessonName#2" }
        };

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
            .ReturnsAsync(null as UserProfile);
        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonDtosQuery>(), default))
            .ReturnsAsync(lessons);

        var validationResult = DataResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, "UserProfile not found");
        _fixture.LessonValidatorMock.Setup(v => v.ValidateLessonsForUser(null, lessons))
            .Returns(validationResult);

        // Act
        var result = await _fixture.LessonService.GetLessonsForUserAsync(userId);

        // Assert
        _fixture.LessonValidatorMock.Verify(x => x.ValidateLessonsForUser(null, lessons), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
    }

    [Fact]
    public async Task GetLessonsForUserAsync_ShouldReturnFailure_LessonsNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = new UserProfile { UserId = userId };
        var lessons = new List<LessonDto>();

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
            .ReturnsAsync(profile);
        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonDtosQuery>(), default))
            .ReturnsAsync(lessons);

        var validationResult = DataResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, "UserProfile not found");
        _fixture.LessonValidatorMock.Setup(v => v.ValidateLessonsForUser(profile, lessons))
            .Returns(validationResult);

        // Act
        var result = await _fixture.LessonService.GetLessonsForUserAsync(userId);

        // Assert
        _fixture.LessonValidatorMock.Verify(x => x.ValidateLessonsForUser(profile, lessons), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.LessonsNotFound, result.Error.Code);
    }

    [Fact]
    public async Task UpdateLessonLectureAsync_ShouldReturnASuccess_LessonExists()
    {
        // Arrange
        var dto = new LessonLectureDto { Id = 1, Name = "Updated Lesson" };
        var currentLesson = new Lesson { Id = dto.Id, Name = "Old Lesson" };

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
            .ReturnsAsync(currentLesson);

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<UpdateLessonCommand>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _fixture.LessonService.UpdateLessonLectureAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Data, dto);
        _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateLessonCommand>(), default), Times.Once);
    }


    [Fact]
    public async Task UpdateLessonLectureAsync_ShouldReturnFailure_LessonDoesNotExist()
    {
        // Arrange
        var dto = new LessonLectureDto { Id = 1, Name = "Updated Lesson" };

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
            .ReturnsAsync(null as Lesson);

        // Act
        var result = await _fixture.LessonService.UpdateLessonLectureAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
        _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default), Times.Once);
    }

    [Fact]
    public async Task UpdateLessonLectureAsync_ShouldReturnFailure_NoChanges()
    {
        // Arrange
        string testMarkup = "<div>test</div>";
        string lessonName = "Name of lesson";
        LessonTypes lessonType = LessonTypes.Common;

        var dto = new LessonLectureDto { Name = lessonName, LessonType = lessonType, LectureMarkup = testMarkup };
        var currentLesson = new Lesson { Name = lessonName, LessonType = lessonType, LectureMarkup = testMarkup };

        _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
            .ReturnsAsync(currentLesson);

        // Act
        var result = await _fixture.LessonService.UpdateLessonLectureAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<UpdateLessonCommand>(), default), Times.Never);
    }
}
