using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class SystemModel
    {
        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        [Required]
        public string OracleDBHost { get; set; }
        [Required]
        public string OracleDBPort { get; set; }
        [Required]
        public string OracleDBServiceName { get; set; }
        [Required]
        public string OracleDBUserId { get; set; }
        [Required]
        public string OracleDBPassword { get; set; }
        [Required]
        public string FromEmailAddress { get; set; }
        [Required]
        public string SmtpUsername { get; set; }
        [Required]
        public string SmtpPassword { get; set; }
        [Required]
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