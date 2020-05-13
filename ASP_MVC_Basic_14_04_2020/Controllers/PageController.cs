using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ASP_MVC_Basic_14_04_2020.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            if (Request.Params["min"] == null || Request.Params["max"] == null)
            {
                ViewBag.value = "Not enought parameters";
                //ViewBag.color = "Black";
            }
            else
            {
                try
                {
                    ViewBag.value = new Random().Next(int.Parse(Request.Params["min"]), 
                        int.Parse(Request.Params["max"])).ToString();
                    //ViewBag.color = Request.Params["color"];
                }
                catch
                {
                    ViewBag.value = "Invalid data format";
                }
            }
            return View();
        }

        public String rnd(String id)
        {
            if(id == null)
                return new Random().Next().ToString();
            else
                return new Random().Next(int.Parse(id)).ToString();
        }

        public String rnd1(String s)
        {
            return new Random().Next(Convert.ToInt32(s)).ToString();
        }

        public String RndLim()
        {
            if(Request.Params["min"] == null || Request.Params["max"] == null)
            {
                return "Not enought parameters";
            }

            try
            {
                return new Random().Next(int.Parse(Request.Params["min"]), int.Parse(Request.Params["max"])).ToString();
            }
            catch
            {
                return "Invalid data format";
            }

        }
    }
}