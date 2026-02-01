using MediatR;

namespace BoardGameTools.Application.Password.Commands
{
    public record ResetPasswordCommand(string Token, string NewPassword, string ConfirmPassword) : IRequest<Unit>;

    public class ResetPasswordCommandHandler() : IRequestHandler<ResetPasswordCommand, Unit>
    {
        public Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}