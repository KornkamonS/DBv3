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
    public class FITNESSesController : Controller
    {
        private Entities db = new Entities();

        // GET: FITNESSes
        public ActionResult Index()
        {
            var fITNESSes = db.FITNESSes.Include(f => f.PERSON);
            return View(fITNESSes.ToList());
        }

        // GET: FITNESSes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FITNESS fITNESS = db.FITNESSes.Find(id);
            if (fITNESS == null)
            {
                return HttpNotFound();
            }
            return View(fITNESS);
        }

        // GET: FITNESSes/Create
        public ActionResult Create()
        {
            ViewBag.RESIDENT = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: FITNESSes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NO,RESIDENT,DAY,TIME,PRICE")] FITNESS fITNESS)
        {
            if (ModelState.IsValid)
            {
                db.FITNESSes.Add(fITNESS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RESIDENT = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", fITNESS.RESIDENT);
            return View(fITNESS);
        }

        // GET: FITNESSes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FITNESS fITNESS = db.FITNESSes.Find(id);
            if (fITNESS == null)
            {
                return HttpNotFound();
            }
            ViewBag.RESIDENT = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", fITNESS.RESIDENT);
            return View(fITNESS);
        }

        // POST: FITNESSes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NO,RESIDENT,DAY,TIME,PRICE")] FITNESS fITNESS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fITNESS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RESIDENT = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", fITNESS.RESIDENT);
            return View(fITNESS);
        }

        // GET: FITNESSes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FITNESS fITNESS = db.FITNESSes.Find(id);
            if (fITNESS == null)
            {
                return HttpNotFound();
            }
            return View(fITNESS);
        }

        // POST: FITNESSes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FITNESS fITNESS = db.FITNESSes.Find(id);
            db.FITNESSes.Remove(fITNESS);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult REGfitness()
        {
            string nowuser = Session["username"].ToString();
            PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
            var fITNESSes =
                (from u in db.People
                 join f in db.FITNESSes on u.IDCARD equals f.RESIDENT
                 where (u.IDROOM == log.IDROOM && u.IDBUILDING == log.IDBUILDING)
                 select f
                 );
            var view = fITNESSes.Include(c => c.PERSON);
            return View(view.ToList()); ;
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
