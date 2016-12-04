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
    public class CARPARKsController : Controller
    {
       
        private Entities db = new Entities();

        // GET: CARPARKs
        public ActionResult Index()
        {
            var cARPARKs = db.CARPARKs.Include(c => c.CAR).Include(c => c.PERSON);
            if(Session["position"].ToString()=="resident")
                {
                    string nowuser = Session["username"].ToString();
                    PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
                    CAR OWN = db.CARs.Where(a => a.IDCARD == log.IDCARD).FirstOrDefault();
                    cARPARKs = db.CARPARKs.Include(c => c.CAR).Include(c => c.PERSON).Where(c => c.CARNUMBER == OWN.CARNUMBER && c.PROVINCE == OWN.PROVINCE);
                    return View("REGcarpark", cARPARKs.ToList());
                }                
            
            return View(cARPARKs.ToList());
        }
        
        // GET: CARPARKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARPARK cARPARK = db.CARPARKs.Find(id);
            if (cARPARK == null)
            {
                return HttpNotFound();
            }
            return View(cARPARK);
        }

        // GET: CARPARKs/Create
        public ActionResult Create()
        {
            ViewBag.CARNUMBER = new SelectList(db.CARs, "CARNUMBER", "PROVINCE");
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: CARPARKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NO,TIMEIN,TIMEOUT,CARNUMBER,PROVINCE,IDRECORDER")] CARPARK cARPARK)
        {
            if (ModelState.IsValid)
            {
                db.CARPARKs.Add(cARPARK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CARNUMBER = new SelectList(db.CARs, "CARNUMBER", "PROVINCE", cARPARK.CARNUMBER);
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", cARPARK.IDRECORDER);
            return View(cARPARK);
        }

        // GET: CARPARKs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARPARK cARPARK = db.CARPARKs.Find(id);
            if (cARPARK == null)
            {
                return HttpNotFound();
            }
            ViewBag.CARNUMBER = new SelectList(db.CARs, "CARNUMBER", "PROVINCE", cARPARK.CARNUMBER);
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", cARPARK.IDRECORDER);
            return View(cARPARK);
        }

        // POST: CARPARKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NO,TIMEIN,TIMEOUT,CARNUMBER,PROVINCE,IDRECORDER")] CARPARK cARPARK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cARPARK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CARNUMBER = new SelectList(db.CARs, "CARNUMBER", "PROVINCE", cARPARK.CARNUMBER);
            ViewBag.IDRECORDER = new SelectList(db.People.Where(a => a.POSITION.ToLower() == "keeper"), "IDCARD", "USERNAME", cARPARK.IDRECORDER);
            return View(cARPARK);
        }

        // GET: CARPARKs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARPARK cARPARK = db.CARPARKs.Find(id);
            if (cARPARK == null)
            {
                return HttpNotFound();
            }
            return View(cARPARK);
        }

        // POST: CARPARKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CARPARK cARPARK = db.CARPARKs.Find(id);
            db.CARPARKs.Remove(cARPARK);
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
