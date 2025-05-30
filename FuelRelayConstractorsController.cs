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
    public class FuelRelayConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: FuelRelayConstractors
        public ActionResult Index()
        {
            return View(db.tbl_FuelRelayConstractors.ToList());
        }

        // GET: FuelRelayConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelayConstractor fuelRelayConstractor = db.tbl_FuelRelayConstractors.Find(id);
            if (fuelRelayConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelayConstractor);
        }

        // GET: FuelRelayConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuelRelayConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FuelRelay,Code,Description")] FuelRelayConstractor fuelRelayConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_FuelRelayConstractors.Add(fuelRelayConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fuelRelayConstractor);
        }

        // GET: FuelRelayConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelayConstractor fuelRelayConstractor = db.tbl_FuelRelayConstractors.Find(id);
            if (fuelRelayConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelayConstractor);
        }

        // POST: FuelRelayConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FuelRelay,Code,Description")] FuelRelayConstractor fuelRelayConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fuelRelayConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fuelRelayConstractor);
        }

        // GET: FuelRelayConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelRelayConstractor fuelRelayConstractor = db.tbl_FuelRelayConstractors.Find(id);
            if (fuelRelayConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fuelRelayConstractor);
        }

        // POST: FuelRelayConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuelRelayConstractor fuelRelayConstractor = db.tbl_FuelRelayConstractors.Find(id);
            db.tbl_FuelRelayConstractors.Remove(fuelRelayConstractor);
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
