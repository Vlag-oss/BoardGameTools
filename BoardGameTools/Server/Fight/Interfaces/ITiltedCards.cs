using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface ITiltedCards
{
    TiltedPhaseModel TiltedPhase(List<Card> cards, ParryModel parryModel, PhysicalAttackModel physicalAttackModel, Monster monster);
}