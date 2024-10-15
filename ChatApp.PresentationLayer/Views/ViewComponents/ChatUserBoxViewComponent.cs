using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.Concrete;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.BusinessLogicLayer.VMs;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class ChatUserBoxViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        

        private readonly IMessageService _messageService;

        public ChatUserBoxViewComponent(UserManager<AppUser> userManager,IMessageService messageService) {

            _userManager = userManager;
            
            _messageService = messageService;
        
        
        }   

        public async Task<IViewComponentResult> InvokeAsync() {

            var hostUser = await _userManager.GetUserAsync(HttpContext.User);

            AppUser hostUserWithGroups = _userManager.Users.Include(i => i.Groups).ThenInclude(i => i.Group).FirstOrDefault(i => i.Id == hostUser.Id);

            var Users = _userManager.Users.AsQueryable().Where(i => i.RowGuid != hostUser.RowGuid).ToList();

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

            foreach (var group in hostUserWithGroups.Groups)
            {
                MessageNotificationsDTO messagesNotificationDTO = new MessageNotificationsDTO()
                {
                    AmountOfNotSeenMsg = _messageService.GetList(i => i.authorGuid == group.Group.RowGuid && i.receiverGuid == hostUser.RowGuid && i.Status == MessageStatus.NotSeen).Count(),
                    receiverGuid = group.Group.RowGuid

                };
                messagesNot.Add(messagesNotificationDTO);
            }
            //kaldığın yerden devam et bu arada forward message yaparken grouplara girmiyor mesaj yani listeye eklemen lazım.

            ChatPartialViewModel chatUserBoxPartialViewModel = new ChatPartialViewModel()
            {
                Groups = hostUserWithGroups.Groups,
                MessageNotifications = messagesNot,
                Users = Users
            };

            return View(chatUserBoxPartialViewModel);
        
        }
    }
}
