using BoardGameTools.Server.Fight;
using BoardGameTools.Shared.Models;
using FluentAssertions;

namespace BoardGameTools.Tests
{
    public class RangedAttackTest
    {
        private readonly RangedAttack _fightController = new();

        [Fact]
        public void TryToFight_RangedAttackCardEqualToArmor_ReturnTrue()
        {
            var result = _fightController.RangedAttackPhase(
                new List<Card>{ 
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "3" },
                }
                , new List<Monster>
                {
                    new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3}
                }
            );

            result.Should().BeTrue();
        }

        [Fact]
        public void TryToFight_RangedAttackCardGreaterThanArmor_ReturnTrue()
        {
            var result = _fightController.RangedAttackPhase(
                new List<Card>{
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "5" },
                }
                , new List<Monster>
                {
                    new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3}
                }
            );

            result.Should().BeTrue();
        }

        [Fact]
        public void TryToFight_RangedAttackCardLessThanArmor_ReturnFalse()
        {
            var result = _fightController.RangedAttackPhase(
                new List<Card>{
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "2" },
                }
                , new List<Monster>
                {
                    new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3}
                }
            );

            result.Should().BeFalse();
        }

        [Fact]
        public void TryToFight_MultipleRangedAttackCardEqualToArmor_ReturnTrue()
        {
            var result = _fightController.RangedAttackPhase(
                new List<Card>{
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "2" },
                    new() { Id = 2, Name = "RangedAttack_2", Characteristic = "5", Value = "1" },
                }
                , new List<Monster>
                {
                    new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3}
                }
            );

            result.Should().BeTrue();
        }

        [Fact]
        public void TryToFight_MultipleRangedAttackCardGreaterThanArmor_ReturnTrue()
        {
            var result = _fightController.RangedAttackPhase(
                new List<Card>{
                    new() { Id = 1, Name = "RangedAttack", Characteristic = "5", Value = "2" },
                    new() { Id = 2, Name = "RangedAttack_2", Characteristic = "5", Value = "3" },
                }
                , new List<Monster>
                {
                    new() { Id = 1, Name = "Monster", Attack = 2, Armor = 3}
                }
            );

            result.Should().BeTrue();
        }
    }
}
