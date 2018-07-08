using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TemplateWeb.Controllers
{
    public class MobileController : Controller
    {
        public ActionResult Index()
        {
            if (!Request.Browser.IsMobileDevice)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}