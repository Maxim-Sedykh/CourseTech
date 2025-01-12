using AutoFixture;
using CourseTech.Application.Helpers;
using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CourseTech.Tests.UnitTests.QuestionHelpersTests
{
    public class QuestionAnswerCheckerTests
    {
        private readonly Mock<IQueryGraphAnalyzer> _mockQueryGraphAnalyzer;
        private readonly Mock<ISqlQueryProvider> _mockSqlProvider;
        private readonly QuestionAnswerChecker _checker;
        private readonly IFixture _fixture;

        public QuestionAnswerCheckerTests()
        {
            _mockQueryGraphAnalyzer = new Mock<IQueryGraphAnalyzer>();
            _mockSqlProvider = new Mock<ISqlQueryProvider>();
            _checker = new QuestionAnswerChecker(_mockQueryGraphAnalyzer.Object, _mockSqlProvider.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CheckUserAnswers_ShouldReturnCorrectAnswers_WhenAnswersAreValid()
        {
            // Arrange
            var checkQuestionDtos = _fixture.CreateMany<ICheckQuestionDto>(3).ToList();
            var userAnswers = _fixture.CreateMany<IUserAnswerDto>(3).ToList();
            var userGrade = new UserGradeDto();
            var questionTypeGrades = new List<QuestionTypeGrade>
            {
                new QuestionTypeGrade { QuestionTypeName = nameof(TestQuestion), Grade = 2 },
                new QuestionTypeGrade { QuestionTypeName = nameof(OpenQuestion), Grade = 3 },
                new QuestionTypeGrade { QuestionTypeName = nameof(PracticalQuestion), Grade = 5 }
            };

            // Установим соответствие между вопросами и ответами
            for (int i = 0; i < userAnswers.Count; i++)
            {
                userAnswers[i].QuestionId = checkQuestionDtos[i].QuestionId;
            }

            // Мокаем метод CheckAnswer
            var correctAnswerDtoMock = new Mock<ICorrectAnswerDto>();
            for (int i = 0; i < userAnswers.Count; i++)
            {
                _mockQueryGraphAnalyzer.Setup(x => x.CalculateUserQueryScore(userAnswers[i], checkQuestionDtos[i], out float userGrade, null))
                    .ReturnsAsync(correctAnswerDtoMock.Object);
            }

            // Act
            var result = await _checker.CheckUserAnswers(checkQuestionDtos, userAnswers, userGrade, questionTypeGrades);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userAnswers.Count, result.Count);
            foreach (var correctAnswer in result)
            {
                Assert.IsType<ICorrectAnswerDto>(correctAnswer);
            }
        }

        [Fact]
        public async Task CheckUserAnswers_ShouldReturnEmptyList_WhenQuestionIdsDoNotMatch()
        {
            // Arrange
            var checkQuestionDtos = _fixture.CreateMany<ICheckQuestionDto>(3).ToList();
            var userAnswers = _fixture.CreateMany<IUserAnswerDto>(3).ToList();
            var userGrade = new UserGradeDto();
            var questionTypeGrades = new List<QuestionTypeGrade>
        {
            new QuestionTypeGrade { QuestionTypeName = nameof(TestQuestion), Grade = 2 }
        };

            // Установим разные QuestionId для проверки
            userAnswers[0].QuestionId = 3; // Не совпадает с checkQuestionDtos[0].QuestionId

            // Act
            var result = await _checker.CheckUserAnswers(checkQuestionDtos, userAnswers, userGrade, questionTypeGrades);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CheckUserAnswers_ShouldSetUserGradeToZero_WhenCalled()
        {
            // Arrange
            var checkQuestionDtos = _fixture.CreateMany<ICheckQuestionDto>(3).ToList();
            var userAnswers = _fixture.CreateMany<IUserAnswerDto>(3).ToList();
            var userGrade = new UserGradeDto();
            var questionTypeGrades = new List<QuestionTypeGrade>
            {
                new QuestionTypeGrade { QuestionTypeName = nameof(TestQuestion), Grade = 2 }
            };

            for (int i = 0; i < userAnswers.Count; i++)
            {
                userAnswers[i].QuestionId = checkQuestionDtos[i].QuestionId;
            }

            // Мокаем метод CheckAnswer
            var correctAnswerDtoMock = new Mock<ICorrectAnswerDto>();
            for (int i = 0; i < userAnswers.Count; i++)
            {
                _mockQueryGraphAnalyzer.Setup(x => x.CheckAnswer(userAnswers[i], checkQuestionDtos[i], userGrade))
                    .ReturnsAsync(correctAnswerDtoMock.Object);
            }

            // Act
            await _checker.CheckUserAnswers(checkQuestionDtos, userAnswers, userGrade, questionTypeGrades);

            // Assert
            Assert.Equal(0, userGrade.Grade);
        }
    }
}
