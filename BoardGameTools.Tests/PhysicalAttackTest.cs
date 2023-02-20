using BoardGameTools.Server.Controllers;
using BoardGameTools.Server.Fight;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests;

public class PhysicalAttackTest
{
    private readonly PhysicalAttack _physicalAttack = new(new CalculateFight());

    [Fact]
    public void AttackPhase_UseGreaterCardsToAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack1", Characteristic = "1", Value = "3" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1", Value = "1" },
                new() { Id = 3, Name = "Attack3", Characteristic = "1", Value = "3" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 6 }
            }
        );

        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 3, Name = "Attack3", Characteristic = "1", Value = "3" },
                new() { Id = 1, Name = "Attack1", Characteristic = "1", Value = "3" }
            },
            6
        ));
    }

    [Fact]
    public void AttackPhase_UseGreaterCardsWithMultipleCharacToAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack1", Characteristic = "0;1", Value = "1;3" },
                new() { Id = 2, Name = "Attack2", Characteristic = "0;1", Value = "2;1" },
                new() { Id = 3, Name = "Attack3", Characteristic = "0;1", Value = "5;3" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 6 }
            }
        );

        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 3, Name = "Attack3", Characteristic = "0;1", Value = "5;3" },
                new() { Id = 1, Name = "Attack1", Characteristic = "0;1", Value = "1;3" }
            },
            6
        ));
    }

    [Theory]
    [InlineData("3")]
    [InlineData("5")]
    public void AttackPhase_CardOnlyAttack_ReturnTrue(string value)
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = value }
            }, 
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
                true,
                new List<Card>
                {
                    new() { Id = 1, Name = "Attack", Characteristic = "1", Value = value }
                },
                int.Parse(value)
            ));
    }

    [Fact]
    public void AttackPhase_CardOnlyAttack_ReturnFalse()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            false,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" }
            },
            2
        ));
    }

    [Fact]
    public void AttackPhase_MultipleCardsOnlyAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1", Value = "4" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 5 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1", Value = "4" }
            },
            6
        ));
    }

    [Fact]
    public void AttackPhase_MultipleCardsOnlyAttack_ReturnFalse()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1", Value = "2" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 5 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            false,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1", Value = "2" }
            },
            4
        ));
    }

    [Fact]
    public void AttackPhase_CardsMultipleCharacWithAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "3;2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "3;2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            4
        ));
    }

    [Fact]
    public void AttackPhase_CardsMultipleCharacWithAttack2_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "2" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            4
        ));
    }

    [Fact]
    public void AttackPhase_CardsMultipleCharacWithAttack_ReturnFalse()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "3;1" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 5 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            false,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;1", Value = "3;1" },
                new() { Id = 2, Name = "Attack2", Characteristic = "1;2", Value = "2;4" }
            },
            3
        ));
    }

    [Fact]
    public void AttackPhase_CardsOnlyRangedAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "5", Value = "5" },
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "5", Value = "5" },
            },
            5
        ));
    }

    [Fact]
    public void AttackPhase_CardsOnlyRangedAttack_ReturnFalse()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "5", Value = "2" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            false,
            new List<Card>{ new() { Id = 1, Name = "Attack", Characteristic = "5", Value = "2" } },
            2
        ));
    }

    [Fact]
    public void AttackPhase_MultipleCardsWithRangedAttack_ReturnTrue()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;5", Value = "1;5" },
                new() { Id = 1, Name = "Attack", Characteristic = "0;3;5;", Value = "3;2;1" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            true,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;5", Value = "1;5" },
            },
            5
        ));
    }

    [Fact]
    public void AttackPhase_MultipleCardsWithRangedAttack_ReturnFalse()
    {
        var result = _physicalAttack.AttackPhase(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;5", Value = "1;2" },
                new() { Id = 1, Name = "Attack", Characteristic = "0;3;5;", Value = "3;2;1" }
            },
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 4 }
            }
        );
        result.Should().BeEquivalentTo(new PhysicalAttackModel(
            false,
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "0;5", Value = "1;2" },
                new() { Id = 1, Name = "Attack", Characteristic = "0;3;5;", Value = "3;2;1" }
            },
            3
        ));
    }
}