using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class IPModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("IP Address")]
        public string IPAddress { get; set; }
        public string Description { get; set; }
        public string LoggedInUser { get; set; }
        public string OldData { get; set; }
    }
}