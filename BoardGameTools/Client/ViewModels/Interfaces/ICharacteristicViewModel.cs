using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels.Interfaces
{
    public interface ICharacteristicViewModel
    {
        public Task<List<Characteristic>> GetCharacteristic();
    }
}
