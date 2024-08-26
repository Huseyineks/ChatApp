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
        public async Task SendMessage(Guid authorGuid,Guid receiverGuid,string message)
        {
           

              var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userGuid == receiverGuid);

            if (onlineUser == null || onlineUser.receiverGuid != authorGuid )
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

                if(onlineUser != null)
                {
                    await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage", authorGuid, message);
                }
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
            var hostUser = _userManager.Users.FirstOrDefault(i => i.Id == int.Parse(Context.UserIdentifier));

            var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userGuid == hostUser.RowGuid);

            onlineUser.userConnectionId = Context.ConnectionId;
            
            _onlineUsersService.Update(onlineUser);
            _onlineUsersService.Save();
          
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           

            OnlineAppUsers disconnectedUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userConnectionId == Context.ConnectionId);

            _onlineUsersService.Remove(disconnectedUser);
            _onlineUsersService.Save();

            await base.OnDisconnectedAsync(exception);
        }

        
    }
}
