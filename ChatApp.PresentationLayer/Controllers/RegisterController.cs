using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.BusinessLogicLayer.Validators;
using ChatApp.EntitiesLayer.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ChatApp.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        
        
        private IValidator<UserInformationDTO> _userValidator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        public RegisterController(IValidator<UserInformationDTO> userValidator, IWebHostEnvironment webHostEnvironment = null, UserManager<AppUser> userManager = null)
        {
            
            _userValidator = userValidator;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View();
        }
        [AllowAnonymous]
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
                UserImage = UploadFile(userInfo)

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
        private string UploadFile(UserInformationDTO userInfo)
        {
            string? fileName = null;

            if (userInfo.UserImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + '-' + userInfo.UserImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    userInfo.UserImage.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
