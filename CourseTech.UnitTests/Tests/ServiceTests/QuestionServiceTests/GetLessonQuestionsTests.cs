using CourseTech.Application.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Application.Queries.Entities.LessonQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Result;
using CourseTech.UnitTests.Configurations.Fixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CourseTech.UnitTests.Tests.ServiceTests.QuestionServiceTests
{
    public class QuestionServiceTests : IClassFixture<QuestionServiceFixture>
    {
        private readonly QuestionServiceFixture _fixture;

        public QuestionServiceTests(QuestionServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetLessonQuestionsAsync_ValidLessonId_ReturnsSuccess()
        {
            // Arrange
            int lessonId = 1;
            var lesson = new Lesson() { Id = lessonId, LessonType = LessonTypes.Common };
            var questions = new List<IQuestionDto>
            {
                new TestQuestionDto { Id = 1 },
                new OpenQuestionDto { Id = 2 }
            };

            // Настройка мока для Mediator
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
                .ReturnsAsync(lesson);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default))
                .ReturnsAsync(questions);

            // Настройка мока для валидатора
            var validationResult = BaseResult.Success();
            _fixture.QuestionValidatorMock
                .Setup(v => v.ValidateLessonQuestions(lesson, questions))
                .Returns(validationResult);

            // Act
            var result = await _fixture.QuestionService.GetLessonQuestionsAsync(lessonId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(lessonId, result.Data.LessonId);
            Assert.Equal(LessonTypes.Common, result.Data.LessonType);
            Assert.Equal(2, result.Data.Questions.Count);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task GetLessonQuestionsAsync_InvalidQuestions_ReturnsFailure()
        {
            // Arrange
            int lessonId = 1;
            var lesson = new LessonDto { Id = lessonId, LessonType = "Test" };
            var questions = new List<QuestionDto>();

            // Настройка мока для Mediator
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
                .ReturnsAsync(lesson);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default))
                .ReturnsAsync(questions);

            // Настройка мока для валидатора с ошибкой валидации
            var validationResult = new ValidationResult { IsSuccess = false, Error = new ValidationError { Code = ErrorCodes.InvalidQuestions, Message = "Questions are invalid." } };
            _fixture.QuestionValidatorMock
                .Setup(v => v.ValidateLessonQuestions(lesson, questions))
                .Returns(validationResult);

            // Act
            var result = await _fixture.QuestionService.GetLessonQuestionsAsync(lessonId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.InvalidQuestions, result.ErrorCode);
            Assert.Equal("Questions are invalid.", result.ErrorMessage);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default), Times.Once);
        }
    }
}
