using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Login.Commands
{
    public sealed class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base("Email ou mot de passe incorrect.") { }
    }
}
