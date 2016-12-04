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
    public class ROOMsController : Controller
    {
        private Entities db = new Entities();

        // GET: ROOMs
        public ActionResult Index(string ROOMTYPE)
        {
            var rOOMs = db.ROOMs.Include(r => r.BUILDING).Include(r => r.REGULAROOM).Include(r => r.VIPROOM);
            ViewBag.ROOMTYPE = new SelectList(Enum.GetValues(typeof(Droplist.ROOMTYPE)));
            ViewBag.Type = ROOMTYPE;
            if (!string.IsNullOrEmpty(ROOMTYPE))
            {
                if (ROOMTYPE == "VIP")
                {
                    rOOMs = rOOMs.Where(s => s.IDROOM>400).OrderBy(s=>s.IDROOM);
                    return View(rOOMs.ToList());
                }
            }
            
            rOOMs = rOOMs.Where(s => s.IDROOM<400).OrderBy(s => s.IDROOM);
            return View(rOOMs.ToList());
         }

        // GET: ROOMs/Details/401?id2=1
        public ActionResult Details(int? id,int? id2)
        {
            if (id == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ROOM rOOM = db.ROOMs.Find(id, id2);
                if (rOOM == null)
                {
                    return HttpNotFound();
                }
                int count = 0;
                using (Entities ue = new Entities())
                {

                    var result = ue.People.Where(a => a.IDROOM == id && a.IDBUILDING == id2);
                    count = result.ToList().Count();
            }
                ViewBag.COUNT = count;
                return View(rOOM);

            } 
            

        }

        // GET: ROOMs/Create
        public ActionResult Create()
        {
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            ViewBag.IDROOM = new SelectList(db.REGULAROOMs, "IDROOM", "SEVICE");
            ViewBag.IDROOM = new SelectList(db.VIPROOMs, "IDROOM", "TIME");
            return View();
        }

        // POST: ROOMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDROOM,STATUS,MAID,CLEANTIME,CLEANDATE,IDBUILDING")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                db.ROOMs.Add(rOOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", rOOM.IDBUILDING);
            ViewBag.IDROOM = new SelectList(db.REGULAROOMs, "IDROOM", "SEVICE", rOOM.IDROOM);
            ViewBag.IDROOM = new SelectList(db.VIPROOMs, "IDROOM", "TIME", rOOM.IDROOM);
            return View(rOOM);
        }

        // GET: ROOMs/Edit/5
        public ActionResult Edit(int? id, int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOMs.Find(id,id2);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", rOOM.IDBUILDING);
            ViewBag.IDROOM = new SelectList(db.REGULAROOMs, "IDROOM", "SEVICE", rOOM.IDROOM);
            ViewBag.IDROOM = new SelectList(db.VIPROOMs, "IDROOM", "TIME", rOOM.IDROOM);
            return View(rOOM);
        }

        // POST: ROOMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDROOM,STATUS,MAID,CLEANTIME,CLEANDATE,IDBUILDING")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rOOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", rOOM.IDBUILDING);
            ViewBag.IDROOM = new SelectList(db.REGULAROOMs, "IDROOM", "SEVICE", rOOM.IDROOM);
            ViewBag.IDROOM = new SelectList(db.VIPROOMs, "IDROOM", "TIME", rOOM.IDROOM);
            return View(rOOM);
        }

        // GET: ROOMs/Delete/5
        public ActionResult Delete(int? id, int? id2)
        {
            if (id == null||id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOMs.Find(id,id2);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            return View(rOOM);
        }

        // POST: ROOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? id2)
        {
            ROOM rOOM = db.ROOMs.Find(id,id2);
            db.ROOMs.Remove(rOOM);
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
