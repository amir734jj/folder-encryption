using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace App.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ICryptoManagement _cryptoManagement;

        public HomeController(ICryptoManagement cryptoManagement)
        {
            _cryptoManagement = cryptoManagement;
        }

        [Route("{*url}")]
        [HttpGet]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Message") && TempData.ContainsKey("MessageType"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.MessageType = TempData["MessageType"];
            }

            return View();
        }
        
        [Route("Encrypt")]
        [HttpPost]
        public async Task<IActionResult> Encrypt(CryptoRequest request)
        {
            TempData.Clear();
            
            request.Action = CryptoAction.Encryption;
            
            await _cryptoManagement.Handle(request);
            
            TempData["MessageType"] = "success";
            TempData["Message"] = $"Successfully Encrypted path: {request.Path}";

            return RedirectToAction("Index");
        }
        
        [Route("Decrypt")]
        [HttpPost]
        public async Task<IActionResult> Decrypt(CryptoRequest request)
        {
            TempData.Clear();
            
            request.Action = CryptoAction.Decryption;
            
            await _cryptoManagement.Handle(request);

            TempData["MessageType"] = "success";
            TempData["Message"] = $"Successfully Decrypted path: {request.Path}";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string messageType, string message)
        {
            TempData["MessageType"] = messageType;
            TempData["Message"] = message;
            
            return RedirectToAction("Index");
        }
    }
}