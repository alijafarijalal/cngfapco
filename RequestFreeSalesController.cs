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
    public class RequestFreeSalesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: RequestFreeSales
        public ActionResult Index()
        {
            var tbl_RequestFreeSales = db.tbl_RequestFreeSales.Include(r => r.CurrencyType).Include(r => r.Equipments).Include(r => r.Workshops);
            return View(tbl_RequestFreeSales.ToList());
        }

        // GET: RequestFreeSales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            if (requestFreeSale == null)
            {
                return HttpNotFound();
            }
            return View(requestFreeSale);
        }

        // GET: RequestFreeSales/Create
        public ActionResult Create()
        {
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title");
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: RequestFreeSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceID,Owners,WorkshopsID,EquipmentsID,ServiceCode,InvoiceCode,CreatedDate,EmployerEconomicalnumber,Employerregistrationnumber,EmployerState,EmployerAddress,EmployerPostalcode,EmployerPhone,EmployerFax,ServiceDesc,Number,UnitofMeasurement,UnitAmount,TotalAmount,DiscountAmount,Description,SaleCondition,Comment,Status,CurrencyTypeID,CreatorUser,ViewStatus,ViewDate,Viewer,FinalStatus")] RequestFreeSale requestFreeSale)
        {
            if (ModelState.IsValid)
            {
                db.tbl_RequestFreeSales.Add(requestFreeSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", requestFreeSale.CurrencyTypeID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", requestFreeSale.EquipmentsID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", requestFreeSale.WorkshopsID);
            return View(requestFreeSale);
        }

        // GET: RequestFreeSales/Edit/5
        public ActionResult RequestFreeSaleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            if (requestFreeSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", requestFreeSale.CurrencyTypeID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", requestFreeSale.EquipmentsID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", requestFreeSale.WorkshopsID);
            return View(requestFreeSale);
        }

        // POST: RequestFreeSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestFreeSaleEdit([Bind(Include = "InvoiceID,Owners,WorkshopsID,EquipmentsID,ServiceCode,InvoiceCode,CreatedDate,EmployerEconomicalnumber,Employerregistrationnumber,EmployerState,EmployerAddress,EmployerPostalcode,EmployerPhone,EmployerFax,ServiceDesc,Number,UnitofMeasurement,UnitAmount,TotalAmount,DiscountAmount,Description,SaleCondition,Comment,Status,CurrencyTypeID,CreatorUser,ViewStatus,ViewDate,Viewer,FinalStatus")] RequestFreeSale requestFreeSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestFreeSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RequestFreeSaleDetails",new { });
            }
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", requestFreeSale.CurrencyTypeID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", requestFreeSale.EquipmentsID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", requestFreeSale.WorkshopsID);
            return View(requestFreeSale);
        }

        // GET: RequestFreeSales/Delete/5
        public ActionResult RequestFreeSaleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            if (requestFreeSale == null)
            {
                return HttpNotFound();
            }
            return View(requestFreeSale);
        }

        // POST: RequestFreeSales/Delete/5
        [HttpPost, ActionName("RequestFreeSaleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            db.tbl_RequestFreeSales.Remove(requestFreeSale);
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
