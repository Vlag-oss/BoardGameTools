using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Auth.Login.Commands
{
    public sealed class AccountLockedException : BusinessException
    {
        public AccountLockedException() : base("Votre compte est verrouillé, réessayez plus tard.")
        {
        }
    }
}
