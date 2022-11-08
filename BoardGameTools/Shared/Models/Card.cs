namespace BoardGameTools.Shared.Models;

public partial class Card
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Characteristic { get; set; } = null!;

    public string Value { get; set; } = null!;
}
