using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Auth.ResetPassword
{
    public class ResetPasswordTokenException : BusinessException
    {
        public ResetPasswordTokenException() : base("Le jeton de réinitialisation est invalide.")
        {
            
        }
    }
}
