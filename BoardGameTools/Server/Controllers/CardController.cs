using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly BoardGameToolsContext context;

        public CardController(BoardGameToolsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<Card> Get()
        {
            return context.Cards.ToList();
        }
    }
}
