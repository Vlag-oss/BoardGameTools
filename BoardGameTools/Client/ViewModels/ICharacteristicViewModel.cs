using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.ViewModels
{
    public interface ICharacteristicViewModel
    {
        public Task<List<Characteristic>> GetCharacteristic();
    }
}
