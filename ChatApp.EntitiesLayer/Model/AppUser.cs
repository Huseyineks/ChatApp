using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class AppUser : IdentityUser<int>
    {
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        public string Nickname { get; set; }

        //relation

        public virtual List<Message> Messages { get; set; }
    }
}
