namespace BoardGameTools.Shared.Models;

public partial class Monster
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long Attack { get; set; }

    public long Armor { get; set; }

    public long? DefensiveAbilities { get; set; }

    public long? OffensiveAbilities { get; set; }
}
