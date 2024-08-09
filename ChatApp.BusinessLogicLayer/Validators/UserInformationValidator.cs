using ChatApp.BusinessLogicLayer.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Validators
{
    public class UserInformationValidator : AbstractValidator<UserInformationDTO>
    {
        public UserInformationValidator() {


            RuleFor(i => i.Email).EmailAddress().WithMessage("Please enter a valid email.");
            RuleFor(i => i.Password).Equal(i => i.ConfirmPassword).WithMessage("Make sure passwords are same.");
            RuleFor(i => i.UserName).Must(beAlphabetic).WithMessage("Please enter a valid username.");
        
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
