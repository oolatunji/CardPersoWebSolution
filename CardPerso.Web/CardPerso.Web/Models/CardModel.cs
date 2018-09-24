using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Models
{
    public class CardModel
    {
        public string Pan { get; set; }
        public string PrintedName { get; set; }
        public string Username { get; set; }
        public int PrintStatus { get; set; }
        public string Status { get; set; }
        public string LoggedInUser { get; set; }
        public string OldData { get; set; }
        public string ID1 { get; set; }
    }
}