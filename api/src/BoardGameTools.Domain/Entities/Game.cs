namespace BoardGameTools.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; } = null!;


        public string Name { get; private set; } = string.Empty;
        public string Source { get; private set; } = string.Empty;
        public string? SourceGameId { get; private set; }

        private Game() { }

        public Game(Guid ownerId, string name, string source, string? sourceGameId)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Name = SetName(name);
            Source = source;
            SourceGameId = sourceGameId;
        }

        public static Game CreateFromBgg(Guid ownerId, string name, int bggId)
        {
            if(bggId <= 0)
                throw new ArgumentOutOfRangeException(nameof(bggId), "L'identifiant BGG doit être un entier positif.");

            return new(ownerId, name, "BGG", bggId.ToString());
        }

        public static Game CreateManual(Guid ownerId, string name) => new(ownerId, name, "Manual", null);

        private static string SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Le nom du jeu de société est obligatoire.", nameof(name));

            return name.Trim();
        }
    }
}
