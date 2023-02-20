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
        private readonly ITiltedCards _tiltedCards;

        public FightController(IRangedAttack rangedAttack, IParry parry, IPhysicalAttack physicalAttack, ITiltedCards tiltedCards)
        {
            _rangedAttack = rangedAttack ?? throw new ArgumentNullException(nameof(rangedAttack));
            _parry = parry ?? throw new ArgumentNullException(nameof(parry));
            _physicalAttack = physicalAttack ?? throw new ArgumentNullException(nameof(physicalAttack));
            _tiltedCards = tiltedCards ?? throw new ArgumentNullException(nameof(tiltedCards));
        }

        [HttpPost("TryToFight")]
        public Task<FightModel> TryToFight([FromBody]SelectedCards input)
        {
            //TODO capacités spéciales des monstres (Fortifié, vif, poison, ...)
            //TODO gérer les monstres invocateurs
            //TODO combat plusieurs monstres
            //TODO Rajouter les cartes inclinés sur la parade et l'attaque
            
            var resultRangedAttack = _rangedAttack.RangedAttackPhase(input.Cards, input.Monster);
            
            if(resultRangedAttack.Result)
                return Task.FromResult(new FightModel(true, 0, resultRangedAttack.CardsUsed));

            //CHECK TOUT LES EXCEPT CAR MISE EN PLACE DU IEQUATABLE 

            var parryPhase = _parry.ParryPhase(input.Cards, input.Monster);

            var cards = input.Cards.Except(parryPhase.CardsUsed).ToList();

            var attackPhase = _physicalAttack.AttackPhase(cards, input.Monster);

            _tiltedCards.TiltedPhase(input.Cards, parryPhase, attackPhase, input.Monster.First());

            var cardsUsed = new List<Card>();
            
            return Task.FromResult(
                new FightModel(
                    attackPhase.Result, 
                    !parryPhase.Result
                        ? (int)input.Monster.First().Attack 
                        : 0,
                    attackPhase.Result
                        ? cardsUsed 
                        : new List<Card>()));
        }
    }
}