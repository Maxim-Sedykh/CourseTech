using AutoMapper;
using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;

namespace CourseTech.Tests.Configurations.Fixture;

public class CourseResultServiceFixture : IDisposable
{
    public Mock<ICacheService> CacheServiceMock { get; }
    public Mock<IMediator> MediatorMock { get; }
    public Mock<IMapper> MapperMock { get; }
    public Mock<ICourseResultValidator> CourseResultValidatorMock { get; }

    public ICourseResultService CourseResultService { get; }

    public CourseResultServiceFixture()
    {
        CacheServiceMock = new Mock<ICacheService>();
        MediatorMock = new Mock<IMediator>();
        MapperMock = new Mock<IMapper>();
        CourseResultValidatorMock = new Mock<ICourseResultValidator>();

        CourseResultService = new CourseResultService(CacheServiceMock.Object, MediatorMock.Object, MapperMock.Object, CourseResultValidatorMock.Object);
    }

    public void Dispose() { }
}
