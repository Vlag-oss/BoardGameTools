namespace BoardGameTools.Client.ViewModels
{
    public interface ICardViewModel
    {
        public Task<List<CardModel>> GetCard();
    }
}
