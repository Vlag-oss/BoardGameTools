using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class PhysicalAttackModel
{
    public bool Result { get; }
    public List<Card> CardsUsed { get; }
    public int TotalAttack { get; }

    public PhysicalAttackModel(bool result, List<Card> cardsUsed, int totalAttack)
    {
        Result = result;
        CardsUsed = cardsUsed;
        TotalAttack = totalAttack;
    }
}