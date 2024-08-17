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
        public async Task<IActionResult> Index()
        {
            var hostUser = await _userManager.GetUserAsync(User);
            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();

            
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users
            };


            return View(chatViewModel);
        }


        public async Task<IActionResult> Text(Guid guid) {

            var hostUser = await _userManager.GetUserAsync(User);

            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();

            AppUser author = hostUser;

            AppUser receiver = _userManager.Users.AsQueryable().SingleOrDefault(i => i.RowGuid == guid);

            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Receiver = receiver,
                Author = author
            };        
            return View(chatViewModel);
        
        }
    }
}
