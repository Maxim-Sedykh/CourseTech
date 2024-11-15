using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CourseTech.UnitTests.Configurations;
using CourseTech.Application.Queries.UserQueries;
using Moq;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Domain.Enum;

namespace CourseTech.UnitTests.Tests.ServiceTests.LessonServiceTests
{
    public class GetLessonsForUserTests : IClassFixture<LessonServiceFixture>
    {
        private readonly LessonServiceFixture _fixture = new();

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
        public async Task GetLessonsForUserAsync_InvalidUserProfile_ReturnsFailure()
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

            var validationResult = BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, "UserProfile not found");
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
        public async Task GetLessonsForUserAsync_LessonsNotExists_ReturnsFailure()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profile = new UserProfile { UserId = userId };
            var lessons = new List<LessonDto>();

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(profile);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonDtosQuery>(), default))
                .ReturnsAsync(lessons);

            var validationResult = BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, "UserProfile not found");
            _fixture.LessonValidatorMock.Setup(v => v.ValidateLessonsForUser(profile, lessons))
                .Returns(validationResult);

            // Act
            var result = await _fixture.LessonService.GetLessonsForUserAsync(userId);

            // Assert
            _fixture.LessonValidatorMock.Verify(x => x.ValidateLessonsForUser(profile, lessons), Times.Once);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonsNotFound, result.Error.Code);
        }
    }
}

