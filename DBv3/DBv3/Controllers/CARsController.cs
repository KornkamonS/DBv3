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
    public class CARsController : Controller
    {
        private Entities db = new Entities();

        // GET: CARs
        public ActionResult Index()
        {
            var cARs = db.CARs.Include(c => c.PERSON);
            return View(cARs.ToList());
        }

        // GET: CARs/Details/5
        public ActionResult Details(int? id,string pr)
        {
            if (id == null|| pr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAR cAR = db.CARs.Find(id,pr);
            if (cAR == null)
            {
                return HttpNotFound();
            }
            return View(cAR);
        }

        // GET: CARs/Create
        public ActionResult Create()
        {
            ViewBag.IDCARD = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: CARs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCARD,CARNUMBER,PROVINCE")] CAR cAR)
        {
            if (ModelState.IsValid)
            {
                db.CARs.Add(cAR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCARD = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", cAR.IDCARD);
            return View(cAR);
        }

        // GET: CARs/Edit/5
        public ActionResult Edit(int? id,string pr)
        {
            if (id == null||pr==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAR cAR = db.CARs.Find(id,pr);
            if (cAR == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCARD = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", cAR.IDCARD);
            return View(cAR);
        }

        // POST: CARs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCARD,CARNUMBER,PROVINCE")] CAR cAR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCARD = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "resident"), "IDCARD", "USERNAME", cAR.IDCARD);
            return View(cAR);
        }

        // GET: CARs/Delete/5
        public ActionResult Delete(int? id,string pr)
        {
            if (id == null||pr==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAR cAR = db.CARs.Find(id,pr);
            if (cAR == null)
            {
                return HttpNotFound();
            }
            return View(cAR);
        }

        // POST: CARs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string pr)
        {
            CAR cAR = db.CARs.Find(id,pr);
            db.CARs.Remove(cAR);
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
