using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;
using Serilog;

namespace CourseTech.UnitTests.Configurations
{
    public class LessonServiceFixture : IDisposable
    {
        public Mock<ICacheService> CacheServiceMock { get; }
        public Mock<IMediator> MediatorMock { get; }
        public Mock<ILogger> LoggerMock { get; }
        public Mock<ILessonValidator> LessonValidatorMock { get; }

        public LessonService LessonService { get; }

        public LessonServiceFixture()
        {
            CacheServiceMock = new Mock<ICacheService>();
            MediatorMock = new Mock<IMediator>();
            LoggerMock = new Mock<ILogger>();
            LessonValidatorMock = new Mock<ILessonValidator>();

            LessonService = new LessonService(CacheServiceMock.Object, MediatorMock.Object, LoggerMock.Object, LessonValidatorMock.Object);
        }

        public void Dispose() { }
    }
}
