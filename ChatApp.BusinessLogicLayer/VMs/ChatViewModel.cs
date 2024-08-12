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

        public AppUser Receiver { get; set; }
    }
}
