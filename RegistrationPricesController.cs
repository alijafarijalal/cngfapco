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
    [Authorize]
    public class RegistrationPricesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: RegistrationPrices
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_RegistrationPrice.ToListAsync());
        }

        // GET: RegistrationPrices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationPrice registrationPrice = await db.tbl_RegistrationPrice.FindAsync(id);
            if (registrationPrice == null)
            {
                return HttpNotFound();
            }
            return View(registrationPrice);
        }

        // GET: RegistrationPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegistrationPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Type,Price,FromDate,ToDate,Description,OldPrice,DepType")] RegistrationPrice registrationPrice)
        {
            if (ModelState.IsValid)
            {
                db.tbl_RegistrationPrice.Add(registrationPrice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(registrationPrice);
        }

        // GET: RegistrationPrices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationPrice registrationPrice = await db.tbl_RegistrationPrice.FindAsync(id);
            if (registrationPrice == null)
            {
                return HttpNotFound();
            }
            return View(registrationPrice);
        }

        // POST: RegistrationPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Type,Price,FromDate,ToDate,Description,OldPrice,DepType")] RegistrationPrice registrationPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationPrice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(registrationPrice);
        }

        // GET: RegistrationPrices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationPrice registrationPrice = await db.tbl_RegistrationPrice.FindAsync(id);
            if (registrationPrice == null)
            {
                return HttpNotFound();
            }
            return View(registrationPrice);
        }

        // POST: RegistrationPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RegistrationPrice registrationPrice = await db.tbl_RegistrationPrice.FindAsync(id);
            db.tbl_RegistrationPrice.Remove(registrationPrice);
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
