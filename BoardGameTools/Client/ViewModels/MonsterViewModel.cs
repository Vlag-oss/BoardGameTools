using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{
    public class MonsterViewModel : IMonsterViewModel
    {
        private readonly HttpClient httpClient;

        public MonsterViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Monster>> GetMonster()
        {
            var monsters = await httpClient.GetFromJsonAsync<List<Monster>>("api/monster") ?? new();
            return monsters;
        }
    }
}
