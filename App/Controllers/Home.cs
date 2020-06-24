using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("")]
    public class Home : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("Encrypt")]
        [HttpPost]
        public IActionResult Encrypt()
        {
            return Ok();
        }
        
        [Route("Decrypt")]
        [HttpPost]
        public IActionResult Decrypt()
        {
            return Ok();
        }
    }
}