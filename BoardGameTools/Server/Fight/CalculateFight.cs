using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class CalculateFight : ICalculateFight
{
    public CalculatedModel CalculatedFight(int monsterAttribute, ICollection<Card> cards, int totalValue, CharacteristicEnum @enum)
    {
        var cardsUsed = new List<Card>();
        return Calculated(monsterAttribute, cards, cardsUsed, totalValue, @enum);
    }

    public CalculatedModel Calculated(int monsterAttribute, ICollection<Card> cards, List<Card> cardsUsed, int totalValue, CharacteristicEnum @enum)
    {
        var remainder = monsterAttribute - totalValue;

        var cardClosestTo = cards.Aggregate((card, nextCard) =>
        {
            var valueCard = RecoverValueCard(card, @enum);
            var valueNextCard = RecoverValueCard(nextCard, @enum);

            var remainderValueCard = valueCard - remainder;
            var remainderValueNextCard = valueNextCard - remainder;

            if (Math.Abs(remainderValueCard) == Math.Abs(remainderValueNextCard))
            {
                if (remainderValueCard < 0)
                    return nextCard;
                if (remainderValueNextCard < 0)
                    return card;
            }

            return Math.Abs(remainderValueCard) < Math.Abs(remainderValueNextCard)
                ? card
                : nextCard;
        });

        totalValue += RecoverValueCard(cardClosestTo, @enum);
        cardsUsed.Add(cardClosestTo);

        if (totalValue >= monsterAttribute)
        {
            return new CalculatedModel(true, totalValue, cardsUsed);
        }
        cards.Remove(cardClosestTo);

        return !cards.Any()
            ? new CalculatedModel(false, totalValue, cardsUsed)
            : Calculated(monsterAttribute, cards, cardsUsed, totalValue, @enum);
    }

    private static int RecoverValueCard(Card card, CharacteristicEnum @enum)
    {
        var value = card.Value;
        if (value.Contains(";"))
        {
            var indexOf = card.Characteristic.IndexOf(((int)@enum).ToString(), StringComparison.Ordinal);
            value = card.Value.Substring(indexOf, 1);
        }

        return int.Parse(value);
    }
}