using BoardGameTools.Application.Common.Interfaces;

namespace BoardGameTools.Infrastructure.Services
{
    public class EmailTemplate : IEmailTemplate
    {
        public string BuildEmailConfirmation(string confirmationLink)
        {
            return $"""
            <html>
                <body>
                    <h2>Bienvenue sur BoardGameTools !</h2>
                    <p>Merci de vous être inscrit. Veuillez cliquer sur le lien ci-dessous pour confirmer votre adresse e-mail :</p>
                    <a href="{confirmationLink}" style="display:inline-block;padding:10px 20px;background-color:#007BFF;color:white;text-decoration:none;border-radius:5px;">Confirmer mon compte</a>
                    <p>Si vous n'avez pas demandé cet e-mail, ignorez-le.</p>
                </body>
            </html>
            """;
        }
    }
}
