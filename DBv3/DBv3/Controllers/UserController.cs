using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBv3.Models;
using System.Net;

namespace DBv3.Controllers
{
    [Authorize]
    // GET: Regular
    public class UserController : Controller
    {
        private Entities db = new Entities();

        public ActionResult REGIndex()
        {
            string user = Session["username"].ToString();
            var log = db.People.Where(a => a.USERNAME.Equals(user)).FirstOrDefault();           
            return View(log);              
        }
        
        public ActionResult WatchmanIndex()
        {           
            return View();
        }
        public ActionResult maidIndex()
        {
            return View();
        }
        public ActionResult RecIndex()
        {
            return View();
        }
        public ActionResult OwnerIndex()
        {
            return View();
        }
        public ActionResult ManagerIndex()
        {
            return View();
        }
        public ActionResult LaundryIndex()
        {
            return View();
        }
        public ActionResult FollowEM()
        {
           return View();
        }
    }
}




