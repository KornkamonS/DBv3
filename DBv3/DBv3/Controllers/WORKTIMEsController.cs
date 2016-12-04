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
    public class WORKTIMEsController : Controller
    {
        private Entities db = new Entities();

        // GET: WORKTIMEs
        public ActionResult Index()
        {
            var wORKTIMEs = db.WORKTIMEs.Include(w => w.PERSON).OrderBy(a=>a.EMID);
            if (Session["username"] != null&& Session["position"].ToString() != "resident")
            {
                if (Session["position"].ToString() != "owner"&& Session["position"].ToString() != "manager")
                {
                    string nowuser = Session["username"].ToString();
                    PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
                    wORKTIMEs = db.WORKTIMEs.Where(a=>a.EMID.Equals(log.IDCARD)).Include(w=>w.PERSON).OrderBy(a=>a.EMID);
                    return View("EmIndex", wORKTIMEs.ToList());
                }
                return View("Index", wORKTIMEs.ToList()); 
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: WORKTIMEs/Details/5
        public ActionResult Details(long? id,string day,string str,string sto)
        {
            if (id == null||day==null||str==null||sto==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKTIME wORKTIME = db.WORKTIMEs.Find(id,day,str,sto);
            if (wORKTIME == null)
            {
                return HttpNotFound();
            }
            return View(wORKTIME);
        }

        // GET: WORKTIMEs/Create
        public ActionResult Create()
        {
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: WORKTIMEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMID,DATEWORKON,TIMESTART,TIMESTOP")] WORKTIME wORKTIME)
        {
            if (ModelState.IsValid)
            {
                db.WORKTIMEs.Add(wORKTIME);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", wORKTIME.EMID);
            return View(wORKTIME);
        }

        // GET: WORKTIMEs/Edit/5
        public ActionResult Edit(long? id, string day, string str, string sto)
        {
            if (id == null || day == null || str == null || sto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKTIME wORKTIME = db.WORKTIMEs.Find(id, day, str, sto);
            if (wORKTIME == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", wORKTIME.EMID);
            return View(wORKTIME);
        }

        // POST: WORKTIMEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMID,DATEWORKON,TIMESTART,TIMESTOP")] WORKTIME wORKTIME)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wORKTIME).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", wORKTIME.EMID);
            return View(wORKTIME);
        }

        // GET: WORKTIMEs/Delete/5
        public ActionResult Delete(long? id, string day, string str, string sto)
        {
            if (id == null || day == null || str == null || sto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKTIME wORKTIME = db.WORKTIMEs.Find(id, day, str, sto);
            if (wORKTIME == null)
            {
                return HttpNotFound();
            }
            return View(wORKTIME);
        }

        // POST: WORKTIMEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, string day, string str, string sto)
        {
            WORKTIME wORKTIME = db.WORKTIMEs.Find(id, day, str, sto);
            db.WORKTIMEs.Remove(wORKTIME);
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
