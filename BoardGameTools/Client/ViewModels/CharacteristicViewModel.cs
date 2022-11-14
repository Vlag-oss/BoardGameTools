using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{
    public class CharacteristicViewModel : ICharacteristicViewModel
    {
        private readonly HttpClient httpClient;

        public CharacteristicViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Characteristic>> GetCharacteristic()
        {
            var characteristics = await httpClient.GetFromJsonAsync<List<Characteristic>>("api/characteristic") ?? new();
            return characteristics;
        }
    }
}
