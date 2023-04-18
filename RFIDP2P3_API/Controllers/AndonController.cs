using Microsoft.AspNetCore.Mvc;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AndonController : Controller
    {
        [HttpGet]
        public IActionResult AndonStock()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AndonPallet()
        {
            return View();
        }
    }
}
