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
    public class LATEsController : Controller
    {
        private Entities db = new Entities();

        // GET: LATEs//
        
        public ActionResult Index(string YEAR, string MONTH)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;            

            if (!string.IsNullOrEmpty(YEAR) && !string.IsNullOrEmpty(MONTH))
            {
                year = int.Parse(YEAR);
                month = int.Parse(MONTH);
            }

            var result = from d in db.LATEs.Include(p => p.PERSON).Where(d => d.DATELATE.Month == month && d.DATELATE.Year == year)
                         group d by d.EMID into grouped
                         select new Count { IDCARD = grouped.Key, count = grouped.Count(t => t.DATELATE != null), username = grouped.Where(a => a.PERSON.IDCARD == grouped.Key).Select(a => a.PERSON.USERNAME).FirstOrDefault(),mon=month,ye=year };
            
            return View(result.ToList());
        }

        // GET: LATEs/Details/5
        public ActionResult Details(int? id,int?mon,int?year)
        {
            if (id == null||mon==null||year==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lATE = db.LATEs.Where(a=>a.EMID==id&&a.DATELATE.Month==mon&&a.DATELATE.Year==year);
            if (lATE == null)
            {
                return HttpNotFound();
            }
            return View(lATE);
        }

        // GET: LATEs/Create
        public ActionResult Create()
        {
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: LATEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NO,EMID,DATELATE,TIMEATTENDANCE")] LATE lATE)
        {
            int id;
            if (db.LATEs.Count() == 0)
            {
                id = 1;
            }
            else
            {
                var find = db.LATEs.OrderByDescending(c => c.NO).FirstOrDefault();
                id = find.NO + 1;
            }
            lATE.NO = id;
            if (ModelState.IsValid)
            {
                db.LATEs.Add(lATE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lATE.EMID);
            return View(lATE);
        }

        // GET: LATEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LATE lATE = db.LATEs.Find(id);
            if (lATE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lATE.EMID);
            return View(lATE);
        }

        // POST: LATEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NO,EMID,DATELATE,TIMEATTENDANCE")] LATE lATE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lATE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lATE.EMID);
            return View(lATE);
        }

        // GET: LATEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LATE lATE = db.LATEs.Find(id);
            if (lATE == null)
            {
                return HttpNotFound();
            }
            return View(lATE);
        }

        // POST: LATEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LATE lATE = db.LATEs.Find(id);
            db.LATEs.Remove(lATE);
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
