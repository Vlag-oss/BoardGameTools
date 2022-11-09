using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels
{
    public interface ICardViewModel
    {
        public Task<List<Card>> GetCard();
    }
}
