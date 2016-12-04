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
    public class BILLsController : Controller
    {
        private Entities db = new Entities();

        // GET: BILLs
        public ActionResult Index()
        {
            var bILLs = db.BILLs.Include(b => b.BUILDING).Include(b => b.PERSON);
            return View(bILLs.ToList());
        }

        // GET: BILLs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            return View(bILL);
        }

        // GET: BILLs/Create
        public ActionResult Create()
        {
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            ViewBag.IDRECEIVER = new SelectList(db.People.Where(a => a.POSITION.ToLower()== "receptionist"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: BILLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBILL,FITNESSBILL,ELECTRICITYBILL,WATERBILL,INTERNETBILL,CLEANBILL,DATEBILL,STATUS,IDBUILDING,IDRECEIVER,IDROOM")] BILL bILL)
        {
            if (ModelState.IsValid)
            {
                db.BILLs.Add(bILL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", bILL.IDBUILDING);
            ViewBag.IDRECEIVER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "receptionist"), "IDCARD", "USERNAME", bILL.IDRECEIVER);
            return View(bILL);
        }

        // GET: BILLs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", bILL.IDBUILDING);
            ViewBag.IDRECEIVER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "receptionist"), "IDCARD", "USERNAME", bILL.IDRECEIVER);
            return View(bILL);
        }

        // POST: BILLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBILL,FITNESSBILL,ELECTRICITYBILL,WATERBILL,INTERNETBILL,CLEANBILL,DATEBILL,STATUS,IDBUILDING,IDRECEIVER,IDROOM")] BILL bILL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bILL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", bILL.IDBUILDING);
            ViewBag.IDRECEIVER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "receptionist"), "IDCARD", "USERNAME", bILL.IDRECEIVER);
            return View(bILL);
        }

        // GET: BILLs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILL bILL = db.BILLs.Find(id);
            if (bILL == null)
            {
                return HttpNotFound();
            }
            return View(bILL);
        }

        // POST: BILLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BILL bILL = db.BILLs.Find(id);
            db.BILLs.Remove(bILL);
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
