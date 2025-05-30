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
    public class WorkshopInsurancesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: WorkshopInsurances
        public ActionResult Index()
        {
            var tbl_WorkshopInsurance = db.tbl_WorkshopInsurance.Include(w => w.InsuranceCompanies).Include(w => w.InsuranceTypes).Include(w => w.Workshop);
            return View(tbl_WorkshopInsurance.ToList());
        }

        // GET: WorkshopInsurances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkshopInsurance workshopInsurance = db.tbl_WorkshopInsurance.Find(id);
            if (workshopInsurance == null)
            {
                return HttpNotFound();
            }
            return View(workshopInsurance);
        }

        // GET: WorkshopInsurances/Create
        public ActionResult Create()
        {
            ViewBag.InsuranceCompaniesID = new SelectList(db.tbl_InsuranceCompanies, "ID", "Title");
            ViewBag.InsuranceTypesID = new SelectList(db.tbl_InsuranceTypes, "ID", "Title");
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: WorkshopInsurances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WorkshopID,InsuranceCompaniesID,InsuranceTypesID,StartDate,FinishDate,Image,Description,Creator,CreateDate,Value")] WorkshopInsurance workshopInsurance)
        {
            workshopInsurance.CreateDate = DateTime.Now;
            workshopInsurance.Creator = User.Identity.Name;
            //
            if (ModelState.IsValid)
            {
                db.tbl_WorkshopInsurance.Add(workshopInsurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InsuranceCompaniesID = new SelectList(db.tbl_InsuranceCompanies, "ID", "Title", workshopInsurance.InsuranceCompaniesID);
            ViewBag.InsuranceTypesID = new SelectList(db.tbl_InsuranceTypes, "ID", "Title", workshopInsurance.InsuranceTypesID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", workshopInsurance.WorkshopID);
            return View(workshopInsurance);
        }

        // GET: WorkshopInsurances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkshopInsurance workshopInsurance = db.tbl_WorkshopInsurance.Find(id);
            if (workshopInsurance == null)
            {
                return HttpNotFound();
            }
            ViewBag.InsuranceCompaniesID = new SelectList(db.tbl_InsuranceCompanies, "ID", "Title", workshopInsurance.InsuranceCompaniesID);
            ViewBag.InsuranceTypesID = new SelectList(db.tbl_InsuranceTypes, "ID", "Title", workshopInsurance.InsuranceTypesID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", workshopInsurance.WorkshopID);
            return View(workshopInsurance);
        }

        // POST: WorkshopInsurances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WorkshopID,InsuranceCompaniesID,InsuranceTypesID,StartDate,FinishDate,Image,Description,Creator,CreateDate,Value")] WorkshopInsurance workshopInsurance)
        {
            workshopInsurance.CreateDate = DateTime.Now;
            workshopInsurance.Creator = User.Identity.Name;
            //
            if (ModelState.IsValid)
            {
                db.Entry(workshopInsurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InsuranceCompaniesID = new SelectList(db.tbl_InsuranceCompanies, "ID", "Title", workshopInsurance.InsuranceCompaniesID);
            ViewBag.InsuranceTypesID = new SelectList(db.tbl_InsuranceTypes, "ID", "Title", workshopInsurance.InsuranceTypesID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", workshopInsurance.WorkshopID);
            return View(workshopInsurance);
        }

        // GET: WorkshopInsurances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkshopInsurance workshopInsurance = db.tbl_WorkshopInsurance.Find(id);
            if (workshopInsurance == null)
            {
                return HttpNotFound();
            }
            return View(workshopInsurance);
        }

        // POST: WorkshopInsurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkshopInsurance workshopInsurance = db.tbl_WorkshopInsurance.Find(id);
            db.tbl_WorkshopInsurance.Remove(workshopInsurance);
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
