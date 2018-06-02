using CardPerso.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Validators
{
    public class PasswordValidator : AbstractValidator<PasswordModel>
    {
        /// <summary>  
        /// Validator rules for Role Model  
        /// </summary>  
        public PasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}