using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface IPhysicalAttack
{
    PhysicalAttackModel AttackPhase(List<Card> cards, List<Monster> monsters);
}