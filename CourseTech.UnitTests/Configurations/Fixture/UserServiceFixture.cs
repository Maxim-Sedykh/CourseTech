using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;
using Serilog;

namespace CourseTech.Tests.Configurations.Fixture;

public class UserServiceFixture : IDisposable
{
    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<IMediator> MediatorMock { get; }

    public Mock<ITransactionManager> UnitOfWorkMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public Mock<IUserValidator> UserValidatorMock { get; }

    public IUserService UserService { get; }

    public UserServiceFixture()
    {
        CacheServiceMock = new Mock<ICacheService>();
        MediatorMock = new Mock<IMediator>();
        UnitOfWorkMock = new Mock<ITransactionManager>();
        UserValidatorMock = new Mock<IUserValidator>();
        LoggerMock = new Mock<ILogger>();

        UserService = new UserService(CacheServiceMock.Object,
            MediatorMock.Object,
            UnitOfWorkMock.Object,
            LoggerMock.Object,
            UserValidatorMock.Object);
    }

    public void Dispose() { }
}
