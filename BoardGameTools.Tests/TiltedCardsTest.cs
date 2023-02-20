using BoardGameTools.Server.Fight;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests;

public class TiltedCardsTest
{
    private readonly TiltedCards _tiltedCards = new();

    [Fact]
    public void TiltedPhase_ParryKOAttackOK_ReturnTrue()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
            },
            new ParryModel(
                false, 
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                }, 
                3),
            new PhysicalAttackModel(
                true, 
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                }, 
                5),
            new Monster { Id = 1, Name = "monster", Armor = 3, Attack = 5 }
        );

        result.Should().BeEquivalentTo(
        new TiltedPhaseModel(
                true, 
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                    new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
                }));
    }

    [Fact]
    public void TiltedPhase_ParryKOAttackOK_ReturnFalse()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" }
            },
            new ParryModel(
                false,
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                },
                3),
            new PhysicalAttackModel(
                true,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                },
                5),
            new Monster { Id = 1, Name = "monster", Armor = 3, Attack = 5 }
        );

        result.Should().BeEquivalentTo(
            new TiltedPhaseModel(
                false,
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" }
                }));
    }

    [Fact]
    public void TiltedPhase_ParryOKAttackKO_ReturnTrue()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
            },
            new ParryModel(
                true,
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                },
                3),
            new PhysicalAttackModel(
                false,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                },
                4),
            new Monster { Id = 1, Name = "monster", Armor = 5, Attack = 3 }
        );

        result.Should().BeEquivalentTo(
            new TiltedPhaseModel(
                true,
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" }
                }));
    }

    [Fact]
    public void TiltedPhase_ParryOKAttackKO_ReturnFalse()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
            },
            new ParryModel(
                true,
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                },
                3),
            new PhysicalAttackModel(
                false,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                },
                4),
            new Monster { Id = 1, Name = "monster", Armor = 7, Attack = 3 }
        );

        result.Should().BeEquivalentTo(
            new TiltedPhaseModel(
                false,
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                    new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
                }));
    }

    [Fact]
    public void TiltedPhase_ParryKOAttackKO_ReturnTrue()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
            },
            new ParryModel(
                false,
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "3" },
                },
                3),
            new PhysicalAttackModel(
                false,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                },
                4),
            new Monster { Id = 1, Name = "monster", Armor = 5, Attack = 4 }
        );

        result.Should().BeEquivalentTo(
            new TiltedPhaseModel(
                true,
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                    new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
                }));
    }

    [Fact]
    public void TiltedPhase_ParryKOAttackKO_ReturnFalse()
    {
        var result = _tiltedCards.TiltedPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "2" },
                new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
            },
            new ParryModel(
                false,
                new List<Card>()
                {
                    new() { Id = 2, Name = "Parry", Characteristic = "2", Value = "2" },
                },
                2),
            new PhysicalAttackModel(
                false,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "4" },
                },
                4),
            new Monster { Id = 1, Name = "monster", Armor = 5, Attack = 4 }
        );

        result.Should().BeEquivalentTo(
            new TiltedPhaseModel(
                false,
                new List<Card>
                {
                    new() { Id = 3, Name = "Movement", Characteristic = "0", Value = "2" },
                    new() { Id = 4, Name = "Movement", Characteristic = "0", Value = "1" }
                }));
    }

}