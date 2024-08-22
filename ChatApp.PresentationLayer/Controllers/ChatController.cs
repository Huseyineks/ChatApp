using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.VMs;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ChatApp.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        
        private readonly IMessageService _messageService;

        public ChatController(UserManager<AppUser> userManager,IMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
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
                Author = author,
                AuthorMessages = _messageService.GetSortedData().Where(i => i.authorGuid == hostUser.RowGuid && i.receiverGuid == receiver.RowGuid).ToList(),
                ReceiverMessages = _messageService.GetSortedData().Where(i => i.authorGuid == receiver.RowGuid && i.receiverGuid == hostUser.RowGuid).ToList()
            };        
            return View(chatViewModel);
        
        }
    }
}
