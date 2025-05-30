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
    public class CRMIndexesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: CRMIndexes
        public ActionResult Index()
        {
            var tbl_CRMIndexes = db.tbl_CRMIndexes.Include(c => c.Parent);
            return View(tbl_CRMIndexes.ToList());
        }

        // GET: CRMIndexes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMIndex cRMIndex = db.tbl_CRMIndexes.Find(id);
            if (cRMIndex == null)
            {
                return HttpNotFound();
            }
            return View(cRMIndex);
        }

        // GET: CRMIndexes/Create
        public ActionResult Create()
        {
            ViewBag.PId = new SelectList(db.tbl_CRMIndexes, "ID", "Title");
            return View();
        }

        // POST: CRMIndexes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,PId,Presentable,Creator,CreateDate")] CRMIndex cRMIndex)
        {
            if (ModelState.IsValid)
            {
                db.tbl_CRMIndexes.Add(cRMIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PId = new SelectList(db.tbl_CRMIndexes, "ID", "Title", cRMIndex.PId);
            return View(cRMIndex);
        }

        // GET: CRMIndexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMIndex cRMIndex = db.tbl_CRMIndexes.Find(id);
            if (cRMIndex == null)
            {
                return HttpNotFound();
            }
            ViewBag.PId = new SelectList(db.tbl_CRMIndexes, "ID", "Title", cRMIndex.PId);
            return View(cRMIndex);
        }

        // POST: CRMIndexes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,PId,Presentable,Creator,CreateDate")] CRMIndex cRMIndex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRMIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PId = new SelectList(db.tbl_CRMIndexes, "ID", "Title", cRMIndex.PId);
            return View(cRMIndex);
        }

        // GET: CRMIndexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMIndex cRMIndex = db.tbl_CRMIndexes.Find(id);
            if (cRMIndex == null)
            {
                return HttpNotFound();
            }
            return View(cRMIndex);
        }

        // POST: CRMIndexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CRMIndex cRMIndex = db.tbl_CRMIndexes.Find(id);
            db.tbl_CRMIndexes.Remove(cRMIndex);
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
