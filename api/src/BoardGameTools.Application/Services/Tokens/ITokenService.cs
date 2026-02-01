using BoardGameTools.Domain.Entities;

namespace BoardGameTools.Application.Services.Tokens
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        RefreshToken CreateRefreshToken(User user);
        PasswordResetToken CreateResetPasswordToken(User user);
        bool VerifyRefreshToken(string token, string tokenHash);
    }
}
