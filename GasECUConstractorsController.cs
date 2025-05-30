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
    public class GasECUConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: GasECUConstractors
        public ActionResult Index()
        {
            return View(db.tbl_GasECUConstractors.ToList());
        }

        // GET: GasECUConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasECUConstractor gasECUConstractor = db.tbl_GasECUConstractors.Find(id);
            if (gasECUConstractor == null)
            {
                return HttpNotFound();
            }
            return View(gasECUConstractor);
        }

        // GET: GasECUConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GasECUConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,GasECU,Code,Description")] GasECUConstractor gasECUConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_GasECUConstractors.Add(gasECUConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gasECUConstractor);
        }

        // GET: GasECUConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasECUConstractor gasECUConstractor = db.tbl_GasECUConstractors.Find(id);
            if (gasECUConstractor == null)
            {
                return HttpNotFound();
            }
            return View(gasECUConstractor);
        }

        // POST: GasECUConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GasECU,Code,Description")] GasECUConstractor gasECUConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gasECUConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gasECUConstractor);
        }

        // GET: GasECUConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasECUConstractor gasECUConstractor = db.tbl_GasECUConstractors.Find(id);
            if (gasECUConstractor == null)
            {
                return HttpNotFound();
            }
            return View(gasECUConstractor);
        }

        // POST: GasECUConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GasECUConstractor gasECUConstractor = db.tbl_GasECUConstractors.Find(id);
            db.tbl_GasECUConstractors.Remove(gasECUConstractor);
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
