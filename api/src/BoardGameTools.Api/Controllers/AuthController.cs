using BoardGameTools.Api.DTOs.Login;
using BoardGameTools.Api.DTOs.Password;
using BoardGameTools.Api.DTOs.Register;
using BoardGameTools.Application.Auth.ConfirmEmail.Commands;
using BoardGameTools.Application.Auth.ForgotPassword;
using BoardGameTools.Application.Auth.Login.Commands;
using BoardGameTools.Application.Auth.Login.DTOs;
using BoardGameTools.Application.Auth.Register.Commands;
using BoardGameTools.Application.Auth.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest login, CancellationToken ct)
        {
            var command = new LoginCommand(login.Email, login.Password);
            var result = await _sender.Send(command, ct);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
        {
            var command = new RegisterCommand(request.Email, request.Password, request.ConfirmPassword);
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email, CancellationToken ct)
        {
            var command = new ForgotPasswordCommand(email);
            await _sender.Send(command, ct);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct)
        {
            var command = new ResetPasswordCommand(request.Token, request.NewPassword, request.ConfirmPassword);
            await _sender.Send(command, ct);
            return Ok();
        }
    }
}
