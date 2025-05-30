using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class CylinderDetailsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: CylinderDetails by id
        public ActionResult CylinderDetails(int? id)
        {
            ViewBag.Id = id;
            var constractor = db.tbl_TankConstractors.Find(id);
            if (constractor != null)
                ViewBag.constractor = constractor.Constractor;

            var tbl_CylinderDetails = db.tbl_CylinderDetails.Include(c => c.Constractors).Where(c=>c.ConstractorId==id);
            return View(tbl_CylinderDetails.ToList());
        }

        // GET: CylinderDetails
        public ActionResult Index()
        {
            var tbl_CylinderDetails = db.tbl_CylinderDetails.Include(c => c.Constractors);
            return View(tbl_CylinderDetails.ToList());
        }

        // GET: CylinderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CylinderDetail cylinderDetail = db.tbl_CylinderDetails.Find(id);
            if (cylinderDetail == null)
            {
                return HttpNotFound();
            }
            return View(cylinderDetail);
        }

        // GET: CylinderDetails/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankConstractor constractor = db.tbl_TankConstractors.Find(id);
            if (constractor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            ViewBag.constractor = constractor.Constractor;
            ViewBag.ConstractorId = new SelectList(db.tbl_TankConstractors, "ID", "Constractor",id);
            return View();
        }

        // POST: CylinderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ConstractorId,Bulk,Lenght,Pressure,Diameter,Rezve,Model")] CylinderDetail cylinderDetail)
        {
            if (ModelState.IsValid)
            {
                db.tbl_CylinderDetails.Add(cylinderDetail);
                db.SaveChanges();
                return RedirectToAction("CylinderDetails",new {id = cylinderDetail.ConstractorId });
            }

            ViewBag.ConstractorId = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", cylinderDetail.ConstractorId);
            return View(cylinderDetail);
        }

        // GET: CylinderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CylinderDetail cylinderDetail = db.tbl_CylinderDetails.Find(id);
            if (cylinderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.constractor = cylinderDetail.Constractors.Constractor;
            ViewBag.ConstractorId = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", cylinderDetail.ConstractorId);
            return View(cylinderDetail);
        }

        // POST: CylinderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ConstractorId,Bulk,Lenght,Pressure,Diameter,Rezve,Model")] CylinderDetail cylinderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cylinderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CylinderDetails", new { id = cylinderDetail.ConstractorId });
            }
            ViewBag.ConstractorId = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", cylinderDetail.ConstractorId);
            return View(cylinderDetail);
        }

        // GET: CylinderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CylinderDetail cylinderDetail = db.tbl_CylinderDetails.Find(id);
            if (cylinderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.constractor = cylinderDetail.Constractors.Constractor;
            return View(cylinderDetail);
        }

        // POST: CylinderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CylinderDetail cylinderDetail = db.tbl_CylinderDetails.Find(id);
            int? ConstractorId = cylinderDetail.ConstractorId;

            db.tbl_CylinderDetails.Remove(cylinderDetail);
            db.SaveChanges();
            return RedirectToAction("CylinderDetails", new { id = ConstractorId });
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
