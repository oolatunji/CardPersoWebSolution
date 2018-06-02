using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class SystemModel
    {
        [Required]
        [DisplayName("Website Url")]
        public string WebsiteUrl { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [Required]
        [DisplayName("Database Host")]
        public string OracleDBHost { get; set; }
        [Required]
        [DisplayName("Database Port")]
        public string OracleDBPort { get; set; }
        [Required]
        [DisplayName("Database Service Name")]
        public string OracleDBServiceName { get; set; }
        [Required]
        [DisplayName("Database Username")]
        public string OracleDBUserId { get; set; }
        [Required]
        [DisplayName("Database User Password")]
        public string OracleDBPassword { get; set; }
        [Required]
        [DisplayName("From Email Address")]
        public string FromEmailAddress { get; set; }
        [Required]        
        public string SmtpUsername { get; set; }
        [Required]
        public string SmtpPassword { get; set; }
        [Required]
        [DisplayName("SMTP Server")]
        public string SmtpHost { get; set; }
        [Required]
        public string SmtpPort { get; set; }
        [Required]
        public string UsesActiveDirectory { get; set; }
        [Required]
        public string ADServer { get; set; }
        [Required]
        public string ADContainer { get; set; }
        [Required]
        public string ADUsername { get; set; }
        [Required]
        public string ADPassword { get; set; }
        [Required]
        public string ADServer2 { get; set; }
        [Required]
        public string ADContainer2 { get; set; }
        [Required]
        public string ADUsername2 { get; set; }
        [Required]
        public string ADPassword2 { get; set; }
    }
}