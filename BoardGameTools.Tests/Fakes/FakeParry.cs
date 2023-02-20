using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Tests.Fakes;

public class FakeParry : IParry
{
    public ParryModel Result { get; set; }
    public ParryModel ParryPhase(List<Card> cards, List<Monster> monsters)
    {
        return Result;
    }
}