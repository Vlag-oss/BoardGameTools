namespace BoardGameTools.Api.DTOs.Register
{
    public record RegisterRequest(string Email, string Password, string ConfirmPassword);
}
