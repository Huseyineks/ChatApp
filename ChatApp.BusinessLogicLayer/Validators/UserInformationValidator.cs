using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.EntitiesLayer.Model;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Validators
{
    public class UserInformationValidator : AbstractValidator<UserInformationDTO>
    {
        private readonly IUserService _userService;
        public UserInformationValidator(IUserService user) {

            _userService = user;


            RuleFor(i => i.Email).EmailAddress().WithMessage("Please enter a valid email.").NotEmpty().WithMessage("Email field is required.");
            RuleFor(i => i.Password).Equal(i => i.ConfirmPassword).WithMessage("Make sure passwords are same.");
            RuleFor(i => i.UserName).Must(beAlphabetic).WithMessage("Please enter a valid username.");
            RuleFor(i => i.ConfirmPassword).NotEmpty().WithMessage("Confirm Password field is required.");
            RuleFor(i => i.Nickname).NotEmpty().WithMessage("Nickname is required.").Must(nicknameExisted).WithMessage("This nickname is already taken.");
            RuleFor(i => i.UserImage).NotEmpty().WithMessage("Profile Image is required.");

        }

        private bool nicknameExisted(string nickname)
        {
            var usersNicknames = _userService.GetAll().Select(i => i.Nickname);

            if(usersNicknames.Any(i => i.Equals(nickname))){

                return false;
            }
            return true;
        }

        private bool beAlphabetic(string username)
        {


            if (username != null)
            {
                return username.All(char.IsLetter);

            }
            return true;

        }
    }
}
