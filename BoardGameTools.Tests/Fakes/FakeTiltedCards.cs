using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Tests.Fakes;

public class FakeTiltedCards : ITiltedCards
{
    public TiltedPhaseModel TiltedPhase(List<Card> cards, ParryModel parryModel, PhysicalAttackModel physicalAttackModel, Monster monster)
    {
        throw new NotImplementedException();
    }
}