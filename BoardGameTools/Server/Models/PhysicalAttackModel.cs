using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class PhysicalAttackModel
{
    public bool Result { get; }
    public List<Card> Cards { get; }

    public PhysicalAttackModel(bool result, List<Card> cards)
    {
        Result = result;
        Cards = cards;
    }
}