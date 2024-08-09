using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ChatApp.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<UserInformationDTO> _userValidator;
        public RegisterController(UserManager<AppUser> userManager, IValidator<UserInformationDTO> userValidator) {
            _userManager = userManager;
            _userValidator = userValidator;
      
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Index(UserInformationDTO userInfo)
        {

            var check = _userValidator.Validate(userInfo);

            foreach(var error in check.Errors) {

                ModelState.AddModelError(String.Empty, error.ErrorMessage);
            
            }
            if(!ModelState.IsValid)
            {
                return View(userInfo);
            }

            AppUser newUser = new AppUser()
            {
                Email = userInfo.Email,
                EmailConfirmed = true,
                Nickname = userInfo.Nickname,
                UserName = userInfo.UserName,

            };

            var result = await _userManager.CreateAsync(newUser,userInfo.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index","Login");
            }
            else
            {
                foreach(var error in  result.Errors) {

                    ModelState.AddModelError(String.Empty, error.Description);
                
                }
            }

            return View(userInfo);


        }
    }
}
