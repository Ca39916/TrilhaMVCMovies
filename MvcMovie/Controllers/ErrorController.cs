using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
   public class ErrorController : Controller
   {
        public ActionResult NotFound(string aspxerrorpath)
        {
            ViewBag.Message = aspxerrorpath;
            return View();
        }
        public ActionResult ServerError(string aspxerrorpath)
        {
            ViewBag.Message = aspxerrorpath;
            return View();
        }
    }
}