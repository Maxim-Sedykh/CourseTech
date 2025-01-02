using CourseTech.Application.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Application.Queries.Entities.LessonQueries;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question;
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
using AutoFixture;

namespace CourseTech.UnitTests.Tests.ServiceTests
{
    public class QuestionServiceTests
    {
        private readonly QuestionServiceFixture _fixture;
        private readonly Fixture _autoFixture;

        public QuestionServiceTests(QuestionServiceFixture fixture)
        {
            _fixture = fixture;
            _autoFixture = new Fixture();
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
            var lesson = new Lesson { Id = lessonId, LessonType = LessonTypes.Common };
            var questions = new List<IQuestionDto>();

            // Настройка мока для Mediator
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
                .ReturnsAsync(lesson);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default))
                .ReturnsAsync(questions);

            // Настройка мока для валидатора с ошибкой валидации
            var validationResult = BaseResult.Failure((int)ErrorCodes.LessonNotFound, "Lesson not found");
            _fixture.QuestionValidatorMock
                .Setup(v => v.ValidateLessonQuestions(lesson, questions))
                .Returns(validationResult);

            // Act
            var result = await _fixture.QuestionService.GetLessonQuestionsAsync(lessonId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
            Assert.Equal("Lesson not found", result.Error.Message);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default), Times.Once);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task PassLessonQuestionsAsync_ShouldReturnFailure_WhenProfileIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = _autoFixture.Create<PracticeUserAnswersDto>();

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync((UserProfile)null);

            // Act
            var result = await _fixture.QuestionService.PassLessonQuestionsAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.UserProfileNotFound, result.Error.Code);
        }

        [Fact]
        public async Task PassLessonQuestionsAsync_ShouldReturnFailure_WhenLessonIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = _autoFixture.Create<PracticeUserAnswersDto>();

            var profile = new UserProfile { UserId = userId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(profile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
                .ReturnsAsync((Lesson)null);

            // Act
            var result = await _fixture.QuestionService.PassLessonQuestionsAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
        }

        [Fact]
        public async Task PassLessonQuestionsAsync_ShouldReturnSuccess_WhenAllValidationsPass()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = _autoFixture.Create<PracticeUserAnswersDto>();

            var profile = new UserProfile { UserId = userId, CurrentGrade = 10 };
            var lesson = new Lesson { Id = dto.LessonId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(profile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
            .ReturnsAsync(lesson);

            var questions = new List<ICheckQuestionDto> { new TestQuestionCheckingDto() };
            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonCheckQuestionDtosQuery>(), default))
                .ReturnsAsync(questions);

            var correctAnswers = new List<ICorrectAnswerDto>(); // Assuming it returns empty for valid answers.
            _fixture.QuestionAnswerCheckerMock
                .Setup(m => m.CheckUserAnswers(questions, dto.UserAnswerDtos, It.IsAny<UserGradeDto>(), It.IsAny<List<QuestionTypeGrade>>()))
                .ReturnsAsync(correctAnswers);

            // Act
            var result = await _fixture.QuestionService.PassLessonQuestionsAsync(dto, userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(lesson.Id, result.Data.LessonId);
            Assert.Equal(correctAnswers, result.Data.QuestionCorrectAnswers);
        }
    }
}
