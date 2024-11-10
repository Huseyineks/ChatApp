using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;

namespace ChatApp.PresentationLayer.Controllers
{
    public class EmailVerification : Controller
    {
        private readonly IUserService _userService;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private int code = 0;

        public EmailVerification(IUserService userService,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Password()
        {
            await SendEmail("Password");

            ViewBag.EmailCode = code;  




            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Password(ConfirmMailDTO confirmMailDTO)
        {
            var user = await _userService.GetHostUser();
            var newPassword = TempData["NewPassword"] as string;
            var currentPassword = TempData["CurrentPassword"] as string;
			if (confirmMailDTO.EmailCode == confirmMailDTO.CodeReceived)
			{

				var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();

                    return RedirectToAction("Index", "Login");
                    
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                }


			}
			else
			{
               


				


                TempData["ErrorMessage"] = "Code does not match";

                return RedirectToAction("ChangePassword", "User");


			}


			return View();
        }

        public async Task<IActionResult> Email()
        {
            await SendEmail("Email");

            ViewBag.EmailCode = code;


            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Email(ConfirmMailDTO confirmMailDTO)
        {
            var user = await _userService.GetHostUser();

            if(confirmMailDTO.EmailCode == confirmMailDTO.CodeReceived)
            {

                user.Email = TempData["NewEmail"] as string;

                await _userManager.UpdateAsync(user);

                TempData["SuccessMessage"] = "Your informations are updated succesfully.";

                return RedirectToAction("Index", "Home");


            }
          
                TempData["ErrorMessage"] = "Code does not match.";

                return RedirectToAction("Index", "User");

           




            
        }



        public async Task SendEmail(string type)
        {
            
            
            Random random = new Random();

            var user = await _userService.GetHostUser();


          
                code = random.Next(10000, 100000);



                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("ChatApp", "huseyineksici02@gmail.com");
                MailboxAddress mailboxAddressTo = new MailboxAddress("User", $"{user.Email}");

                mimeMessage.From.Add(mailboxAddressFrom);
                mimeMessage.To.Add(mailboxAddressTo);

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = $"Verification Code to Change Your {type}: " + code;

                mimeMessage.Body = bodyBuilder.ToMessageBody();

                mimeMessage.Subject = "Chat App";

                var client = new SmtpClient();

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("huseyineksici02@gmail.com", "utvewewppwnddgck");
                client.Send(mimeMessage);
                client.Disconnect(true);



               






                

           



            return;
        }
    }
}
