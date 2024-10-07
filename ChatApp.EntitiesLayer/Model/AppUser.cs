using ChatApp.EntitiesLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class AppUser : IdentityUser<int>,IReceiver
    {
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        public string Nickname { get; set; }

        public string UserImage { get; set; }

        //relation
        public virtual List<AppUserGroup> Groups { get; set; }

        
        public virtual List<Message> Messages { get; set; }
    }
}
