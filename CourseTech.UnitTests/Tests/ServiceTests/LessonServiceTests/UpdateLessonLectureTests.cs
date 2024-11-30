using CourseTech.Application.Commands.LessonCommands;
using CourseTech.Application.Queries.Entities.LessonQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.UnitTests.Configurations;
using Microsoft.AspNetCore.Html;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CourseTech.UnitTests.Tests.ServiceTests.LessonServiceTests
{
    public class UpdateLessonLectureTests : IClassFixture<LessonServiceFixture>
    {
        private readonly LessonServiceFixture _fixture = new();

        [Fact]
        public async Task UpdateLessonLectureAsync_LessonExists_UpdatesSuccessfully()
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
        public async Task UpdateLessonLectureAsync_LessonDoesNotExist_ReturnsFailure()
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
        public async Task UpdateLessonLectureAsync_NoChanges_ReturnsFailure()
        {
            // Arrange
            string testMarkup = "<div>test</div>";
            string lessonName = "Name of lesson";
            LessonTypes lessonType = LessonTypes.Common;

            var dto = new LessonLectureDto { Name = lessonName, LessonType = lessonType, LessonMarkup = new HtmlString(testMarkup) };
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
}
