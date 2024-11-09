using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Validators
{
    public class UserUpdateInformationValidator : AbstractValidator<UserUpdateInformationDTO>
    {

        private readonly IUserService _userService;
        public UserUpdateInformationValidator(IUserService user)
        {

            _userService = user;



            RuleFor(i => i.Email).EmailAddress().WithMessage("Please enter a valid email.").When(i => i.Email != null);
            RuleFor(i => i.Password).Equal(i => i.ConfirmPassword).WithMessage("Make sure passwords are same.").When(i => i.Password != null);
            RuleFor(i => i.UserName).Must(beAlphabetic).WithMessage("Please enter a valid username.").When(i => i.UserName != null);
            RuleFor(i => i.Nickname).Must(nicknameExisted).WithMessage("This nickname is already taken.").When(i => i.Nickname != null);
            



        }

        private bool nicknameExisted(string nickname)
        {
            var usersNicknames = _userService.GetAll().Select(i => i.Nickname);

            if (usersNicknames.Any(i => i.Equals(nickname)))
            {

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
