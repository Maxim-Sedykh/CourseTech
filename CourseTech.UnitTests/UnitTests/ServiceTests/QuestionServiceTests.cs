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
using AutoMapper;
using CourseTech.Application.Queries.Views;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using CourseTech.Domain.Dto.Question.Pass;

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
        public async Task GetLessonQuestionsAsync_ShouldReturnSuccess_ValidLessonId()
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
        public async Task PassLessonQuestionsAsync_ShouldReturnError_WhenCorrectAnswersAreEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new PracticeUserAnswersDto
            {
                LessonId = 1,
                UserAnswerDtos = []
            };

            var profile = new UserProfile { CurrentGrade = 5 };
            var lesson = new Lesson { Id = 1, LessonType = LessonTypes.Common };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lesson);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonCheckQuestionDtosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _fixture.QuestionValidatorMock.Setup(v => v.ValidateUserLessonOnNull(profile, lesson))
                .Returns(BaseResult.Success());

            _fixture.QuestionValidatorMock.Setup(v => v.ValidateQuestions(It.IsAny<List<ICheckQuestionDto>>(), It.IsAny<int>(), It.IsAny<LessonTypes>()))
                .Returns(BaseResult.Success());

            var questionTypeGrades = new List<QuestionTypeGrade>(); // Пустой список для проверки
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetQuestionTypeGradeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionTypeGrades);

            _fixture.QuestionAnswerCheckerMock.Setup(c => c.CheckUserAnswers(It.IsAny<List<ICheckQuestionDto>>(), It.IsAny<List<IUserAnswerDto>>(), It.IsAny<UserGradeDto>(), It.IsAny<List<QuestionTypeGrade>>()))
                .ReturnsAsync(new List<ICorrectAnswerDto>()); // Пустые правильные ответы

            _fixture.UnitOfWorkMock
                .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            // Act
            var result = await _fixture.QuestionService.PassLessonQuestionsAsync(dto, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)ErrorCodes.AnswerCheckError, result.Error.Code);
        }

        [Fact]
        public async Task PassLessonQuestionsAsync_ShouldReturnSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new PracticeUserAnswersDto
            {
                LessonId = 1,
                UserAnswerDtos = []
            };

            var profile = new UserProfile { CurrentGrade = 5 };
            var lesson = new Lesson { Id = 1, LessonType = LessonTypes.Common };

            var correctAnswers = new List<ICorrectAnswerDto>() { new TestQuestionCorrectAnswerDto() };

            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetProfileByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lesson);
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetLessonCheckQuestionDtosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _fixture.QuestionValidatorMock.Setup(v => v.ValidateUserLessonOnNull(profile, lesson))
                .Returns(BaseResult.Success());

            _fixture.QuestionValidatorMock.Setup(v => v.ValidateQuestions(It.IsAny<List<ICheckQuestionDto>>(), It.IsAny<int>(), It.IsAny<LessonTypes>()))
                .Returns(BaseResult.Success());

            var questionTypeGrades = new List<QuestionTypeGrade>(); // Пустой список для проверки
            _fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetQuestionTypeGradeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionTypeGrades);

            _fixture.QuestionAnswerCheckerMock.Setup(c => c.CheckUserAnswers(It.IsAny<List<ICheckQuestionDto>>(), It.IsAny<List<IUserAnswerDto>>(), It.IsAny<UserGradeDto>(), It.IsAny<List<QuestionTypeGrade>>()))
                .ReturnsAsync(correctAnswers); // Пустые правильные ответы

            _fixture.UnitOfWorkMock
                .Setup(u => u.BeginTransactionAsync(It.IsAny<IsolationLevel>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>()); // Пустые правильные ответы

            // Act
            var result = await _fixture.QuestionService.PassLessonQuestionsAsync(dto, userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(result.Data.QuestionCorrectAnswers, correctAnswers);
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<GetQuestionTypeGradeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
