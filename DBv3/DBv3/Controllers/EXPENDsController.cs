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
    public class EXPENDsController : Controller
    {
        private Entities db = new Entities();

        // GET: EXPENDs
        public ActionResult Index()
        {
            var eXPENDs = db.EXPENDs.Include(e => e.PERSON);
            return View(eXPENDs.ToList());
        }

        // GET: EXPENDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPEND eXPEND = db.EXPENDs.Find(id);
            if (eXPEND == null)
            {
                return HttpNotFound();
            }
            return View(eXPEND);
        }

        // GET: EXPENDs/Create
        public ActionResult Create()
        {
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: EXPENDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DETAIL,PRICE,DATEEXPEND,IDRECORDER")] EXPEND eXPEND)
        {
            if (ModelState.IsValid)
            {
                db.EXPENDs.Add(eXPEND);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDRECORDER = new SelectList(db.People, "IDCARD", "USERNAME", eXPEND.IDRECORDER);
            return View(eXPEND);
        }

        // GET: EXPENDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPEND eXPEND = db.EXPENDs.Find(id);
            if (eXPEND == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", eXPEND.IDRECORDER);
            return View(eXPEND);
        }

        // POST: EXPENDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DETAIL,PRICE,DATEEXPEND,IDRECORDER")] EXPEND eXPEND)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eXPEND).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", eXPEND.IDRECORDER);
            return View(eXPEND);
        }

        // GET: EXPENDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPEND eXPEND = db.EXPENDs.Find(id);
            if (eXPEND == null)
            {
                return HttpNotFound();
            }
            return View(eXPEND);
        }

        // POST: EXPENDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EXPEND eXPEND = db.EXPENDs.Find(id);
            db.EXPENDs.Remove(eXPEND);
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
