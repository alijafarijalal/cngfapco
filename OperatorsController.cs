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
    [Authorize]
    public class OperatorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Operators
        public ActionResult Index()
        {
            var tbl_Operators = db.tbl_Operators.Include(o=> o.Workshop);
            return View(tbl_Operators.ToList());
        }

        // GET: Operators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = db.tbl_Operators.Find(id);
            if (@operator == null)
            {
                return HttpNotFound();
            }
            return View(@operator);
        }

        // GET: Operators/Create
        public ActionResult Create()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: Operators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WorkshopID,Name,Family,PhoneNumber,MobileNumber,Email,Address")] Operator @operator,HttpPostedFileBase Image)
        {
            @operator.CreateDate = DateTime.Now;
            @operator.Creator = User.Identity.Name;
            @operator.Status = true;

            if (Image != null)
            {
                if (@operator.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/Operators/" + @operator.Image));
                }

                @operator.Image = @operator.Family+"_"+ @operator.Name + Image.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/Operators/" + @operator.Image);

                Image.SaveAs(ImagePath);
            }

            if (ModelState.IsValid)
            {
                db.tbl_Operators.Add(@operator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", @operator.WorkshopID);
            return View(@operator);
        }

        // GET: Operators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = db.tbl_Operators.Find(id);
            if (@operator == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", @operator.WorkshopID);
            return View(@operator);
        }

        // POST: Operators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WorkshopID,Name,Family,PhoneNumber,MobileNumber,Email,Address,Image")] Operator @operator, HttpPostedFileBase Image)
        {
            @operator.CreateDate = DateTime.Now;
            @operator.Creator = User.Identity.Name;
            @operator.Status = true;

            if (Image != null)
            {
                if (@operator.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/Operators/" + @operator.Image));
                }

                @operator.Image = @operator.Family + "_" + @operator.Name + Image.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/Operators/" + @operator.Image);

                Image.SaveAs(ImagePath);
            }

            if (ModelState.IsValid)
            {
                db.Entry(@operator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", @operator.WorkshopID);
            return View(@operator);
        }

        // GET: Operators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = db.tbl_Operators.Find(id);
            if (@operator == null)
            {
                return HttpNotFound();
            }
            return View(@operator);
        }

        // POST: Operators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Operator @operator = db.tbl_Operators.Find(id);
            db.tbl_Operators.Remove(@operator);
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
