using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class Parry : IParry
{
    private readonly ICalculateFight _calculateFight;

    public Parry(ICalculateFight calculateFight)
    {
        _calculateFight = calculateFight ?? throw new ArgumentNullException(nameof(calculateFight));
    }

    public ParryModel ParryPhase(List<Card> cards, List<Monster> monsters)
    {
        var result = false;
        var totalParry = 0;
        var cardsUsed = new List<Card>();
        var monsterAttack = (int)monsters.First().Attack;

        var cardsOnlyParry = cards
            .Where(card => card.Characteristic == ((int)CharacteristicEnum.Parry).ToString())
            .ToList();

        if (cardsOnlyParry.Any())
        {
            var checkTotalParry = _calculateFight.CalculatedFight(monsterAttack, new List<Card>(cardsOnlyParry), totalParry, CharacteristicEnum.Parry);
            result = checkTotalParry.Result;
            totalParry = checkTotalParry.TotalValue;
            cardsUsed.AddRange(checkTotalParry.CardsUsed);
        }

        if (!result)
        {
            var cardsWithParry = cards
                .Where(CardContainParryCharac)
                .Where(CardDoesntContainAttackCharac)
                .Except(cardsOnlyParry)
                .ToList();

            if (cardsWithParry.Any())
            {
                var checkTotalParry = _calculateFight.CalculatedFight(monsterAttack, new List<Card>(cardsWithParry), totalParry, CharacteristicEnum.Parry);
                result = checkTotalParry.Result;
                totalParry = checkTotalParry.TotalValue;
                cardsUsed.AddRange(checkTotalParry.CardsUsed);
            }

            if (!result)
            {
                var cardsWithParryAndAttack = cards
                    .Where(CardContainParryCharac)
                    .Except(cardsOnlyParry)
                    .Except(cardsWithParry)
                    .ToList();

                if (cardsWithParryAndAttack.Any())
                {
                    var checkTotalParry = _calculateFight.CalculatedFight(monsterAttack, new List<Card>(cardsWithParryAndAttack), totalParry, CharacteristicEnum.Parry);
                    result = checkTotalParry.Result;
                    totalParry = checkTotalParry.TotalValue;
                    cardsUsed.AddRange(checkTotalParry.CardsUsed);
                }
            }
        }
        
        return new ParryModel(result, cardsUsed, totalParry);

        bool CardContainParryCharac(Card card) => card.Characteristic.Contains(((int)CharacteristicEnum.Parry).ToString());
        bool CardDoesntContainAttackCharac(Card card) => !card.Characteristic.Contains(((int)CharacteristicEnum.Attack).ToString());
    }
}