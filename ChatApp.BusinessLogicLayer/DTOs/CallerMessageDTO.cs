using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.DTOs
{
    public class CallerMessageDTO
    {
        //message,replyingMessage,messageId,repliedMessageId,messageType,createdAt

        public string message { get; set; }

        public string? replyingMessage { get; set; }

        public int? messageId { get; set; }

        public int? repliedMessageId { get; set; }

        public string messageType { get; set; }

        public string createdAt { get; set; }

        public string? replyingTo { get; set; }


    }
}
