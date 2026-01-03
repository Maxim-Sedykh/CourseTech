using AutoMapper;
using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Moq;
using Serilog;

namespace CourseTech.Tests.Configurations.Fixture;

public class RoleServiceFixture : IDisposable
{
    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<IMediator> MediatorMock { get; }

    public Mock<ITransactionManager> UnitOfWorkMock { get; }

    public Mock<IMapper> MapperMock { get; }

    public Mock<IRoleValidator> RoleValidatorMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public IRoleService RoleService { get; }

    public RoleServiceFixture()
    {
        CacheServiceMock = new Mock<ICacheService>();
        MediatorMock = new Mock<IMediator>();
        UnitOfWorkMock = new Mock<ITransactionManager>();
        MapperMock = new Mock<IMapper>();
        RoleValidatorMock = new Mock<IRoleValidator>();
        LoggerMock = new Mock<ILogger>();

        RoleService = new RoleService(CacheServiceMock.Object,
            MediatorMock.Object,
            UnitOfWorkMock.Object,
            MapperMock.Object,
            RoleValidatorMock.Object,
            LoggerMock.Object);
    }

    public void Dispose() { }
}
