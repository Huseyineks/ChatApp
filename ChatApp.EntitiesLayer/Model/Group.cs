using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class Group
    {

        public int Id { get; set; }

        public string? groupName {  get; set; }


        public string? GroupImage { get; set; }


        public DateTime createdAt { get; set; } = DateTime.Now;



        //relations


        public virtual List<AppUserGroup>? Users { get; set; }








    }
}
