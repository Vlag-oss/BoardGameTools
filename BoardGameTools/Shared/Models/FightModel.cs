namespace BoardGameTools.Shared.Models;

public class FightModel
{
    public bool Result { get; }
    public int Wound { get; }
    public List<Card> CardsUsed { get; }

    public FightModel(bool result, int wound, List<Card> cardsUsed)
    {
        Result = result;
        Wound = wound;
        CardsUsed = cardsUsed;
    }
}