using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PollyBefore.Controllers
{
    [Produces("application/json")]
    [Route("api/Inventory")]
    public class InventoryController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(100);
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
        }
    }
}
