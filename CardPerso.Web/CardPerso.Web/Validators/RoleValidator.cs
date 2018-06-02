using CardPerso.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Validators
{
    public class RoleValidator : AbstractValidator<RoleModel>
    {
        /// <summary>  
        /// Validator rules for User Model  
        /// </summary>  
        public RoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Functions).NotEmpty();
        }
    }
}