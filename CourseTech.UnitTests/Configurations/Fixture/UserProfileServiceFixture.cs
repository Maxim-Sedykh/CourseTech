using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using MediatR;
using Moq;
using Serilog;
using StackExchange.Redis;

namespace CourseTech.Tests.Configurations.Fixture;

public class UserProfileServiceFixture : IDisposable
{
    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<IMediator> MediatorMock { get; }

    public Mock<IDatabase> RedisDatabaseMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public IUserProfileService UserProfileService { get; }

    public UserProfileServiceFixture()
    {
        CacheServiceMock = new Mock<ICacheService>();
        MediatorMock = new Mock<IMediator>();
        RedisDatabaseMock = new Mock<IDatabase>();
        LoggerMock = new Mock<ILogger>();

        UserProfileService = new UserProfileService(CacheServiceMock.Object,
            MediatorMock.Object);
    }

    public void Dispose() { }
}
