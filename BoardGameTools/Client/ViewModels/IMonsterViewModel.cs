using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels
{
    public interface IMonsterViewModel
    {
        public Task<List<Monster>> GetMonster();
    }
}
