using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class ReplacementPlanValvePricesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ReplacementPlanValvePrices
        public async Task<ActionResult> Index()
        {
            var tbl_ReplacementPlanValvePrice = db.tbl_ReplacementPlanValvePrice.Include(r => r.EquipmentType);
            return View(await tbl_ReplacementPlanValvePrice.ToListAsync());
        }

        // GET: ReplacementPlanValvePrices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanValvePrice replacementPlanPrice = await db.tbl_ReplacementPlanValvePrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            return View(replacementPlanPrice);
        }

        // GET: ReplacementPlanValvePrices/Create
        public ActionResult Create()
        {
            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            return View();
        }

        // POST: ReplacementPlanValvePrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EquipmentTypeID,Price,DiscountedPrice,FromDate,ToDate,Description")] ReplacementPlanValvePrice replacementPlanPrice)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ReplacementPlanValvePrice.Add(replacementPlanPrice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title", replacementPlanPrice.EquipmentTypeID);
            return View(replacementPlanPrice);
        }

        // GET: ReplacementPlanValvePrices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanValvePrice replacementPlanPrice = await db.tbl_ReplacementPlanValvePrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title", replacementPlanPrice.EquipmentTypeID);
            return View(replacementPlanPrice);
        }

        // POST: ReplacementPlanValvePrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EquipmentTypeID,Price,DiscountedPrice,FromDate,ToDate,Description")] ReplacementPlanValvePrice replacementPlanPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replacementPlanPrice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title", replacementPlanPrice.EquipmentTypeID);
            return View(replacementPlanPrice);
        }

        // GET: ReplacementPlanValvePrices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanValvePrice replacementPlanPrice = await db.tbl_ReplacementPlanValvePrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            return View(replacementPlanPrice);
        }

        // POST: ReplacementPlanValvePrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReplacementPlanValvePrice replacementPlanPrice = await db.tbl_ReplacementPlanValvePrice.FindAsync(id);
            db.tbl_ReplacementPlanValvePrice.Remove(replacementPlanPrice);
            await db.SaveChangesAsync();
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
