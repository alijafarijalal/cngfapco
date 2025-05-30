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
    public class ListofServicesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: ListofServices
        public ActionResult Index()
        {
            return View(db.tbl_ListofServices.ToList());
        }

        // GET: ListofServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListofServices listofServices = db.tbl_ListofServices.Find(id);
            if (listofServices == null)
            {
                return HttpNotFound();
            }
            return View(listofServices);
        }

        // GET: ListofServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListofServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ServiceRent,Description,Type,Unit,Presentable")] ListofServices listofServices)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ListofServices.Add(listofServices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listofServices);
        }

        // GET: ListofServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListofServices listofServices = db.tbl_ListofServices.Find(id);
            if (listofServices == null)
            {
                return HttpNotFound();
            }
            return View(listofServices);
        }

        // POST: ListofServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ServiceRent,Description,Type,Unit,Presentable")] ListofServices listofServices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listofServices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(listofServices);
        }

        // GET: ListofServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListofServices listofServices = db.tbl_ListofServices.Find(id);
            if (listofServices == null)
            {
                return HttpNotFound();
            }
            return View(listofServices);
        }

        // POST: ListofServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListofServices listofServices = db.tbl_ListofServices.Find(id);
            db.tbl_ListofServices.Remove(listofServices);
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
