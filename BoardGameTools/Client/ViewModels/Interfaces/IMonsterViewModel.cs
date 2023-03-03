using BoardGameTools.Client.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface IMonsterViewModel
    {
        public List<MonsterModel> SelectedMonster { get; set; }
        public string WarningMessage { get; set; }
        public Task<List<MonsterModel>> GetMonster();
        public void AddMonster(MonsterModel monster);
        public void RemoveMonster(MonsterModel monster);
    }
}
