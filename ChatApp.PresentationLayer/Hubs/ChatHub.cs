using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.PresentationLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        public ChatHub(UserManager<AppUser> userManager) { 

            _userManager = userManager;
        
        }
        public async Task SendMessage(string user,string message)
        {
            

            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }
    }
}
