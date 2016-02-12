using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardPerso.Web.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult Configuration()
        {
            return View();
        }

        public ActionResult SystemConfiguration()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult AuditTrail()
        {
            return View();
        }

        public ActionResult NotFoundError()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}