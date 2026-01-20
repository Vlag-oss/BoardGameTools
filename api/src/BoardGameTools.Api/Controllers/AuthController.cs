using BoardGameTools.Api.DTOs.Register;
using BoardGameTools.Application.Users.Commands.AddUser;
using BoardGameTools.Application.Users.Commands.ConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Api.Endpoints
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
        {
            var command = new AddUserCommand(request.Email, request.Password, request.ConfirmPassword);
            var userId = await _sender.Send(command, ct);

            return Ok(new RegisterResponse(userId));
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request, CancellationToken ct)
        {
            var command = new ConfirmEmailCommand(request.Token);
            await _sender.Send(command, ct);
            return NoContent();
        }
    }
}
