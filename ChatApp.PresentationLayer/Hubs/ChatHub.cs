﻿using ChatApp.BusinessLogicLayer.Abstract;
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
        public async Task SendMessage(Guid authorGuid,string userId,string message)
        {
            var onlineUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userId == userId);

            

            if (onlineUser == null)
            {
                Message newMessage = new Message()
                {
                    ReceiverId = userId,

                    authorId = authorGuid,

                    Status = MessageStatus.NotSeen
                    

                };

                _messageService.Add(newMessage);
                _messageService.Save();
            }
            else
            {
                await Clients.Client(onlineUser.userConnectionId).SendAsync("ReceiveMessage", message);

            }

            
        }

        public override Task OnConnectedAsync()
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
            
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            OnlineAppUsers disconnectedUser = _onlineUsersService.GetAll().SingleOrDefault(i => i.userId == Context.UserIdentifier);
            
            _onlineUsersService.Remove(disconnectedUser);
            _onlineUsersService.Save();

            return base.OnDisconnectedAsync(exception);
        }
    }
}
