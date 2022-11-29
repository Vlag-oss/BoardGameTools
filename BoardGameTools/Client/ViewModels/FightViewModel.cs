using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{

    public class FightViewModel : IFightViewModel
    {
        private readonly HttpClient httpClient;

        public FightViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async void Fight(List<CardModel> cards, List<Monster> monster)
        {
            var transformedCard = CardModel.Transform(cards);
            var response = await httpClient.PostAsJsonAsync("api/fight/TryToFight", new SelectedCards(transformedCard, monster));
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
