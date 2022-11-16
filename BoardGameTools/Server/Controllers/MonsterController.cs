using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonsterController : ControllerBase
    {
        private readonly BoardGameToolsContext context;

        public MonsterController(BoardGameToolsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<Monster> Get()
        {
            return context.Monsters.ToList();
        }
    }
}
