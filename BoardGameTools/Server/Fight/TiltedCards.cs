using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight;

public class TiltedCards : ITiltedCards
{
    public TiltedPhaseModel TiltedPhase(List<Card> cards, ParryModel parryModel, PhysicalAttackModel physicalAttackModel, Monster monster)
    {
        var remainingCards = cards
            .Except(parryModel.CardsUsed)
            .Except(physicalAttackModel.CardsUsed)
            .ToList();

        if (remainingCards.Any())
        {
            switch (parryModel.Result)
            {
                case false when physicalAttackModel.Result:
                    {
                        var result = CalculatedTiltedCard(remainingCards, parryModel.TotalParry, monster.Attack);
                        return new TiltedPhaseModel(result.Result, result.UsedCards);
                    }
                case true when !physicalAttackModel.Result:
                    {
                        var result = CalculatedTiltedCard(remainingCards, physicalAttackModel.TotalAttack, monster.Armor);
                        return new TiltedPhaseModel(result.Result, result.UsedCards);
                    }
                case false when !physicalAttackModel.Result:
                    {
                        var resultAttack = CalculatedTiltedCard(remainingCards, physicalAttackModel.TotalAttack, monster.Armor);
                        var resultParry = CalculatedTiltedCard(remainingCards.Except(resultAttack.UsedCards).ToList(), parryModel.TotalParry, monster.Attack);

                        var usedCards = new List<Card>();
                        usedCards.AddRange(resultAttack.UsedCards);
                        usedCards.AddRange(resultParry.UsedCards);

                        return new TiltedPhaseModel(resultAttack.Result && resultParry.Result, usedCards);
                    }
            }
        }

        return new TiltedPhaseModel(false, new List<Card>());
    }

    private static TiltedPhaseModel CalculatedTiltedCard(List<Card> remainingCards, int cardAttribute, long monsterAttribute)
    {
        var usedCards = new List<Card>();
        var totalTiltedCards = cardAttribute;
        foreach (var card in remainingCards)
        {
            totalTiltedCards++;
            usedCards.Add(card);
            if (totalTiltedCards >= monsterAttribute)
                return new TiltedPhaseModel(true, usedCards);
        }

        return new TiltedPhaseModel(false, usedCards);
    }
}