using BoardGameTools.Client.Models;
using BoardGameTools.Client.ViewModels.Interfaces;
using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels;

public class FightViewModel : IFightViewModel
{
    public FightModel FightModel { get; set; } = new();
    private readonly HttpClient _httpClient;
    private readonly ICharacteristicViewModel _characteristicViewModel;

    public FightViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _characteristicViewModel = characteristicViewModel;
    }

    public async Task<FightModel> Fight(List<CardModel> cards, List<Monster> monster)
    {
        var characteristics = await _characteristicViewModel.GetCharacteristic();
        var response = await _httpClient.PostAsJsonAsync("api/fight/TryToFight", new SelectedCards(CardModel.Transform(cards, characteristics), monster));
        return await response.Content.ReadFromJsonAsync<FightModel>() ?? new FightModel();
    }
}
