using CardPerso.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    [Validator(typeof(UserValidator))]
    public class UserModel
    {
        public int Id { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DisplayName("Other names")]
        public string Othernames { get; set; }        
        public string Gender { get; set; }
        [DisplayName("Valid Email")]
        public string Email { get; set; }        
        public string Username { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [DisplayName("Branch")]
        public int BranchId { get; set; }        
        public string BranchName { get; set; }
        public string LoggedInUser { get; set; }
    }
}