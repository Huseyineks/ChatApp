using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.DTOs
{
    public class UserInformationDTO
    {
        public IFormFile? UserImage {  get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string? Email { get; set; }

        public string? Nickname { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
