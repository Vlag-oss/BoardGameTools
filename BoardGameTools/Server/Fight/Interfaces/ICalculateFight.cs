using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Fight.Interfaces;

public interface ICalculateFight
{
    CalculatedModel CalculatedFight(int monsterAttributes, ICollection<Card> cards, int totalValue, CharacteristicEnum @enum);
}