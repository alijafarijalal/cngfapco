using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class UploadFilesController : Controller
    {
        private ContextDB db = new ContextDB();
        DAL objdal = new DAL();

        #region Upload Equipments From File
        public FileResult DownloadExcel()
        {
            string path = "/UploadedFiles/DownloadExcel.xlsx";
            return File(path, "application/vnd.ms-excel", "DownloadExcel.xlsx");
        }
        // GET: UploadFiles/ Upload Tanks Information
        public ActionResult UploadTanks_Old()
        {
            var ImportTanksList = db.tbl_Tanks.OrderByDescending(t => t.ID);
            DateTime existDate = DateTime.Now;

            if (ImportTanksList!=null)
                existDate = ImportTanksList.FirstOrDefault().CreateDate.GetValueOrDefault();
            var workshopId = "0"; //Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
            ViewBag.existDate = existDate.ToShortDateString();

            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                ViewBag.oldCount = ImportTanksList.Where(i => i.workshop.Equals(workshopId)).Count();
                return View(ImportTanksList.Where(i => i.workshop.Equals(workshopId)).ToList());
            }

            else
            {
                ViewBag.oldCount = ImportTanksList.Count();
                return View(ImportTanksList.Where(t=>t.CreateDate == Convert.ToDateTime(existDate)).ToList());
            }
            //return View();
        }

        public ActionResult UploadTanks(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            //Post = false;
            var list = db.tbl_Tanks.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon="icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";                        
                    }

                };
            }
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }
                
            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }                

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string bulk = "";
            string pressure = "";
            string length = "";
            string diameter = "";
            string productDate = "";
            string expireDate = "";
            string rezve = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_CylinderList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            bulk = reader["bulk"].ToString();
                            length = reader["length"].ToString();
                            pressure = reader["pressure"].ToString();
                            diameter = reader["diameter"].ToString();
                            productDate = reader["productDate"].ToString();
                            expireDate = reader["expireDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue? RefreshDate.GetValueOrDefault().ToShortDateString():"",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = rezve,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop,
                                bulk=bulk,
                                diameter=diameter,
                                expireDate=expireDate,
                                length=length,
                                pressure=pressure,
                                statusColor= statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        //
        public class CylinderList
        {
            public string ID { get; set; }
            public string bulk { get; set; }
            public string model { get; set; }
            public string type { get; set; }
            public string length { get; set; }
            public string pressure { get; set; }
            public string diameter { get; set; }
            public string rezve { get; set; }
            public string productDate { get; set; }
            public string expireDate { get; set; }
            public string serialNumber { get; set; }
            public string constractor { get; set; }
            public string workshop { get; set; }
            public string status { get; set; }
            public string CreateDate { get; set; }
            public string Creator { get; set; }
            public string statusColor { get; set; }
            public string RefreshDate { get; set; }
        }

        // GET: UploadFiles/UploadTankEdit/5
        [RBAC]
        public ActionResult UploadTankEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = db.tbl_Tanks.Find(id);            
            if (tank == null)
            {
                return HttpNotFound();
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", tank.workshop);
            return View(tank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTankEdit(Tank tank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UploadTanks");
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", tank.workshop);
            return View(tank);
        }

        [RBAC]
        // GET: UploadFiles/UploadTankDelete/5
        public ActionResult UploadTankDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = db.tbl_Tanks.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // POST: UploadFiles/UploadTankDelete/5
        [HttpPost, ActionName("UploadTankDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTankDeleteConfirmed(int id)
        {
            Tank tank = db.tbl_Tanks.Find(id);
            db.tbl_Tanks.Remove(tank);
            db.SaveChanges();
            return RedirectToAction("UploadTanks");
        }
        //
        [RBAC]
        public ActionResult UploadTankValveEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankValve tank = db.tbl_TankValves.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", tank.workshop);
            return View(tank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTankValveEdit(TankValve tank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UploadTankValves");
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", tank.workshop);
            return View(tank);
        }

        [RBAC]
        // GET: UploadFiles/UploadTankValveDelete/5
        public ActionResult UploadTankValveDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankValve tank = db.tbl_TankValves.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // POST: UploadFiles/UploadTankValveDelete/5
        [HttpPost, ActionName("UploadTankValveDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTankValveDeleteConfirmed(int id)
        {
            TankValve tank = db.tbl_TankValves.Find(id);
            db.tbl_TankValves.Remove(tank);
            db.SaveChanges();
            return RedirectToAction("UploadTankValves");
        }
        //
        public ActionResult BankTanks(DateTime fromDate, DateTime toDate, bool? Post)
        {
            //Post = false;
            var list = db.tbl_BankTanks.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //           
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string bulk = "";
            string pressure = "";
            string length = "";
            string diameter = "";
            string productDate = "";
            string expireDate = "";
            string rezve = "";
            string serialNumber = "";
            //string status = "";
            string type = "";
            string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_CylinderList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        //command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            bulk = reader["bulk"].ToString();
                            length = reader["length"].ToString();
                            pressure = reader["pressure"].ToString();
                            diameter = reader["diameter"].ToString();
                            productDate = reader["productDate"].ToString();
                            expireDate = reader["expireDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            //status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            Creator = reader["Creator"].ToString();
                            //statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = rezve,
                                serialNumber = serialNumber,
                                //status = status,
                                type = type,
                                workshop = workshop,
                                bulk = bulk,
                                diameter = diameter,
                                expireDate = expireDate,
                                length = length,
                                pressure = pressure
                                //statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        public ActionResult ExportData()
        {
            var ImportTanksList = db.tbl_Tanks;
            var workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();

            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                ViewBag.oldCount = ImportTanksList.Where(i => i.workshop.Equals(workshopId)).Count();
                return View(ImportTanksList.Where(i => i.workshop.Equals(workshopId)).ToList());
            }

            else
            {
                ViewBag.oldCount = ImportTanksList.Count();
                return View(ImportTanksList.ToList());
            }
        }

        [HttpPost]
        public JsonResult RefreshStatus(string id)
        {
            int updatedCount = 0;
            string Message = "";
            string success = "error";
            string serialNumbers = "";
            //تغییر وضعیت مخزن های ثبت شده
            if (id=="1")
            {
                //var bankTanks = db.tbl_BankTanks.ToList();
                var refreshList = db.tbl_Tanks.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                Tank tank = new Tank();
                
                foreach (var item in refreshList)
                {
                    var bankList = db.tbl_BankTanks.Where(t => t.serialNumber.Equals(item.serialNumber)).FirstOrDefault();
                    if (bankList != null)
                    {
                        if(item.status.Equals("نیاز به بررسی"))
                        {
                            updatedCount += 1;
                            Tank updateTank = db.tbl_Tanks.Find(item.ID);
                            //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                            updateTank.status = "تایید شده";
                            //updateTank.constractor = bankList.constractor;
                            updateTank.RefreshCreator = User.Identity.Name;
                            updateTank.RefreshDate = DateTime.Now;

                            db.Entry(updateTank).State = EntityState.Modified;
                            db.SaveChanges();
                            //
                            serialNumbers += item.serialNumber + ",  ";
                        }                        
                    }

                }
                if (updatedCount > 0)
                {
                    Message = "تعداد " + updatedCount +" "+ "ردیف با شماره سریال های  : " + serialNumbers + "  مخزن با موفقیت تایید شد ";
                    success = "success";
                }
                else
                {
                    Message = "هیچ مخزنی برای تایید یافت نشد!";
                    success = "error";
                }
                
            }
            //تغییر وضعیت شیر مخزن های ثبت شده
            if (id == "2")
            {
                //var bankTankValves = db.tbl_BankTankValves.ToList();
                var refreshList = db.tbl_TankValves.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                Tank tank = new Tank();

                foreach (var item in refreshList)
                {
                    var bankList = db.tbl_BankTankValves.Where(t => t.serialNumber.Equals(item.serialNumber)).FirstOrDefault();
                    if (bankList != null)
                    {
                        if(item.status.Equals("نیاز به بررسی"))
                        {
                            updatedCount += 1;
                            TankValve updateTankValve = db.tbl_TankValves.Find(item.ID);
                            //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                            updateTankValve.status = "تایید شده";
                            updateTankValve.RefreshCreator = User.Identity.Name;
                            updateTankValve.RefreshDate = DateTime.Now;

                            db.Entry(updateTankValve).State = EntityState.Modified;
                            db.SaveChanges();
                            //
                            serialNumbers += item.serialNumber + ",  ";
                        }
                        
                    }

                }
                if (updatedCount > 0)
                {
                    Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  شیر مخزن با موفقیت تایید شد";
                    success = "success";
                }
                else
                {
                    Message = "هیچ شیر مخزنی برای تایید یافت نشد!";
                    success = "error";
                }
            }
            //تغییر وضعیت رگلاتورهای ثبت شده
            if (id == "3")
            {
                //var bankKits = db.tbl_BankKits.ToList();
                var refreshList = db.tbl_Kits.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                Kit tank = new Kit();

                foreach (var item in refreshList)
                {
                    string serialNumber = item.serialNumber.Replace(" ", "");

                    var bankList = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).FirstOrDefault();
                    if (bankList != null)
                    {
                        if (item.status.Equals("نیاز به بررسی"))
                        {
                            updatedCount += 1;
                            Kit updateKit = db.tbl_Kits.Find(item.ID);
                            //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                            updateKit.status = "تایید شده";
                            updateKit.RefreshCreator = User.Identity.Name;
                            updateKit.RefreshDate = DateTime.Now;

                            db.Entry(updateKit).State = EntityState.Modified;
                            db.SaveChanges();
                            //
                            serialNumbers += item.serialNumber + ",  ";
                        }

                    }

                }
                if (updatedCount > 0)
                {
                    Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  رگلاتور با موفقیت تایید شد";
                    success = "success";
                }
                else
                {
                    Message = "هیچ رگلاتوری برای تایید یافت نشد!";
                    success = "error";
                }
            }
            //تغییر وضعیت شیر قطع کن های ثبت شده
            if (id == "4")
            {
                //var bankcutofvalve = db.tbl_BankCutofValves.ToList();
                var refreshList = db.tbl_CutofValves.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                CutofValve tank = new CutofValve();

                foreach (var item in refreshList)
                {
                    string serialNumber = item.serialNumber.Replace(" ", "");

                    var bankList = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).FirstOrDefault();
                    if (bankList != null)
                    {
                        if (item.status.Equals("نیاز به بررسی"))
                        {
                            updatedCount += 1;
                            CutofValve updatecutofvalve = db.tbl_CutofValves.Find(item.ID);
                            //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                            updatecutofvalve.status = "تایید شده";
                            //updatecutofvalve.constractor = bankList.constractor;
                            //updatecutofvalve.model = bankList.model;
                            updatecutofvalve.RefreshCreator = User.Identity.Name;
                            updatecutofvalve.RefreshDate = DateTime.Now;

                            db.Entry(updatecutofvalve).State = EntityState.Modified;
                            db.SaveChanges();
                            //
                            serialNumbers += item.serialNumber + ",  ";
                        }

                    }

                }
                if (updatedCount > 0)
                {
                    Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  شیر قطع کن با موفقیت تایید شد";
                    success = "success";
                }
                else
                {
                    Message = "هیچ شیر قطع کنی برای تایید یافت نشد!";
                    success = "error";
                }
            }
            //تغییر وضعیت شیر پر کن های ثبت شده
            if (id == "5")
            {
                //var bankfillingvalve = db.tbl_BankFillingValves.ToList();
                var refreshList = db.tbl_FillingValves.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                FillingValve fillingvalve = new FillingValve();
                try
                {
                    foreach (var item in refreshList)
                    {
                        string serialNumber = item.serialNumber.Replace(" ", "");

                        var bankList = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).FirstOrDefault();
                        if (bankList != null)
                        {
                            if (item.status.Equals("نیاز به بررسی"))
                            {
                                updatedCount += 1;
                                FillingValve updatefillingvalve = db.tbl_FillingValves.Find(item.ID);
                                //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                                updatefillingvalve.status = "تایید شده";
                                //updatefillingvalve.constractor = bankList.constractor;
                                //updatefillingvalve.model = bankList.model;
                                updatefillingvalve.RefreshCreator = User.Identity.Name;
                                updatefillingvalve.RefreshDate = DateTime.Now;

                                db.Entry(updatefillingvalve).State = EntityState.Modified;
                                db.SaveChanges();
                                //
                                serialNumbers += item.serialNumber + ",  ";
                            }

                        }

                    }
                    if (updatedCount > 0)
                    {
                        Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  شیر پر کن با موفقیت تایید شد";
                        success = "success";
                    }
                    else
                    {
                        Message = "هیچ شیر پرکنی برای تایید یافت نشد!";
                        success = "error";
                    }
                }
                catch
                {
                    Message = "در ردیف " + updatedCount + " " + " با شماره سریال های  : " + serialNumbers + "  مشکلی بوجود آمده!";
                    success = "error";
                }
            }
            //تغییر وضعیت رله سوخت های ثبت شده
            if (id == "6")
            {
                var refreshList = db.tbl_FuelRelays.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                FuelRelay fuelrelay = new FuelRelay();
                try
                {
                    foreach (var item in refreshList)
                    {
                        string serialNumber = item.serialNumber.Replace(" ", "");

                        var bankList = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).FirstOrDefault();
                        if (bankList != null)
                        {
                            if (item.status.Equals("نیاز به بررسی"))
                            {
                                updatedCount += 1;
                                FuelRelay updatefuelrelay = db.tbl_FuelRelays.Find(item.ID);
                                updatefuelrelay.status = "تایید شده";                                
                                updatefuelrelay.RefreshCreator = User.Identity.Name;
                                updatefuelrelay.RefreshDate = DateTime.Now;

                                db.Entry(updatefuelrelay).State = EntityState.Modified;
                                db.SaveChanges();
                                //
                                serialNumbers += item.serialNumber + ",  ";
                            }

                        }

                    }
                    if (updatedCount > 0)
                    {
                        Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  رله سوخت با موفقیت تایید شد";
                        success = "success";
                    }
                    else
                    {
                        Message = "هیچ رله سوختی برای تایید یافت نشد!";
                        success = "error";
                    }
                }
                catch
                {
                    Message = "در ردیف " + updatedCount + " " + " با شماره سریال های  : " + serialNumbers + "  مشکلی بوجود آمده!";
                    success = "error";
                }
            }
            //تغییر وضعیت GAS ECU های ثبت شده
            if (id == "7")
            {
                var refreshList = db.tbl_GasECU.Where(t => t.status.Equals("نیاز به بررسی")).ToList();
                GasECU gasecu = new GasECU();
                try
                {
                    foreach (var item in refreshList)
                    {
                        string serialNumber = item.serialNumber.Replace(" ", "");

                        var bankList = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).FirstOrDefault();
                        if (bankList != null)
                        {
                            if (item.status.Equals("نیاز به بررسی"))
                            {
                                updatedCount += 1;
                                GasECU updategasecu = db.tbl_GasECU.Find(item.ID);
                                updategasecu.status = "تایید شده";
                                updategasecu.RefreshCreator = User.Identity.Name;
                                updategasecu.RefreshDate = DateTime.Now;

                                db.Entry(updategasecu).State = EntityState.Modified;
                                db.SaveChanges();
                                //
                                serialNumbers += item.serialNumber + ",  ";
                            }

                        }

                    }
                    if (updatedCount > 0)
                    {
                        Message = "تعداد " + updatedCount + " " + "ردیف با شماره سریال های  : " + serialNumbers + "  GAS ECU با موفقیت تایید شد";
                        success = "success";
                    }
                    else
                    {
                        Message = "هیچ GAS ECU برای تایید یافت نشد!";
                        success = "error";
                    }
                }
                catch
                {
                    Message = "در ردیف " + updatedCount + " " + " با شماره سریال های  : " + serialNumbers + "  مشکلی بوجود آمده!";
                    success = "error";
                }
            }

            return Json(new { success = success, responseText = Message }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  

        [HttpPost]
        public JsonResult UploadTanks(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش مخزن"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {                    
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات مخازن تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var tanks = from a in excelFile.Worksheet<Tank>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in tanks)
                    {
                        try
                        {
                            dataCount = db.tbl_BankTanks.Where(t => t.serialNumber.Equals(a.serialNumber)).Count();
                            
                            rowDoubledCount += dataCount;

                            if (dataCount==0)
                            {
                                countRow+=1;
                                if (a.serialNumber != null && a.bulk != null && a.rezve != null && a.productDate != null && a.expireDate != null && a.constractor != null)
                                {
                                    BankTank TU = new BankTank();
                                    TU.bulk = a.bulk;
                                    TU.constractor = a.constractor;
                                    TU.diameter = a.diameter;
                                    TU.expireDate = a.expireDate;
                                    if (!string.IsNullOrEmpty(a.gregorianEMonth))
                                        TU.gregorianEMonth = a.gregorianEMonth;
                                    if (!string.IsNullOrEmpty(a.gregorianEYear))
                                        TU.gregorianEYear = a.gregorianEYear;
                                    if (!string.IsNullOrEmpty(a.gregorianPMonth))
                                        TU.gregorianPMonth = a.gregorianPMonth;
                                    if (!string.IsNullOrEmpty(a.gregorianPYear))
                                        TU.gregorianPYear = a.gregorianPYear;
                                    TU.length = a.length;
                                    TU.model = a.model;
                                    TU.pressure = a.pressure;
                                    TU.productDate = a.productDate;
                                    TU.rezve = a.rezve;
                                    TU.serialNumber = a.serialNumber;
                                    TU.type = a.type;
                                    if(!string.IsNullOrEmpty(a.workshop))
                                        TU.workshop = a.workshop;
                                    if (!string.IsNullOrEmpty(a.status))
                                        TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "مخزن";

                                    db.tbl_BankTanks.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.bulk == "" || a.bulk == null) data.Add("<li> bulk is required</li>");
                                    if (a.rezve == "" || a.rezve == null) data.Add("<li> rezve is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");
                                    if (a.expireDate == null) data.Add("<li> expireDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber=a.serialNumber,
                                    Message="تکراری و در بانک ثبت نشد!",
                                    Success= "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                            
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }

                    if (countRow > 0)
                    {
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }

                    //ViewBag.Message = Message;

                    //return RedirectToAction("UploadExcel");

                    // string Message =  "ردیف تکراری شناسایی شد که در بانک اطلاعاتی ثبت نشد!";    

                    //return View();
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
           
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }

        // GET: UploadFiles/ Upload Tank Valves Information
        public class GridView
        {
            public List<TankValve> data { get; set; }
        }
        public JsonResult GetValves()
        {
            GridView dataTables = new GridView();
            List<TankValve> valve = (from items in db.tbl_TankValves
                                     select new TankValve
                                     {
                                         workshop=items.workshop,
                                         type=items.type,
                                         status=items.status,
                                         serialNumber=items.serialNumber,
                                         rezve=items.rezve,
                                         constractor=items.constractor,
                                         CreateDate=items.CreateDate,
                                         Creator=items.Creator,
                                         ID=items.ID,
                                         model=items.model,
                                         productDate=items.productDate
                                     }).ToList();
            //

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string rezve = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            //
            List<TankValve> TableOuts = new List<TankValve>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_TankValveList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = 0;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            constractor = reader["constractor"].ToString();
                            if (reader["CreateDate"] != DBNull.Value)
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            Creator = reader["Creator"].ToString();
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();

                            TableOuts.Add(new TankValve
                            {
                                constractor = constractor,
                                CreateDate = CreateDate,
                                Creator = Creator,
                                ID = ID,
                                model = model,
                                productDate = productDate,
                                rezve = rezve,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop
                            });
                        }

                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TankValveTableOuts = null;
            }
            //The magic happens here
            dataTables.data = TableOuts;
            return Json(dataTables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadTankValves(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            //Post = false;
            var list = db.tbl_TankValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            string constractor = "";
            DateTime? CreateDate=null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string rezve = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_TankValveList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor=constractor,
                                CreateDate=CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue ? RefreshDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator =Creator,
                                ID=ID.ToString(),
                                model=model,
                                productDate=productDate,
                                rezve=rezve,
                                serialNumber=serialNumber,
                                status=status,
                                type=type,
                                workshop=workshop,
                                statusColor=statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>
        public ActionResult BankTankValves(DateTime fromDate, DateTime toDate, bool? Post)
        {
            //Post = false;
            var list = db.tbl_BankTankValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }
            
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string rezve = "";
            string serialNumber = "";
            //string status = "";
            string type = "";
            //string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_TankValveList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            //status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            //workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            Creator = reader["Creator"].ToString();
                            //statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = rezve,
                                serialNumber = serialNumber,
                                //status = status,
                                type = type
                                //workshop = workshop,
                                //statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        [HttpPost]
        public JsonResult UploadTankValves(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش شیر مخزن"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {                    
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات مخازن تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var tankvalves = from a in excelFile.Worksheet<TankValve>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in tankvalves)
                    {
                        try
                        {
                            dataCount = db.tbl_BankTankValves.Where(t => t.serialNumber.Equals(a.serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if(dataCount==0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.rezve != null && a.productDate != null && a.type != null)
                                {
                                    BankTankValve TU = new BankTankValve();
                                    TU.serialNumber = a.serialNumber;
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.type = a.type;
                                    TU.rezve = a.rezve;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "شیر مخزن";

                                    db.tbl_BankTankValves.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.type == "" || a.type == null) data.Add("<li> type is required</li>");
                                    if (a.rezve == "" || a.rezve == null) data.Add("<li> rezve is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber,
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        public ActionResult BankKits(DateTime fromDate, DateTime toDate, bool? Post)
        {
            //Post = false;
            var list = db.tbl_BankKits.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }
                       
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            //string status = "";
            string type = "";
            //string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_RegulatorList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            //status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            //workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            Creator = reader["Creator"].ToString();
                            //statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                //status = status,
                                type = type,
                                //workshop = workshop,
                                //statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        // GET: UploadFiles/ Upload Kits Information
        public ActionResult UploadKits(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            //Post = false;
            var list = db.tbl_Kits.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_RegulatorList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate= RefreshDate.HasValue? RefreshDate.GetValueOrDefault().ToShortDateString():"",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop,
                                statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        
        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  

        [HttpPost]
        public JsonResult UploadKits(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش رگلاتور"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {                   
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات مخازن تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var kits = from a in excelFile.Worksheet<Kit>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in kits)
                    {
                        try
                        {
                            var serialNumber = a.serialNumber.Replace(" ", "");
                            dataCount = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if (dataCount == 0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.generation != null && a.productDate != null && a.type != null)
                                {
                                    BankKit TU = new BankKit();
                                    TU.serialNumber = a.serialNumber.Replace(" ", "");
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.type = a.type;
                                    TU.generation = a.generation;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "رگلاتور";

                                    db.tbl_BankKits.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.type == "" || a.type == null) data.Add("<li> type is required</li>");
                                    if (a.generation == "" || a.generation == null) data.Add("<li> rezve is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber,
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        //[RBAC]
        public ActionResult UploadKitEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kit kit = db.tbl_Kits.Find(id);
            if (kit == null)
            {
                return HttpNotFound();
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", kit.workshop);
            return View(kit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadKitEdit(Kit kit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UploadKits");
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", kit.workshop);
            return View(kit);
        }

        [RBAC]
        // GET: UploadFiles/UploadKitDelete/5
        public ActionResult UploadKitDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kit kit = db.tbl_Kits.Find(id);
            if (kit == null)
            {
                return HttpNotFound();
            }
            return View(kit);
        }

        // POST: UploadFiles/UploadKitDelete/5
        [HttpPost, ActionName("UploadKitDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadKitDeleteConfirmed(int id)
        {
            Kit kit = db.tbl_Kits.Find(id);
            db.tbl_Kits.Remove(kit);
            db.SaveChanges();
            return RedirectToAction("UploadKits");
        }
        //
        #endregion

        #region Checked Upload Files with Workshops
        // GET: UploadFiles/ Upload Kits Information
        public ActionResult CheckedRecieveTanks(string message)
        {
            ViewBag.message = message;
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            var typeofTank = db.tbl_TypeofTanks.ToList();
            ViewBag.TypeofTankID = new SelectList(typeofTank.Distinct(), "ID", "Type");
            var ImportTanksList = db.tbl_Tanks;
            return View(ImportTanksList.ToList());
        }
        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  

        [HttpPost]
        public JsonResult CheckedRecieveTanks(HttpPostedFileBase FileUpload,string[] serialNumber,string TankConstractorID,string TypeofTankID,string rezve, string[] ProductMonth,string[] ProductYear,string[] ExpirationMonth,string[] ExpirationYear)
        {
            int countRow = 0;
            string Message = "";
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            Tank tank = new Tank();

            //if(serialNumber==null)
            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber!=null)
            {
                //foreach(var item in serialNumber)
                for(int i=0;i<serialNumber.Count();i++)
                {
                    if(!string.IsNullOrEmpty(serialNumber[i]))
                    {
                        try
                        {

                            if (TankConstractorID != null)
                            {
                                TankConstractor constractor = db.tbl_TankConstractors.Find(Convert.ToInt32(TankConstractorID));
                                if (constractor != null)
                                {
                                    tank.constractor = constractor.Constractor;                                    
                                }

                                if (TypeofTankID != null)
                                {
                                    string bulk = Helper.Helpers.GetBulk(Convert.ToInt32(TypeofTankID)).Type;
                                    var details = db.tbl_CylinderDetails.Where(c => c.ConstractorId.ToString().Equals(TankConstractorID) && c.Bulk == bulk).ToList();
                                    if (details.Count > 0)
                                    {
                                        tank.diameter = details.SingleOrDefault().Diameter;
                                        tank.length = details.SingleOrDefault().Lenght;
                                        tank.pressure = details.SingleOrDefault().Pressure;
                                        tank.rezve = details.SingleOrDefault().Rezve;
                                        tank.model = details.SingleOrDefault().Model;
                                    }
                                    else
                                    {
                                        tank.diameter = "مشکل در یافتن اطلاعات سازنده!";
                                        tank.length = "مشکل در یافتن اطلاعات سازنده!";
                                        tank.pressure = "مشکل در یافتن اطلاعات سازنده!";
                                        tank.rezve = "مشکل در یافتن اطلاعات سازنده!";
                                        tank.model = "مشکل در یافتن اطلاعات سازنده!";
                                    }
                                }
                            }

                            tank.bulk = TypeofTankID;                            
                            tank.gregorianEMonth = ExpirationMonth[i];
                            tank.gregorianEYear = ExpirationYear[i];
                            tank.gregorianPMonth = ProductMonth[i];
                            tank.gregorianPYear = ProductYear[i];
                            //tank.rezve = rezve;
                            //tank.pressure = "200";
                            tank.type = "نوع اول";
                            tank.serialNumber = serialNumber[i];
                            tank.status = "تخصیص یافته";
                            tank.CreateDate = DateTime.Now;
                            tank.workshop = Helper.Helpers.GetWorkshopCurrentUser().Title;
                            tank.MaterailName = "مخزن";

                            db.tbl_Tanks.Add(tank);
                            db.SaveChanges();                            

                            //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                            //var dataCount = db.tbl_Tanks.Where(t => t.serialNumber.Equals(item)).Count();
                            //if (dataCount == 1)
                            //{
                            //    var rowId = db.tbl_Tanks.Where(t => t.serialNumber.Equals(item)).SingleOrDefault().ID;
                            //    Tank updateTank = db.tbl_Tanks.Find(rowId);
                            //    updateTank.status = "تخصیص یافته";
                            //    db.Entry(updateTank).State = EntityState.Modified;
                            //    db.SaveChanges();
                            //    countRow += 1;
                            //    Message = "تعداد " + countRow + "با موفقیت تایید شد";
                            //}
                            //else
                            //{
                            //    var rowId = db.tbl_Tanks.Where(t => t.serialNumber.Equals(item)).FirstOrDefault().ID;
                            //    Tank updateTank = db.tbl_Tanks.Find(rowId);
                            //    updateTank.status = "تخصیص یافته";
                            //    db.Entry(updateTank).State = EntityState.Modified;
                            //    db.SaveChanges();
                            //    countRow += 1;
                            //}
                        }
                        catch
                        {
                            //Message = "حتما یک ردیف باید پر شود!";
                            doubletankitems.Add(new DoubledTankItems
                            {
                                serialNumber = serialNumber[i]
                            });
                        }
                    }
                    else
                    {
                        Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }

                countRow += 1;
                Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }
            
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpPost]
        public ActionResult AddRecieveTanks(HttpPostedFileBase FileUpload, string[] serialNumber, string TankConstractorID, string TypeofTankID, string rezve, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear)
        {
            //int countRow = 0;
            //string Message = "";
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            Tank tank = new Tank();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber != null)
            {
                //foreach (var item in serialNumber)
                for(int i=0;i<serialNumber.Count();i++)
                {
                    //if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(ProductMonth[i]) && !string.IsNullOrEmpty(ProductYear[i]) && !string.IsNullOrEmpty(ExpirationMonth[i]) && !string.IsNullOrEmpty(ExpirationYear[i]))
                    if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(TankConstractorID) && !string.IsNullOrEmpty(TypeofTankID) )
                    {
                        insertValue = true;
                        var serialId = serialNumber[i];
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExistTank = db.tbl_Tanks.Where(t => t.serialNumber.Equals(serialId)).Count();
                        var isExistBankTank = db.tbl_BankTanks.Where(t => t.serialNumber.Equals(serialId)).Count();
                        
                        if (isExistBankTank > 0)
                        {
                            if(isExistTank == 0)
                            {
                                var ExistBankTank = db.tbl_BankTanks.Where(t => t.serialNumber.Equals(serialId)).FirstOrDefault();

                                tank.bulk = ExistBankTank.bulk;
                                tank.constractor = ExistBankTank.constractor;
                                tank.gregorianEMonth = ExistBankTank.gregorianEMonth;
                                tank.gregorianEYear = ExistBankTank.gregorianEYear;
                                tank.gregorianPMonth = ExistBankTank.gregorianPMonth;
                                tank.gregorianPYear = ExistBankTank.gregorianPYear;
                                tank.rezve = ExistBankTank.rezve;
                                tank.pressure = ExistBankTank.pressure;
                                tank.type = ExistBankTank.type;
                                tank.serialNumber = ExistBankTank.serialNumber;
                                //tank.status = "تخصیص یافته- ثبت توسط کارگاه";
                                tank.status = "تایید شده";
                                tank.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                tank.expireDate = ExistBankTank.expireDate;
                                tank.productDate = ExistBankTank.productDate;
                                tank.CreateDate = DateTime.Now;
                                tank.Creator = User.Identity.Name;
                                tank.model = ExistBankTank.model;
                                tank.diameter = ExistBankTank.diameter;
                                tank.length = ExistBankTank.length;
                                tank.MaterailName = "مخزن";

                                db.tbl_Tanks.Add(tank);
                                db.SaveChanges();
                            }                           
                            //Tank updateTank = db.tbl_Tanks.Find(rowId);
                            //updateTank.status = "تخصیص یافته- ثبت از طریق فایل";
                            //updateTank.status = "تایید شده";
                            //updateTank.workshop= Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                            //updateTank.CreateDate = DateTime.Now;
                            //updateTank.Creator = User.Identity.Name;

                            //db.Entry(updateTank).State = EntityState.Modified;
                            //db.SaveChanges();
                            //countRow += 1;
                            //Message = "تعداد " + countRow + "با موفقیت تایید شد";
                        }

                        else
                        {
                            if (isExistTank == 0 )
                            {
                                if (TankConstractorID != null)
                                {
                                    TankConstractor constractor = db.tbl_TankConstractors.Find(Convert.ToInt32(TankConstractorID));
                                    if (constractor != null)
                                    {
                                        tank.constractor = constractor.Constractor;                                        
                                    }

                                    if (TypeofTankID != null)
                                    {
                                        //string bulk = Helper.Helpers.GetBulk(Convert.ToInt32(TypeofTankID)).Type;
                                        var details = db.tbl_CylinderDetails.Where(c => c.ConstractorId.ToString().Equals(TankConstractorID) && c.Bulk == TypeofTankID).ToList();
                                        if (details.Count > 0)
                                        {
                                            tank.bulk = TypeofTankID;
                                            tank.diameter = details.SingleOrDefault().Diameter;
                                            tank.length = details.SingleOrDefault().Lenght;
                                            tank.pressure = details.SingleOrDefault().Pressure;
                                            tank.rezve = details.SingleOrDefault().Rezve;
                                            tank.model = details.SingleOrDefault().Model;
                                        }
                                        else
                                        {
                                            tank.bulk = TypeofTankID;
                                            tank.diameter = "مشکل در یافتن اطلاعات سازنده!";
                                            tank.length = "مشکل در یافتن اطلاعات سازنده!";
                                            tank.pressure = "مشکل در یافتن اطلاعات سازنده!";
                                            tank.rezve = "مشکل در یافتن اطلاعات سازنده!";
                                            tank.model = "مشکل در یافتن اطلاعات سازنده!";
                                        }
                                    }
                                }
                                
                                tank.constractor = TankConstractorID;
                                tank.gregorianEMonth = ExpirationMonth[i];
                                tank.gregorianEYear = ExpirationYear[i];
                                tank.gregorianPMonth = ProductMonth[i];
                                tank.gregorianPYear = ProductYear[i];
                               // tank.rezve = rezve;
                                //tank.pressure = "200";
                                tank.type = "نوع اول";
                                tank.serialNumber = serialNumber[i];
                                //tank.status = "تخصیص یافته- ثبت توسط کارگاه";
                                tank.status = "نیاز به بررسی";
                                tank.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                tank.expireDate = ExpirationYear[i] + "/" + ExpirationMonth[i] + "/01";
                                tank.productDate = ProductYear[i] + "/" + ProductMonth[i] + "/01";
                                tank.CreateDate = DateTime.Now;
                                tank.Creator = User.Identity.Name;
                                tank.MaterailName = "مخزن";

                                db.tbl_Tanks.Add(tank);
                                db.SaveChanges();
                                //countRow += 1;
                            }

                        }

                        
                    }
                    else if (insertValue==false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if(status==true)
                {
                    return RedirectToAction("UploadTanks");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }

            return RedirectToAction("CheckedRecieveTanks",new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
        }
        /// <summary>
        /// for check tanks valve
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedRecieveTankValves(string message)
        {
            ViewBag.message = message;
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            var ImportTankValveList = db.tbl_TankValves;
            return View(ImportTankValveList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddRecieveTankValves(HttpPostedFileBase FileUpload, string[] serialNumber, string ValveConstractorID, string TypeofValveID, string rezve, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear)
        {
            //int countRow = 0;
            //string Message = "";
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            TankValve valve = new TankValve();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (!string.IsNullOrEmpty(rezve) && !string.IsNullOrEmpty(TypeofValveID) && !string.IsNullOrEmpty(ValveConstractorID))
            {
                //foreach (var item in serialNumber)
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]))
                    {
                        insertValue = true;
                        var serialId = serialNumber[i];
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExistTank = db.tbl_TankValves.Where(t => t.serialNumber.Equals(serialId)).Count();
                        var isExistBankTank = db.tbl_BankTankValves.Where(t => t.serialNumber.Equals(serialId)).Count();

                        if (isExistBankTank > 0)
                        {
                            if(isExistTank == 0)
                            {
                                var ExistBankTank = db.tbl_BankTankValves.Where(t => t.serialNumber.Equals(serialId)).FirstOrDefault();
                                valve.model = ExistBankTank.model;
                                valve.type = ExistBankTank.type;
                                valve.constractor = ExistBankTank.constractor;
                                valve.rezve = ExistBankTank.rezve;
                                valve.serialNumber = ExistBankTank.serialNumber;
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                valve.status = "تایید شده";
                                valve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                valve.productDate = ExistBankTank.productDate;
                                valve.CreateDate = DateTime.Now;
                                valve.Creator = User.Identity.Name;
                                valve.MaterailName = "شیر مخزن";

                                db.tbl_TankValves.Add(valve);
                                db.SaveChanges();
                            }                            
                            //TankValve updateTankValve = db.tbl_TankValves.Find(rowId);
                            //updateTankValve.status = "تخصیص یافته- ثبت از طریق فایل";
                            //updateTankValve.status = "تایید شده";
                            //updateTankValve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                            //updateTankValve.CreateDate = DateTime.Now;
                            //updateTankValve.Creator = User.Identity.Name;

                            //db.Entry(updateTankValve).State = EntityState.Modified;
                            //db.SaveChanges();
                            //countRow += 1;
                            //Message = "تعداد " + countRow + "با موفقیت تایید شد";
                        }
                        else
                        {
                            if (isExistTank == 0)
                            {
                                //شرکت البرز یدک
                                if (ValveConstractorID == "1")
                                {
                                    valve.model = "W28.8";
                                }
                                valve.type = TypeofValveID;
                                valve.constractor = ValveConstractorID;
                                valve.rezve = rezve;
                                valve.serialNumber = serialNumber[i];
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                valve.status = "نیاز به بررسی";
                                valve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                valve.productDate = ProductYear[i];
                                valve.CreateDate = DateTime.Now;
                                valve.Creator = User.Identity.Name;
                                valve.MaterailName = "شیر مخزن";

                                db.tbl_TankValves.Add(valve);
                                db.SaveChanges();
                                //countRow += 1;
                            }

                        }


                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("UploadTankValves");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }
            return RedirectToAction("CheckedRecieveTankValves", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
        }
        //
        public ActionResult ExportTankValvesData()
        {
            //var ImportTankValvesList = db.tbl_TankValves;
            var workshopId = "0";

            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                //ViewBag.oldCount = ImportTankValvesList.Where(i => i.workshop.Equals(workshopId)).Count();
                //return View(ImportTankValvesList.Where(i => i.workshop.Equals(workshopId)).ToList());
            }

            //else
            //{
            //    ViewBag.oldCount = ImportTankValvesList.Count();
            //    return View(ImportTankValvesList.ToList());
            //}

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string rezve = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";

            List<TankValve> TableOuts = new List<TankValve>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_TankValveList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = workshopId;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            constractor = reader["constractor"].ToString();
                            if (reader["CreateDate"] != DBNull.Value)
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            Creator = reader["Creator"].ToString();
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            rezve = reader["rezve"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();

                            TableOuts.Add(new TankValve
                            {
                                constractor = constractor,
                                CreateDate = CreateDate,
                                Creator = Creator,
                                ID = ID,
                                model = model,
                                productDate = productDate,
                                rezve = rezve,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop
                            });
                        }

                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TankValveTableOuts = null;
            }
            return View(TableOuts.ToList());
        }

        /// <summary>
        /// for check kit
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedRecieveKits(string message)
        {
            ViewBag.message = message;
            ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator");
            var ImportTanksList = db.tbl_Kits;
            return View(ImportTanksList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddRecieveKits(HttpPostedFileBase FileUpload, string[] serialNumber, string RegulatorConstractorID, string TypeofKitID, string Genaration, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear)
        {
            //int countRow = 0;
            //string Message = "";
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            Kit kit = new Kit();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (!String.IsNullOrEmpty(RegulatorConstractorID) && !String.IsNullOrEmpty(TypeofKitID) && !String.IsNullOrEmpty(Genaration))
            {
                //foreach (var item in serialNumber)
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]))
                    {
                        insertValue = true;
                        var serialId = serialNumber[i].Replace(" ","");
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExistKit = db.tbl_Kits.Where(t => t.serialNumber.Replace(" ","").Equals(serialId)).Count();
                        var isExistBankKit = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                        if (isExistBankKit > 0)
                        {
                            if (isExistKit == 0)
                            {
                                var ExistBankKit = db.tbl_BankKits.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                kit.model = ExistBankKit.model;
                                kit.type = ExistBankKit.type;
                                kit.constractor = ExistBankKit.constractor;
                                kit.generation = ExistBankKit.generation;
                                kit.serialNumber = ExistBankKit.serialNumber;
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                kit.status = "تایید شده";
                                kit.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                kit.productDate = ExistBankKit.productDate;
                                kit.CreateDate = DateTime.Now;
                                kit.Creator = User.Identity.Name;
                                kit.MaterailName = "رگلاتور";

                                db.tbl_Kits.Add(kit);
                                db.SaveChanges();
                            }
                            //TankValve updateTankValve = db.tbl_TankValves.Find(rowId);
                            //updateTankValve.status = "تخصیص یافته- ثبت از طریق فایل";
                            //updateTankValve.status = "تایید شده";
                            //updateTankValve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                            //updateTankValve.CreateDate = DateTime.Now;
                            //updateTankValve.Creator = User.Identity.Name;

                            //db.Entry(updateTankValve).State = EntityState.Modified;
                            //db.SaveChanges();
                            //countRow += 1;
                            //Message = "تعداد " + countRow + "با موفقیت تایید شد";
                        }
                        else
                        {
                            if (isExistKit == 0)
                            {
                                //شرکت الکتروفن
                                if (RegulatorConstractorID == "1")
                                {
                                    kit.model = "EF10.199";
                                }
                                //شرکت شهاب گازسوز
                                if (RegulatorConstractorID == "2")
                                {
                                    kit.model = "SHAHAB";
                                }
                                kit.type = TypeofKitID;
                                kit.generation = Genaration;
                                kit.constractor = RegulatorConstractorID;
                                kit.generation = Genaration;
                                kit.serialNumber = serialNumber[i];
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                kit.status = "نیاز به بررسی";
                                kit.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                kit.productDate = ProductYear[i];
                                kit.CreateDate = DateTime.Now;
                                kit.Creator = User.Identity.Name;
                                kit.MaterailName = "رگلاتور";

                                db.tbl_Kits.Add(kit);
                                db.SaveChanges();
                                //countRow += 1;
                            }

                        }


                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("UploadKits");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }

            return RedirectToAction("CheckedRecieveKits", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
        }
        //
        public ActionResult ExportKitsData()
        {
            var ImportKitsList = db.tbl_Kits;
            var workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();

            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                ViewBag.oldCount = ImportKitsList.Where(i => i.workshop.Equals(workshopId)).Count();
                return View(ImportKitsList.Where(i => i.workshop.Equals(workshopId)).ToList());
            }

            else
            {
                ViewBag.oldCount = ImportKitsList.Count();
                return View(ImportKitsList.ToList());
            }
        }

        #endregion

        public class DoubledTankItems
        {
            public string serialNumber { get; set; }
        }

        /// <summary>
        /// for check Cut of Valves
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedCutofValves(string message)
        {
            ViewBag.message = message;
            ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve");
            var ImportCutofValvesList = db.tbl_CutofValves;
            return View(ImportCutofValvesList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddCutofValves(HttpPostedFileBase FileUpload, string[] serialNumber, string CutofValveConstractorID, string Model, string[] ProductDate)
        {            
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            CutofValve cutofvalve = new CutofValve();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber != null)
            {
                //foreach (var item in serialNumber)
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(CutofValveConstractorID) && !string.IsNullOrEmpty(Model))
                    {
                        var serialId = serialNumber[i].Replace(" ", "");
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExist = db.tbl_CutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                        var isExistBank = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                        if (isExistBank > 0)
                        {
                            if (isExist == 0)
                            {
                                var ExistBank = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                cutofvalve.model = ExistBank.model;
                                cutofvalve.constractor = ExistBank.constractor;
                                cutofvalve.serialNumber = ExistBank.serialNumber.ToUpper();
                                cutofvalve.status = "تایید شده";
                                cutofvalve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                cutofvalve.productDate = ExistBank.productDate;
                                cutofvalve.CreateDate = DateTime.Now;
                                cutofvalve.Creator = User.Identity.Name;
                                cutofvalve.MaterailName = "شیر قطع کن";

                                db.tbl_CutofValves.Add(cutofvalve);
                                db.SaveChanges();
                            }                           
                        }
                        else
                        {
                            if (isExist == 0 && !string.IsNullOrEmpty(CutofValveConstractorID)) //&& !string.IsNullOrEmpty(ProductDate[i])
                            {                                  
                                if (!string.IsNullOrEmpty(CutofValveConstractorID))
                                {
                                    cutofvalve.model = Helper.Helpers.GetCutoffValveConstractor(Convert.ToInt32(CutofValveConstractorID)).Code;
                                    cutofvalve.constractor = Helper.Helpers.GetCutoffValveConstractor(Convert.ToInt32(CutofValveConstractorID)).CutofValve;
                                }
                                else
                                {
                                    cutofvalve.model = "";
                                    cutofvalve.constractor = "";
                                }
                                cutofvalve.serialNumber = serialNumber[i].ToUpper();
                                cutofvalve.status = "نیاز به بررسی";
                                cutofvalve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                cutofvalve.productDate = ProductDate[i];
                                cutofvalve.CreateDate = DateTime.Now;
                                cutofvalve.Creator = User.Identity.Name;
                                cutofvalve.MaterailName = "شیر قطع کن";

                                db.tbl_CutofValves.Add(cutofvalve);
                                db.SaveChanges();
                                //countRow += 1;
                            }
                        }
                        //                       
                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("CutofValves");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }
            return RedirectToAction("CheckedCutofValves", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
            //return View();
        }

        // GET: CutofValves List
        public ActionResult CutofValves(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            // Post = false;
            var list = db.tbl_CutofValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            //
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string serialNumber = "";
            string status = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_CutoffValveList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue ? RefreshDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                serialNumber = serialNumber,
                                status = status,
                                workshop = workshop,
                                statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
            //
        }
        //
        /// <summary>
        /// for check filling valves
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedFillingValves(string message)
        {
            ViewBag.message = message;
            ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve");
            var ImportFillingValvesList = db.tbl_FillingValves;
            return View(ImportFillingValvesList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddFillingValves(HttpPostedFileBase FileUpload, string[] serialNumber, string FillingValveConstractorID, string Model, string[] ProductDate)
        {
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            FillingValve fillingvalve = new FillingValve();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber != null)
            {
                //foreach (var item in serialNumber)
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(FillingValveConstractorID) && !string.IsNullOrEmpty(Model))
                    {
                        var serialId = serialNumber[i].Replace(" ", "");
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExist = db.tbl_FillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                        var isExistBank = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                        if (isExistBank > 0)
                        {
                            if (isExist == 0)
                            {
                                var ExistBank = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                fillingvalve.model = ExistBank.model;
                                fillingvalve.constractor = ExistBank.constractor;
                                fillingvalve.serialNumber = ExistBank.serialNumber.ToUpper();
                                fillingvalve.status = "تایید شده";
                                fillingvalve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                fillingvalve.productDate = ExistBank.productDate;
                                fillingvalve.CreateDate = DateTime.Now;
                                fillingvalve.Creator = User.Identity.Name;
                                fillingvalve.MaterailName = "شیر پرکن";

                                db.tbl_FillingValves.Add(fillingvalve);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (isExist == 0 && !string.IsNullOrEmpty(FillingValveConstractorID))// !string.IsNullOrEmpty(ProductDate[i]) &&
                            {
                                if (!string.IsNullOrEmpty(FillingValveConstractorID))
                                {
                                    fillingvalve.model = Helper.Helpers.GetFillingValveConstractor(Convert.ToInt32(FillingValveConstractorID)).Code;
                                    fillingvalve.constractor = Helper.Helpers.GetFillingValveConstractor(Convert.ToInt32(FillingValveConstractorID)).FillingValve;
                                }
                                else
                                {
                                    fillingvalve.model = "";
                                    fillingvalve.constractor = "";
                                }
                                fillingvalve.serialNumber = serialNumber[i].ToUpper();
                                fillingvalve.status = "نیاز به بررسی";
                                fillingvalve.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                fillingvalve.productDate = ProductDate[i];
                                fillingvalve.CreateDate = DateTime.Now;
                                fillingvalve.Creator = User.Identity.Name;
                                fillingvalve.MaterailName = "شیر پرکن";

                                db.tbl_FillingValves.Add(fillingvalve);
                                db.SaveChanges();
                                //countRow += 1;
                            }
                        }
                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("FillingValves");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }

            return RedirectToAction("CheckedFillingValves", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
            //return View();
        }

        // GET: Filling Valves List
        public ActionResult FillingValves(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            // Post = false;
            var list = db.tbl_FillingValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            //
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string serialNumber = "";
            string status = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_FillingValveList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue ? RefreshDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                serialNumber = serialNumber,
                                status = status,
                                workshop = workshop,
                                statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
            //
        }
        //
        [HttpPost]
        public JsonResult UploadCutofValves(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش شیر قطع کن"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات مخازن تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var cutofvalves = from a in excelFile.Worksheet<CutofValve>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in cutofvalves)
                    {
                        try
                        {
                            var serialNumber = a.serialNumber.Replace(" ", "");
                            dataCount = db.tbl_BankCutofValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if (dataCount == 0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.model != null && a.productDate != null )
                                {
                                    BankCutofValve TU = new BankCutofValve();
                                    TU.serialNumber = a.serialNumber.Replace(" ", "").ToUpper();
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "شیر قطع کن";

                                    db.tbl_BankCutofValves.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.model == "" || a.model == null) data.Add("<li> model is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber.ToUpper(),
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        [HttpPost]
        public JsonResult UploadFillingValves(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش شیر پر کن"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات مخازن تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var fillingvalves = from a in excelFile.Worksheet<FillingValve>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in fillingvalves)
                    {
                        try
                        {
                            var serialNumber = a.serialNumber.Replace(" ", "");
                            dataCount = db.tbl_BankFillingValves.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if (dataCount == 0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.model != null && a.productDate != null)
                                {
                                    BankFillingValve TU = new BankFillingValve();
                                    TU.serialNumber = a.serialNumber.Replace(" ", "").ToUpper();
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "شیر پرکن";

                                    db.tbl_BankFillingValves.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.model == "" || a.model == null) data.Add("<li> model is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber.ToUpper(),
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        public ActionResult BankCutofValves(DateTime fromDate, DateTime toDate, bool? Post)
        {
            // Post = false;
            var list = db.tbl_BankCutofValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();           
            //
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string serialNumber = "";
            //string status = "";
            //string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_CutoffValveList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;                        
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            serialNumber = reader["serialNumber"].ToString();                            
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;                           
                            Creator = reader["Creator"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.HasValue ? CreateDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                serialNumber = serialNumber
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
            //
        }
        //
        public ActionResult BankfillingValves(DateTime fromDate, DateTime toDate, bool? Post)
        {
            // Post = false;
            var list = db.tbl_BankFillingValves.OrderByDescending(t => t.ID);
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            //
            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string serialNumber = "";
            //string status = "";
            //string workshop = "";
            //string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_FillingValveList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;                           
                            Creator = reader["Creator"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.HasValue ? CreateDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                serialNumber = serialNumber
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
            //
        }
        
        //Get Cut of Valve Constractor Code
        public JsonResult GetConstractorCode(int id)
        {
            string countrystring = "select * from [dbo].[tbl_CutofValveConstractors] where [ID]='" + id + "'";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Text = "--انتخاب --", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //Get Filling Valve Constractor Code
        public JsonResult GetFillingConstractorCode(int id)
        {
            string countrystring = "select * from [dbo].[tbl_FillingValveConstractors] where [ID]='" + id + "'";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Text = "--انتخاب --", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //Get Cylinder bulk with select Constractor 
        public JsonResult GetTypeofTank(int id)
        {
            string countrystring = "select * from [dbo].[tbl_CylinderDetails] where [ConstractorId]='" + id + "' order by CONVERT(int, [Bulk])";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Text = "--انتخاب --", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[2]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        //
        public ActionResult UploadCutoffValveEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutofValve cutofvalve = db.tbl_CutofValves.Find(id);
            if (cutofvalve == null)
            {
                return HttpNotFound();
            }
            int myInt;
            bool isNumerical = int.TryParse(cutofvalve.constractor, out myInt);
            if (isNumerical == false)
                cutofvalve.constractor = db.tbl_CutofValveConstractors.Where(f => f.CutofValve.Contains(cutofvalve.constractor)).SingleOrDefault().ID.ToString();
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", cutofvalve.workshop);
            ViewBag.constractor = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve", cutofvalve.constractor);
            ViewBag.model = new SelectList(db.tbl_FillingValveConstractors, "ID", "Code", cutofvalve.constractor);
            return View(cutofvalve);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCutoffValveEdit(CutofValve cutofvalve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cutofvalve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CutofValvess");
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", cutofvalve.workshop);
            return View(cutofvalve);
        }

        //
        [RBAC]
        // GET: UploadFiles/UploadCutoffValveDelete/5
        public ActionResult UploadCutoffValveDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutofValve cutofvalve = db.tbl_CutofValves.Find(id);
            if (cutofvalve == null)
            {
                return HttpNotFound();
            }
            return View(cutofvalve);
        }

        // POST: UploadFiles/UploadKitDelete/5
        [HttpPost, ActionName("UploadCutoffValveDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCutoffValveDeleteConfirmed(int id)
        {
            CutofValve cutofvalve = db.tbl_CutofValves.Find(id);
            db.tbl_CutofValves.Remove(cutofvalve);
            db.SaveChanges();
            return RedirectToAction("CutofValves");
        }

        //// GET: UploadFiles/UploadFillingValveEdit/5
        public ActionResult UploadFillingValveEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillingValve fillingvalve = db.tbl_FillingValves.Find(id);
            if (fillingvalve == null)
            {
                return HttpNotFound();
            }
            int myInt;
            bool isNumerical = int.TryParse(fillingvalve.constractor, out myInt);
            if (isNumerical == false)
                fillingvalve.constractor = db.tbl_FillingValveConstractors.Where(f => f.FillingValve.Contains(fillingvalve.constractor)).SingleOrDefault().ID.ToString();
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", fillingvalve.workshop);
            ViewBag.constractor = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve", fillingvalve.constractor);
            ViewBag.model = new SelectList(db.tbl_FillingValveConstractors, "ID", "Code", fillingvalve.constractor);
            return View(fillingvalve);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFillingValveEdit(FillingValve fillingvalve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fillingvalve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FillingValves");
            }
            ViewBag.workshop = new SelectList(db.tbl_Workshops, "ID", "Title", fillingvalve.workshop);
            return View(fillingvalve);
        }

        //
        [RBAC]
        // GET: UploadFiles/UploadFillingValveDelete/5
        public ActionResult UploadFillingValveDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillingValve fillingvalve = db.tbl_FillingValves.Find(id);
            if (fillingvalve == null)
            {
                return HttpNotFound();
            }
            return View(fillingvalve);
        }

        // POST: UploadFiles/UploadFillingValveDelete/5
        [HttpPost, ActionName("UploadFillingValveDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFillingValveDeleteConfirmed(int id)
        {
            FillingValve fillingvalve = db.tbl_FillingValves.Find(id);
            db.tbl_FillingValves.Remove(fillingvalve);
            db.SaveChanges();
            return RedirectToAction("FillingValves");
        }

        // GET: UploadFiles/ Upload Bank FuelRelay Information
        public ActionResult BankFuelRelay(DateTime fromDate, DateTime toDate, bool? Post)
        {
            //Post = false;
            var list = db.tbl_BankFuelRelays.OrderByDescending(t => t.ID).ToList();
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            //string status = "";
            string type = "";
            //string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_FuelRelayList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            //status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            //workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            Creator = reader["Creator"].ToString();
                            //statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                //status = status,
                                type = type,
                                //workshop = workshop,
                                //statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        
        // GET: UploadFiles/ Upload Bank FuelRelay Information
        public ActionResult UploadFuelRelay(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            //Post = false;
            var list = db.tbl_FuelRelays.OrderByDescending(t => t.ID).ToList();
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_FuelRelayList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue ? RefreshDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop,
                                statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }

        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  
        [HttpPost]
        public JsonResult UploadFuelRelay(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش رله سوخت"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات رله سوخت تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var FuelRelay = from a in excelFile.Worksheet<FuelRelay>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in FuelRelay)
                    {
                        try
                        {
                            var serialNumber = a.serialNumber.Replace(" ", "");
                            dataCount = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if (dataCount == 0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.generation != null && a.productDate != null && a.type != null)
                                {
                                    BankFuelRelay TU = new BankFuelRelay();
                                    TU.serialNumber = a.serialNumber.Replace(" ", "");
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.type = a.type;
                                    TU.generation = a.generation;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "رله سوخت";

                                    db.tbl_BankFuelRelays.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.type == "" || a.type == null) data.Add("<li> type is required</li>");
                                    if (a.generation == "" || a.generation == null) data.Add("<li> rezve is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber,
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        /// <summary>
        /// for check kit
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedFuelRelay(string message)
        {
            ViewBag.message = message;
            ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay");
            var ImportFuelRelayList = db.tbl_FuelRelays;
            return View(ImportFuelRelayList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddRecieveFuelRelay(HttpPostedFileBase FileUpload, string[] serialNumber, string FuelRelayConstractorID, string TypeofFuelRelayID, string Genaration, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear)
        {
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            FuelRelay fuelrelay = new FuelRelay();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber != null)
            {
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(FuelRelayConstractorID) && !string.IsNullOrEmpty(TypeofFuelRelayID) && !string.IsNullOrEmpty(Genaration))
                    {
                        var serialId = serialNumber[i].Replace(" ", "");
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExistFuelRelay = db.tbl_FuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                        var isExistBankFuelrelay = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                        if (isExistBankFuelrelay > 0)
                        {
                            if (isExistFuelRelay == 0)
                            {
                                var ExistBankFuelRelay = db.tbl_BankFuelRelays.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                fuelrelay.model = ExistBankFuelRelay.model;
                                fuelrelay.type = ExistBankFuelRelay.type;
                                fuelrelay.constractor = ExistBankFuelRelay.constractor;
                                fuelrelay.generation = ExistBankFuelRelay.generation;
                                fuelrelay.serialNumber = ExistBankFuelRelay.serialNumber;
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                fuelrelay.status = "تایید شده";
                                fuelrelay.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                fuelrelay.productDate = ExistBankFuelRelay.productDate;
                                fuelrelay.expireDate = ExistBankFuelRelay.expireDate;
                                fuelrelay.CreateDate = DateTime.Now;
                                fuelrelay.Creator = User.Identity.Name;
                                fuelrelay.MaterailName = "رله سوخت";

                                db.tbl_FuelRelays.Add(fuelrelay);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (isExistFuelRelay == 0)
                            {
                               
                                fuelrelay.type = TypeofFuelRelayID;
                                fuelrelay.generation = Genaration;
                                fuelrelay.constractor = FuelRelayConstractorID;
                                fuelrelay.generation = Genaration;
                                fuelrelay.serialNumber = serialNumber[i];
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                fuelrelay.status = "نیاز به بررسی";
                                fuelrelay.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                fuelrelay.productDate = ProductYear[i];
                                //fuelrelay.expireDate = ProductYear[i];
                                fuelrelay.CreateDate = DateTime.Now;
                                fuelrelay.Creator = User.Identity.Name;
                                fuelrelay.MaterailName = "رله سوخت";

                                db.tbl_FuelRelays.Add(fuelrelay);
                                db.SaveChanges();
                                //countRow += 1;
                            }

                        }


                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("UploadFuelRelay");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }

            return RedirectToAction("CheckedFuelRelay", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
            //return View();
        }
        //

        // GET: UploadFiles/ Upload Bank Gas ECU Information
        public ActionResult BankGasECU(DateTime fromDate, DateTime toDate, bool? Post)
        {
            //Post = false;
            var list = db.tbl_BankGasECU.OrderByDescending(t => t.ID).ToList();
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            //string status = "";
            string type = "";
            //string workshop = "";
            //string statusColor = "";

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GasECUList_Bank]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            //status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            //workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            Creator = reader["Creator"].ToString();
                            //statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                //status = status,
                                type = type,
                                //workshop = workshop,
                                //statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }

        // GET: UploadFiles/ Upload Bank GasECU Information
        public ActionResult UploadGasECU(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, string dr_status)
        {
            //Post = false;
            var list = db.tbl_GasECU.OrderByDescending(t => t.ID).ToList();
            DateTime? existDate = null;

            if (list.Count() > 0)
                existDate = list.FirstOrDefault().CreateDate.GetValueOrDefault();
            else
                existDate = DateTime.Now.AddDays(-5);
            if (Post != true)
                fromDate = existDate.GetValueOrDefault();
            //toDate = DateTime.Now;
            ViewBag.icon = "icon-chevron-down";
            ViewBag.style = "";
            if (Post == true)
            {
                ViewBag.icon = "icon-chevron-down";
                ViewBag.style = "";
            }

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            //
            foreach (var item in workshops)
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop _workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (_workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };
            }
            //
            if (string.IsNullOrEmpty(dr_status))
                dr_status = "1,2";
            if (dr_status.Equals("1,2"))
            {
                ViewBag.status = "همه";
                ViewBag.statusValue = "";
            }

            if (dr_status.Equals("1"))
            {
                ViewBag.status = "تایید شده";
                ViewBag.statusValue = "1";
            }

            if (dr_status.Equals("2"))
            {
                ViewBag.status = "نیاز به بررسی";
                ViewBag.statusValue = "2";
            }

            string constractor = "";
            DateTime? CreateDate = null;
            string Creator = "";
            int ID = 0;
            string model = "";
            string productDate = "";
            string generation = "";
            string serialNumber = "";
            string status = "";
            string type = "";
            string workshop = "";
            string statusColor = "";
            DateTime? RefreshDate = null;

            List<CylinderList> TableOuts = new List<CylinderList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GasECUList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        command.Parameters.Add("@status", SqlDbType.VarChar).Value = dr_status.TrimEnd(',');
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = Convert.ToInt32(reader["ID"].ToString());
                            model = reader["model"].ToString();
                            productDate = reader["productDate"].ToString();
                            generation = reader["generation"].ToString();
                            serialNumber = reader["serialNumber"].ToString();
                            status = reader["status"].ToString();
                            type = reader["type"].ToString();
                            workshop = reader["workshop"].ToString();
                            constractor = reader["constractor"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("CreateDate")))
                                CreateDate = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = Convert.ToDateTime(reader["RefreshDate"].ToString());
                            if (reader.IsDBNull(reader.GetOrdinal("RefreshDate")))
                                RefreshDate = null;
                            Creator = reader["Creator"].ToString();
                            statusColor = reader["statusColor"].ToString();

                            TableOuts.Add(new CylinderList
                            {
                                constractor = constractor,
                                CreateDate = CreateDate.GetValueOrDefault().ToShortDateString(),
                                RefreshDate = RefreshDate.HasValue ? RefreshDate.GetValueOrDefault().ToShortDateString() : "",
                                Creator = Creator,
                                ID = ID.ToString(),
                                model = model,
                                productDate = productDate,
                                rezve = generation,
                                serialNumber = serialNumber,
                                status = status,
                                type = type,
                                workshop = workshop,
                                statusColor = statusColor
                            });
                        }

                        conn.Close();
                    }
                }//end using
                ViewBag.TableOuts = TableOuts;
            }
            catch
            {
                ViewBag.TableOuts = null;
            }
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  
        [HttpPost]
        public JsonResult UploadGasECU(HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            int countRow = 0;
            string filename = FileUpload.FileName;
            string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
            FileUpload.SaveAs(targetpath + filename);
            string pathToExcelFile = targetpath + filename;
            var connectionString = "";

            #region"بخش Gas ECU"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود اطلاعات رله سوخت تحویلی از تامین کنندگان از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var GasECU = from a in excelFile.Worksheet<GasECU>(sheetName) select a;
                    int dataCount = 0;
                    int rowDoubledCount = 0;
                    List<rowDoubledTank> rowdoubledtank = new List<rowDoubledTank>();
                    string Message = "";

                    foreach (var a in GasECU)
                    {
                        try
                        {
                            var serialNumber = a.serialNumber.Replace(" ", "");
                            dataCount = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialNumber)).Count();
                            rowDoubledCount += dataCount;

                            if (dataCount == 0)
                            {
                                countRow += 1;
                                if (a.serialNumber != null && a.constractor != null && a.generation != null && a.productDate != null && a.type != null)
                                {
                                    BankGasECU TU = new BankGasECU();
                                    TU.serialNumber = a.serialNumber.Replace(" ", "");
                                    TU.constractor = a.constractor;
                                    TU.model = a.model;
                                    TU.type = a.type;
                                    TU.generation = a.generation;
                                    TU.productDate = a.productDate;
                                    TU.workshop = a.workshop;
                                    TU.status = a.status;
                                    TU.CreateDate = DateTime.Now;
                                    TU.Creator = User.Identity.Name;
                                    TU.MaterailName = "Gas ECU";

                                    db.tbl_BankGasECU.Add(TU);
                                    db.SaveChanges();
                                    //return Json("داده ها با موفقیت در بانک اطلاعاتی ثبت شدند.", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    data.Add("<ul>");
                                    if (a.serialNumber == "" || a.serialNumber == null) data.Add("<li> serialNumber is required</li>");
                                    if (a.constractor == "" || a.constractor == null) data.Add("<li> constractor is required</li>");
                                    if (a.type == "" || a.type == null) data.Add("<li> type is required</li>");
                                    if (a.generation == "" || a.generation == null) data.Add("<li> rezve is required</li>");
                                    if (a.productDate == null) data.Add("<li> productDate is required</li>");

                                    data.Add("</ul>");
                                    data.ToArray();
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }

                            }

                            else
                            {
                                rowdoubledtank.Add(new rowDoubledTank
                                {
                                    SerialNumber = a.serialNumber,
                                    Message = "تکراری و در بانک ثبت نشد!",
                                    Success = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد "
                                });
                            }
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    if (countRow > 0)
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        Message = "تعداد " + countRow + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                        //RedirectToAction("UploadExcel","UploadFiles");
                        return Json(Message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        return Json(rowdoubledtank, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        /// <summary>
        /// for check kit
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedGasECU(string message)
        {
            ViewBag.message = message;
            ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU");
            var ImportFuelRelayList = db.tbl_GasECU;
            return View(ImportFuelRelayList.ToList());
        }
        //
        [HttpPost]
        public ActionResult AddRecieveGasECU(HttpPostedFileBase FileUpload, string[] serialNumber, string GasECUConstractorID, string TypeofGasECUID, string Genaration, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear)
        {
            List<DoubledTankItems> doubletankitems = new List<DoubledTankItems>();
            GasECU gasecu = new GasECU();
            bool status = true;
            bool insertValue = false;

            //    Message = "حتما یک ردیف باید پر شود!";
            if (serialNumber != null)
            {
                for (int i = 0; i < serialNumber.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(serialNumber[i]) && !string.IsNullOrEmpty(GasECUConstractorID) && !string.IsNullOrEmpty(TypeofGasECUID) && !string.IsNullOrEmpty(Genaration))
                    {
                        var serialId = serialNumber[i].Replace(" ", "");
                        //برای مواردی که در بانک اطلاعاتی سریال وجود دارد
                        var isExistGasECU = db.tbl_GasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();
                        var isExistBankGasECU = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).Count();

                        if (isExistBankGasECU > 0)
                        {
                            if (isExistGasECU == 0)
                            {
                                var ExistBankGasECU = db.tbl_BankGasECU.Where(t => t.serialNumber.Replace(" ", "").Equals(serialId)).FirstOrDefault();
                                gasecu.model = ExistBankGasECU.model;
                                gasecu.type = ExistBankGasECU.type;
                                gasecu.constractor = ExistBankGasECU.constractor;
                                gasecu.generation = ExistBankGasECU.generation;
                                gasecu.serialNumber = ExistBankGasECU.serialNumber;
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                gasecu.status = "تایید شده";
                                gasecu.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                gasecu.productDate = ExistBankGasECU.productDate;
                                gasecu.expireDate = ExistBankGasECU.expireDate;
                                gasecu.CreateDate = DateTime.Now;
                                gasecu.Creator = User.Identity.Name;
                                gasecu.MaterailName = "Gas ECU";

                                db.tbl_GasECU.Add(gasecu);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (isExistGasECU == 0)
                            {

                                gasecu.type = TypeofGasECUID;
                                gasecu.generation = Genaration;
                                gasecu.constractor = GasECUConstractorID;
                                gasecu.generation = Genaration;
                                gasecu.serialNumber = serialNumber[i];
                                //valve.status = "تخصیص یافته- ثبت توسط کارگاه";
                                gasecu.status = "نیاز به بررسی";
                                gasecu.workshop = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                                gasecu.productDate = ProductYear[i];
                                //fuelrelay.expireDate = ProductYear[i];
                                gasecu.CreateDate = DateTime.Now;
                                gasecu.Creator = User.Identity.Name;
                                gasecu.MaterailName = "Gas ECU";

                                db.tbl_GasECU.Add(gasecu);
                                db.SaveChanges();
                                //countRow += 1;
                            }

                        }


                    }
                    else if (insertValue == false)
                    {
                        status = false;
                        //Message = "سریال وارد شده صحیح نمی باشد!";
                    }
                }
                if (status == true)
                {
                    return RedirectToAction("UploadGasECU");
                }
                //countRow += 1;
                //Message = "تعداد " + countRow + "با موفقیت ثبت شد";
            }

            return RedirectToAction("CheckedGasECU", new { message = "خطا در اطلاعات ورودی، در صورت تمایل مجدد با تکمیل اطلاعات ضروری سعی نمایید...!" });
            //return View();
        }
        //
        //Get Genarations
        public JsonResult GetGenarations(string TypeofKit)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (TypeofKit.Equals("OPEN_LOOP"))
            {
                list.Add(new SelectListItem { Text = "نسل 1", Value = "نسل 1" });
            }
            if (TypeofKit.Equals("CLOSE_LOOP"))
            {
                list.Add(new SelectListItem { Text = "نسل 2", Value = "نسل 2" });
            }
            if (TypeofKit.Equals("SEQUENTIAL"))
            {
                list.Add(new SelectListItem { Text = "نسل 4", Value = "نسل 4" });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
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