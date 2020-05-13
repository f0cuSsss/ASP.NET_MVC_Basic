using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_Basic_14_04_2020.Controllers
{
    public class MVController : Controller
    {
        // GET: MV
        public ActionResult Index(Models.AuthData authData)
        {

            if(Session["regOK"] != null)
            {
                ViewBag.RegMsg = Session["regOK"];
                Session["regOK"] = null;
            }

            return View();
        }

        public ActionResult RegUser()
        {
            if(Session["AddError"] != null)
            {
                ViewBag.ErrorMsg = Session["AddError"];
                Session["AddError"] = null;
            }
            return View();
        }

        public ActionResult AddUser(Models.RegData regData)
        {
            if (String.IsNullOrEmpty(regData.RegLogin))
            {
                Session["AddError"] = "Login is empty";
                return RedirectToRoute(new { controller="MV", action = "RegUser" });
            }

            if (String.IsNullOrEmpty(regData.RegPassword))
            {
                Session["AddError"] = "Password is empty";
                return RedirectToRoute(new { controller = "MV", action = "RegUser" });
            }

            if (String.IsNullOrEmpty(regData.RegRepeat))
            {
                Session["AddError"] = "Repeat password is empty";
                return RedirectToRoute(new { controller = "MV", action = "RegUser" });
            }

            if ( ! regData.RegPassword.Equals(regData.RegRepeat))
            {
                Session["AddError"] = "Passwords do not match";
                return RedirectToRoute(new { controller = "MV", action = "RegUser" });
            }

            // ============================

            // 1. Connection to DB
            SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["db1"].ConnectionString);

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                Session["AddError"] = ex.Message;
                return RedirectToRoute(new { controller = "MV", action = "RegUser" });
            }

            // 2. Command
            SqlCommand cmd = new SqlCommand(
                String.Format("INSERT INTO Users ([Login], [PassHash], [RealName], [RegDT]) " +
                                "VALUES ('{0}', '{1}', N'{2}', CURRENT_TIMESTAMP)", 
                                regData.RegLogin, Models.Util.GetSHA_256(regData.RegPassword), regData.RegRealName), con);

            // 3. 
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Session["AddError"] = ex.Message;
                return RedirectToRoute(new { controller = "MV", action = "RegUser" });
            }


            Session["regOK"] = "Registration successfully completed";
            return RedirectToRoute(new { controller="MV", action="Index" });
        }

        public ActionResult RazorDemo()
        {
            return View();
        }
    }
}