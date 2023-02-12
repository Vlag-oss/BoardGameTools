using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface IParry
{
    ParryModel ParryPhase(List<Card> cards, List<Monster> monsters);
}