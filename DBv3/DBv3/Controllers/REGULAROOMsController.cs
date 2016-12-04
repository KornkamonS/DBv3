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
    public class REGULAROOMsController : Controller
    {
        private Entities db = new Entities();

        // GET: REGULAROOMs
        public ActionResult Index()
        {
            var rEGULAROOMs = db.REGULAROOMs.Include(r => r.ROOM);
            return View(rEGULAROOMs.ToList());
        }

        // GET: REGULAROOMs/Details/5
        public ActionResult Details(int? id,int? id2)
        {
            if (id == null|| id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REGULAROOM rEGULAROOM = db.REGULAROOMs.Find(id,id2);
            if (rEGULAROOM == null)
            {
                return HttpNotFound();
            }
            return View(rEGULAROOM);
        }

        // GET: REGULAROOMs/Create
        public ActionResult Create()
        {
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM");
            return View();
        }

        // POST: REGULAROOMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SEVICE,INTERNET,IDROOM,IDBUILDING,PRICE")] REGULAROOM rEGULAROOM)
        {
            if (ModelState.IsValid)
            {
                db.REGULAROOMs.Add(rEGULAROOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", rEGULAROOM.IDROOM);
            return View(rEGULAROOM);
        }

        // GET: REGULAROOMs/Edit/5
        public ActionResult Edit(int? id,int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REGULAROOM rEGULAROOM = db.REGULAROOMs.Find(id,id2);
            if (rEGULAROOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "STATUS", rEGULAROOM.IDROOM);
            return View(rEGULAROOM);
        }

        // POST: REGULAROOMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SEVICE,INTERNET,IDROOM,IDBUILDING,PRICE")] REGULAROOM rEGULAROOM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEGULAROOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", rEGULAROOM.IDROOM);
            return View(rEGULAROOM);
        }

        // GET: REGULAROOMs/Delete/5
        public ActionResult Delete(int? id,int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REGULAROOM rEGULAROOM = db.REGULAROOMs.Find(id,id2);
            if (rEGULAROOM == null)
            {
                return HttpNotFound();
            }
            return View(rEGULAROOM);
        }

        // POST: REGULAROOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int id2)
        {
            REGULAROOM rEGULAROOM = db.REGULAROOMs.Find(id,id2);
            db.REGULAROOMs.Remove(rEGULAROOM);
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
