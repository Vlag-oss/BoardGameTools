using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class RangedAttackModel
{
    public bool Result { get; }
    public List<Card> CardsUsed { get; }

    public RangedAttackModel(bool result, List<Card> cardsUsed)
    {
        Result = result;
        CardsUsed = cardsUsed;
    }
}