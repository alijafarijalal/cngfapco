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
    public class AuditorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Auditors
        public ActionResult Index()
        {
            var auditors = db.tbl_Users.Where(a=>a.AuditCompaniesID!=null).Include(a=>a.Companies);
            return View(auditors.ToList());
        }

        // GET: Auditors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditor auditor = db.tbl_Auditors.Find(id);
            if (auditor == null)
            {
                return HttpNotFound();
            }
            return View(auditor);
        }

        // GET: Auditors/Create
        public ActionResult Create()
        {
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title");
            return View();
        }

        // POST: Auditors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,CreateDate,Creator,CompaniesID")] Auditor auditor,HttpPostedFileBase Picture)
        {
            auditor.CreateDate = DateTime.Now;
            auditor.Creator = User.Identity.Name;
            //
            if (Picture != null)
            {
                if (auditor.Picture != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Auditors/" + auditor.Picture));
                }

                auditor.Picture = Picture.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Auditors/" + auditor.Picture);

                Picture.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                db.tbl_Auditors.Add(auditor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title",auditor.CompaniesID);
            return View(auditor);
        }

        // GET: Auditors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditor auditor = db.tbl_Auditors.Find(id);
            if (auditor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", auditor.CompaniesID);
            return View(auditor);
        }

        // POST: Auditors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,CompaniesID,CreateDate,Creator")] Auditor auditor, HttpPostedFileBase Picture)
        {
            auditor.CreateDate = DateTime.Now;
            auditor.Creator = User.Identity.Name;
            //
            if (Picture != null)
            {
                if (auditor.Picture != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Auditors/" + auditor.Picture));
                }

                auditor.Picture = Picture.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Auditors/" + auditor.Picture);

                Picture.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                db.Entry(auditor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", auditor.CompaniesID);
            return View(auditor);
        }

        // GET: Auditors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auditor auditor = db.tbl_Auditors.Find(id);
            if (auditor == null)
            {
                return HttpNotFound();
            }
            return View(auditor);
        }

        // POST: Auditors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Auditor auditor = db.tbl_Auditors.Find(id);
            db.tbl_Auditors.Remove(auditor);
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
