using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface IRangedAttack
{
    RangedAttackModel RangedAttackPhase(List<Card> rangedAttackCards, IReadOnlyCollection<Monster> monster);
}