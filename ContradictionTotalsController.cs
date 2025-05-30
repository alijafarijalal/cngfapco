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
    public class ContradictionTotalsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ContradictionTotals
        public ActionResult Index()
        {
            var tbl_ContradictionTotals = db.tbl_ContradictionTotals.Include(c => c.ContradictionType).Include(c => c.Workshops);
            return View(tbl_ContradictionTotals.ToList());
        }

        // GET: ContradictionTotals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionTotal contradictionTotal = db.tbl_ContradictionTotals.Find(id);
            if (contradictionTotal == null)
            {
                return HttpNotFound();
            }
            return View(contradictionTotal);
        }

        // GET: ContradictionTotals/Create
        public ActionResult Create()
        {
            ViewBag.ContradictionTypeId = new SelectList(db.tbl_ContradictionTypes, "ID", "Description");
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: ContradictionTotals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WorkshopId,ContradictionTypeId,Date,Count,CreateDate,Creator")] ContradictionTotal contradictionTotal)
        {
            contradictionTotal.CreateDate = DateTime.Now;
            contradictionTotal.Creator = User.Identity.Name;
            //
            if (ModelState.IsValid)
            {
                db.tbl_ContradictionTotals.Add(contradictionTotal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContradictionTypeId = new SelectList(db.tbl_ContradictionTypes, "ID", "Description", contradictionTotal.ContradictionTypeId);
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradictionTotal.WorkshopId);
            return View(contradictionTotal);
        }

        // GET: ContradictionTotals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionTotal contradictionTotal = db.tbl_ContradictionTotals.Find(id);
            if (contradictionTotal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContradictionTypeId = new SelectList(db.tbl_ContradictionTypes, "ID", "Description", contradictionTotal.ContradictionTypeId);
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradictionTotal.WorkshopId);
            return View(contradictionTotal);
        }

        // POST: ContradictionTotals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WorkshopId,ContradictionTypeId,Date,Count,CreateDate,Creator")] ContradictionTotal contradictionTotal)
        {
            contradictionTotal.CreateDate = DateTime.Now;
            contradictionTotal.Creator = User.Identity.Name;
            //
            if (ModelState.IsValid)
            {
                db.Entry(contradictionTotal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContradictionTypeId = new SelectList(db.tbl_ContradictionTypes, "ID", "Description", contradictionTotal.ContradictionTypeId);
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradictionTotal.WorkshopId);
            return View(contradictionTotal);
        }

        // GET: ContradictionTotals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionTotal contradictionTotal = db.tbl_ContradictionTotals.Find(id);
            if (contradictionTotal == null)
            {
                return HttpNotFound();
            }
            return View(contradictionTotal);
        }

        // POST: ContradictionTotals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContradictionTotal contradictionTotal = db.tbl_ContradictionTotals.Find(id);
            db.tbl_ContradictionTotals.Remove(contradictionTotal);
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
