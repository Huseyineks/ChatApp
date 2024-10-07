using ChatApp.EntitiesLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class Group : IReceiver
    {

        public int Id { get; set; }


        public Guid RowGuid { get; set; } = Guid.NewGuid();

        public string? groupName {  get; set; }


        public string? GroupImage { get; set; }


        public DateTime createdAt { get; set; } = DateTime.Now;



        //relations


        public virtual List<AppUserGroup>? Users { get; set; }








    }
}
