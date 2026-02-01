using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BoardGameTools.Application.Services.Tokens
{
    public class TokenService(IOptions<TokenOptions> tokenOptions, IDateTimeProvider dateTimeProvider) : ITokenService
    {
        private readonly TokenOptions _options = tokenOptions.Value;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

        public string CreateAccessToken(User user)
        {
            var now = _dateTimeProvider.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixTimeSeconds(now).ToString(), ClaimValueTypes.Integer64),

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.Value),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_options.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static long ToUnixTimeSeconds(DateTime utc)
            => new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc)).ToUnixTimeSeconds();

        public RefreshToken CreateRefreshToken(User user)
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            var token = Convert.ToBase64String(bytes);

            return RefreshToken.Create(HashRefreshToken(token), _dateTimeProvider.UtcNow.AddDays(_options.RefreshTokenExpirationDays), user.Id);
        }

        public PasswordResetToken CreateResetPasswordToken(User user)
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            var token = Convert.ToBase64String(bytes);

            return PasswordResetToken.Create(HashRefreshToken(token), _dateTimeProvider.UtcNow.AddMinutes(_options.ResetPasswordTokenExpirationMinutes), user.Id);
        }

        private string HashRefreshToken(string token)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.RefreshKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }

        public bool VerifyRefreshToken(string token, string tokenHash)
        { 
            var computedHash = HashRefreshToken(token);

            var computed = Convert.FromBase64String(computedHash);
            var stored = Convert.FromBase64String(tokenHash);

            return CryptographicOperations.FixedTimeEquals(computed, stored);
        }
    }
}
