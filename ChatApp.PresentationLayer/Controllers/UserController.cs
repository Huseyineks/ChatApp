using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using ChatApp.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ChatApp.EntitiesLayer.Model;
using ChatApp.BusinessLogicLayer.Concrete;



namespace ChatApp.PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserUpdateInformationDTO> _validator;
        private readonly UserManager<AppUser> _userManager;
        
        public UserController(UserManager<AppUser> userManager,IUserService userService,IValidator<UserUpdateInformationDTO> validator) { 
        
            _userService = userService;
            _validator = validator;
            _userManager = userManager;
            
        
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.SidebarSize = "Small";


            var user = await _userService.GetHostUser();

            UserUpdateInformationDTO userInformationDTO = new UserUpdateInformationDTO()
            {

                Email = user.Email,
                Nickname = user.Nickname,
                UserName = user.UserName


            };

            return View(userInformationDTO);
        }

        [HttpPost]
        public  async Task<IActionResult> Index(UserUpdateInformationDTO userInformationDTO)
        {
            ViewBag.SidebarSize = "Small";

            var user = await _userService.GetHostUser();

            var result = _validator.Validate(userInformationDTO);

           
                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    
                }
            
                if(!ModelState.IsValid)
                {

                return View();

                }

         
            user.UserName = userInformationDTO.UserName != null ? userInformationDTO.UserName : user.UserName;
            
            user.Nickname = userInformationDTO.Nickname != null ? userInformationDTO.Nickname : user.Nickname;
            user.SecurityStamp = Guid.NewGuid().ToString();

           
                
                await _userManager.UpdateAsync(user);
            

            

            int code;
            Random random = new Random();
            
            
            
            
            if (userInformationDTO.Email  != null) { 
           
            code = random.Next(10000, 100000);
           


                 MimeMessage mimeMessage= new MimeMessage();
                 MailboxAddress mailboxAddressFrom = new MailboxAddress("ChatApp", "huseyineksici02@gmail.com");
                 MailboxAddress mailboxAddressTo = new MailboxAddress("User",$"{user.Email}");

                mimeMessage.From.Add(mailboxAddressFrom);
                mimeMessage.To.Add(mailboxAddressTo);

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Verification Code to Change Your Password: " + code;

                mimeMessage.Body = bodyBuilder.ToMessageBody();

                mimeMessage.Subject = "Chat App";

                var client = new SmtpClient();

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("huseyineksici02@gmail.com", "utvewewppwnddgck");
                client.Send(mimeMessage);
                client.Disconnect(true);

                

                ViewBag.ConfirmMailModal = true;
                ViewBag.Code = code;
                

            
                

                return View();

            }
          



            return View(userInformationDTO);
        }


        [HttpPost]
        public async Task<JsonResult> EmailVerification(int codeReceived, int emailCode)
        {
            if (codeReceived == emailCode)
            {

                Console.WriteLine("SAAAAAAAAAAA");

            }




            return Json(Ok());
        }


    }
}
