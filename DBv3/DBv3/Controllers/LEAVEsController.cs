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
    public class LEAVEsController : Controller
    {
        private Entities db = new Entities();

        // GET: LEAVEs
        public ActionResult Index()
        {
            var lEAVEs = db.LEAVEs.Include(l => l.PERSON);
            string x = Session["username"].ToString();
            if(Session["position"].ToString()!="owner"&& Session["position"].ToString() != "mananger")
            {
                lEAVEs = db.LEAVEs.Include(l => l.PERSON).Where(d => d.PERSON.USERNAME.Equals(x));
                return View("EmLe",lEAVEs.ToList());
            }
            return View(lEAVEs.ToList());
        }

        // GET: LEAVEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LEAVE lEAVE = db.LEAVEs.Find(id);
            if (lEAVE == null)
            {
                return HttpNotFound();
            }
            return View(lEAVE);
        }

        // GET: LEAVEs/Create
        public ActionResult Create()
        {
            ViewBag.EMID = new SelectList(db.People.Where(a=>a.POSITION.ToLower()!="resident"), "IDCARD", "USERNAME");
            return View();
        }

        // POST: LEAVEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NO,EMID,DATELEAVE,TIME,DETAIL,STATUS")] LEAVE lEAVE)
        {
            
            int id;
            if (db.LEAVEs.Count() == 0)
            {
                id = 1;
            }
            else
            {
                var find = db.LEAVEs.OrderByDescending(c => c.NO).FirstOrDefault();
                id = find.NO + 1;
            }
            lEAVE.NO = id;     
                if (ModelState.IsValid)
                {
                    db.LEAVEs.Add(lEAVE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
                       
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            return View(lEAVE);
        }
        public ActionResult EachCreate()
        {
            if (Session["position"] != null)
                if (Session["position"].ToString() != "resident")
                {
                    return View();
                }
            return RedirectToAction("Index", "Home");           
        }

        // POST: LEAVEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EachCreate([Bind(Include = "NO,EMID,DATELEAVE,TIME,DETAIL,STATUS")] LEAVE lEAVE)
        {

            string nowuser = Session["username"].ToString();
            int id;
            if (db.LEAVEs.Count() == 0)
            {
                id = 1;
            }
            else
            {
                var find = db.LEAVEs.OrderByDescending(c => c.NO).FirstOrDefault();
                id = find.NO + 1;
            }

            PERSON log = db.People.Where(a => a.USERNAME.Equals(nowuser)).FirstOrDefault();
            if (log != null)
            {
                lEAVE.EMID = log.IDCARD;
                lEAVE.NO = id;
                lEAVE.STATUS = "Waiting";
                if (ModelState.IsValid)
                {
                    db.LEAVEs.Add(lEAVE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            return View(lEAVE);
        }

        // GET: LEAVEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LEAVE lEAVE = db.LEAVEs.Find(id);
            if (lEAVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            return View(lEAVE);
        }

        // POST: LEAVEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NO,EMID,DATELEAVE,TIME,DETAIL,STATUS")] LEAVE lEAVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lEAVE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            return View(lEAVE);
        }

        // GET: LEAVEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LEAVE lEAVE = db.LEAVEs.Find(id);
            if (lEAVE == null)
            {
                return HttpNotFound();
            }
            return View(lEAVE);
        }

        // POST: LEAVEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LEAVE lEAVE = db.LEAVEs.Find(id);
            db.LEAVEs.Remove(lEAVE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StatusE(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LEAVE lEAVE = db.LEAVEs.Find(id);
            if (lEAVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            ViewBag.STATUS = new SelectList(Enum.GetValues(typeof(Droplist.YesorNo)));
            return View(lEAVE);
        }

        // POST: LEAVEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatusE([Bind(Include = "NO,EMID,DATELEAVE,TIME,DETAIL,STATUS")] LEAVE lEAVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lEAVE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMID = new SelectList(db.People.Where(a => a.POSITION.ToLower() != "resident"), "IDCARD", "USERNAME", lEAVE.EMID);
            ViewBag.STATUS = new SelectList(Enum.GetValues(typeof(Droplist.YesorNo)));
            return View(lEAVE);
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
