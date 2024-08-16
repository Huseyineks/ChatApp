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
        public async Task SendMessage(string userId,string message)
        {
            

            await Clients.User(userId).SendAsync("ReceiveMessage",message);
        }

        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("UserDisconnected",Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
