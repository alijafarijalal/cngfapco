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
    public class ContradictionTypesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ContradictionTypes
        public ActionResult Index()
        {
            return View(db.tbl_ContradictionTypes.ToList());
        }

        // GET: ContradictionTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionType contradictionType = db.tbl_ContradictionTypes.Find(id);
            if (contradictionType == null)
            {
                return HttpNotFound();
            }
            return View(contradictionType);
        }

        // GET: ContradictionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContradictionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description")] ContradictionType contradictionType)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ContradictionTypes.Add(contradictionType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contradictionType);
        }

        // GET: ContradictionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionType contradictionType = db.tbl_ContradictionTypes.Find(id);
            if (contradictionType == null)
            {
                return HttpNotFound();
            }
            return View(contradictionType);
        }

        // POST: ContradictionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description")] ContradictionType contradictionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contradictionType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contradictionType);
        }

        // GET: ContradictionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContradictionType contradictionType = db.tbl_ContradictionTypes.Find(id);
            if (contradictionType == null)
            {
                return HttpNotFound();
            }
            return View(contradictionType);
        }

        // POST: ContradictionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContradictionType contradictionType = db.tbl_ContradictionTypes.Find(id);
            db.tbl_ContradictionTypes.Remove(contradictionType);
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
