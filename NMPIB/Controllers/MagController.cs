using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using NMPIB.Models.Repositories;

namespace NMPIB.Controllers
{
    public class MagController : Controller
    {
        //
        // GET: /Mag/
        IMagazineRepository db = new MagazineRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var maglist = from m in db.GetMagazines()
                          where m.active == true
                          select new
                          {
                              m.id,
                              m.name
                          };
            return Json(maglist);
        }

    }
}
