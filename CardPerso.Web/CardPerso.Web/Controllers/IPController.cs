using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardPerso.Web.Controllers
{
    public class IPController : Controller
    {
        // GET: IP
        public ActionResult AddIP()
        {
            return View();
        }

        public ActionResult ViewIP()
        {
            return View();
        }
    }
}