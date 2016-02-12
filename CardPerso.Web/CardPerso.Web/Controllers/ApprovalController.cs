using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardPerso.Web.Controllers
{
    public class ApprovalController : Controller
    {
        // GET: Approval
        public ActionResult Configuration()
        {
            return View();
        }

        public ActionResult ApprovalList()
        {
            return View();
        }

        public ActionResult ViewApproval()
        {
            return View();
        }
    }
}