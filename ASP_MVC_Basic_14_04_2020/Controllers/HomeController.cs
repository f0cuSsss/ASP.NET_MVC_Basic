using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_Basic_14_04_2020.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult History()
        {
            ViewBag.Message = "Your history page";

            return View();
        }

        public ActionResult Result(String id)
        {
            //String f = id.Substring(0, 2);

            //if(id.Substring(0, 2) == id.Substring(2, 2))
            //{
            //    ViewBag.id = "Greatings!";
            //}
            //else
            //{
            //    ViewBag.id = id;
            //}

            String[] x = id.Split('-');

            ViewBag.Message = "Welcome to result page!";

            ViewBag.id = Convert.ToInt32(x[0]) - Convert.ToInt32(x[1]);
            //ViewBag.id = id;

            return View();
        }
    }
}