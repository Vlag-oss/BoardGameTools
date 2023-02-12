using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class ParryModel
{
    public bool Result { get; }
    public List<Card> Cards { get; }

    public ParryModel(bool result, List<Card> cards)
    {
        Result = result;
        Cards = cards;
    }
}