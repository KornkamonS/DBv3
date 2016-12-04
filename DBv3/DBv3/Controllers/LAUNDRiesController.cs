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
    public class LAUNDRiesController : Controller
    {
        private Entities db = new Entities();

        // GET: LAUNDRies
        public ActionResult Index(string IDROOM,string IDBUILDING,string but)
        {
            var lAUNDRies = db.LAUNDRies.Include(l => l.ROOM1);
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM");
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            
            if (Session["position"].ToString() == "resident")
            {
                string nowuser = Session["username"].ToString();
                PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
                ViewBag.Room = log.IDROOM;
                BUILDING temp = db.BUILDINGs.Where(a => a.IDBUILDING == log.IDBUILDING).FirstOrDefault();
                ViewBag.Building = temp.NAME;

                lAUNDRies = db.LAUNDRies.Include(l => l.ROOM1).Where(a => a.ROOM == log.IDROOM && a.BUILDING == log.IDBUILDING);
                return View("REGlaundry", lAUNDRies.ToList());
            }
            else if (!string.IsNullOrEmpty(but))
            { if (!string.IsNullOrEmpty(IDROOM) && !string.IsNullOrEmpty(IDBUILDING) && but == "Search")
                {
                    int ROOMi = int.Parse(IDROOM);
                    int BUILDINGi = int.Parse(IDBUILDING);
                    lAUNDRies = db.LAUNDRies.Include(l => l.ROOM1).Where(a => a.ROOM == ROOMi && a.BUILDING == BUILDINGi);
                }
                else if (but == "reset") lAUNDRies = db.LAUNDRies.Include(l => l.ROOM1); }

            return View(lAUNDRies.ToList());
                  
            
        }
        
        // GET: LAUNDRies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LAUNDRY lAUNDRY = db.LAUNDRies.Find(id);
            if (lAUNDRY == null)
            {
                return HttpNotFound();
            }
            return View(lAUNDRY);
        }

        // GET: LAUNDRies/Create
        public ActionResult Create()
        {
            ViewBag.ROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM");
            ViewBag.BUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            return View();
        }

        // POST: LAUNDRies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ROOM,BUILDING,DATELAUNDRY,PRICE,STATUS")] LAUNDRY lAUNDRY)
        {
            if (ModelState.IsValid)
            {
                db.LAUNDRies.Add(lAUNDRY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ROOM = new SelectList(db.ROOMs, "IDROOM", "STATUS", lAUNDRY.ROOM);
            ViewBag.BUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", lAUNDRY.BUILDING);
            return View(lAUNDRY);
        }

        // GET: LAUNDRies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LAUNDRY lAUNDRY = db.LAUNDRies.Find(id);
            if (lAUNDRY == null)
            {
                return HttpNotFound();
            }
            ViewBag.ROOM = new SelectList(db.ROOMs, "IDROOM", "STATUS", lAUNDRY.ROOM);
            ViewBag.BUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", lAUNDRY.BUILDING);
            return View(lAUNDRY);
        }

        // POST: LAUNDRies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ROOM,BUILDING,DATELAUNDRY,PRICE,STATUS")] LAUNDRY lAUNDRY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lAUNDRY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ROOM = new SelectList(db.ROOMs, "IDROOM", "STATUS", lAUNDRY.ROOM);
            ViewBag.BUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", lAUNDRY.BUILDING);
            return View(lAUNDRY);
        }

        // GET: LAUNDRies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LAUNDRY lAUNDRY = db.LAUNDRies.Find(id);
            if (lAUNDRY == null)
            {
                return HttpNotFound();
            }
            return View(lAUNDRY);
        }

        // POST: LAUNDRies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LAUNDRY lAUNDRY = db.LAUNDRies.Find(id);
            db.LAUNDRies.Remove(lAUNDRY);
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
