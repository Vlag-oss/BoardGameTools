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

        public string BuildEmailForgotPassword(string resetLink)
        {
            return $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333;'>
                        <h2>Réinitialisation du mot de passe</h2>
                        <p>Vous avez demandé à réinitialiser votre mot de passe.</p>
                        <p>Cliquez sur le lien ci-dessous pour créer un nouveau mot de passe :</p>
                        <p>
                            <a href='{resetLink}' style='display:inline-block;padding:10px 15px;background-color:#1976D2;color:white;text-decoration:none;border-radius:5px;'>
                                Réinitialiser le mot de passe
                            </a>
                        </p>
                        <p>Ce lien expirera dans 30 minutes.</p>
                        <p>Si vous n'avez pas fait cette demande, ignorez ce message.</p>
                    </body>
                </html>
            ";
        }
    }
}
