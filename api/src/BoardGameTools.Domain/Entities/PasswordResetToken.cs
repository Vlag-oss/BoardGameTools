namespace BoardGameTools.Domain.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; private set; }
        public string TokenHash { get; private set; } = string.Empty;
        public DateTime Expires { get; private set; }
        public bool IsUsed { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        private PasswordResetToken() { }

        public PasswordResetToken(string tokenHash, DateTime expires, Guid userId)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            Expires = expires;
            UserId = userId;
        }

        public static PasswordResetToken Create(string tokenHash, DateTime expires, Guid userId)
        {
            if (string.IsNullOrEmpty(tokenHash))
                throw new ArgumentException("Le jeton de reset doit contenir une valeur", nameof(tokenHash));

            return new PasswordResetToken(tokenHash, expires, userId);
        }

        public void MarkAsUsed()
        {
            if(IsUsed)
                throw new InvalidOperationException("Le jeton de reset a déjà été utilisé.");

            IsUsed = true;
        }
    }
}
