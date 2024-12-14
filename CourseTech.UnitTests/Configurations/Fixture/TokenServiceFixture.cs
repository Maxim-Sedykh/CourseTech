using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;

namespace CourseTech.UnitTests.Configurations.Fixture
{
    public class TokenServiceFixture : IDisposable
    {
        public Mock<IMediator> MediatorMock { get; }

        public JwtSettings JwtSettings { get; }

        public ITokenService TokenService { get; }

        public TokenServiceFixture()
        {
            MediatorMock = new Mock<IMediator>();

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

            TokenService = new TokenService(
                MediatorMock.Object,
                jwtOptions);
        }

        public void Dispose() { }
    }
}
