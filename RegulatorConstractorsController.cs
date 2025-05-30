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
    public class RegulatorConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: RegulatorConstractors
        public ActionResult Index()
        {
            return View(db.tbl_RegulatorConstractors.ToList());
        }

        // GET: RegulatorConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegulatorConstractor regulatorConstractor = db.tbl_RegulatorConstractors.Find(id);
            if (regulatorConstractor == null)
            {
                return HttpNotFound();
            }
            return View(regulatorConstractor);
        }

        // GET: RegulatorConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegulatorConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Regulator,Description,Code")] RegulatorConstractor regulatorConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_RegulatorConstractors.Add(regulatorConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(regulatorConstractor);
        }

        // GET: RegulatorConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegulatorConstractor regulatorConstractor = db.tbl_RegulatorConstractors.Find(id);
            if (regulatorConstractor == null)
            {
                return HttpNotFound();
            }
            return View(regulatorConstractor);
        }

        // POST: RegulatorConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Regulator,Description,Code")] RegulatorConstractor regulatorConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regulatorConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(regulatorConstractor);
        }

        // GET: RegulatorConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegulatorConstractor regulatorConstractor = db.tbl_RegulatorConstractors.Find(id);
            if (regulatorConstractor == null)
            {
                return HttpNotFound();
            }
            return View(regulatorConstractor);
        }

        // POST: RegulatorConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegulatorConstractor regulatorConstractor = db.tbl_RegulatorConstractors.Find(id);
            db.tbl_RegulatorConstractors.Remove(regulatorConstractor);
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
