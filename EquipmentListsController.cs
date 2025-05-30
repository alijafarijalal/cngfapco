using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class EquipmentListsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: EquipmentLists
        public ActionResult Index()
        {
            var tbl_EquipmentList = db.tbl_EquipmentList.Include(e => e.Parent).OrderByDescending(e => e.CreateDate);
            return View(tbl_EquipmentList.ToList());
        }

        // GET: EquipmentLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentList equipmentList = db.tbl_EquipmentList.Find(id);
            if (equipmentList == null)
            {
                return HttpNotFound();
            }
            return View(equipmentList);
        }

        // GET: EquipmentLists/Create
        public ActionResult Create()
        {
            ViewBag.Pid = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            return View();
        }

        // POST: EquipmentLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,FinancialCode,Address,Pid,Presentable")] EquipmentList equipmentList, string Value, string Value2)
        {
            equipmentList.CreateDate = DateTime.Now;
            equipmentList.Creator = User.Identity.Name;
            equipmentList.Value = Convert.ToDouble(Value.Replace(",", ""));
            equipmentList.Value2 = Convert.ToDouble(Value2.Replace(",", ""));

            if (ModelState.IsValid)
            {
                db.tbl_EquipmentList.Add(equipmentList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Pid = new SelectList(db.tbl_EquipmentList, "ID", "Title", equipmentList.Pid);
            return View(equipmentList);
        }

        // GET: EquipmentLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentList equipmentList = db.tbl_EquipmentList.Find(id);
            if (equipmentList == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pid = new SelectList(db.tbl_EquipmentList, "ID", "Title", equipmentList.Pid);
            return View(equipmentList);
        }

        // POST: EquipmentLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,FinancialCode,Address,Pid,Presentable")] EquipmentList equipmentList, string Value, string Value2)
        {
            equipmentList.CreateDate = DateTime.Now;
            equipmentList.Creator = User.Identity.Name;
            equipmentList.Value = Convert.ToDouble(Value.Replace(",", ""));
            equipmentList.Value2 = Convert.ToDouble(Value2.Replace(",", ""));

            if (ModelState.IsValid)
            {
                db.Entry(equipmentList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Pid = new SelectList(db.tbl_EquipmentList, "ID", "Title", equipmentList.Pid);
            return View(equipmentList);
        }

        // GET: EquipmentLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentList equipmentList = db.tbl_EquipmentList.Find(id);
            if (equipmentList == null)
            {
                return HttpNotFound();
            }
            return View(equipmentList);
        }

        // POST: EquipmentLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipmentList equipmentList = db.tbl_EquipmentList.Find(id);
            db.tbl_EquipmentList.Remove(equipmentList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        // GET: EquipmentLists
        [HttpGet]
        public ActionResult UpdateEquipmentList()
        {
            var tbl_EquipmentList = db.tbl_EquipmentList.Include(e => e.Parent);
            return View(tbl_EquipmentList.ToList());
        }
        //
        [HttpPost]
        public ActionResult UpdateEquipmentList(int[] ID, string[] Value, string[] Value2)
        {           
            
            for (int i = 0; i < ID.Count(); i++)
            {
                EquipmentList updateList = db.tbl_EquipmentList.Find(ID[i]);
                if (!string.IsNullOrEmpty(Value[i]))
                    updateList.Value = Convert.ToDouble(Value[i].Replace(",", ""));
                else
                    updateList.Value = 0;
                if (!string.IsNullOrEmpty(Value2[i]))
                    updateList.Value2 = Convert.ToDouble(Value2[i].Replace(",", ""));
                else
                    updateList.Value2 = 0;
                db.Entry(updateList).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("UpdateEquipmentList");
        }
        //
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
