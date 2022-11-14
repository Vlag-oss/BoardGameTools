using BoardGameTools.Server.Models;
using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacteristicController : ControllerBase
    {
        private readonly BoardGameToolsContext context;

        public CharacteristicController(BoardGameToolsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<List<Characteristic>> Get()
        {
            return await context.Characteristics.ToListAsync();
        }

    }
}
