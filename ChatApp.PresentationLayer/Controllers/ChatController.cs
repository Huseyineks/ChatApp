using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
