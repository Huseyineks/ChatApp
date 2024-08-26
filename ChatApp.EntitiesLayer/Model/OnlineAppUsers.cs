using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Model
{
    public class OnlineAppUsers
    {
        [Key]
        public int Id { get; set; }

        public Guid userGuid { get; set; }

        public Guid receiverGuid { get; set; }

        public string? userConnectionId { get; set; }

        public DateTime activeAt { get; set; } = DateTime.Now;
    }
}
