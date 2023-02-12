using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class RangedAttack : IRangedAttack
{
    public bool RangedAttackPhase(List<Card> cards, IReadOnlyCollection<Monster> monster)
    {
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
                if (valueOfRangedAttack >= monster.First().Armor)
                {
                    return true;
                }
            }
        }

        return false;
    }
}