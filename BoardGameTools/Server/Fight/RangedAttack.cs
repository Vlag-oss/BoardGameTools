using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class RangedAttack : IRangedAttack
{
    public RangedAttackModel RangedAttackPhase(List<Card> cards, IReadOnlyCollection<Monster> monster)
    {
        var cardsUsed = new List<Card>();
        var rangedAttackCards = cards
            .Where(c => c.Characteristic == ((int)CharacteristicEnum.RangedAttack).ToString())
            .ToList();
        if (rangedAttackCards.Any())
        {
            var valueOfRangedAttack = 0;

            foreach (var rangedAttackCard in rangedAttackCards)
            {
                int.TryParse(rangedAttackCard.Value, out var value);
                valueOfRangedAttack += value;
                cardsUsed.Add(rangedAttackCard);
                if (valueOfRangedAttack >= monster.First().Armor)
                {
                    return new RangedAttackModel(true, cardsUsed);
                }
            }
        }

        return new RangedAttackModel(false, new List<Card>());
    }
}