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
    public class BUILDINGsController : Controller
    {
        private Entities db = new Entities();

        // GET: BUILDINGs
        public ActionResult Index()
        {
            var bUILDINGs = db.BUILDINGs.Include(b => b.PERSON);
            return View(bUILDINGs.ToList());
        }

        // GET: BUILDINGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BUILDING bUILDING = db.BUILDINGs.Find(id);
            if (bUILDING == null)
            {
                return HttpNotFound();
            }
            return View(bUILDING);
        }

        // GET: BUILDINGs/Create
        public ActionResult Create()
        {
            ViewBag.KEEPER = new SelectList(db.People.Where(a=>a.POSITION.ToLower()=="keeper"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: BUILDINGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KEEPER,NAME,IDBUILDING")] BUILDING bUILDING)
        {
            if (ModelState.IsValid)
            {
                db.BUILDINGs.Add(bUILDING);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KEEPER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", bUILDING.KEEPER);
            return View(bUILDING);
        }

        // GET: BUILDINGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BUILDING bUILDING = db.BUILDINGs.Find(id);
            if (bUILDING == null)
            {
                return HttpNotFound();
            }
            ViewBag.KEEPER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", bUILDING.KEEPER);
            return View(bUILDING);
        }

        // POST: BUILDINGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KEEPER,NAME,IDBUILDING")] BUILDING bUILDING)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bUILDING).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KEEPER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", bUILDING.KEEPER);
            return View(bUILDING);
        }

        // GET: BUILDINGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BUILDING bUILDING = db.BUILDINGs.Find(id);
            if (bUILDING == null)
            {
                return HttpNotFound();
            }
            return View(bUILDING);
        }

        // POST: BUILDINGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BUILDING bUILDING = db.BUILDINGs.Find(id);
            db.BUILDINGs.Remove(bUILDING);
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
