﻿using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Migrations;
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

        private readonly IUserGroupService _userUserGroupService;
        public ChatHub(UserManager<AppUser> userManager, IOnlineUsersService onlineUsersService,IMessageService messageService,IUserGroupService userGroupService) { 

            _userManager = userManager;
            _onlineUsersService = onlineUsersService;
            _messageService = messageService;
            _userUserGroupService = userGroupService;
        
        }
        public async Task SendMessage(Guid authorGuid,Guid receiverGuid,string message,int? replyingMessageId)
        {
           

              var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userGuid == receiverGuid);
            Message newMessage;
            
            
            var rmsg = _messageService.GetAll().FirstOrDefault(i => i.Id == replyingMessageId);
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

                    repliedMessageId = rmsg?.Id
                    
                    
                    

                };

                _messageService.Add(newMessage);
                _messageService.Save();

                if(onlineUser != null)
                {
                    await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage", authorGuid, message,rmsg?.message);
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

                    repliedMessageId = rmsg?.Id




                 };

                _messageService.Add(newMessage);
                _messageService.Save();


                await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage",authorGuid,message,rmsg?.message,newMessage.Id,newMessage.replyingTo,newMessage.repliedMessageId);

            }

            await Clients.Caller.SendAsync("CallerMessage",message,rmsg?.message,newMessage.Id,newMessage.repliedMessageId);

            
        }

        public async Task SendMessageToGroup(string message,Guid authorGuid, Guid receiverGuid)
        {

            var usersWithGroup = _userUserGroupService.GetUsers(receiverGuid).Where(i => i.RowGuid != authorGuid).ToList();

            var onlineUsers = _onlineUsersService.GetAll();
                Message newMessage = new Message()
                {
                    authorGuid = authorGuid,
                    receiverGuid = receiverGuid,
                    message = message,
                    Status = MessageStatus.Seen
                };

            _messageService.Add(newMessage);

            _messageService.Save();

            List<string> connectionIds = new List<string>();

            foreach (var user in usersWithGroup)
            {
                var onlineUser = onlineUsers.FirstOrDefault(i => i.userGuid == user.RowGuid);

                if(onlineUser != null)
                {
                    connectionIds.Add(onlineUser.userConnectionId);
                }


            }




            await Clients.Clients(connectionIds).SendAsync("GroupMessage", message, receiverGuid, newMessage.Id);


            await Clients.Caller.SendAsync("CallerMessage",message,null,newMessage.Id,null);


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
