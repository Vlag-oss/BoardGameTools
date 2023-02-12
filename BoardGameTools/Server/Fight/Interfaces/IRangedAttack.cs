using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface IRangedAttack
{
    bool RangedAttackPhase(List<Card> rangedAttackCards, IReadOnlyCollection<Monster> monster);
}