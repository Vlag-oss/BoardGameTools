namespace BoardGameTools.Application.Services.Passwords
{
    public static class PasswordPolicy
    {
        const int MinimumLength = 8;

        public static string[] Validate(string password)
        { 
            var errors = new List<string>();
            if(password.Length < MinimumLength)
                errors.Add($"Le mot de passe doit contenir au moins {MinimumLength} caractères.");
            if(!HasUppercase(password))
                errors.Add("Le mot de passe doit contenir au moins une lettre majuscule.");
            if(!HasLowercase(password))
                errors.Add("Le mot de passe doit contenir au moins une lettre minuscule.");
            if(!HasDigit(password))
                errors.Add("Le mot de passe doit contenir au moins un chiffre.");
            if(!HasSpecialCharacter(password))
                errors.Add("Le mot de passe doit contenir au moins un caractère spécial.");

            return [.. errors];
        }

        private static bool HasUppercase(string password) => password.Any(char.IsUpper);
        private static bool HasLowercase(string password) => password.Any(char.IsLower);
        private static bool HasDigit(string password) => password.Any(char.IsDigit);
        private static bool HasSpecialCharacter(string password)
        {
            var specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~";
            return password.Any(specialChars.Contains);
        }
    }
}
