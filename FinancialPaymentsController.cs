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
    [RBACAttribute.NoCache]
    public class FinancialPaymentsController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbstatic = new ContextDB();

        // GET: FinancialPayments / 
        public ActionResult Index()
        {
            var tbl_FinancialPayments = db.tbl_FinancialPayments.Include(f => f.FinancialDesc).Include(f => f.Workshops);
            return View(tbl_FinancialPayments.OrderByDescending(f=>f.CreateDate).ToList());
        }

        // GET: FinancialPayments /WorkshopPage 
        public ActionResult WorkshopPage()
        {
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true);
            return View(workshops.ToList());
        }
        //
        public static string GetSumofPayment(int? id)
        {
            if (id != null)
            {
                var sumofPayment = dbstatic.tbl_FinancialPayments.Where(f => f.WorkshopID == id);
                if (sumofPayment.Count() > 0)
                    return sumofPayment.Sum(f => f.Value).ToString("#,##");
                else
                    return "0";
            }
            else
                return "0";
        }
        // GET: FinancialPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialPayment financialPayment = db.tbl_FinancialPayments.Find(id);
            if (financialPayment == null)
            {
                return HttpNotFound();
            }
            return View(financialPayment);
        }

        // GET: FinancialPayments/Create
        public ActionResult Create(int? id)
        {
            ViewBag.workshopsId = id;
            ViewBag.FinancialDescID = new SelectList(db.tbl_FinancialDescs, "ID", "Title");
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops.Where(w=>w.isServices==true), "ID", "Title",id);
            return View();
        }

        // POST: FinancialPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FinancialDescID,WorkshopID,Date,Description,Status,Comment")] FinancialPayment financialPayment, string Value)
        {
            financialPayment.CreateDate = DateTime.Now;
            financialPayment.Creator = User.Identity.Name;
            financialPayment.Value = Convert.ToDouble(Value.Replace(",", ""));

            if (ModelState.IsValid)
            {
                db.tbl_FinancialPayments.Add(financialPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FinancialDescID = new SelectList(db.tbl_FinancialDescs, "ID", "Title", financialPayment.FinancialDescID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops.Where(w => w.isServices == true), "ID", "Title", financialPayment.WorkshopID);
            return View(financialPayment);
        }

        // GET: FinancialPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialPayment financialPayment = db.tbl_FinancialPayments.Find(id);
            if (financialPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.FinancialDescID = new SelectList(db.tbl_FinancialDescs, "ID", "Title", financialPayment.FinancialDescID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops.Where(w => w.isServices == true), "ID", "Title", financialPayment.WorkshopID);
            ViewBag.statusVal = financialPayment.Status;
            if (financialPayment.Status == false)
                ViewBag.status = "غیر قطعی";
            else
                ViewBag.status = "قطعی";
            return View(financialPayment);
        }

        // POST: FinancialPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FinancialDescID,WorkshopID,Date,Description,Comment,CreateDate,Creator")] FinancialPayment financialPayment, string Value,bool Status)
        {
            financialPayment.CreateDate = DateTime.Now;
            financialPayment.Creator = User.Identity.Name;
            financialPayment.Value = Convert.ToDouble(Value.Replace(",", ""));
            financialPayment.Status = Status;

            if (ModelState.IsValid)
            {
                db.Entry(financialPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FinancialDescID = new SelectList(db.tbl_FinancialDescs, "ID", "Title", financialPayment.FinancialDescID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops.Where(w => w.isServices == true), "ID", "Title", financialPayment.WorkshopID);
            ViewBag.statusVal = financialPayment.Status;
            if (financialPayment.Status == false)
                ViewBag.status = "غیر قطعی";
            else
                ViewBag.status = "قطعی";

            return View(financialPayment);
        }

        // GET: FinancialPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialPayment financialPayment = db.tbl_FinancialPayments.Find(id);
            if (financialPayment == null)
            {
                return HttpNotFound();
            }
            return View(financialPayment);
        }

        // POST: FinancialPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FinancialPayment financialPayment = db.tbl_FinancialPayments.Find(id);
            db.tbl_FinancialPayments.Remove(financialPayment);
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
