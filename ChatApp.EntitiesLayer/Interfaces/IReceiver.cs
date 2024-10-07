using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EntitiesLayer.Interfaces
{
    public interface IReceiver
    {
        Guid RowGuid { get; set; }
    }
}
