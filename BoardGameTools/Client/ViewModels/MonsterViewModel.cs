using BoardGameTools.Shared.Models;
using System.Net.Http.Json;
using BoardGameTools.Client.Models;
using BoardGameTools.Client.ViewModels.Interfaces;

namespace BoardGameTools.Client.ViewModels
{
    public class MonsterViewModel : IMonsterViewModel
    {
        public List<MonsterModel> SelectedMonster { get; set; } = new();
        public string WarningMessage { get; set; } = string.Empty;

        private readonly HttpClient httpClient;

        public MonsterViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<MonsterModel>> GetMonster()
        {
            var monsters = await httpClient.GetFromJsonAsync<List<Monster>>("api/monster") ?? new List<Monster>();
            return MonsterModel.Transform(monsters);
        }

        public void AddMonster(MonsterModel monster)
        {
            if (SelectedMonster.Count >= 1)
                WarningMessage = "Vous ne devez sélectionner qu'un seul monstre";
            else
                SelectedMonster.Add(monster);
        }

        public void RemoveMonster(MonsterModel monster) => SelectedMonster.Remove(monster);

    }
}
