using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels
{
    public interface IFightViewModel
    {
        void Fight(List<CardModel> cards, List<Monster> monster);
    }
}
