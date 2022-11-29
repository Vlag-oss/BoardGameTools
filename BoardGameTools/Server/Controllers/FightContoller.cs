using BoardGameTools.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameTools.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightContoller : ControllerBase
    {
        [HttpPost("trytofight")]
        public ActionResult TryToFight()
        {
            return Ok();
        }
    }
}
