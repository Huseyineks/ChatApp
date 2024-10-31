using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.BusinessLogicLayer.VMs;

using ChatApp.EntitiesLayer.Interfaces;
using ChatApp.EntitiesLayer.Model;
using ChatApp.PresentationLayer.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ChatApp.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        
        private readonly IMessageService _messageService;

        private readonly IOnlineUsersService _onlineUsersService;

        private readonly IGroupService _groupService;

        private readonly IHubContext<ChatHub> _hubContext;
        
        private readonly IUserGroupService _userGroupService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IUserService _userService;

        public ChatController(UserManager<AppUser> userManager,IMessageService messageService, IOnlineUsersService onlineUsersService,IHubContext<ChatHub> hubContext, IGroupService groupService, IUserGroupService userGroupService, IWebHostEnvironment webHostEnvironment,IUserService userService)
        {
            _userManager = userManager;
            _messageService = messageService;
            _onlineUsersService = onlineUsersService;
            _hubContext = hubContext;
            _groupService = groupService;
            _userGroupService = userGroupService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }


        public IActionResult Index()
        {
           

            return View();
        }
        public IActionResult Deneme()
        {


            return View();
        }

        public async Task<IActionResult> Text(Guid guid) {

            var hostUser = await  _userService.GetHostUser();


            var Users = _userService.GetFilteredList(i => i.RowGuid != hostUser.RowGuid);

            AppUser hostUserWithGroups = _userService.IncludeGroup(i => i.Id == hostUser.Id);

            var oldUser = _onlineUsersService.Get(i => i.userGuid == hostUser.RowGuid); 

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


            

            IReceiver receiver = _userService.Get(i => i.RowGuid == guid);

            List<Message> ReceiverMessages = new List<Message>();
            if (receiver == null)
            {
                 receiver = _userGroupService.GetGroup(guid);

                ReceiverMessages = _messageService.GetSortedList(i => i.authorGuid == guid && i.receiverGuid == hostUser.RowGuid);


            }
            else {

                ReceiverMessages = _messageService.GetSortedList(i => i.authorGuid == receiver.RowGuid && i.receiverGuid == hostUser.RowGuid);


            }

           
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Receiver = receiver,
                AuthorMessages = _messageService.GetSortedList(i => i.authorGuid == hostUser.RowGuid && i.receiverGuid == receiver.RowGuid),
                ReceiverMessages = ReceiverMessages,
                
            };        

            ViewBag.hostNickname = hostUser.Nickname;
            return View(chatViewModel);
        
        }

        [HttpPost]
        public async Task<JsonResult> MessageNotification(int messageId,string messageType)
        {

            Message message = null;

            if(messageType == "Group")
            {
                var hostUser = await _userService.GetHostUser();

                 message = _messageService.Get(i => i.groupMessageId == messageId && i.receiverGuid == hostUser.RowGuid);

                

                
                
            }
            else
            {
                 message = _messageService.Get(i => i.Id == messageId);
            }



            

            if (message != null)
            {
                message.Status = MessageStatus.Seen;

                _messageService.Update(message);
                _messageService.Save();
            }



            return Json(Ok());
        }

        [HttpPost]

        public async Task<JsonResult> DeleteMessage(int messageId, string messageType,Guid receiverGuid)
        {
            var hostUser = await _userService.GetHostUser();

           
            if(messageType == "Group")
            {
                List<Message> messages = _messageService.GetList(i => i.groupMessageId == messageId);

                foreach(var msg in messages)
                {
                    msg.Status = MessageStatus.Deleted;
                    msg.message = "This message is deleted.";
                    _messageService.Update(msg);

                    

                

                }
                var groupMembers = _userGroupService.GetUsers(receiverGuid);

                foreach (var member in groupMembers)
                {
                    var onlineUser = _onlineUsersService.Get(i => i.userGuid == member.RowGuid);

                    if (onlineUser != null && onlineUser.receiverGuid == receiverGuid)
                    {

                        await _hubContext.Clients.Client(onlineUser.userConnectionId).SendAsync("DeleteMessage", messageId, messageType);

                    }
                }
            }
            else
            {
                var message = _messageService.Get(i => i.Id == messageId);

                message.Status = MessageStatus.Deleted;
                message.message = "This message is deleted.";
                _messageService.Update(message);


                var receiver = _onlineUsersService.Get(i => i.userGuid == receiverGuid);

                if(receiver != null && receiver.receiverGuid == hostUser.RowGuid) {


                    await _hubContext.Clients.Client(receiver.userConnectionId).SendAsync("DeleteMessage", messageId,messageType);
                
                
                }

                
                

            }
            
            _messageService.Save();

            return Json(Ok());
        }

        [HttpPost]

        public async Task<JsonResult> ForwardMessage(List<Guid> usersGuid,string message)
        {
            var hostUser = await _userService.GetHostUser();

            Guid authorGuid = hostUser.RowGuid;

            var hostUserGroups = _userGroupService.GetGroups(hostUser.Id);

            var connectionIds = new List<string>();

            var authorUser = _onlineUsersService.Get(i => i.userGuid == authorGuid);

           

            Message newMessage = null;
            
            foreach (var userGuid in usersGuid)
            {
                if (hostUserGroups.Any(i => i.RowGuid == userGuid))
                {
                    
                    ForwardMessageToGroup(userGuid, message);

                    continue;
                    
                }
                
                var onlineUser = _onlineUsersService.Get(i => i.userGuid == userGuid);

                 newMessage = new Message()
                {
                    authorGuid = authorGuid,

                    message = message,

                    authorId = hostUser.Id,

                    receiverGuid = userGuid,

                    messageType = "Private",

                    groupMessageId = 0
                };

                if (onlineUser != null)
                {

                    connectionIds.Add(onlineUser.userConnectionId);


                    if(onlineUser.receiverGuid == authorGuid)
                    {
                        
                        
                        newMessage.Status = MessageStatus.Seen;
                        
                    }
                    else
                    {
                     
                        
                        newMessage.Status = MessageStatus.NotSeen;

                    }
                    

                }
                else {

                   
                    
                    newMessage.Status = MessageStatus.NotSeen;

                }


                _messageService.Add(newMessage);
                _messageService.Save();

                var createdAt = newMessage.createdAt.ToString("HH:mm");

               

             if(authorUser.receiverGuid == userGuid)
                {
                    CallerMessageDTO callerMessageDTO = new CallerMessageDTO()
                    {

                        createdAt = createdAt,
                        message = message,
                        messageId = newMessage.Id,
                        messageType = "Private",
                        repliedMessageId = null,
                        replyingMessage = null

                    };


                    await _hubContext.Clients.Client(authorUser.userConnectionId).SendAsync("CallerMessage", callerMessageDTO);


                }



            }
            ReceiveMessageDTO receiveMessageDTO = new ReceiveMessageDTO()
            {
                authorGuid = authorGuid,
                message = message,
                messageId = newMessage.Id,
                repliedMessageId = null,
                replyingMessage = null,
                replyingTo = null,
                createdAt = newMessage.createdAt.ToString("HH:mm"),
                
            };
            if (connectionIds.Count > 0)
            {

                await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage",receiveMessageDTO);

            }

            return Json(Ok());
        }
        
        public async Task ForwardMessageToGroup(Guid guid, string message)
        {

            var hostUser = await _userService.GetHostUser();

            var onlineUsers = _onlineUsersService.GetAll();

            var Users = _userGroupService.GetUsers(guid).Where(i => i.RowGuid != hostUser.RowGuid).ToList();


            var authorUser = onlineUsers.FirstOrDefault(i => i.userGuid == hostUser.RowGuid);

            var groupMessageId = _messageService.MaxValueOfGroupMessageId();

            groupMessageId++;

            Message newMessageSent = new Message()
            {
                authorGuid = hostUser.RowGuid,
                receiverGuid = guid,
                Status = MessageStatus.Seen,
                message = message,
                authorId = hostUser.Id,
                messageType = "Group",
                groupMessageId = groupMessageId
            };
            _messageService.Add(newMessageSent);
            _messageService.Save();
            Message newMessageReceived = null;

            List<string> connectionIds = new List<string>();

            foreach(var user in Users)
            {
                var onlineUser = onlineUsers.FirstOrDefault(i => i.userGuid == user.RowGuid);

                newMessageReceived = new Message()
                {
                    
                    authorGuid = guid,
                    receiverGuid = user.RowGuid,
                    message = message,
                    authorId = hostUser.Id,
                    messageType = "Group",
                    groupMessageId= groupMessageId
                    

                };

                
                if(onlineUser != null)
                {
                    connectionIds.Add(onlineUser.userConnectionId);

                    if(onlineUser.receiverGuid == guid)
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
                _messageService.Save();

               



            }

            var createdAt = newMessageSent.createdAt.ToString("HH:mm");


            GroupMessageDTO groupMessageDTO = new GroupMessageDTO()
            {
                message = message,
                messageId = newMessageSent.groupMessageId,
                repliedMessageId = null,
                replyingMessage = null,
                receiverGuid = guid,
                createdAt = createdAt,
                authorNickname = newMessageSent.Author.Nickname
            };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("GroupMessage", groupMessageDTO);

            

            CallerMessageDTO messageDTO = new CallerMessageDTO()
            {
                createdAt = createdAt,
                message = message,
                messageType = "Group",
                replyingMessage = null,
                repliedMessageId = null,
                messageId = newMessageSent.groupMessageId

            };

            if (authorUser.receiverGuid == guid)
            {
                await _hubContext.Clients.Client(authorUser.userConnectionId).SendAsync("CallerMessage", messageDTO);
            }






            
        }

        [HttpPost]

        public async Task<JsonResult> CreateGroup(List<Guid> usersGuid,string groupName, IFormFile groupImage)
        {

            var hostUser = await _userService.GetHostUser();


            usersGuid.Add(hostUser.RowGuid);

            var users = _userService.GetFilteredList(i => usersGuid.Contains(i.RowGuid));

            Group newGroup = new Group()
            {

                groupName = groupName,

                GroupImage = UploadFile(groupImage) // burdaki sorun formfile null olarak geliyor ve js tarafındaki değeri .value ile almamak lazım başka yöntemler var araştır


            };


            _groupService.Add(newGroup);
            _groupService.Save();

            foreach(var user in users) {



                AppUserGroup appUserGroup = new AppUserGroup()
                {
                    AppUserId = user.Id,
                    GroupId = newGroup.Id
                };
            _userGroupService.Add(appUserGroup);
            
            }

            
            _userGroupService.Save();

            return Json(Ok());
        }

        private string UploadFile(IFormFile image)
        {
            string? fileName = null;

            if (image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + '-' +image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return fileName;
        }


    }

   


}
