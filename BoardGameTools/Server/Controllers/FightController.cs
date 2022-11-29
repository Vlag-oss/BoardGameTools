using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        [HttpPost("TryToFight")]
        public Task<string> TryToFight([FromBody]SelectedCards input)
        {
            return Task.FromResult("toto");
        }
    }
}
