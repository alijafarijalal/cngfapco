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
    public class ReplacementPlanPricesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ReplacementPlanPrices
        public async Task<ActionResult> Index()
        {
            var tbl_ReplacementPlanPrice = db.tbl_ReplacementPlanPrice.Include(r => r.EquipmentType);
            return View(await tbl_ReplacementPlanPrice.ToListAsync());
        }

        // GET: ReplacementPlanPrices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanPrice replacementPlanPrice = await db.tbl_ReplacementPlanPrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            return View(replacementPlanPrice);
        }

        // GET: ReplacementPlanPrices/Create
        public ActionResult Create()
        {
            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            return View();
        }

        // POST: ReplacementPlanPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EquipmentTypeID,Price,DiscountedPrice,FromDate,ToDate,Description")] ReplacementPlanPrice replacementPlanPrice)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ReplacementPlanPrice.Add(replacementPlanPrice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title", replacementPlanPrice.EquipmentTypeID);
            return View(replacementPlanPrice);
        }

        // GET: ReplacementPlanPrices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanPrice replacementPlanPrice = await db.tbl_ReplacementPlanPrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipmentTypeID = new SelectList(db.tbl_EquipmentList, "ID", "Title", replacementPlanPrice.EquipmentTypeID);
            return View(replacementPlanPrice);
        }

        // POST: ReplacementPlanPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EquipmentTypeID,Price,DiscountedPrice,FromDate,ToDate,Description")] ReplacementPlanPrice replacementPlanPrice)
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

        // GET: ReplacementPlanPrices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPlanPrice replacementPlanPrice = await db.tbl_ReplacementPlanPrice.FindAsync(id);
            if (replacementPlanPrice == null)
            {
                return HttpNotFound();
            }
            return View(replacementPlanPrice);
        }

        // POST: ReplacementPlanPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReplacementPlanPrice replacementPlanPrice = await db.tbl_ReplacementPlanPrice.FindAsync(id);
            db.tbl_ReplacementPlanPrice.Remove(replacementPlanPrice);
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
