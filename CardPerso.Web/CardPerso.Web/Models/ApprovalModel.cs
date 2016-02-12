using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class ApprovalModel
    {
        public int Id { get; set; }
        public string ApprovedBy { get; set; }
        public string Status { get; set; }
    }
}