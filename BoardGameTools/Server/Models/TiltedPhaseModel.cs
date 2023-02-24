using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class TiltedPhaseModel
{
    public bool Result { get; }
    public List<Card> CardsUsed { get; }

    public TiltedPhaseModel(bool result, List<Card> cardsUsed)
    {
        Result = result;
        CardsUsed = cardsUsed;
    }
}