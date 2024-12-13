using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.UnitTests.Configurations.Fixture
{
    public class LessonRecordServiceFixture : IDisposable
    {
        public Mock<ICacheService> CacheServiceMock { get; }

        public Mock<IMediator> MediatorMock { get; }

        public LessonRecordService LessonRecordService { get; }

        public LessonRecordServiceFixture()
        {
            CacheServiceMock = new Mock<ICacheService>();
            MediatorMock = new Mock<IMediator>();

            LessonRecordService = new LessonRecordService(CacheServiceMock.Object, MediatorMock.Object);
        }

        public void Dispose() { }
    }
}
