using AutoMapper;
using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;

namespace CourseTech.Tests.Configurations.Fixture;

public class AuthServiceFixture : IDisposable
{
    public Mock<IMediator> MediatorMock { get; }

    public Mock<IMapper> MapperMock { get; }

    public Mock<ITokenService> TokenServiceMock { get; }

    public Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public Mock<IAuthValidator> AuthValidatorMock { get; }

    public Mock<ICacheService> CacheServiceMock { get; }

    public Mock<ILogger> LoggerMock { get; }

    public JwtSettings JwtSettings { get; }

    public IAuthService AuthService { get; }


    public AuthServiceFixture()
    {
        MediatorMock = new Mock<IMediator>();
        MapperMock = new Mock<IMapper>();
        TokenServiceMock = new Mock<ITokenService>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        AuthValidatorMock = new Mock<IAuthValidator>();
        CacheServiceMock = new Mock<ICacheService>();
        LoggerMock = new Mock<ILogger>();

        JwtSettings = new JwtSettings
        {
            Issuer = "test",
            Audience = "test",
            Authority = "test",
            JwtKey = "test",
            Lifetime = "test",
            RefreshTokenValidityInDays = 11
        };

        var jwtOptions = Options.Create(JwtSettings);

        AuthService = new AuthService(
            MediatorMock.Object,
            MapperMock.Object,
            TokenServiceMock.Object,
            UnitOfWorkMock.Object,
            AuthValidatorMock.Object,
            CacheServiceMock.Object,
            LoggerMock.Object,
            jwtOptions);
    }

    public void Dispose() { }
}
