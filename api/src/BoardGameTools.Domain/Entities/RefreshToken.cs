namespace BoardGameTools.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string TokenHash { get; private set; } = string.Empty;
        public DateTime Expires { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool IsExpired(DateTime utcNow) => utcNow >= Expires;
        public DateTime Created { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        private RefreshToken() { }

        public RefreshToken(string tokenHash, DateTime expires, User user)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            Expires = expires;
            Created = DateTime.UtcNow;
            User = user;
        }

        public static RefreshToken Create(string tokenHash, DateTime expires, User user)
        {
            if (string.IsNullOrEmpty(tokenHash))
                throw new ArgumentException("Le jeton de rafraîchissement doit contenir une valeur", nameof(tokenHash));

            return new RefreshToken(tokenHash, expires, user);
        }

        public void Revoke(DateTime revokedAtUtc)
        {
            RevokedAt = revokedAtUtc;
        }
    }
}
