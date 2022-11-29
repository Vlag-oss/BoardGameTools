using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface IFightViewModel
    {
        void Fight(List<CardModel> cards, List<Monster> monster);
        public bool ResultFight { get; set; }
    }
}
