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
    public class RemittanceDetailsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: RemittanceDetails
        public ActionResult Index()
        {
            var tbl_RemittanceDetails = db.tbl_RemittanceDetails.Include(r => r.Remittances);
            return View(tbl_RemittanceDetails.ToList());
        }

        // GET: RemittanceDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemittanceDetails remittanceDetails = db.tbl_RemittanceDetails.Find(id);
            if (remittanceDetails == null)
            {
                return HttpNotFound();
            }
            return View(remittanceDetails);
        }

        // GET: RemittanceDetails/Create
        public ActionResult Create()
        {
            ViewBag.RemittancesID = new SelectList(db.tbl_Remittances, "ID", "Number");
            return View();
        }

        // POST: RemittanceDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RemittancesID,Date,Vehicle,Plate,BillofLading,Transferee,Description,CreateDate,Creator,CarryFare,CarrierName,Attachment")] RemittanceDetails remittanceDetails)
        {
            if (ModelState.IsValid)
            {
                db.tbl_RemittanceDetails.Add(remittanceDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RemittancesID = new SelectList(db.tbl_Remittances, "ID", "Number", remittanceDetails.RemittancesID);
            return View(remittanceDetails);
        }

        // GET: RemittanceDetails/Edit/5
        public ActionResult Edit(int? id,int? remittanceId, string url)
        {
            ViewBag.remittanceId = remittanceId;
            ViewBag.url = url;
            //
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemittanceDetails remittanceDetails = db.tbl_RemittanceDetails.Find(id);
            if (remittanceDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.RemittancesID = new SelectList(db.tbl_Remittances, "ID", "Number", remittanceDetails.RemittancesID);
            return View(remittanceDetails);
        }

        // POST: RemittanceDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RemittancesID,Date,Vehicle,Plate,BillofLading,Transferee,Description,CarrierName,Attachment")] RemittanceDetails remittanceDetails,int? remittanceId, string CarryFare, string url, HttpPostedFileBase Attachment)
        {
            remittanceDetails.CreateDate = DateTime.Now;
            remittanceDetails.Creator = User.Identity.Name;
            remittanceDetails.CarryFare= double.Parse(CarryFare.Replace(",", "").Replace(".", "").Replace("/", ""), System.Globalization.CultureInfo.InvariantCulture);

            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    if (remittanceDetails.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Remittance/" + remittanceDetails.Attachment));
                    }
                    remittanceDetails.Attachment = remittanceDetails.Creator + "_" + remittanceDetails.RemittancesID + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/Remittance/" + remittanceDetails.Attachment);
                    Attachment.SaveAs(ImagePath);
                }
                db.Entry(remittanceDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailsIndex", "Remittances", new { id = remittanceId, url=url });
            }
            ViewBag.RemittancesID = new SelectList(db.tbl_Remittances, "ID", "Number", remittanceDetails.RemittancesID);
            return View(remittanceDetails);
        }

        // GET: RemittanceDetails/Delete/5
        public ActionResult Delete(int? id, int? remittanceId, string url)
        {
            ViewBag.remittanceId = remittanceId;
            ViewBag.url = url;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemittanceDetails remittanceDetails = db.tbl_RemittanceDetails.Find(id);
            if (remittanceDetails == null)
            {
                return HttpNotFound();
            }
            return View(remittanceDetails);
        }

        // POST: RemittanceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? remittanceId, string url)
        {
            RemittanceDetails remittanceDetails = db.tbl_RemittanceDetails.Find(id);
            string Attachment = remittanceDetails.Attachment;

            db.tbl_RemittanceDetails.Remove(remittanceDetails);
            db.SaveChanges();
            //
            if (Attachment != null)
            {
                System.IO.File.Delete(Server.MapPath("/UploadedFiles/Remittance/" + Attachment));
            }

            return RedirectToAction("DetailsIndex", "Remittances", new { id = remittanceId,url=url });
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
