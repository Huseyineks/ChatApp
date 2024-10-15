using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Data;

using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ChatApp.PresentationLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IOnlineUsersService _onlineUsersService;

        private readonly IMessageService _messageService;

        private readonly IUserGroupService _userUserGroupService;

        private readonly IUserService _userService;

        
        public ChatHub(UserManager<AppUser> userManager, IOnlineUsersService onlineUsersService,IMessageService messageService,IUserGroupService userGroupService,IUserService userService) { 

            _userManager = userManager;
            _onlineUsersService = onlineUsersService;
            _messageService = messageService;
            _userUserGroupService = userGroupService;
            _userService = userService;
            
        
        }
        public async Task SendMessage(Guid receiverGuid,string message,int? replyingMessageId)
        {
            var hostUser = await _userService.GetHostUser();

            Guid authorGuid = hostUser.RowGuid;

              var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userGuid == receiverGuid);
            Message newMessage;
            
            
            var rmsg = _messageService.Get(i => i.Id == replyingMessageId);
            if (onlineUser == null || onlineUser.receiverGuid != authorGuid )
            {
                 newMessage = new Message()
                {
                    receiverGuid = receiverGuid,

                    authorGuid = authorGuid,

                    Status = MessageStatus.NotSeen,
                   
                    message = message,

                    replyingToMessage = rmsg?.message,

                    replyingTo = rmsg?.authorGuid == authorGuid ? "self" : "other",

                    repliedMessageId = rmsg?.Id,

                    authorId = int.Parse(Context.UserIdentifier)
                    
                    
                    

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
                 newMessage = new Message()
                {
                    receiverGuid = receiverGuid,

                    authorGuid = authorGuid,

                    Status = MessageStatus.Seen,

                    message = message,

                    replyingToMessage= rmsg?.message,

					replyingTo = rmsg?.authorGuid == authorGuid ? "self" : "other",

                    repliedMessageId = rmsg?.Id,

                     authorId = int.Parse(Context.UserIdentifier)




                 };

                _messageService.Add(newMessage);
                _messageService.Save();


                await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage",authorGuid,message,rmsg?.message,newMessage.Id,newMessage.replyingTo,newMessage.repliedMessageId);

            }

            await Clients.Caller.SendAsync("CallerMessage",message,rmsg?.message,newMessage.Id,newMessage.repliedMessageId);

            
        }

        public async Task SendMessageToGroup(string message, Guid receiverGuid, int? replyingMessageId)
        {
            var hostUser = await _userService.GetHostUser();

            Guid authorGuid = hostUser.RowGuid;

            var usersWithGroup = _userUserGroupService.GetUsers(receiverGuid).Where(i => i.RowGuid != authorGuid).ToList();

            var onlineUsers = _onlineUsersService.GetAll();

            var rmsg = _messageService.Get(i => i.Id == replyingMessageId);

            Message newMessageSended = new Message() // this is for the one Who sended the message
            {
                authorGuid = authorGuid,
                receiverGuid = receiverGuid,
                message = message,
                Status = MessageStatus.Seen,
                replyingToMessage = rmsg?.message,

                replyingTo = rmsg?.authorGuid == authorGuid ? "self" : "other",

                repliedMessageId = rmsg?.Id,
                authorId =int.Parse(Context.UserIdentifier)
            };


            _messageService.Add(newMessageSended);






            List<string> connectionIds = new List<string>();

            foreach (var user in usersWithGroup)
            {
                var onlineUser = onlineUsers.FirstOrDefault(i => i.userGuid == user.RowGuid);

                Message newMessageReceived = new Message()
                {
                    authorGuid = receiverGuid, // since we are working on groups now, to get past messages later from group this authorGuid is equal to groupguid and that means message sended by group
                    receiverGuid = user.RowGuid,
                    message = message,
                    replyingToMessage = rmsg?.message,

                    replyingTo = rmsg?.authorGuid == authorGuid ? "self" : "other",

                    repliedMessageId = rmsg?.Id,
                    authorId = int.Parse(Context.UserIdentifier) // this guid is equal to guid of author from group
                };
               

                if (onlineUser != null)
                {
                    connectionIds.Add(onlineUser.userConnectionId);

                    if (onlineUser.receiverGuid == receiverGuid)
                    {


                        newMessageReceived.Status = MessageStatus.Seen;
                        
                       
                    }
                    else
                    {

                        newMessageReceived.Status = MessageStatus.NotSeen;
                        
                        
                    }
                }
                else
                {

                    newMessageReceived.Status = MessageStatus.NotSeen;
                    
                    
                }
               

                _messageService.Add(newMessageReceived);
               

            }



            
            
                _messageService.Save();
           



            await Clients.Clients(connectionIds).SendAsync("GroupMessage", message, receiverGuid, newMessageSended.Id, rmsg?.message, newMessageSended.replyingTo, newMessageSended.repliedMessageId);


            await Clients.Caller.SendAsync("CallerMessage",message,rmsg?.message,newMessageSended.Id,newMessageSended.repliedMessageId);

            
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
