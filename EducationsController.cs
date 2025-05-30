using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;
using System.Globalization;

namespace cngfapco.Controllers
{
    [Authorize]
    public class EducationsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Educations1
        public ActionResult Index()
        {
            var tbl_Educations = db.tbl_Educations.Include(e => e.VehicleType);
            return View(tbl_Educations.ToList());
        }
        // GET: Educations1
        public ActionResult WorkshopIndex(int? id)
        {
            var tbl_Educations = db.tbl_Educations.Where(e=>e.VehicleTypeId==id && e.Status==true).Include(e => e.VehicleType);
            return View(tbl_Educations.ToList());
        }
        // GET: Educations1
        public ActionResult SystemIndex(int? id)
        {
            var tbl_Educations = db.tbl_Educations.Where(e => e.ID == id && e.Status == false).Include(e => e.VehicleType);
            return View(tbl_Educations.ToList());
        }

        // GET: Educations1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.tbl_Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // GET: Educations1/Create
        public ActionResult Create()
        {
            ViewBag.VehicleTypeId = new SelectList(db.tbl_VehicleTypes, "ID", "Type");
            return View();
        }

        // POST: Educations1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VehicleTypeId,Description,Status,Version,Title,DownloadUrl,Cat")] Education education, HttpPostedFileBase File)
        {
            education.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if(education.Status==true)
                {
                    if (File != null)
                    {
                        if (education.File != null)
                        {
                            //System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/Procedure/" + education.File));
                        }
                        education.File = education.VehicleTypeId + "_" + File.FileName;
                        string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/Procedure/" + education.File);
                        File.SaveAs(ImagePath);
                    }
                }
                else
                {
                    if (File != null)
                    {
                        if (education.File != null)
                        {
                            //System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + education.File));
                        }
                        education.File = File.FileName;
                        string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/Education/" + education.File);
                        File.SaveAs(ImagePath);
                    }
                }
                //  
                db.tbl_Educations.Add(education);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VehicleTypeId = new SelectList(db.tbl_VehicleTypes, "ID", "Type", education.VehicleTypeId);
            return View(education);
        }

        // GET: Educations1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.tbl_Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleTypeId = new SelectList(db.tbl_VehicleTypes, "ID", "Type", education.VehicleTypeId);
            return View(education);
        }

        // POST: Educations1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VehicleTypeId,File,Description,Status,Version,Title,DownloadUrl")] Education education,HttpPostedFileBase File)
        {
            education.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    if (education.File != null)
                    {
                       // System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/Procedure/" + education.File));
                    }
                    education.File = education.VehicleTypeId + "_" + File.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/Procedure/" + education.File);
                    File.SaveAs(ImagePath);
                }
                //  
                db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehicleTypeId = new SelectList(db.tbl_VehicleTypes, "ID", "Type", education.VehicleTypeId);
            return View(education);
        }

        // GET: Educations1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.tbl_Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Educations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Education education = db.tbl_Educations.Find(id);
            db.tbl_Educations.Remove(education);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult System()
        {
            var tbl_Educations = db.tbl_Educations.Where(e => e.Status == false).Include(e => e.VehicleType);
            return View(tbl_Educations.ToList());
        }

        //GET: Educations/Procedure 
        public ActionResult Procedure()
        {
            var items = db.tbl_VehicleTypes;
            return View(items.ToList());
        }

        //GET: Educations/kit sequential 
        public ActionResult Sequential()
        {
            var items = db.tbl_Educations.Include(e=>e.VehicleType).Where(e=>e.Cat.Equals("Sequential"));
            return View(items.ToList());
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
