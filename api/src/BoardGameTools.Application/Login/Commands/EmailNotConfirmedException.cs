using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Login.Commands
{
    public sealed class EmailNotConfirmedException : BusinessException
    {
        public EmailNotConfirmedException() : base("L'email n'a pas encore été confirmé.") { }
    }
}
