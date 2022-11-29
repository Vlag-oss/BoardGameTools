using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class CalculatedModel
{
    public bool Result { get; }
    public int TotalValue { get; }
    public List<Card> CardsUsed { get; }

    public CalculatedModel(bool result, int totalValue, List<Card> cardsUsed)
    {
        Result = result;
        TotalValue = totalValue;
        CardsUsed = cardsUsed;
    }
}