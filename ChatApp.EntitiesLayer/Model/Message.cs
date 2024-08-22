﻿using System;
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

        public string message { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;

        //relation

        public virtual AppUser Author { get; set; }

        public Guid authorGuid { get; set; }
    }
}
