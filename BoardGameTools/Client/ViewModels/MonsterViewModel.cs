using BoardGameTools.Shared.Models;
using System.Net.Http.Json;
using BoardGameTools.Client.ViewModels.Interfaces;

namespace BoardGameTools.Client.ViewModels
{
    public class MonsterViewModel : IMonsterViewModel
    {
        public List<Monster> SelectedMonster { get; set; } = new();
        public string WarningMessage { get; set; } = string.Empty;

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

        public void AddMonster(Monster monster)
        {
            if (SelectedMonster.Count >= 1)
                WarningMessage = "Vous ne devez sélectionner qu'un seul monstre";
            else
                SelectedMonster.Add(monster);
        }

        public void RemoveMonster(Monster monster) => SelectedMonster.Remove(monster);

    }
}
