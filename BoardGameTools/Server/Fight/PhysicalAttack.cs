using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class PhysicalAttack : IPhysicalAttack
{
    private readonly ICalculateFight _calculateFight;

    public PhysicalAttack(ICalculateFight calculateFight)
    {
        _calculateFight = calculateFight ?? throw new ArgumentNullException(nameof(calculateFight));
    }

    public PhysicalAttackModel AttackPhase(List<Card> cards, List<Monster> monsters)
    {
        var result = false;
        var totalAttack = 0;
        var cardsUsed = new List<Card>();
        var monsterArmor = (int)monsters.First().Armor;

        var attackCards = cards
            .Where(card => card.Characteristic == ((int)CharacteristicEnum.Attack).ToString())
            .ToList();

        if (attackCards.Any())
        {
            var checkTotalAttack = _calculateFight.CalculatedFight(monsterArmor, new List<Card>(attackCards), totalAttack, CharacteristicEnum.Attack);
            result = checkTotalAttack.Result;
            totalAttack = checkTotalAttack.TotalValue;
            cardsUsed.AddRange(checkTotalAttack.CardsUsed);
        }

        if (!result)
        {
            var cardsWithAttack = cards
                .Except(attackCards)
                .Where(card => card.Characteristic.Contains(((int)CharacteristicEnum.Attack).ToString()))
                .ToList();

            if (cardsWithAttack.Any())
            {
                var checkTotalAttack = _calculateFight.CalculatedFight(monsterArmor, new List<Card>(cardsWithAttack), totalAttack, CharacteristicEnum.Attack);
                result = checkTotalAttack.Result;
                totalAttack = checkTotalAttack.TotalValue;
                cardsUsed.AddRange(checkTotalAttack.CardsUsed);
            }

            if (!result)
            {
                var cardsWithRangedAttack = cards
                        .Except(attackCards)
                        .Except(cardsWithAttack)
                        .Where(card => card.Characteristic.Contains(((int)CharacteristicEnum.RangedAttack).ToString()))
                        .ToList();

                if (cardsWithRangedAttack.Any())
                {
                    var checkTotalAttack = _calculateFight.CalculatedFight(monsterArmor, new List<Card>(cardsWithRangedAttack), totalAttack, CharacteristicEnum.RangedAttack);
                    result = checkTotalAttack.Result;
                    cardsUsed.AddRange(checkTotalAttack.CardsUsed);
                }
            }
        }

        return new PhysicalAttackModel(result, result ? cardsUsed : new List<Card>());
    }
}