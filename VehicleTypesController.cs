using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
   // [RBAC]
   [Authorize]
    public class VehicleTypesController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: VehicleTypes
        public ActionResult Index()
        {
            return View(db.tbl_VehicleTypes.ToList());
        }

        // GET: VehicleTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.tbl_VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Description")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                db.tbl_VehicleTypes.Add(vehicleType);
                db.SaveChanges();
                //برای افزودن نوع خودر در جدول مخزن
                TypeofTank typeoftanks = new TypeofTank();
                typeoftanks.Type= Regex.Match(vehicleType.Description, @"\d+").Value;
                typeoftanks.VehicleTypeId = vehicleType.ID;
                db.tbl_TypeofTanks.Add(typeoftanks);
                db.SaveChanges();
                //برای افزودن نوع خودر در جدول پایه مخزن
                TypeofTankBase typeoftankbase = new TypeofTankBase();
                typeoftankbase.Type = vehicleType.Type;
                typeoftankbase.VehicleTypeId = vehicleType.ID;
                db.tbl_TypeofTankBases.Add(typeoftankbase);
                db.SaveChanges();
                //برای افزودن نوع خودر در جدول کاور مخزن
                TypeofTankCover typeoftankcover = new TypeofTankCover();
                typeoftankcover.Type = vehicleType.Type;
                typeoftankcover.VehicleTypeId = vehicleType.ID;
                db.tbl_TypeofTankCovers.Add(typeoftankcover);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.tbl_VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Description")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.tbl_VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleType vehicleType = db.tbl_VehicleTypes.Find(id);
            db.tbl_VehicleTypes.Remove(vehicleType);
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
