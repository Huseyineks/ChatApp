using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.VMs
{
    public class ChatPartialViewModel
    {
        public List<MessageNotificationsDTO>? MessageNotifications { get; set; }

        public List<AppUserGroup>? Groups { get; set; }

        public List<AppUser>? Users { get; set; }
    }
}
