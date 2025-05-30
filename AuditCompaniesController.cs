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
    public class AuditCompaniesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: AuditCompanies
        public ActionResult Index()
        {
            return View(db.tbl_AuditComponies.ToList());
        }

        // GET: AuditCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditCompany auditCompany = db.tbl_AuditComponies.Find(id);
            if (auditCompany == null)
            {
                return HttpNotFound();
            }
            return View(auditCompany);
        }

        // GET: AuditCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description")] AuditCompany auditCompany)
        {
            if (ModelState.IsValid)
            {
                db.tbl_AuditComponies.Add(auditCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auditCompany);
        }

        // GET: AuditCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditCompany auditCompany = db.tbl_AuditComponies.Find(id);
            if (auditCompany == null)
            {
                return HttpNotFound();
            }
            return View(auditCompany);
        }

        // POST: AuditCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description")] AuditCompany auditCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditCompany);
        }

        // GET: AuditCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditCompany auditCompany = db.tbl_AuditComponies.Find(id);
            if (auditCompany == null)
            {
                return HttpNotFound();
            }
            return View(auditCompany);
        }

        // POST: AuditCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditCompany auditCompany = db.tbl_AuditComponies.Find(id);
            db.tbl_AuditComponies.Remove(auditCompany);
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
