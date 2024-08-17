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

        public string ReceiverId { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;

        //relation

        public virtual AppUser Author { get; set; }

        public Guid authorId { get; set; }
    }
}
