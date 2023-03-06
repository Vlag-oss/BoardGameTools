using BoardGameTools.Client.Models;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests;

public class MonsterModelTest
{
    [Fact]
    public void Transform_MonsterToMonsterModel_WithValue_ReturnNotNull()
    {
        var card = MonsterModel.Transform(new List<Monster> { new() { Id = 0, Name = "Rôdeurs", Attack = 4, Armor = 3 } });
        card.Should().NotBeNull();
    }

    [Fact]
    public void Transform_MonsterToMonsterModel_WithoutValue_ReturnEmptyList()
    {
        var card = MonsterModel.Transform(new List<Monster>());
        card.Should().BeEquivalentTo(new List<MonsterModel>());
    }

    [Fact]
    public void Transform_MonsterToMonsterModel_WithValue_ReturnTransformedValue()
    {
        var monster = MonsterModel.Transform(new List<Monster>
        {
            new() { Id = 0, Name = "Rôdeurs", Attack = 4, Armor = 3 },
            new() { Id = 1, Name = "Creuseurs", Attack = 3, Armor = 3, DefensiveAbilities = 1 },
            new() { Id = 2, Name = "Sorcières Maudites", Attack = 3, Armor = 5, OffensiveAbilities = 3 },
            new() { Id = 3, Name = "Cogneurs", Attack = 4, Armor = 3, DefensiveAbilities = 2, OffensiveAbilities = 2 }
        });

        monster.Should().BeEquivalentTo(new List<MonsterModel>
        {
            new(0, "Rôdeurs", 4, 3, string.Empty, string.Empty),
            new(1, "Creuseurs", 3, 3, "Fortifié", string.Empty),
            new(2, "Sorcières Maudites", 3, 5, string.Empty, "Toxique"),
            new(3, "Cogneurs", 4, 3, "Résistance_physique", "Brutal")
        });
    }

    [Fact]
    public void Transform_MonsterModelToMonster_WithValue_ReturnNotNull()
    {
        var card = MonsterModel.Transform(new List<MonsterModel> { new(0, "Rôdeurs", 4, 3, string.Empty, string.Empty) });
        card.Should().NotBeNull();
    }

    [Fact]
    public void Transform_MonsterModelToMonster_WithoutValue_ReturnEmptyList()
    {
        var card = MonsterModel.Transform(new List<MonsterModel>());
        card.Should().BeEquivalentTo(new List<Monster>());
    }

    [Fact]
    public void Transform_MonsterModelToMonster_WithValue_ReturnTransformedValue()
    {
        var monster = MonsterModel.Transform(new List<MonsterModel>
        {
            new(0, "Rôdeurs", 4, 3, string.Empty, string.Empty),
            new(1, "Creuseurs", 3, 3, "Fortifié", string.Empty),
            new(2, "Sorcières Maudites", 3, 5, string.Empty, "Toxique"),
            new(3, "Cogneurs", 4, 3, "Résistance_physique", "Brutal")
        });

        monster.Should().BeEquivalentTo(new List<Monster>
        {
            new() { Id = 0, Name = "Rôdeurs", Attack = 4, Armor = 3, DefensiveAbilities = null, OffensiveAbilities  = null},
            new() { Id = 1, Name = "Creuseurs", Attack = 3, Armor = 3, DefensiveAbilities = 1, OffensiveAbilities = null},
            new() { Id = 2, Name = "Sorcières Maudites", Attack = 3, Armor = 5, DefensiveAbilities = null, OffensiveAbilities = 3},
            new() { Id = 3, Name = "Cogneurs", Attack = 4, Armor = 3, DefensiveAbilities = 2, OffensiveAbilities = 2}
        });
    }
}