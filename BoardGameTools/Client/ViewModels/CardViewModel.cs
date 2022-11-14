using BoardGameTools.Shared.Models;
using System.Net.Http.Json;

namespace BoardGameTools.Client.ViewModels
{
    public class CardViewModel : ICardViewModel
    {
        public List<Card> Cards { get; set; } = new();
        private readonly HttpClient httpClient;
        private readonly ICharacteristicViewModel characteristicViewModel;

        public CardViewModel(HttpClient httpClient, ICharacteristicViewModel characteristicViewModel)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.characteristicViewModel = characteristicViewModel;
        }

        public async Task<List<CardModel>> GetCard()
        {
            var cards = await httpClient.GetFromJsonAsync<List<Card>>("api/card") ?? new();
            var characteristics = await characteristicViewModel.GetCharacteristic();

            return cards.Select(card =>
            {
                return CardModel.Transform(card, characteristics);
            }).ToList();
        }
    }

    public class CardModel
    {
        public long Id { get; }

        public string Name { get; }

        public List<string> Characteristics { get; }

        public List<string> Values { get; }

        private CardModel(long id, string name, List<string> characteristics, List<string> values)
        {
            Id = id;
            Name = name;
            Characteristics = characteristics;
            Values = values;
        }

        public static CardModel Transform(Card card, List<Characteristic> characteristics)
        {
            return new CardModel(card.Id, card.Name, GetNameOfCharacteristic(card.Characteristic), GetListOfValue(card.Value));

            List<string> GetNameOfCharacteristic(string characteristic)
            {
                var nameOfCharacteristic = new List<string>();

                var intCharacteristic = characteristic.Contains(";") == true 
                    ? characteristic.Split(';').ToList() 
                    : new List<string>() { characteristic };

                intCharacteristic.ForEach(t =>
                {
                    int.TryParse(t, out var y);
                    nameOfCharacteristic.Add(characteristics.FirstOrDefault(i => i.Id == y).Name);
                });

                return nameOfCharacteristic;
            }

            List<string> GetListOfValue(string value) => value.Contains(";") == true 
                ? value.Split(';').ToList() 
                : new List<string>() { value };
        }

    }
}
