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
    public class CutofValveConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: CutofValveConstractors
        public ActionResult Index()
        {
            return View(db.tbl_CutofValveConstractors.ToList());
        }

        // GET: CutofValveConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutofValveConstractor cutofValveConstractor = db.tbl_CutofValveConstractors.Find(id);
            if (cutofValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(cutofValveConstractor);
        }

        // GET: CutofValveConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CutofValveConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CutofValve,Code,Description")] CutofValveConstractor cutofValveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_CutofValveConstractors.Add(cutofValveConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cutofValveConstractor);
        }

        // GET: CutofValveConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutofValveConstractor cutofValveConstractor = db.tbl_CutofValveConstractors.Find(id);
            if (cutofValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(cutofValveConstractor);
        }

        // POST: CutofValveConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CutofValve,Code,Description")] CutofValveConstractor cutofValveConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cutofValveConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cutofValveConstractor);
        }

        // GET: CutofValveConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutofValveConstractor cutofValveConstractor = db.tbl_CutofValveConstractors.Find(id);
            if (cutofValveConstractor == null)
            {
                return HttpNotFound();
            }
            return View(cutofValveConstractor);
        }

        // POST: CutofValveConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CutofValveConstractor cutofValveConstractor = db.tbl_CutofValveConstractors.Find(id);
            db.tbl_CutofValveConstractors.Remove(cutofValveConstractor);
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
