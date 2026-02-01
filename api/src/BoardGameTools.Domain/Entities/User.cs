using BoardGameTools.Domain.ValueObjects;

namespace BoardGameTools.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsLocked { get; private set; } = false;
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LastFailedLogin { get; private set; }
        public string? EmailConfirmationToken { get; private set; }
        public bool EmailConfirmed { get; private set; } = false;

        public IReadOnlyCollection<Game> LibraryGames { get; private set; } = [];
        public RefreshToken? RefreshToken { get; private set; }

        private User() { }

        public User(Email email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            FailedLoginAttempts = 0;
        }

        public static User Create(Email email, string passwordHash)
        {
            if(string.IsNullOrEmpty(passwordHash))
                throw new ArgumentException("Le mot de passe doit contenir une valeur", nameof(passwordHash));

            return new(email, passwordHash);
        }

        public void RequireEmailConfirmation(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token invalide", nameof(token));

            EmailConfirmationToken = token;
            EmailConfirmed = false;
        }

        public void ConfirmEmail()
        {
            EmailConfirmationToken = null;
            EmailConfirmed = true;
        }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public void RegisterFailedLoginAttempt(DateTime failedAtUtc)
        {
            FailedLoginAttempts++;
            LastFailedLogin = failedAtUtc;
        }

        public void ResetFailedLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LastFailedLogin = null;
        }

        public void ResetPassword(string newHashedPassword)
        {
            if(string.IsNullOrEmpty(newHashedPassword))
                throw new ArgumentException("Le mot de passe doit contenir une valeur", nameof(newHashedPassword));

            PasswordHash = newHashedPassword;
        }
    }
}
