using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PollyBefore.Controllers
{
    [Produces("application/json")]
    [Route("api/Inventory")]
    public class InventoryController : Controller
    {
        static int _requestCount = 0;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(100);// simulate some data processing by delaying for 100 milliseconds 
            _requestCount++;

            return _requestCount % 4 == 0 ? Ok(15) : StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
        }
    }
}
