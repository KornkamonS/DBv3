using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBv3.Models;
using DBv3.ViewModels;
namespace DBv3.Controllers
{
    [Authorize]
    public class ABSENCsController : Controller
    {
        private Entities db = new Entities();

        // GET: ABSENCs
        public ActionResult Index(string YEAR, string MONTH)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (!string.IsNullOrEmpty(YEAR) && !string.IsNullOrEmpty(MONTH))
            {
                year = int.Parse(YEAR);
                month = int.Parse(MONTH);
            }

            var result = from d in db.ABSENCs.Include(p => p.PERSON).Where(d => d.DATEABSENC.Month == month && d.DATEABSENC.Year == year)
                         group d by d.EMID into grouped
                         select new Count { IDCARD = grouped.Key, count = grouped.Count(t => t.DATEABSENC != null), username = grouped.Where(a => a.PERSON.IDCARD == grouped.Key).Select(a => a.PERSON.USERNAME).FirstOrDefault(), mon = month, ye = year };

            return View(result.ToList());
        }

        // GET: ABSENCs/Details/5
        public ActionResult Details(int? id, int? mon, int? year)
        {
            if (id == null || mon == null || year == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aBSENC = db.ABSENCs.Where(a => a.EMID == id && a.DATEABSENC.Month == mon && a.DATEABSENC.Year == year);
            if (aBSENC == null)
            {
                return HttpNotFound();
            }
            return View(aBSENC);
        }

        // GET: ABSENCs/Create
        public ActionResult Create()
        {
            ViewBag.EMID = new SelectList(db.People.Where(a=>a.POSITION.ToLower()!="resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: ABSENCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NO,EMID,DATEABSENC")] ABSENC aBSENC)
        {
            int id;
            if (db.ABSENCs.Count() == 0)
            {
                id = 1;
            }
            else
            {
                var find = db.ABSENCs.OrderByDescending(c => c.NO).FirstOrDefault();
                id = find.NO + 1;
            }
            aBSENC.NO = id;
            if (ModelState.IsValid)
            {
                db.ABSENCs.Add(aBSENC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", aBSENC.EMID);
            return View(aBSENC);
        }

        // GET: ABSENCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ABSENC aBSENC = db.ABSENCs.Find(id);
            if (aBSENC == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", aBSENC.EMID);
            return View(aBSENC);
        }

        // POST: ABSENCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NO,EMID,DATEABSENC")] ABSENC aBSENC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aBSENC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", aBSENC.EMID);
            return View(aBSENC);
        }

        // GET: ABSENCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ABSENC aBSENC = db.ABSENCs.Find(id);
            if (aBSENC == null)
            {
                return HttpNotFound();
            }
            return View(aBSENC);
        }

        // POST: ABSENCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ABSENC aBSENC = db.ABSENCs.Find(id);
            db.ABSENCs.Remove(aBSENC);
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
