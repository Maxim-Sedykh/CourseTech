using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using MediatR;
using Moq;
using Serilog;

namespace CourseTech.UnitTests.Configurations.Fixture
{
    public class ReviewServiceFixture : IDisposable
    {
        public Mock<ICacheService> CacheServiceMock { get; }
        public Mock<IMediator> MediatorMock { get; }
        public Mock<IUnitOfWork> UnitOfWorkMock { get; }
        public Mock<ILogger> LoggerMock { get; }

        public ReviewService ReviewService { get; }

        public ReviewServiceFixture()
        {
            CacheServiceMock = new Mock<ICacheService>();
            MediatorMock = new Mock<IMediator>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            LoggerMock = new Mock<ILogger>();

            ReviewService = new ReviewService(CacheServiceMock.Object, MediatorMock.Object, UnitOfWorkMock.Object, LoggerMock.Object);
        }

        public void Dispose() { }
    }
}
