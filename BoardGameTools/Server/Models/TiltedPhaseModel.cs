using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class TiltedPhaseModel
{
    public bool Result { get; }
    public List<Card> UsedCards { get; }

    public TiltedPhaseModel(bool result, List<Card> usedCards)
    {
        Result = result;
        UsedCards = usedCards;
    }
}