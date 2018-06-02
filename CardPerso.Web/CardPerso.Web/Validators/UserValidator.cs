using CardPerso.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        /// <summary>  
        /// Validator rules for User Model  
        /// </summary>  
        public UserValidator()
        {
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Othernames).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.RoleId).NotEqual(0);
            RuleFor(x => x.BranchId).NotEqual(0);
        }
    }
}