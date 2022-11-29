using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;
using BoardGameTools.Client.ViewModels.Interfaces;

namespace BoardGameTools.Client.ViewModels
{

    public class FightViewModel : IFightViewModel
    {
        private readonly HttpClient httpClient;
        private readonly ICharacteristicViewModel characteristicViewModel;

        public FightViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.characteristicViewModel = characteristicViewModel;
        }

        public async void Fight(List<CardModel> cards, List<Monster> monster)
        {
            var characteristics = await characteristicViewModel.GetCharacteristic();
            var response = await httpClient.PostAsJsonAsync("api/fight/TryToFight", new SelectedCards(CardModel.Transform(cards, characteristics), monster));
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
