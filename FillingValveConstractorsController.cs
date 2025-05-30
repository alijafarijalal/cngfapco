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
    public class FillingValveConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: FillingValveConstractors
        public ActionResult Index()
        {
            return View(db.tbl_FillingValveConstractors.ToList());
        }

        // GET: FillingValveConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillingValveConstractor fillingValveConstractor = db.tbl_FillingValveConstractors.Find(id);
            if (fillingValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fillingValveConstractor);
        }

        // GET: FillingValveConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FillingValveConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FillingValve,Code,Description")] FillingValveConstractor fillingValveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_FillingValveConstractors.Add(fillingValveConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fillingValveConstractor);
        }

        // GET: FillingValveConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillingValveConstractor fillingValveConstractor = db.tbl_FillingValveConstractors.Find(id);
            if (fillingValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fillingValveConstractor);
        }

        // POST: FillingValveConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FillingValve,Code,Description")] FillingValveConstractor fillingValveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fillingValveConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fillingValveConstractor);
        }

        // GET: FillingValveConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillingValveConstractor fillingValveConstractor = db.tbl_FillingValveConstractors.Find(id);
            if (fillingValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(fillingValveConstractor);
        }

        // POST: FillingValveConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FillingValveConstractor fillingValveConstractor = db.tbl_FillingValveConstractors.Find(id);
            db.tbl_FillingValveConstractors.Remove(fillingValveConstractor);
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
