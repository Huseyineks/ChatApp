using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.VMs
{
    public class ChatViewModel
    {
        public List<AppUser> Users { get; set; }

        public AppUser Author { get; set; }
        public AppUser Receiver { get; set; }

        public List<Message> AuthorMessages { get; set; }

        public List<Message> ReceiverMessages { get; set; }

        public List<MessageNotificationsDTO> Notifications { get; set; }

        public List<AppUserGroup>? Groups { get; set; }
    }
}
