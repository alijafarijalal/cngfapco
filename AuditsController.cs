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
    public class AuditsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Auditors
        public ActionResult Auditors(int? id)
        {
            var auditors = db.tbl_Users.Include(a => a.Companies).Where(a=>a.AuditCompaniesID==id);
            var company = db.tbl_AuditComponies.Find(id);
            var user = db.tbl_Users.Include(a => a.Companies).Where(a => a.Username == User.Identity.Name);
            ViewBag.Companies = company.Title;
            bool isAuditManager = false;
            if (auditors != null)
            {
                int userId = user.SingleOrDefault().UserID;
                isAuditManager = user.SingleOrDefault().isAuditManager;
                if (user.SingleOrDefault().AuditCompaniesID != null)
                {
                    if (isAuditManager == true)
                    {
                        return View(auditors.Where(a => a.isAuditManager == false).ToList());
                    }
                    else
                    {
                        return View(auditors.Where(a => a.UserID == userId).ToList());
                    }
                }
                else
                {
                    return View(auditors.ToList());
                }

            }
            else
            {
                return View(auditors.ToList());
            }
        }

        // GET: Audits
        public ActionResult Index(int? id)
        {
            var audits = db.tbl_Audits.Include(a => a.Workshops).Include(a => a.Users).Where(a=>a.UserID==id);
            var auditor = db.tbl_Users.Find(id);
            int? companiesID = auditor.AuditCompaniesID;
            var company = db.tbl_AuditComponies.Find(companiesID);
            ViewBag.CompaniesID = companiesID;
            ViewBag.Companies = company.Title;
            ViewBag.UserID = auditor.UserID;
            ViewBag.Auditor = auditor.Firstname + " " + auditor.Lastname;
            return View(audits.ToList());
        }

        // GET: Audits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit audit = db.tbl_Audits.Find(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            ViewBag.files = db.tbl_AuditFiles.Where(a => a.AuditID == audit.ID).Include(a=>a.Category).ToList();
            return View(audit);
        }

        // GET: Audits/Create
        public ActionResult Create(int? UserID, string Auditor)
        {
            ViewBag.Category = db.tbl_AuditCategories.ToList();
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            ViewBag.UserID = UserID; //new SelectList(db.tbl_Users, "UserID", "FullName",UserID);
            ViewBag.Auditor = Auditor;
            return View();
        }

        // POST: Audits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Auditer,AuditDate,WorkshopID,Description,UserID")] Audit audit, int?[] CategoryID,HttpPostedFileBase[] CheckList, HttpPostedFileBase[] Picture)
        {
            audit.CreateDate = DateTime.Now;
            audit.Creator = User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.tbl_Audits.Add(audit);
                db.SaveChanges();                
            }
            //
            AuditFile attached = new AuditFile();
            if(CategoryID.Count()>0)
            {
                for(int i=0;i < CategoryID.Count(); i++)
                {
                    if (CategoryID[i] != null)
                    {
                        attached.CategoryID = CategoryID[i];
                        attached.AuditID = audit.ID;
                        //
                        if (CheckList[i] != null)
                        {
                            //if (attached.CheckList != null)
                            //{
                            //    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Audits/CheckListFiles/" + attached.CheckList));
                            //}

                            attached.CheckList = attached.CategoryID + "_"+audit.WorkshopID + "_" + DateTime.Now.Year+DateTime.Now.Month + CheckList[i].FileName;

                            string ImagePath = Server.MapPath("/UploadedFiles/Audits/CheckListFiles/" + attached.CheckList);
                            CheckList[i].SaveAs(ImagePath);
                        }
                        //
                        if (Picture[i] != null)
                        {
                            attached.Picture = attached.CategoryID + "_" + audit.WorkshopID + "_" + DateTime.Now.Year + DateTime.Now.Month + Picture[i].FileName;

                            string ImagePath = Server.MapPath("/UploadedFiles/Audits/Pictures/" + attached.Picture);
                            Picture[i].SaveAs(ImagePath);
                        }
                        //
                        db.tbl_AuditFiles.Add(attached);
                        db.SaveChanges();                        
                    }
                }

                return RedirectToAction("Index",new { id=audit.UserID});
            }

            ViewBag.Category = db.tbl_AuditCategories.ToList();
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", audit.WorkshopID);
            ViewBag.UserID = audit.UserID; //new SelectList(db.tbl_Users, "UserID", "FullName",audit.UserID);
            ViewBag.Auditor = audit.Users.Firstname + " " + audit.Users.Lastname;
            return View(audit);
        }

        // GET: Audits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit audit = db.tbl_Audits.Find(id);
            if (audit == null)
            {
                return HttpNotFound();
            }

            ViewBag.Category = db.tbl_AuditCategories.ToList();
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", audit.WorkshopID);
            ViewBag.UserID = audit.UserID; //new SelectList(db.tbl_Users, "UserID", "FullName",UserID);
            ViewBag.Auditor = audit.Users.Firstname + " " + audit.Users.Lastname;
            return View(audit);
        }

        // POST: Audits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Auditer,AuditDate,WorkshopID,Description,UserID")] Audit audit,int?[] CategoryID, HttpPostedFileBase[] CheckList, HttpPostedFileBase[] Picture)
        {
            audit.CreateDate = DateTime.Now;
            audit.Creator = User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.Entry(audit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = audit.UserID });
            }
            //
            
            ViewBag.Category = db.tbl_AuditCategories.ToList();
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", audit.WorkshopID);
            ViewBag.UserID = audit.UserID; //new SelectList(db.tbl_Users, "UserID", "FullName",UserID);
            ViewBag.Auditor = audit.Users.Firstname + " " + audit.Users.Lastname;
            return View(audit);
        }

        // GET: Audits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit audit = db.tbl_Audits.Find(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            return View(audit);
        }

        // POST: Audits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Audit audit = db.tbl_Audits.Find(id);
            int? userId = audit.UserID;
            //for delete audit files
            var files = db.tbl_AuditFiles.Where(a => a.AuditID == id).ToList();
            foreach(var item in files)
            {
                //AuditFile file = db.tbl_AuditFiles.Find(item.AuditID);
                if (item.CheckList != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Audits/CheckListFiles/" + item.CheckList));
                }
                if (item.Picture != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Audits/Pictures/" + item.Picture));
                }
                db.tbl_AuditFiles.Remove(item);
                db.SaveChanges();
            }
            //
            db.tbl_Audits.Remove(audit);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = userId });
        }
        //
        public ActionResult AuditComponies()
        {
            int? companiesID=0;
            var userCompanies = db.tbl_Users.Where(u => u.Username == User.Identity.Name);
            if (userCompanies != null)
                companiesID = userCompanies.SingleOrDefault().AuditCompaniesID;
            var componyList = db.tbl_AuditComponies.ToList();
            if (companiesID > 0)
                return View(componyList.Where(c=>c.ID==companiesID));
            else
                return View(componyList);
        }
        //
        public ActionResult AuditFiles(int? id,string Auditor,string Workshop,int? AuditorID)
        {
            ViewBag.AuditorID = AuditorID;
            ViewBag.Workshop = Workshop;
            ViewBag.Auditor = Auditor;
            var files = db.tbl_AuditFiles.Where(u => u.AuditID == id);
            return View(files.ToList());
        }
        //
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
