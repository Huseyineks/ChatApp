using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ChatApp.PresentationLayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Index(UserInformationDTO userInfo)
        {
            var logIn = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.Password,false,true);

            if (logIn.Succeeded) {

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError(String.Empty, "Username or password is wrong.");
                
                return View(userInfo);
            
            }


          

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Login");
        }

    }
}
