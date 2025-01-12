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
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using CourseTech.Tests.Configurations.Fixture;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using Xunit.Sdk;

namespace CourseTech.Tests.UnitTests.ServiceTests
{
    public class QuestionServiceTests : IClassFixture<QuestionServiceFixture>
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
        }

        [Fact]
        public async Task GetLessonQuestionsAsync_ShouldReturnFailure_WhenValidationFailed()
        {
            // Arrange
            int lessonId = 1;
            var lesson = new Lesson { Id = lessonId, LessonType = LessonTypes.Common };
            var questions = new List<IQuestionDto>();
            var errorMessage = "Lesson not found";

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lesson);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonQuestionDtosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questions);
            
            var validationResult = BaseResult.Failure((int)ErrorCodes.LessonNotFound, errorMessage);
            _fixture.QuestionValidatorMock
                .Setup(v => v.ValidateLessonQuestions(lesson, questions))
                .Returns(validationResult);

            // Act
            var result = await _fixture.QuestionService.GetLessonQuestionsAsync(lessonId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.LessonNotFound, result.Error.Code);
            Assert.Equal(errorMessage, result.Error.Message);
        }

        [Fact]
        public async Task PassLessonQuestionsAsync_ShouldReturnFailure_WhenValidationFailed()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new PracticeUserAnswersDto()
            {
                LessonId = 1,
                UserAnswerDtos =
                [
                    new TestQuestionUserAnswerDto()
                    {
                        QuestionId = 1,
                        UserAnswerNumberOfVariant = 1
                    },
                    new OpenQuestionUserAnswerDto()
                    {
                        QuestionId = 2,
                        UserAnswer = "Test"
                    }
                ]
            };

            var profile = new UserProfile { UserId = userId };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), default))
                .ReturnsAsync(profile);

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), default))
                .ReturnsAsync((Lesson)null);

            //var validationResult = BaseResult.Failure((int)ErrorCodes.LessonNotFound, errorMessage);
            //_fixture.QuestionValidatorMock
            //    .Setup(v => v.ValidateLessonQuestions(lesson, questions))
            //    .Returns(validationResult);

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
            var dto = new PracticeUserAnswersDto()
            {
                LessonId = 1,
                UserAnswerDtos =
                [
                    new TestQuestionUserAnswerDto()
                    {
                        QuestionId = 1,
                        UserAnswerNumberOfVariant = 1
                    },
                    new OpenQuestionUserAnswerDto()
                    {
                        QuestionId = 2,
                        UserAnswer = "Test"
                    }
                ]
            };

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
