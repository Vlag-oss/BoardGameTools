using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Tests.Fakes;

public class FakeRangedAttack : IRangedAttack
{
    public RangedAttackModel Result { get; set; }

    public RangedAttackModel RangedAttackPhase(List<Card> rangedAttackCards, IReadOnlyCollection<Monster> monster)
    {
        return Result;
    }
}