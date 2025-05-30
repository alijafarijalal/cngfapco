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
    public class TypeofUsesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: TypeofUses
        public ActionResult Index()
        {
            return View(db.tbl_TypeofUses.ToList());
        }

        // GET: TypeofUses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUse typeofUse = db.tbl_TypeofUses.Find(id);
            if (typeofUse == null)
            {
                return HttpNotFound();
            }
            return View(typeofUse);
        }

        // GET: TypeofUses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeofUses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Description")] TypeofUse typeofUse)
        {
            if (ModelState.IsValid)
            {
                db.tbl_TypeofUses.Add(typeofUse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeofUse);
        }

        // GET: TypeofUses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUse typeofUse = db.tbl_TypeofUses.Find(id);
            if (typeofUse == null)
            {
                return HttpNotFound();
            }
            return View(typeofUse);
        }

        // POST: TypeofUses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Description")] TypeofUse typeofUse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeofUse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeofUse);
        }

        // GET: TypeofUses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUse typeofUse = db.tbl_TypeofUses.Find(id);
            if (typeofUse == null)
            {
                return HttpNotFound();
            }
            return View(typeofUse);
        }

        // POST: TypeofUses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeofUse typeofUse = db.tbl_TypeofUses.Find(id);
            db.tbl_TypeofUses.Remove(typeofUse);
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
