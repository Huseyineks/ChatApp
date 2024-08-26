using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.DTOs
{
    public class MessageNotificationsDTO
    {
        public Guid receiverGuid {  get; set; }

        public int AmountOfNotSeenMsg { get; set; }
    }
}
