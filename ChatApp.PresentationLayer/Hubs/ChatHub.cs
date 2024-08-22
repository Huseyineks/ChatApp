using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.PresentationLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IOnlineUsersService _onlineUsersService;

        private readonly IMessageService _messageService;
        public ChatHub(UserManager<AppUser> userManager, IOnlineUsersService onlineUsersService,IMessageService messageService) { 

            _userManager = userManager;
            _onlineUsersService = onlineUsersService;
            _messageService = messageService;
        
        }
        public async Task SendMessage(Guid authorGuid,Guid receiverGuid,string userId,string message)
        {
            var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userId == userId);

            

            if (onlineUser == null)
            {
                Message newMessage = new Message()
                {
                    receiverGuid = receiverGuid,

                    authorGuid = authorGuid,

                    Status = MessageStatus.NotSeen,
                   
                    message = message
                    

                };

                _messageService.Add(newMessage);
                _messageService.Save();
            }
            else
            {
                Message newMessage = new Message()
                {
                    receiverGuid = receiverGuid,

                    authorGuid = authorGuid,

                    Status = MessageStatus.Seen,

                    message = message


                };

                _messageService.Add(newMessage);
                _messageService.Save();


                await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage",authorGuid,message);

            }

            await Clients.Caller.SendAsync("CallerMessage",message);

            
        }

        public override async Task OnConnectedAsync()
        {

            OnlineAppUsers user = new OnlineAppUsers()
            {
                userConnectionId = Context.ConnectionId,
                userId = Context.UserIdentifier
            };

            OnlineAppUsers oldUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userId == Context.ConnectionId); // May be there are users with same userId thats why ı did it.

            if (oldUser != null)
            {
                _onlineUsersService.Remove(oldUser);
                _onlineUsersService.Save();

            }

            _onlineUsersService.Add(user);
            _onlineUsersService.Save();
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            OnlineAppUsers disconnectedUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userId == Context.UserIdentifier);
            
            _onlineUsersService.Remove(disconnectedUser);
            _onlineUsersService.Save();

            await base.OnDisconnectedAsync(exception);
        }

        
    }
}
