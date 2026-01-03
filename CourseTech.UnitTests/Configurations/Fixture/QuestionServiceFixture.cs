using CourseTech.Application.Services.Question;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services.Question;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;
using Serilog;

namespace CourseTech.Tests.Configurations.Fixture;

public class QuestionServiceFixture : IDisposable
{
    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<IMediator> MediatorMock { get; }

    public Mock<ITransactionManager> UnitOfWorkMock { get; }
    public Mock<IBaseRepository<LessonRecord>> LessonRecordRepositoryMock { get; }

    public Mock<IQuestionAnswerChecker> QuestionAnswerCheckerMock { get; }

    public Mock<IQuestionValidator> QuestionValidatorMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public IQuestionService QuestionService { get; }

    public QuestionServiceFixture()
    {
        CacheServiceMock = new Mock<ICacheService>();
        MediatorMock = new Mock<IMediator>();
        UnitOfWorkMock = new Mock<ITransactionManager>();
        QuestionAnswerCheckerMock = new Mock<IQuestionAnswerChecker>();
        QuestionValidatorMock = new Mock<IQuestionValidator>();
        LoggerMock = new Mock<ILogger>();

        QuestionService = new QuestionService(CacheServiceMock.Object,
            MediatorMock.Object,
            UnitOfWorkMock.Object,
            QuestionAnswerCheckerMock.Object,
            QuestionValidatorMock.Object,
            LessonRecordRepositoryMock.Object,
            LoggerMock.Object);
    }

    public void Dispose() { }
}
