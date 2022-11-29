using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface IMonsterViewModel
    {
        public List<Monster> SelectedMonster { get; set; }
        public string WarningMessage { get; set; }
        public Task<List<Monster>> GetMonster();
        public void AddMonster(Monster monster);
        public void RemoveMonster(Monster monster);
    }
}
