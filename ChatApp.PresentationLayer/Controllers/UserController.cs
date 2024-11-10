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
            
        if(userInformationDTO.Email != null)
            {
                TempData["NewEmail"] = userInformationDTO.Email;

                return RedirectToAction("Email", "EmailVerification");
            }
          



            return View(userInformationDTO);
        }

        public  IActionResult ChangePassword()
        {
			ViewBag.SidebarSize = "Small";


			return View();
        }

        [HttpPost]


        public IActionResult ChangePassword(UserUpdateInformationDTO userInformationDTO)
        {
            ViewBag.SidebarSize = "Small";

            var result = _validator.Validate(userInformationDTO);

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty,error.ErrorMessage);
            }

            if(!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if(userInformationDTO.Password != null)
                {
                    TempData["NewPassword"] = userInformationDTO.Password;
                    TempData["CurrentPassword"] = userInformationDTO.CurrentPassword;


                    return RedirectToAction("Password", "EmailVerification");
                }
            }


            return View();
        }

       


    }
}
