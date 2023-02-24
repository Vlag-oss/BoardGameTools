using BoardGameTools.Server.Controllers;
using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using BoardGameTools.Tests.Fakes;
using FluentAssertions;

namespace BoardGameTools.Tests;

public class FightTest
{
    private readonly FakeRangedAttack _fakeRangedAttack = new() { Result = new RangedAttackModel(false, new List<Card>()) };
    private readonly FakeParry _fakeParry = new() { Result = new ParryModel(false, new List<Card>(), 0) };
    private readonly FakePhysicalAttack _fakePhysicalAttack = new() { Result = new PhysicalAttackModel(false, new List<Card>(), 0) };
    private readonly FakeTiltedCards _fakeTiltedCards = new();

    [Fact]
    public async Task TryToFight_RangedAttackIsOk_ReturnTrueAndCardsUsed()
    {
        var fakeRangedAttack = new FakeRangedAttack
        {
            Result = new RangedAttackModel(
                true, 
                new List<Card>
                {
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "4" }
                })
        };

        var fight = new FightController(fakeRangedAttack, _fakeParry, _fakePhysicalAttack, _fakeTiltedCards);

        var result = await fight.TryToFight(new SelectedCards(
            new List<Card>
            {
                new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "4" }
            }, 
            new List<Monster>
            {
                new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3 }
            }));

        result.Should().BeEquivalentTo(new FightModel
            {
                Result = true, 
                Wound = 0, 
                CardsUsed = new List<Card>{ new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "4" } }
            });
    }

    [Fact]
    public async Task TryToFight_ParryIsOkButAttackIsNotOk_ReturnFalse()
    {
        var fakeParry = new FakeParry
        {
            Result = new ParryModel(
                true,
                new List<Card> { new() { Id = 2, Name = "Parry1", Characteristic = "2", Value = "3" } },
                0)
        };

        var fight = new FightController(_fakeRangedAttack, fakeParry, _fakePhysicalAttack, _fakeTiltedCards);

        var result = await fight.TryToFight(new SelectedCards(
            new List<Card>
            {
                new() { Id = 2, Name = "Parry1", Characteristic = "2", Value = "3" }
            },
            new List<Monster>()));

        result.Should().BeEquivalentTo(new FightModel{
            Result = false,
            Wound = 0,
            CardsUsed = new List<Card>()});
    }

    [Fact]
    public async Task TryToFight_AttackIsOk_ReturnTrueAndCardsUsed()
    {
        var fakeAttack = new FakePhysicalAttack
        {
            Result = new PhysicalAttackModel(
                true,
                new List<Card> { new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "3" } },
                0)
        };

        var fakeParry = new FakeParry
        {
            Result = new ParryModel(
                true,
                new List<Card>(),
                0)
        };

        var fight = new FightController(_fakeRangedAttack, fakeParry, fakeAttack, _fakeTiltedCards);

        var result = await fight.TryToFight(new SelectedCards(
            new List<Card>
            {
                new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "3" }
            },
            new List<Monster>()));

        result.Should().BeEquivalentTo(new FightModel{
            Result = true,
            Wound = 0,
            CardsUsed = new List<Card> { new() { Id = 1, Name = "Attack", Characteristic = "1", Value = "3" } }
        });
    }
}