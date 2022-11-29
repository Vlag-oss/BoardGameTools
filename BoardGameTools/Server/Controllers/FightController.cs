using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IRangedAttack _rangedAttack;
        private readonly IParry _parry;
        private readonly IPhysicalAttack _physicalAttack;

        public FightController(IRangedAttack rangedAttack, IParry parry, IPhysicalAttack physicalAttack)
        {
            _rangedAttack = rangedAttack ?? throw new ArgumentNullException(nameof(rangedAttack));
            _parry = parry ?? throw new ArgumentNullException(nameof(parry));
            _physicalAttack = physicalAttack ?? throw new ArgumentNullException(nameof(physicalAttack));
        }

        [HttpPost("TryToFight")]
        public Task<FightModel> TryToFight([FromBody]SelectedCards input)
        {
            //TODO capacités spéciales des monstres (Fortifié, vif, poison, ...)
            //TODO gérer les monstres invocateurs
            //TODO combat plusieurs monstres
            //TODO Rajouter les cartes inclinés sur la parade et l'attaque

            var resultRangedAttack = _rangedAttack.RangedAttackPhase(input.Cards, input.Monster);

            if(resultRangedAttack)
                return Task.FromResult(new FightModel{ Result = true, Wound = 0});

            var resultParry = _parry.ParryPhase(input.Cards, input.Monster);

            var cards = input.Cards.Except(resultParry.Cards).ToList();

            var resultAttack = _physicalAttack.AttackPhase(cards, input.Monster);

            return Task.FromResult(new FightModel { Result = resultAttack.Result, Wound = !resultParry.Result ? (int)input.Monster.First().Attack : 0 });
        }
        
    }
}
