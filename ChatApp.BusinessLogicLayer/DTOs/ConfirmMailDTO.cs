using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.DTOs
{
    public class ConfirmMailDTO
    {

        public int EmailCode { get; set; }

        public int CodeReceived { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

    }
}
