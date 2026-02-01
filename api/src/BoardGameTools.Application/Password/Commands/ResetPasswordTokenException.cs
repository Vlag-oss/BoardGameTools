using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Password.Commands
{
    public class ResetPasswordTokenException : BusinessException
    {
        public ResetPasswordTokenException() : base("Le jeton de réinitialisation est invalide.")
        {
            
        }
    }
}
