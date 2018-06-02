using CardPerso.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    [Validator(typeof(PasswordValidator))]
    public class PasswordModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}