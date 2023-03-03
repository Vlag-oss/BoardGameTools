using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface IFightViewModel
    {
        public FightModel FightModel { get; set; }
        public Task<FightModel> Fight(List<CardModel> cards, List<MonsterModel> monster);
    }
}
