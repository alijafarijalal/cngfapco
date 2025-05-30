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
    //[RBAC]
    [Authorize]
    public class TankConstractorsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: TankConstractors
        public ActionResult Index()
        {
            return View(db.tbl_TankConstractors.ToList());
        }

        // GET: TankConstractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankConstractor tankConstractor = db.tbl_TankConstractors.Find(id);
            if (tankConstractor == null)
            {
                return HttpNotFound();
            }
            return View(tankConstractor);
        }

        // GET: TankConstractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TankConstractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Constractor,Description,Code")] TankConstractor tankConstractor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_TankConstractors.Add(tankConstractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tankConstractor);
        }

        // GET: TankConstractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankConstractor tankConstractor = db.tbl_TankConstractors.Find(id);
            if (tankConstractor == null)
            {
                return HttpNotFound();
            }
            return View(tankConstractor);
        }

        // POST: TankConstractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Constractor,Description,Code")] TankConstractor tankConstractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tankConstractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tankConstractor);
        }

        // GET: TankConstractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankConstractor tankConstractor = db.tbl_TankConstractors.Find(id);
            if (tankConstractor == null)
            {
                return HttpNotFound();
            }
            return View(tankConstractor);
        }

        // POST: TankConstractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TankConstractor tankConstractor = db.tbl_TankConstractors.Find(id);
            db.tbl_TankConstractors.Remove(tankConstractor);
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
