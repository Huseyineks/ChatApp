using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class AppUserGroup
    {
        public int AppUserId { get; set; }

        public virtual AppUser User { get; set; }


        public int GroupId { get; set; }


        public virtual Group Group { get; set; }





    }
}
