﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardPerso.Web.Responses
{
    public class AuthenticationResponse
    {
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; }
    }
}