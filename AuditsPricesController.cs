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
    public class AuditsPricesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: AuditsPrices
        public async Task<ActionResult> Index()
        {
            var tbl_AuditsPrice = db.tbl_AuditsPrice.Include(a => a.AuditCompanies);
            return View(await tbl_AuditsPrice.ToListAsync());
        }

        // GET: AuditsPrices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditsPrice auditsPrice = await db.tbl_AuditsPrice.FindAsync(id);
            if (auditsPrice == null)
            {
                return HttpNotFound();
            }
            return View(auditsPrice);
        }

        // GET: AuditsPrices/Create
        public ActionResult Create()
        {
            ViewBag.AuditCompanyID = new SelectList(db.tbl_AuditComponies, "ID", "Title");
            return View();
        }

        // POST: AuditsPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AuditCompanyID,Price,FromDate,ToDate,Description,OldPrice,Type")] AuditsPrice auditsPrice)
        {
            if (ModelState.IsValid)
            {
                db.tbl_AuditsPrice.Add(auditsPrice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AuditCompanyID = new SelectList(db.tbl_AuditComponies, "ID", "Title", auditsPrice.AuditCompanyID);
            return View(auditsPrice);
        }

        // GET: AuditsPrices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditsPrice auditsPrice = await db.tbl_AuditsPrice.FindAsync(id);
            if (auditsPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuditCompanyID = new SelectList(db.tbl_AuditComponies, "ID", "Title", auditsPrice.AuditCompanyID);
            return View(auditsPrice);
        }

        // POST: AuditsPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AuditCompanyID,Price,FromDate,ToDate,Description,OldPrice,Type")] AuditsPrice auditsPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditsPrice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AuditCompanyID = new SelectList(db.tbl_AuditComponies, "ID", "Title", auditsPrice.AuditCompanyID);
            return View(auditsPrice);
        }

        // GET: AuditsPrices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditsPrice auditsPrice = await db.tbl_AuditsPrice.FindAsync(id);
            if (auditsPrice == null)
            {
                return HttpNotFound();
            }
            return View(auditsPrice);
        }

        // POST: AuditsPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AuditsPrice auditsPrice = await db.tbl_AuditsPrice.FindAsync(id);
            db.tbl_AuditsPrice.Remove(auditsPrice);
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
