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
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var command = new AddUserCommand(request.Email, request.Password, request.ConfirmPassword, baseUrl);
            var userId = await _sender.Send(command, ct);

            return Ok(new RegisterResponse(userId));
        }

        [HttpGet("/confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, CancellationToken ct)
        {
            var command = new ConfirmEmailCommand(token);
            await _sender.Send(command, ct);
            return Ok();
        }
    }
}
