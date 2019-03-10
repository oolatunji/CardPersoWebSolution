using CardPerso.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    [Validator(typeof(RoleValidator))]
    public class RoleModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }        
        public FuncModel[] Functions { get; set; }
        public string SuperAdminRole { get; set; }

        public string LoggedInUser { get; set; }
        public string OldData { get; set; }
    }

    public class FuncModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}