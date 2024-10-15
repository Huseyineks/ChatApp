using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class Message
    {
        public int Id { get; set; }

        public Guid receiverGuid { get; set; }

        public Guid authorGuid { get; set; }

        public string message { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;

        public string? replyingTo { get; set; }

        public string? replyingToMessage {  get; set; }

        public int? repliedMessageId { get; set; }
        //relation

        public virtual AppUser Author { get; set; }

        public int authorId { get; set; }

       

        
    }
}
