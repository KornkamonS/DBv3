using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBv3.Models;

namespace DBv3.Controllers
{
    [Authorize]
    public class COMPLAINsController : Controller
    {
        private Entities db = new Entities();

        // GET: COMPLAINs
        public ActionResult Index()
        {
            var cOMPLAINs = db.COMPLAINs.Include(c => c.PERSON).OrderBy(c => c.IDCOMPLAIN);
            if (Session["position"] != null)
            {
                string nowuser = Session["position"].ToString();
                if (nowuser.ToLower() == "resident")
                { return View("REGindex", cOMPLAINs.ToList()); }
              
            }
            return View(cOMPLAINs.ToList());
        }

        // GET: COMPLAINs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPLAIN cOMPLAIN = db.COMPLAINs.Find(id);
            if (cOMPLAIN == null)
            {
                return HttpNotFound();
            }
            return View(cOMPLAIN);
        }

        // GET: COMPLAINs/Create
        public ActionResult Create()
        {
            ViewBag.IDCARD = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME");
            if (Session["position"] != null)
            {
                string nowuser = Session["position"].ToString();
                if (nowuser.ToLower() == "resident")
                { return View("REGCreate"); }
            }
            return View();
        }

        // POST: COMPLAINs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCARD,DETAIL,STATUS,IDCOMPLAIN")] COMPLAIN cOMPLAIN)
        {
            string nowuser = Session["username"].ToString();
            int id;
            if (db.COMPLAINs.Count()==0)
            {
                id = 1;
            }
            else
            {
                var find = db.COMPLAINs.OrderByDescending(c => c.IDCOMPLAIN).FirstOrDefault();
                id = find.IDCOMPLAIN + 1;
            }

            PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
                if (log != null)
                {
                    cOMPLAIN.IDCARD = log.IDCARD;
                    cOMPLAIN.IDCOMPLAIN = id;
                    cOMPLAIN.STATUS = "รอการตรวจสอบ";
                    if (ModelState.IsValid)
                    {
                        db.COMPLAINs.Add(cOMPLAIN);
                        db.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
            
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", cOMPLAIN.IDCARD);
            Response.Write("<script> alert('Error')</script>");
            return View(cOMPLAIN);
        }

        // GET: COMPLAINs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPLAIN cOMPLAIN = db.COMPLAINs.Find(id);
            if (cOMPLAIN == null)
            {
                return HttpNotFound();
            }           
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", cOMPLAIN.IDCARD);
            if (Session["position"] != null)
            {
                string nowuser = Session["position"].ToString();
                if (nowuser == "resident")
                { return View("REGedit", cOMPLAIN); }
                else { return View("Status", cOMPLAIN); }
            }
            return View(cOMPLAIN);
        }

        // POST: COMPLAINs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCARD,DETAIL,STATUS,IDCOMPLAIN")] COMPLAIN cOMPLAIN)
        {
            string nowuser = Session["position"].ToString();
            
                if (ModelState.IsValid)
            {
                db.Entry(cOMPLAIN).State = EntityState.Modified;
                db.SaveChanges();               
                return RedirectToAction("Index");
            }
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", cOMPLAIN.IDCARD);
            return View(cOMPLAIN);
        }

        // GET: COMPLAINs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPLAIN cOMPLAIN = db.COMPLAINs.Find(id);
            if (cOMPLAIN == null)
            {
                return HttpNotFound();
            }
            
            return View(cOMPLAIN);
        }

        // POST: COMPLAINs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMPLAIN cOMPLAIN = db.COMPLAINs.Find(id);
            db.COMPLAINs.Remove(cOMPLAIN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
