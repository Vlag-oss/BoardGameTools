namespace BoardGameTools.Shared.Models;

public class Card : IEquatable<Card>
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Characteristic { get; set; } = null!;

    public string Value { get; set; } = null!;

    public bool Equals(Card? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name && Characteristic == other.Characteristic && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Card)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Characteristic, Value);
    }
}
