using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.BusinessLogicLayer.VMs;
using ChatApp.DataAccesLayer.Migrations;
using ChatApp.EntitiesLayer.Model;
using ChatApp.PresentationLayer.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace ChatApp.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        
        private readonly IMessageService _messageService;

        private readonly IOnlineUsersService _onlineUsersService;

        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(UserManager<AppUser> userManager,IMessageService messageService, IOnlineUsersService onlineUsersService,IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _messageService = messageService;
            _onlineUsersService = onlineUsersService;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            var hostUser = await _userManager.GetUserAsync(User);
            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();

            List<MessageNotificationsDTO> messagesNot = new List<MessageNotificationsDTO>();

            foreach (var user in Users)
            {
                MessageNotificationsDTO messageNotificationsDTO = new MessageNotificationsDTO()
                {
                    AmountOfNotSeenMsg = _messageService.GetAll().Where(i => i.authorGuid == user.RowGuid && i.receiverGuid == hostUser.RowGuid && i.Status == MessageStatus.NotSeen).ToList().Count(),
                    receiverGuid = user.RowGuid
                };
                messagesNot.Add(messageNotificationsDTO);
            }
            
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Notifications = messagesNot
            };


            return View(chatViewModel);
        }


        public async Task<IActionResult> Text(Guid guid) {

            var hostUser = await _userManager.GetUserAsync(User);

            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();
            

            var oldUser = _onlineUsersService.GetAll().FirstOrDefault(i => i.userGuid == hostUser.RowGuid);

            if (oldUser != null)
            {
                _onlineUsersService.Remove(oldUser);
                _onlineUsersService.Save();
            }

            OnlineAppUsers onlineAppUser = new OnlineAppUsers()
            {
                receiverGuid = guid,
                userGuid = hostUser.RowGuid
            };
            _onlineUsersService.Add(onlineAppUser);
            _onlineUsersService.Save();


            AppUser author = hostUser;

            AppUser receiver = _userManager.Users.AsQueryable().SingleOrDefault(i => i.RowGuid == guid);

            List<MessageNotificationsDTO> messagesNot = new List<MessageNotificationsDTO>();
            foreach (var user in Users)
            {
                MessageNotificationsDTO messageNotificationsDTO = new MessageNotificationsDTO()
                {
                    AmountOfNotSeenMsg = _messageService.GetAll().Where(i => i.authorGuid == user.RowGuid && i.receiverGuid == hostUser.RowGuid && i.Status == MessageStatus.NotSeen).ToList().Count(),
                    receiverGuid = user.RowGuid
                };
                messagesNot.Add(messageNotificationsDTO);
            }
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Receiver = receiver,
                Author = author,
                AuthorMessages = _messageService.GetSortedData().Where(i => i.authorGuid == hostUser.RowGuid && i.receiverGuid == receiver.RowGuid).ToList(),
                ReceiverMessages = _messageService.GetSortedData().Where(i => i.authorGuid == receiver.RowGuid && i.receiverGuid == hostUser.RowGuid).ToList(),
                Notifications = messagesNot
            };        
            return View(chatViewModel);
        
        }

        [HttpPost]
        public JsonResult MessageNotification(int messageId)
        {
            var message = _messageService.GetAll().FirstOrDefault(i => i.Id  == messageId);

            if (message != null)
            {
                message.Status = MessageStatus.Seen;

                _messageService.Update(message);
                _messageService.Save();
            }



            return Json(Ok());
        }

        [HttpPost]

        public JsonResult DeleteMessage(int messageId)
        {
            var message = _messageService.GetAll().FirstOrDefault(i => i.Id == messageId);

            message.Status = MessageStatus.Deleted;
            message.message = "This message is deleted.";
            _messageService.Update(message);
            _messageService.Save();

            return Json(Ok());
        }

        [HttpPost]

        public async Task<JsonResult> ForwardMessage(Guid authorGuid,List<Guid> usersGuid,string message)
        {
            var onlineUsers = _onlineUsersService.GetAll();

            var connectionIds = new List<string>();

            var authorUser = onlineUsers.FirstOrDefault(i => i.userGuid == authorGuid);

            Message newMessage = new Message();

            foreach(var userGuid in usersGuid)
            {
            var onlineUser = onlineUsers.FirstOrDefault(i => i.userGuid == userGuid);

                if (onlineUser != null)
                {

                    connectionIds.Add(onlineUser.userConnectionId);


                    if(onlineUser.receiverGuid == authorGuid)
                    {
                         newMessage = new Message()
                        {
                            authorGuid = authorGuid,
                            receiverGuid = userGuid,
                            message = message,
                            Status = MessageStatus.Seen
                        };
                        
                    }
                    else
                    {
                         newMessage = new Message()
                        {
                            authorGuid = authorGuid,
                            receiverGuid = userGuid,
                            message = message,
                            Status = MessageStatus.NotSeen
                        };
                       

                    }
                    

                }
                else {

                     newMessage = new Message()
                    {
                        authorGuid = authorGuid,
                        receiverGuid = userGuid,
                        message = message,
                        Status = MessageStatus.NotSeen
                    };
                   
                }


                _messageService.Add(newMessage);
                _messageService.Save();

             if(authorUser.receiverGuid == userGuid)
                {


                    await _hubContext.Clients.Client(authorUser.userConnectionId).SendAsync("CallerMessage", message, null, newMessage.Id, null);


                }



            }
            if (connectionIds.Count > 0)
            {

                await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage",authorGuid, message,null,newMessage.Id,null,null);

            }

            return Json(Ok());
        }


    }
}
