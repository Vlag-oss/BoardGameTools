using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{

    public class CardViewModel : ICardViewModel
    {
        public List<Card> Cards { get; set; } = new();
        private readonly HttpClient httpClient;

        public CardViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Card>> GetCard()
        {
            var cards = await httpClient.GetFromJsonAsync<List<Card>>("api/card") ?? new();
            return cards;
        }
    }
}
