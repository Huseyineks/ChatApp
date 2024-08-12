using ChatApp.BusinessLogicLayer.VMs;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
           var Users = _userManager.Users.ToList();

            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users
            };


            return View(chatViewModel);
        }


        public IActionResult Text(Guid guid) {

            var Users = _userManager.Users.ToList();

            AppUser receiver = _userManager.Users.AsQueryable().SingleOrDefault(i => i.RowGuid == guid);

            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users.ToList(),
                Receiver = receiver
            };        
            return View(chatViewModel);
        
        }
    }
}
