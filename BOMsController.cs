using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class BOMsController : Controller
    {
        private ContextDB db = new ContextDB();
        DAL objdal = new DAL();

        // GET: BOMs
        public ActionResult Index()
        {
            var tbl_BOMs = db.tbl_BOMs.Include(b => b.EquipmentList).Include(b => b.VehicleType);
            return View(tbl_BOMs.ToList());
        }

        // GET: BOMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOM bOM = db.tbl_BOMs.Find(id);
            if (bOM == null)
            {
                return HttpNotFound();
            }
            return View(bOM);
        }

        // GET: BOMs/Create
        public ActionResult Create()
        {
            var vehicle = db.tbl_VehicleTypes.ToList();
            List<VehicleType> droplist = new List<VehicleType>();
            foreach(var item in vehicle)
            {
                droplist.Add(new VehicleType
                {
                    ID=item.ID,
                    Type=item.Type + " - " + item.Description
                });
            }
            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
            ViewBag.EquipmentListID = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            ViewBag.VehicleTypeID = new SelectList(droplist, "ID", "Type");
            return View();
        }

        // POST: BOMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VehicleTypeID,EquipmentListID,Unit,Address,Presentable,GenerationID")] BOM bOM, string Ratio)
        {
            bOM.Creator = User.Identity.Name;
            bOM.CreateDate = DateTime.Now;
            bOM.Ratio = double.Parse(Ratio, System.Globalization.CultureInfo.InvariantCulture);

            //
            if (ModelState.IsValid)
            {
                db.tbl_BOMs.Add(bOM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title", bOM.GenerationID);
            ViewBag.EquipmentListID = new SelectList(db.tbl_EquipmentList, "ID", "Title", bOM.EquipmentListID);
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", bOM.VehicleTypeID);
            return View(bOM);
        }

        // GET: BOMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOM bOM = db.tbl_BOMs.Find(id);
            if (bOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title", bOM.GenerationID);
            ViewBag.EquipmentListID = new SelectList(db.tbl_EquipmentList, "ID", "Title", bOM.EquipmentListID);
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", bOM.VehicleTypeID);
            return View(bOM);
        }

        // POST: BOMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VehicleTypeID,EquipmentListID,Ratio,Unit,Address,Presentable,GenerationID")] BOM bOM)
        {
            bOM.Creator = User.Identity.Name;
            bOM.CreateDate = DateTime.Now;
            //
            if (ModelState.IsValid)
            {
                db.Entry(bOM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title", bOM.GenerationID);
            ViewBag.EquipmentListID = new SelectList(db.tbl_EquipmentList, "ID", "Title", bOM.EquipmentListID);
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", bOM.VehicleTypeID);
            return View(bOM);
        }

        // GET: BOMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOM bOM = db.tbl_BOMs.Find(id);
            if (bOM == null)
            {
                return HttpNotFound();
            }
            return View(bOM);
        }

        // POST: BOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BOM bOM = db.tbl_BOMs.Find(id);
            db.tbl_BOMs.Remove(bOM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        public List<Dictionary<string, object>> Read(DbDataReader reader)
        {
            List<Dictionary<string, object>> expandolist = new List<Dictionary<string, object>>();
            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }
                expandolist.Add(new Dictionary<string, object>(expando));
            }
            return expandolist;
        }
        public ActionResult VehicleBOMinOneView(int? generationId, int? @vehicleTypeId)
        {
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            //
            if (vehicleTypeId != null)
                ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", vehicleTypeId);
            else
            {
                ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text");
                vehicleTypeId = 0;
            }

            //
            if (generationId != null)
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title",generationId);
            else
            {
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
                generationId = 0;
            }


            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleBOMinOneView]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@generationId", SqlDbType.VarChar).Value = generationId;
                    command.Parameters.Add("@vehicleTypeId", SqlDbType.VarChar).Value = vehicleTypeId;

                    conn.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var model = Read(reader).ToList();
                        return View(model);
                    }
                    //conn.Close();
                }

            }//end using
        }
        //
        public ActionResult UpdateBOMinOneView(int? VehicleTypeID, int? generationId)
        {           
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            //
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", VehicleTypeID);
            //
            //
            if (generationId != null)
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title", generationId);
            else
            {
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
                generationId = 0;
            }
            //
            return View();
        }

        [HttpPost]
        public ActionResult UpdateBOMinOneView(int?[] ID,int?[] EquipmentListID, string[] Ratio,string[] Unit, int? VehicleTypeID, int? generationId)
        {
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            //
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", VehicleTypeID);
            ViewBag.VehicleType = VehicleTypeID;
            //
            if (generationId != null)
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title", generationId);
            else
            {
                ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
                generationId = 0;
            }
            //
            int counUpdate = 1;

            if (ID != null)
            {
                for (int i = 0; i < ID.Count(); i++)
                {
                    BOM updateBOMList = db.tbl_BOMs.Find(ID[i]);
                    counUpdate = i + 1;
                    updateBOMList.EquipmentListID = EquipmentListID[i];
                    updateBOMList.Ratio = double.Parse(Ratio[i].Replace("/","."), System.Globalization.CultureInfo.InvariantCulture);
                    updateBOMList.Unit = Unit[i];

                    db.Entry(updateBOMList).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //
                ViewBag.countUpdate = " تعداد : " + " " + counUpdate + " " + "ردیف بروزرسانی شد!";
            }
            
            return View();
        }
        //\
        public ActionResult ShowBOMs(int? VehicleTypeID, int? generationId)
        {
            ViewBag.GenerationID = generationId;
            ViewBag.VehicleType = VehicleTypeID;

            if (VehicleTypeID != null && generationId==null)
            {
                var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == VehicleTypeID).Include(b => b.EquipmentList).Include(b => b.VehicleType);
                return PartialView(bomList.ToList());
            }
            if (VehicleTypeID == null && generationId != null)
            {
                var bomList = db.tbl_BOMs.Where(b => b.GenerationID == generationId).Include(b => b.EquipmentList).Include(b => b.VehicleType);
                return PartialView(bomList.ToList());
            }
            if (VehicleTypeID != null && generationId != null)
            {
                var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == VehicleTypeID && b.GenerationID == generationId).Include(b => b.EquipmentList).Include(b => b.VehicleType);
                return PartialView(bomList.ToList());
            }
            else
            {
                var bomList = db.tbl_BOMs.Include(b => b.EquipmentList).Include(b => b.VehicleType);
                return PartialView(bomList.ToList());
            }
        }
        //
        public JsonResult GetEquipmentListID(string FinancialCode)
        {
            if(!string.IsNullOrEmpty(FinancialCode))
            {
                var findID = db.tbl_EquipmentList.Where(e => e.FinancialCode.Equals(FinancialCode)).SingleOrDefault().ID;
                return Json(new { result = findID }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "یافت نشد!" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        //
        public JsonResult GetEquipmentListTitle(string FinancialCode)
        {
            if (!string.IsNullOrEmpty(FinancialCode))
            {
                var findTitle = db.tbl_EquipmentList.Where(e => e.FinancialCode.Equals(FinancialCode)).SingleOrDefault().Title;
                return Json(new { result = findTitle }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "یافت نشد!" }, JsonRequestBehavior.AllowGet);
            }

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
