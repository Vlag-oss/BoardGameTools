using BoardGameTools.Shared.Models;

namespace BoardGameTools.Shared.Models
{
    public class SelectedCards
    {
        public List<Card> Cards { get; }
        public List<Monster> Monster { get; }

        public SelectedCards(List<Card> cards, List<Monster> monster)
        {
            Cards = cards;
            Monster = monster;
        }
    }
}
