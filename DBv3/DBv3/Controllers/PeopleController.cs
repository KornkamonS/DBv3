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
    public class PeopleController : Controller
    {
        private Entities db = new Entities();

        // GET: People
        
        public ActionResult Index(string searchString)
        {
            var residents = db.People.Include(p => p.ROOM);

            if (!string.IsNullOrEmpty(searchString))
            {
                residents = residents.Where(s => s.USERNAME.ToLower().Contains(searchString.ToLower()));          
            }
            return View(residents.ToList());            
        }
        public ActionResult Resident(string searchString)
        {
            var residents = db.People.Include(p => p.ROOM);

            if (!string.IsNullOrEmpty(searchString))
            {
                residents = residents.Where(s => s.USERNAME.ToLower().Contains(searchString.ToLower()) && (s.POSITION.ToLower() == "resident"));
                return View(residents.ToList());
            }
            else
            {
                residents = residents.Where(s => s.POSITION.ToLower() == "resident");
                return View(residents.ToList());
            }
        }
        public ActionResult Employee(string searchString)
        {
            var residents = db.People.Include(p => p.ROOM);

            if (!string.IsNullOrEmpty(searchString))
            {
                residents = residents.Where(s => s.USERNAME.ToLower().Contains(searchString.ToLower()) && (s.POSITION.ToLower() != "resident"));
                return View(residents.ToList());
            }
            else
            {
                residents = residents.Where(s => s.POSITION.ToLower() != "resident");
                return View(residents.ToList());
            }
        }

        // GET: People/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string x = Session["username"].ToString();
            var nowuser = db.People.Where(a => a.USERNAME.Equals(x)).FirstOrDefault();
            PERSON pERSON = db.People.Find(id);
            if (pERSON == null)
            {
                return HttpNotFound();
            }
            if(Session["position"].ToString()!="resident"&&pERSON.POSITION=="resident")
            {
                return View("ResDetails", pERSON);
            }
            else if (Session["position"].ToString() != "resident" && pERSON.POSITION != "resident")
            {
                return View("EmDetails", pERSON);
            }
            else if (nowuser!=null && nowuser.IDCARD==pERSON.IDCARD)
            { return View(pERSON); }
            return RedirectToAction("Index","Home");

        }

        // GET: People/Create
        public ActionResult ResCreate()
        {
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM");
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME");
            ViewBag.GENDER = new SelectList(Enum.GetValues(typeof(Droplist.Gender)));
            
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResCreate([Bind(Include = "IDCARD,USERNAME,PASSWORD,NAME,LASTNAME,GENDER,POSITION,IDROOM,HIREDATE,ADDRESS,PHONENUMBER,IDBUILDING,EMAIL")] PERSON pERSON)
        {
            if (ModelState.IsValid)
            {
                int count = db.People.Where(p => p.USERNAME.Equals(pERSON.USERNAME)).ToList().Count();
                if (count==0)
                {
                    int roommate = db.People.Where(a => a.IDBUILDING == pERSON.IDBUILDING && a.IDROOM == pERSON.IDROOM).ToList().Count();
                    if(roommate>2) { Response.Write("<script> alert('Room is full')</script>"); }
                    else
                    {
                        pERSON.POSITION = "Resident";
                        db.People.Add(pERSON);
                        db.SaveChanges();
                        ROOM rest = db.ROOMs.Find(pERSON.IDROOM, pERSON.IDBUILDING);
                        rest.STATUS = "Not available";
                        db.Entry(rest).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Resident");
                    }
                }
                else
                {
                    Response.Write("<script> alert('Username is duplicate')</script>");
                }        
            }

            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", pERSON.IDROOM);           
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", pERSON.IDBUILDING); 
            ViewBag.GENDER = new SelectList(Enum.GetValues(typeof(Droplist.Gender)));
            return View(pERSON);
        }

        public ActionResult EmCreate()
        {
            
            ViewBag.GENDER = new SelectList(Enum.GetValues(typeof(Droplist.Gender)));
            ViewBag.POSITION = new SelectList(Enum.GetValues(typeof(Droplist.Position)));
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmCreate([Bind(Include = "IDCARD,USERNAME,PASSWORD,NAME,LASTNAME,GENDER,POSITION,IDROOM,HIREDATE,ADDRESS,PHONENUMBER,IDBUILDING,EMAIL")] PERSON pERSON)
        {
            if (ModelState.IsValid)
            {
                int count = db.People.Where(p => p.USERNAME.Equals(pERSON.USERNAME)).ToList().Count();
                if (count == 0)
                {
                    db.People.Add(pERSON);
                    db.SaveChanges();
                    return RedirectToAction("Employee");
                }
                else
                {
                    Response.Write("<script> alert('Username is duplicate')</script>");
                }
            }
           
            ViewBag.GENDER = new SelectList(Enum.GetValues(typeof(Droplist.Gender)));
            ViewBag.POSITION = new SelectList(Enum.GetValues(typeof(Droplist.Position)));
            return View(pERSON);
        }

        // GET: People/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PERSON pERSON = db.People.Find(id);
            if (pERSON == null)
            {
                return HttpNotFound();
            }

            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", pERSON.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", pERSON.IDBUILDING);  
            ViewBag.POSITION = new SelectList(Enum.GetValues(typeof(Droplist.Position)));

            string x = Session["username"].ToString();
            var nowuser = db.People.Where(a => a.USERNAME.Equals(x)).FirstOrDefault();           
            if (pERSON == null)
            {
                return HttpNotFound();
            }
            if (Session["position"].ToString() != "resident" && pERSON.POSITION == "resident")
            {
                return View("ResEdit", pERSON);
            }
            else if (Session["position"].ToString() != "resident" && pERSON.POSITION != "resident")
            {
                return View("EmEdit", pERSON);
            }
            else if (nowuser != null && nowuser.IDCARD == pERSON.IDCARD)
            { return View(pERSON); }
            return RedirectToAction("Index", "Home");           
        }
        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCARD,USERNAME,PASSWORD,NAME,LASTNAME,GENDER,POSITION,IDROOM,HIREDATE,ADDRESS,PHONENUMBER,IDBUILDING,EMAIL")] PERSON pERSON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pERSON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.IDROOM = new SelectList(db.ROOMs, "IDROOM", "IDROOM", pERSON.IDROOM);
            ViewBag.IDBUILDING = new SelectList(db.BUILDINGs, "IDBUILDING", "NAME", pERSON.IDBUILDING); 
            ViewBag.POSITION = new SelectList(Enum.GetValues(typeof(Droplist.Position)));
            return RedirectToAction("Edit",pERSON.IDCARD); 
        }

        // GET: People/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PERSON pERSON = db.People.Find(id);
            if (pERSON == null)
            {
                return HttpNotFound();
            }

            string x = Session["username"].ToString();
            var nowuser = db.People.Where(a => a.USERNAME.Equals(x)).FirstOrDefault();
            
            if (pERSON == null)
            {
                return HttpNotFound();
            }
            if (Session["position"].ToString() != "resident" && pERSON.POSITION == "resident")
            {
                return View("ResDelete", pERSON);
            }
            else if (Session["position"].ToString() != "resident" && pERSON.POSITION != "resident")
            {
                return View("EmDelete", pERSON);
            }
            else if (nowuser != null && nowuser.IDCARD == pERSON.IDCARD)
            { return View(pERSON); }
            return RedirectToAction("Index", "Home");
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PERSON pERSON = db.People.Find(id);
            db.People.Remove(pERSON);
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
