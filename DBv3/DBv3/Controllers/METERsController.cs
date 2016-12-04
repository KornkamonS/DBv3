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
    public class METERsController : Controller
    {
        private Entities db = new Entities();

        // GET: METERs
        public ActionResult Index()
        {
            var mETERs = db.METERs.Include(m => m.ROOM);
            if (Session["position"].ToString() == "resident")
                {
                    string nowuser = Session["username"].ToString();
                    PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();

                    mETERs = db.METERs.Include(m => m.ROOM).Where(m => m.IDROOM == log.IDROOM && m.IDBUILDING == log.IDBUILDING);

                    return View("REGmeter",mETERs.ToList());
                }
            
            return View(mETERs.ToList());
        }
        
        // GET: METERs/Details/5
        public ActionResult Details(int? room, int? building, string year, string month)
        {
            if (room == null || building == null || year == null||month==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            METER mETER = db.METERs.Find(room, year, month, building);
            if (mETER == null)
            {
                return HttpNotFound();
            }
            return View(mETER);
        }

        // GET: METERs/Create
        public ActionResult Create()
        {
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM");
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            ViewBag.MONTH = new SelectList(Enum.GetValues(typeof(Droplist.MONTH)));
            return View();
        }

        // POST: METERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDROOM,YEAR,MONTH,IDBUILDING,NOWATER,NOELECTRICITY")] METER mETER)
        {
            if (ModelState.IsValid)
            {
                db.METERs.Add(mETER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", mETER.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", mETER.IDBUILDING);
            ViewBag.MONTH = new SelectList(Enum.GetValues(typeof(Droplist.MONTH)), mETER.MONTH);
            return View(mETER);
        }

        // GET: METERs/Edit/5
        public ActionResult Edit(decimal? room, decimal? building, string year, string month)
        {
            if (room == null || building == null || year == null || month == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            METER mETER = db.METERs.Find(room,year,month,building);
            if (mETER == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", mETER.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME" ,mETER.IDBUILDING);
            ViewBag.MONTH = new SelectList(Enum.GetValues(typeof(Droplist.MONTH)), mETER.MONTH);
            return View(mETER);
        }

        // POST: METERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDROOM,YEAR,MONTH,IDBUILDING,NOWATER,NOELECTRICITY")] METER mETER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mETER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "STATUS", mETER.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", mETER.IDBUILDING);
            ViewBag.MONTH = new SelectList(Enum.GetValues(typeof(Droplist.MONTH)), mETER.MONTH);
            return View(mETER);
        }

        // GET: METERs/Delete/5
        public ActionResult Delete(int? room, int? building, string year, string month)
        {
            if (room == null || building == null || year==null || month == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            METER mETER = db.METERs.Find(room, year, month, building);
            if (mETER == null)
            {
                return HttpNotFound();
            }
            return View(mETER);
        }

        // POST: METERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int room, int building, string year, string month)
        {
            METER mETER = db.METERs.Find(room, year, month, building);
            db.METERs.Remove(mETER);
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
