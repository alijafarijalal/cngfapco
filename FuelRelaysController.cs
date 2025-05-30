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
    public class FuelRelaysController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: FuelRelays
        public ActionResult Index()
        {
            return View(db.tbl_FuelRelays.ToList());
        }

        // GET: FuelRelays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelay fuelRelay = db.tbl_FuelRelays.Find(id);
            if (fuelRelay == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelay);
        }

        // GET: FuelRelays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuelRelays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaterailName,serialNumber,constractor,generation,model,type,productDate,expireDate,workshop,status,CreateDate,Creator,RefreshDate,RefreshCreator")] FuelRelay fuelRelay)
        {
            if (ModelState.IsValid)
            {
                db.tbl_FuelRelays.Add(fuelRelay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fuelRelay);
        }

        // GET: FuelRelays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelay fuelRelay = db.tbl_FuelRelays.Find(id);
            if (fuelRelay == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelay);
        }

        // POST: FuelRelays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaterailName,serialNumber,constractor,generation,model,type,productDate,expireDate,workshop,status,CreateDate,Creator,RefreshDate,RefreshCreator")] FuelRelay fuelRelay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fuelRelay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fuelRelay);
        }

        // GET: FuelRelays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelay fuelRelay = db.tbl_FuelRelays.Find(id);
            if (fuelRelay == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelay);
        }

        // POST: FuelRelays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuelRelay fuelRelay = db.tbl_FuelRelays.Find(id);
            db.tbl_FuelRelays.Remove(fuelRelay);
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
