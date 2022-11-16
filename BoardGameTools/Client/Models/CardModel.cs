using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.Models
{
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
