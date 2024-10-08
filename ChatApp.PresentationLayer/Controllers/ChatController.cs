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

        public ChatController(UserManager<AppUser> userManager,IMessageService messageService, IOnlineUsersService onlineUsersService,IHubContext<ChatHub> hubContext, IGroupService groupService, IUserGroupService userGroupService, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _messageService = messageService;
            _onlineUsersService = onlineUsersService;
            _hubContext = hubContext;
            _groupService = groupService;
            _userGroupService = userGroupService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var hostUser = await _userManager.GetUserAsync(User);
            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();


            AppUser hostUserWithGroups = _userManager.Users.Include(i => i.Groups).ThenInclude(i => i.Group).FirstOrDefault(i => i.Id == hostUser.Id);

         


            List<MessageNotificationsDTO> messagesNot = new List<MessageNotificationsDTO>();

            foreach (var user in Users)
            {
                MessageNotificationsDTO messageNotificationsDTO = new MessageNotificationsDTO()
                {
                    AmountOfNotSeenMsg = _messageService.GetList(i => i.authorGuid == user.RowGuid && i.receiverGuid == hostUser.RowGuid && i.Status == MessageStatus.NotSeen).Count(),
                    receiverGuid = user.RowGuid
                };
                messagesNot.Add(messageNotificationsDTO);
            }
            
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Notifications = messagesNot,
                Groups = hostUserWithGroups.Groups.ToList()
                
            };


            return View(chatViewModel);
        }


        public async Task<IActionResult> Text(Guid guid) {

            var hostUser = await _userManager.GetUserAsync(User);

            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();

            AppUser hostUserWithGroups = _userManager.Users.Include(i => i.Groups).ThenInclude(i => i.Group).FirstOrDefault(i => i.Id == hostUser.Id);

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


            AppUser author = hostUser;

            IReceiver receiver = _userManager.Users.AsQueryable(). FirstOrDefault(i => i.RowGuid == guid);

            List<Message> ReceiverMessages = new List<Message>();
            if (receiver == null)
            {
                 receiver = _userGroupService.GetGroup(guid);

                ReceiverMessages = _messageService.GetSortedList(i => i.authorGuid == guid && i.receiverGuid == hostUser.RowGuid);


            }
            else {

                ReceiverMessages = _messageService.GetSortedList(i => i.authorGuid == receiver.RowGuid && i.receiverGuid == hostUser.RowGuid);


            }

            List<MessageNotificationsDTO> messagesNot = new List<MessageNotificationsDTO>();
            foreach (var user in Users)
            {
                MessageNotificationsDTO messageNotificationsDTO = new MessageNotificationsDTO()
                {
                    AmountOfNotSeenMsg = _messageService.GetList(i => i.authorGuid == user.RowGuid && i.receiverGuid == hostUser.RowGuid && i.Status == MessageStatus.NotSeen).Count(),
                    receiverGuid = user.RowGuid
                };
                messagesNot.Add(messageNotificationsDTO);
            }
            ChatViewModel chatViewModel = new ChatViewModel()
            {
                Users = Users,
                Receiver = receiver,
                Author = author,
                AuthorMessages = _messageService.GetSortedList(i => i.authorGuid == hostUser.RowGuid && i.receiverGuid == receiver.RowGuid),
                ReceiverMessages = ReceiverMessages,
                Notifications = messagesNot,
                Groups = hostUserWithGroups?.Groups.ToList()
            };        
            return View(chatViewModel);
        
        }

        [HttpPost]
        public JsonResult MessageNotification(int messageId)
        {
            var message = _messageService.Get(i => i.Id  == messageId);

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
            var message = _messageService.Get(i => i.Id == messageId);

            message.Status = MessageStatus.Deleted;
            message.message = "This message is deleted.";
            _messageService.Update(message);
            _messageService.Save();

            return Json(Ok());
        }

        [HttpPost]

        public async Task<JsonResult> ForwardMessage(Guid authorGuid,List<Guid> usersGuid,string message)
        {
            var authorId =  int.Parse(_userManager.GetUserId(User));

            var connectionIds = new List<string>();

            var authorUser = _onlineUsersService.Get(i => i.userGuid == authorGuid);

            Message newMessage = new Message();

            foreach(var userGuid in usersGuid)
            {
                var onlineUser = _onlineUsersService.Get(i => i.userGuid == userGuid);

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
                            Status = MessageStatus.Seen,
                            authorId = authorId
                        };
                        
                    }
                    else
                    {
                         newMessage = new Message()
                        {
                            authorGuid = authorGuid,
                            receiverGuid = userGuid,
                            message = message,
                            Status = MessageStatus.NotSeen,
                            authorId = authorId
                        };
                       

                    }
                    

                }
                else {

                     newMessage = new Message()
                    {
                        authorGuid = authorGuid,
                        receiverGuid = userGuid,
                        message = message,
                        Status = MessageStatus.NotSeen,
                         authorId = authorId
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

        [HttpPost]

        public async Task<JsonResult> CreateGroup(List<Guid> usersGuid,string groupName, IFormFile groupImage)
        {

            var hostUser = await _userManager.GetUserAsync(User);


            usersGuid.Add(hostUser.RowGuid);

            var users =  _userManager.Users.Where(i => usersGuid.Contains(i.RowGuid)).ToList();

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
