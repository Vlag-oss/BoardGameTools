using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MonsterController : ControllerBase
    {
        private readonly BoardGameToolsContext _context;

        public MonsterController(BoardGameToolsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Monster> Get() => _context.Monsters.ToList();
    }
}
