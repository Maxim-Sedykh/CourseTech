using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using MediatR;
using Moq;

namespace CourseTech.UnitTests.Configurations.Fixture
{
    public class LessonRecordServiceFixture : IDisposable
    {
        public Mock<ICacheService> CacheServiceMock { get; }

        public Mock<IMediator> MediatorMock { get; }

        public ILessonRecordService LessonRecordService { get; }

        public LessonRecordServiceFixture()
        {
            CacheServiceMock = new Mock<ICacheService>();
            MediatorMock = new Mock<IMediator>();

            LessonRecordService = new LessonRecordService(CacheServiceMock.Object, MediatorMock.Object);
        }

        public void Dispose() { }
    }
}
