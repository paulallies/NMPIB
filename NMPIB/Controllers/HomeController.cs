using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Configuration;

namespace NMPIB.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return RedirectToAction("Index","Image");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
