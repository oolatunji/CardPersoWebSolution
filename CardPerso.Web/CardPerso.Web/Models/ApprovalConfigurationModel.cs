using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class ApprovalConfigurationModel
    {
        public int Id { get; set; }
        public int Approve { get; set; }
        public ApprovalConfigurationModel[] Configurations { get; set; }
    }
}