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
    public class SALARYTBsController : Controller
    {
        private Entities db = new Entities();

        // GET: SALARYTBs
        public ActionResult Index()
        {
            var sALARYTBs = db.SALARYTBs.Include(s => s.PERSON);
            return View(sALARYTBs.ToList());
        }

        // GET: SALARYTBs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYTB sALARYTB = db.SALARYTBs.Find(id);
            if (sALARYTB == null)
            {
                return HttpNotFound();
            }
            return View(sALARYTB);
        }

        // GET: SALARYTBs/Create
        public ActionResult Create()
        {
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME");
            return View();
        }

        // POST: SALARYTBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCARD,SALARY")] SALARYTB sALARYTB)
        {
            if (ModelState.IsValid)
            {
                db.SALARYTBs.Add(sALARYTB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", sALARYTB.IDCARD);
            return View(sALARYTB);
        }

        // GET: SALARYTBs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYTB sALARYTB = db.SALARYTBs.Find(id);
            if (sALARYTB == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", sALARYTB.IDCARD);
            return View(sALARYTB);
        }

        // POST: SALARYTBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCARD,SALARY")] SALARYTB sALARYTB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sALARYTB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCARD = new SelectList(db.People, "IDCARD", "USERNAME", sALARYTB.IDCARD);
            return View(sALARYTB);
        }

        // GET: SALARYTBs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYTB sALARYTB = db.SALARYTBs.Find(id);
            if (sALARYTB == null)
            {
                return HttpNotFound();
            }
            return View(sALARYTB);
        }

        // POST: SALARYTBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SALARYTB sALARYTB = db.SALARYTBs.Find(id);
            db.SALARYTBs.Remove(sALARYTB);
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
