using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;
using BoardGameTools.Client.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BoardGameTools.Client.ViewModels
{

    public class FightViewModel : IFightViewModel
    {
        private readonly HttpClient httpClient;
        private readonly ICharacteristicViewModel characteristicViewModel;
        private readonly NavigationManager _navigationManager;

        public bool ResultFight { get; set; }

        public FightViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel, NavigationManager navigationManager)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.characteristicViewModel = characteristicViewModel;
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        }

        public async void Fight(List<CardModel> cards, List<Monster> monster)
        {
            var characteristics = await characteristicViewModel.GetCharacteristic();
            var response = await httpClient.PostAsJsonAsync("api/fight/TryToFight", new SelectedCards(CardModel.Transform(cards, characteristics), monster));
            var result = await response.Content.ReadFromJsonAsync<FightModel>();

            ResultFight = result.Result;
        }
    }
}
