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
    public class ValveConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ValveConstractors
        public ActionResult Index()
        {
            return View(db.tbl_ValveConstractors.ToList());
        }

        // GET: ValveConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValveConstractor valveConstractor = db.tbl_ValveConstractors.Find(id);
            if (valveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(valveConstractor);
        }

        // GET: ValveConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ValveConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Valve,Description")] ValveConstractor valveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ValveConstractors.Add(valveConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(valveConstractor);
        }

        // GET: ValveConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValveConstractor valveConstractor = db.tbl_ValveConstractors.Find(id);
            if (valveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(valveConstractor);
        }

        // POST: ValveConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Valve,Description")] ValveConstractor valveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(valveConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(valveConstractor);
        }

        // GET: ValveConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValveConstractor valveConstractor = db.tbl_ValveConstractors.Find(id);
            if (valveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(valveConstractor);
        }

        // POST: ValveConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ValveConstractor valveConstractor = db.tbl_ValveConstractors.Find(id);
            db.tbl_ValveConstractors.Remove(valveConstractor);
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
