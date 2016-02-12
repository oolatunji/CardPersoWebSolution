using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public FuncModel[] Functions { get; set; }

        public string LoggedInUser { get; set; }
    }

    public class FuncModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}