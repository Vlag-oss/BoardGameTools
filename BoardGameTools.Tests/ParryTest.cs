using BoardGameTools.Server.Controllers;
using BoardGameTools.Server.Fight;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests;

public class ParryTest
{
    private readonly Parry _parry = new(new CalculateFight());
    
    [Fact]
    public void ParadePhase_CardOnlyParade_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "3" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Armor = 1, Attack = 2 }
            });

        result.Should().BeEquivalentTo(new ParryModel(
            true, 
            new List<Card>
                {
                    new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "3" }
                }));
    }

    [Fact]
    public void ParadePhase_CardOnlyParade_ReturnFalse()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "1" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Armor = 1, Attack = 2 }
            });

        result.Should().BeEquivalentTo(new ParryModel(
                false,
                new List<Card>()
            ));
    }

    [Fact]
    public void ParadePhase_MultipleCardsOnlyParade_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "2" },
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Armor = 2, Attack = 4 }
            });

        result.Should().BeEquivalentTo(new ParryModel(
                true,
                new List<Card>
                {
                    new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "3" },
                    new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "2" },
                }
            ));
    }

    [Fact]
    public void ParadePhase_MultipleCardsOnlyParade_ReturnFalse()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 1, Name = "Parry", Characteristic = "2", Value = "2" },
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Armor = 4, Attack = 6 }
            });

        result.Should().BeEquivalentTo(new ParryModel(
                false,
                new List<Card>()
            ));
    }

    [Fact]
    public void ParadePhase_CardsMultipleCharacWithParadeAndNoAttack_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 2, Name = "Card1", Characteristic = "1", Value = "1" },
                new(){ Id = 3, Name = "Card2", Characteristic = "0;2;3", Value = "2;1;3" },
                new(){ Id = 4, Name = "Card3", Characteristic = "0;2;3", Value = "2;3;1" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 5 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            true,
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 4, Name = "Card3", Characteristic = "0;2;3", Value = "2;3;1" },
            }
        ));
    }

    [Fact]
    public void ParadePhase_CardsMultipleCharacWithParadeAndNoAttack_ReturnFalse()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 3, Name = "Card2", Characteristic = "0;2;3", Value = "2;1;3" },
                new(){ Id = 4, Name = "Card3", Characteristic = "0;2;3", Value = "2;3;1" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 8 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            false,
            new List<Card>()
        ));
    }

    [Fact]
    public void ParadePhase_CardsMultipleCharacWithParadeAndAttack_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 3, Name = "Card2", Characteristic = "0;1;2;3", Value = "2;4;1;3" },
                new(){ Id = 4, Name = "Card3", Characteristic = "2;3", Value = "3;1" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 7 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            true,
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 3, Name = "Card2", Characteristic = "0;1;2;3", Value = "2;4;1;3" },
                new(){ Id = 4, Name = "Card3", Characteristic = "2;3", Value = "3;1" }
            }
        ));
    }
    
    [Fact]
    public void ParadePhase_ChooseTheGreaterCards_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 2, Name = "Card1", Characteristic = "2", Value = "2" },
                new(){ Id = 3, Name = "Card2", Characteristic = "2", Value = "3" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 6 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            true,
            new List<Card>
            {
                new(){ Id = 1, Name = "Card", Characteristic = "2", Value = "3" },
                new(){ Id = 3, Name = "Card2", Characteristic = "2", Value = "3" }
            }
        ));
    }


    [Fact]
    public void ParadePhase_ChooseTheGreaterCardsWithMultipleCharac_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card1", Characteristic = "0;2", Value = "1;3" },
                new(){ Id = 2, Name = "Card2", Characteristic = "0;2", Value = "1;2" },
                new(){ Id = 3, Name = "Card3", Characteristic = "0;2;3", Value = "1;3;2" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 6 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            true,
            new List<Card>
            {
                new(){ Id = 1, Name = "Card1", Characteristic = "0;2", Value = "1;3" },
                new(){ Id = 3, Name = "Card3", Characteristic = "0;2;3", Value = "1;3;2" }
            }
        ));
    }

    [Fact]
    public void ParadePhase_SameGapBetweenTwoCards_ReturnTrue()
    {
        var result = _parry.ParryPhase(
            new List<Card>
            {
                new(){ Id = 1, Name = "Card1", Characteristic = "2", Value = "3" },
                new(){ Id = 2, Name = "Card2", Characteristic = "2", Value = "1" },
                new(){ Id = 3, Name = "Card3", Characteristic = "2", Value = "3" }
            },
            new List<Monster>
            {
                new(){ Id = 1, Name = "Monster", Armor = 2, Attack = 5 }
            }
        );

        result.Should().BeEquivalentTo(new ParryModel(
            true,
            new List<Card>
            {
                new(){ Id = 1, Name = "Card1", Characteristic = "2", Value = "3" },
                new(){ Id = 3, Name = "Card3", Characteristic = "2", Value = "3" }
            }
        ));
    }
}