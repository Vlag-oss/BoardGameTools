using BoardGameTools.Client.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface IHeroesViewModel
    {
        public List<CardModel> SelectedCards { get; set; }
        public string WarningMessage { get; set; }
        public Task<List<CardModel>> GetCard();
        public void AddCard(CardModel card);
        public void RemoveCard(CardModel card);
    }
}
