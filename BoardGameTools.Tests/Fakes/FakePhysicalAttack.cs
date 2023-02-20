using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Tests.Fakes;

public class FakePhysicalAttack : IPhysicalAttack
{
    public PhysicalAttackModel Result { get; set; }
    public PhysicalAttackModel AttackPhase(List<Card> cards, List<Monster> monsters)
    {
        return Result;
    }
}