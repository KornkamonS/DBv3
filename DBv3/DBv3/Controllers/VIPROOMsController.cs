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
    public class VIPROOMsController : Controller
    {
        private Entities db = new Entities();

        // GET: VIPROOMs
        public ActionResult Index()
        {
            var vIPROOMs = db.VIPROOMs.Include(v => v.ROOM);
            return View(vIPROOMs.ToList());
        }

        // GET: VIPROOMs/Details/5
        public ActionResult Details(int? id,int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIPROOM vIPROOM = db.VIPROOMs.Find(id,id2);
            if (vIPROOM == null)
            {
                return HttpNotFound();
            }
            return View(vIPROOM);
        }

        // GET: VIPROOMs/Create
        public ActionResult Create()
        {
            ViewBag.IDROOM = new SelectList(db.ROOMs.Where(s=>s.IDROOM>400), "IDROOM", "IDROOM");
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            return View();
        }

        // POST: VIPROOMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDROOM,IDBUILDING,PRICE,TIME")] VIPROOM vIPROOM)
        {
            if (ModelState.IsValid)
            {
                db.VIPROOMs.Add(vIPROOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", vIPROOM.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME",vIPROOM.IDBUILDING);
            return View(vIPROOM);
        }

        // GET: VIPROOMs/Edit/5
        public ActionResult Edit(int? id, int? id2)
        {
            if (id == null||id2==null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIPROOM vIPROOM = db.VIPROOMs.Find(id,id2);
            if (vIPROOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", vIPROOM.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", vIPROOM.IDBUILDING);
            return View(vIPROOM);
        }

        // POST: VIPROOMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDROOM,IDBUILDING,PRICE,TIME")] VIPROOM vIPROOM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vIPROOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", vIPROOM.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", vIPROOM.IDBUILDING);
            return View(vIPROOM);
        }

        // GET: VIPROOMs/Delete/5
        public ActionResult Delete(int? id,int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIPROOM vIPROOM = db.VIPROOMs.Find(id,id2);
            if (vIPROOM == null)
            {
                return HttpNotFound();
            }
            return View(vIPROOM);
        }

        // POST: VIPROOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int id2)
        {
            VIPROOM vIPROOM = db.VIPROOMs.Find(id,id2);
            db.VIPROOMs.Remove(vIPROOM);
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
