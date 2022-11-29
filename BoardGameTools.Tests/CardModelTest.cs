using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests
{
    public class CardModelTest
    {
        private readonly List<Characteristic> _characteristics = new()
        {
            new Characteristic { Id = 0, Name = "MOUVEMENT" },
            new Characteristic { Id = 1, Name = "ATTAQUE" },
            new Characteristic { Id = 2, Name = "PARADE" },
            new Characteristic { Id = 3, Name = "INFLUENCE" },
            new Characteristic { Id = 4, Name = "PARADE DE GLACE" }
        };

        [Fact]
        public void Transform_CardToCardModel_WithValue_ReturnNotNull()
        {
            var card = CardModel.Transform(new List<Card> { new() { Id = 1, Name = "NotBeNull", Characteristic = "0", Value = "2" } }, _characteristics);
            card.Should().NotBeNull();
        }

        [Fact]
        public void Transform_CardToCardModel_WithoutValue_ReturnEmptyList()
        {
            var card = CardModel.Transform(new List<Card>(), new List<Characteristic>());
            card.Should().BeEquivalentTo(new List<CardModel>());
        }

        private static IEnumerable<object[]> TransformTestData()
        {
            yield return new object[] { "0", "2", new List<string> { "MOUVEMENT" } };
            yield return new object[] { "2", "1", new List<string> { "PARADE" } };
            yield return new object[] { "0;3;4", "2", new List<string> { "MOUVEMENT", "INFLUENCE", "PARADE DE GLACE" } };
        }

        [Theory]
        [MemberData(nameof(TransformTestData))]
        public void Transform_CardToCardModel_WithValue_ReturnTransformedValue(string characteristic, string value, List<string> result)
        {
            var card = CardModel.Transform(
                new List<Card> { new() { Id = 1, Name = "NotBeNull", Characteristic = characteristic, Value = value }},
                _characteristics);
            card.First().Characteristics.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void Transform_CardModelToCard_WithValue_ReturnNotNull()
        {
            List<Card> cards = new()
            {
                new Card { Id = 1, Name = "test1", Characteristic = "0", Value = "1" },
                new Card { Id = 2, Name = "test2", Characteristic = "2", Value = "4" },
                new Card { Id = 3, Name = "test3", Characteristic = "1;3", Value = "1" },
                new Card { Id = 4, Name = "test4", Characteristic = "0;1;2;3", Value = "1;2;3;4" },
            };

            var cardsModel = CardModel.Transform(cards, _characteristics);

            var card = CardModel.Transform(cardsModel, _characteristics);
            card.Should().NotBeNull();
        }

        [Fact]
        public void Transform_CardModelToCard_WithoutValue_ReturnEmptyList()
        {
            var card = CardModel.Transform(new List<CardModel>(), new List<Characteristic>());
            card.Should().BeEquivalentTo(new List<Card>());
        }

        [Fact]
        public void Transform_CardModelToCard_WithValue_ReturnTransformedValue()
        {
            List<Card> cards = new()
            {
                new Card { Id = 1, Name = "test1", Characteristic = "0", Value = "1" },
                new Card { Id = 2, Name = "test2", Characteristic = "2", Value = "4" },
                new Card { Id = 3, Name = "test3", Characteristic = "1;3", Value = "1" },
                new Card { Id = 4, Name = "test4", Characteristic = "0;1;2;3", Value = "1;2;3;4" },
            };

            var cardsModel = CardModel.Transform(cards, _characteristics);

            var card = CardModel.Transform(cardsModel, _characteristics);
            card.Should().BeEquivalentTo(cards);
        }
    }
}