using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Services.Tokens;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace BoardGameTools.UnitTests.Token
{
    public class TokenServiceTests
    {
        private readonly Mock<IDateTimeProvider> _dateTimeMock;
        private readonly IOptions<TokenOptions> _tokenOptions;

        public TokenServiceTests()
        {
            _dateTimeMock = new Mock<IDateTimeProvider>();
            _tokenOptions = Options.Create(new TokenOptions(
                "test_audience",
                "test_issuer",
                15,
                1,
                "super_secret_signing_key_123456789_123"
            ));
        }


        [Fact]
        public void CreateAccessToken_Should_CreateValidToken()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed-password");
            _dateTimeMock.Setup(d => d.UtcNow).Returns(new DateTime(2026, 01, 24));
            var service = new TokenService(_tokenOptions, _dateTimeMock.Object);

            //Act
            var token = service.CreateAccessToken(user);

            //Assert
            token.Should().NotBeNull();
        }

        [Fact]
        public void CreateAccessToken_ShouldThrow_WhenSigningKeyIsLessThan256()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed-password");
            _dateTimeMock.Setup(d => d.UtcNow).Returns(new DateTime(2026, 01, 24));

            var tokenOptions = Options.Create(new TokenOptions(
                "test_audience",
                "test_issuer",
                10,
                1,
                "short_key"
            ));

            var service = new TokenService(tokenOptions, _dateTimeMock.Object);

            //Act
            Action act = () =>  service.CreateAccessToken(user);

            //Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void CreateAccessToken_ShouldContain_Sub_And_Exp()
        {
            // Arrange
            var now = new DateTime(2026, 01, 01, 12, 0, 0, DateTimeKind.Utc);
            var user = User.Create(Email.Create("test@gmail.com"), "hashed");
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);
            var service = new TokenService(_tokenOptions, _dateTimeMock.Object);

            // Act
            var tokenString = service.CreateAccessToken(user);

            // Assert
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
            jwt.Subject.Should().Be(user.Id.ToString());
            jwt.ValidTo.Should().Be(now.AddMinutes(15));
        }
    }
}
