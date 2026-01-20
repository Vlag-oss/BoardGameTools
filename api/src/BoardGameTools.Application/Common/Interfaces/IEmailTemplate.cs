namespace BoardGameTools.Application.Common.Interfaces
{
    public interface IEmailTemplate
    {
        string BuildEmailConfirmation(string confirmationLink);
    }
}
