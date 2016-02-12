﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class SearhFilter
    {
        public string Username { get; set; }
        public string Type { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string Status { get; set; }
        public DateTime? RequestedFrom { get; set; }
        public DateTime? RequestedTo { get; set; }
    }
}