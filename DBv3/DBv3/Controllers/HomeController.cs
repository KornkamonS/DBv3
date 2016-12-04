using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBv3.Models;
using System.Web.Security;

namespace DBv3.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private Entities db = new Entities();

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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login lg)
        {
            if(ModelState.IsValid)
            {
                using (Entities ue = new Entities())
                {
                    PERSON log = ue.People.Where(a => a.USERNAME.Equals(lg.username) && a.PASSWORD.Equals(lg.password)).FirstOrDefault();                    
                    if(log!=null)
                    {
                        FormsAuthentication.SetAuthCookie(log.USERNAME, false);
                        Session["username"] = log.USERNAME;
                        Session["position"] = log.POSITION.ToLower();
                        return RedirectToAction("UserHome");                     
                    }
                    else
                    {
                        Response.Write("<script> alert('Invalid Password')</script>");
                    }
                }
            }
            return View();
        }
        public ActionResult UserHome()
        {
            string x = Session["username"].ToString();
            PERSON log = db.People.Where(a => a.USERNAME.Equals(x)).FirstOrDefault();

            if (log.POSITION == "resident")
            { return RedirectToAction("REGIndex", "User");     }
            else if (log.POSITION.ToLower() == "keeper")
            { return RedirectToAction("WatchmanIndex", "User"); }
            else if (log.POSITION.ToLower() == "maid")
            { return RedirectToAction("maidIndex", "User"); }
            else if (log.POSITION.ToLower() == "receptionist")
            { return RedirectToAction("RecIndex", "User"); }
            else if (log.POSITION.ToLower() == "owner")
            { return RedirectToAction("OwnerIndex", "User"); }
            else if (log.POSITION.ToLower() == "mananger")
            { return RedirectToAction("ManagerIndex", "User"); }
            else if (log.POSITION.ToLower() == "laundry")
            { return RedirectToAction("LaundryIndex", "User"); }
            return View("Index");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Home");
        }
    }
}