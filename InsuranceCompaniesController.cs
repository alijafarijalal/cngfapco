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
    public class InsuranceCompaniesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: InsuranceCompanies
        public ActionResult Index()
        {
            return View(db.tbl_InsuranceCompanies.ToList());
        }

        // GET: InsuranceCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InsuranceCompany insuranceCompany = db.tbl_InsuranceCompanies.Find(id);
            if (insuranceCompany == null)
            {
                return HttpNotFound();
            }
            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InsuranceCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description")] InsuranceCompany insuranceCompany)
        {
            if (ModelState.IsValid)
            {
                db.tbl_InsuranceCompanies.Add(insuranceCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InsuranceCompany insuranceCompany = db.tbl_InsuranceCompanies.Find(id);
            if (insuranceCompany == null)
            {
                return HttpNotFound();
            }
            return View(insuranceCompany);
        }

        // POST: InsuranceCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description")] InsuranceCompany insuranceCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuranceCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuranceCompany);
        }

        // GET: InsuranceCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InsuranceCompany insuranceCompany = db.tbl_InsuranceCompanies.Find(id);
            if (insuranceCompany == null)
            {
                return HttpNotFound();
            }
            return View(insuranceCompany);
        }

        // POST: InsuranceCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InsuranceCompany insuranceCompany = db.tbl_InsuranceCompanies.Find(id);
            db.tbl_InsuranceCompanies.Remove(insuranceCompany);
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
