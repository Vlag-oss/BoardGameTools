using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{

    public class CardViewModel : ICardViewModel
    {
        private readonly HttpClient httpClient;
        private readonly ICharacteristicViewModel characteristicViewModel;

        public CardViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.characteristicViewModel = characteristicViewModel;
        }

        public async Task<List<CardModel>> GetCard()
        {
            var cards = await httpClient.GetFromJsonAsync<List<Card>>("api/card") ?? new();
            var characteristics = await characteristicViewModel.GetCharacteristic();

            return cards.Select(card =>
            {
                return CardModel.Transform(card, characteristics);
            }).ToList();
        }
    }
}
