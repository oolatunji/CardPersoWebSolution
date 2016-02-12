using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardPerso.Web.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        public ActionResult RetrieveCards()
        {
            return View();
        }
    }
}