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

        public static CardModel Transform(Card card, List<Characteristic> characteristicsList)
        {
            return new CardModel(card.Id, card.Name, GetNameOfCharacteristic(card.Characteristic), GetListOfValue(card.Value));

            List<string> GetNameOfCharacteristic(string characteristic)
            {
                var characteristics = characteristic.Contains(";") == true
                    ? characteristic.Split(';').ToList()
                    : new List<string>() { characteristic };

                return characteristics.Select(t =>
                {
                    int.TryParse(t, out var y);
                    return characteristicsList.FirstOrDefault(i => i.Id == y).Name;
                }).ToList();
            }

            List<string> GetListOfValue(string value) => value.Contains(";") == true
                ? value.Split(';').ToList()
                : new List<string>() { value };
        }

        public static List<Card> Transform(List<CardModel> cards, List<Characteristic> characteristicsList)
        {
            return cards.Select(card =>
            {
                return new Card
                {
                    Id = card.Id,
                    Name = card.Name,
                    Characteristic = string.Join(";", GetIdOfCharacteristic(card.Characteristics)),
                    Value = string.Join(";", card.Values)
                };
            }).ToList();

            IEnumerable<long> GetIdOfCharacteristic(List<string> characteristics)
            {
                List<long> idList = new();

                foreach(var charac in characteristics)
                {
                    foreach(var c in characteristicsList)
                    {
                        if(string.Equals(charac, c.Name))
                        {
                            idList.Add(c.Id);
                        }
                    }
                }

                return idList;
            }
        }

    }
}
