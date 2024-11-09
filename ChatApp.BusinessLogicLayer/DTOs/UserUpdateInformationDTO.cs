using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.DTOs
{
    public class UserUpdateInformationDTO : UserInformationBaseDTO
    {
        public string? CurrentPassword { get; set; }
        
    }
}
