using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Hosting;
using Rotativa;
using System.Globalization;

namespace cngfapco.Controllers
{
    //[RBAC]
    [Authorize]
    [RBACAttribute.NoCache]
    public class VehicleRegistrationsController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        private string IP = cngfapco.Helper.Helpers.GetVisitorIPAddress();
        DAL objdal = new DAL();

        public const int RecordsPerPage = 20;
        public List<Registration> ProjectData;

        public VehicleRegistrationsController()
        {
            ViewBag.RecordsPerPage = RecordsPerPage;
        }

        [HttpPost]
        public JsonResult CheckedOut(string[] Items)
        {
            string Workshop = "";

            foreach (var item in Items)
            {
                VehicleRegistration selectItem = db.tbl_VehicleRegistrations.Find(Convert.ToInt32(item));
                Workshop = selectItem.Workshop.Title;
                selectItem.Checked = true;
                selectItem.CheckedDate = DateTime.Now;
                selectItem.Checker = User.Identity.Name;
                db.Entry(selectItem).State = EntityState.Modified;
                db.SaveChanges();
            }

            //return RedirectToAction("VehicleRegistrationDefects2",new { Workshop= Workshop });
            return Json(new { result = "با موفقیت ثبت شد!" }, JsonRequestBehavior.AllowGet);
        }

        // GET: VehicleRegistrations /WorkshopPage 
        public ActionResult WorkshopPage()
        {
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            List<Workshop> list = new List<Workshop>();

            foreach (var item in workshops)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    list.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title,
                        OwnerName = item.OwnerName,
                        OwnerFamily = item.OwnerFamily,
                        City=item.City,
                        closedServices=item.closedServices,
                        closedDate=item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }

        // GET: VehicleRegistrations /WorkshopPage2
        public ActionResult WorkshopPage2()
        {
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            List<Workshop> list = new List<Workshop>();

            foreach (var item in workshops)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    list.Add(new Workshop
                    {
                        ID=item.ID,
                        Title=item.Title,
                        OwnerName=item.OwnerName,
                        OwnerFamily=item.OwnerFamily,
                        City=item.City,
                        closedServices=item.closedServices,
                        closedDate=item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }

        // GET: Pre Select Type VehicleRegistrations
        public ActionResult PreSelectType()
        {
            ViewBag.Type = db.tbl_RegistrationTypes.ToList();
            return PartialView();
        }
        // GET: VehicleRegistrations
        public ActionResult RegistrationIndex()
        {
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var rolId = db.tbl_Roles.Where(r => r.IsSysAdmin == true).ToList();
            //string isAdmin= "false";
            foreach (var item in rolId)
            {
                var itemuserId = item.Users.SingleOrDefault().UserID;

                if (itemuserId == userId)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    //if (role.Users.Contains(user))
                    //{
                    //    isAdmin = "true";
                    //}
                    //else
                    //{
                    //    isAdmin = "false";
                    //}
                }
                
            }

            List<VehicleRegistration> tbl_VehicleRegistrations = new List<VehicleRegistration>();
            var VehicleRegistrations = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true).Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);

            foreach (var item in VehicleRegistrations)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    tbl_VehicleRegistrations.Add(new VehicleRegistration
                    {
                        Address=item.Address,
                        AlphaPlate=item.AlphaPlate,
                        ChassisNumber=item.ChassisNumber,
                        ConstructionYear=item.ConstructionYear,
                        CreateDate=item.CreateDate,
                        Creator=item.Creator,
                        Description=item.Description,
                        EditDate=item.EditDate,
                        Editor=item.Editor,
                        EngineNumber=item.EngineNumber,
                        FuelCard=item.FuelCard,
                        ID=item.ID,
                        InstallationStatus=item.InstallationStatus,
                        IranNumberPlate=item.IranNumberPlate,
                        LeftNumberPlate=item.LeftNumberPlate,
                        License=item.License,
                        LicenseImage=item.LicenseImage,
                        MobileNumber=item.MobileNumber,
                        NationalCard=item.NationalCard,
                        NationalCode=item.NationalCode,
                        OwnerFamily=item.OwnerFamily,
                        OwnerName=item.OwnerName,
                        PhoneNumber=item.PhoneNumber,
                        RefuelingLable=item.RefuelingLable,
                        RightNumberPlate=item.RightNumberPlate,
                        SerialKey=item.SerialKey,
                        SerialKit=item.SerialKit,
                        SerialRefuelingValve=item.SerialRefuelingValve,
                        SerialSparkPreview=item.SerialSparkPreview,
                        System=item.System,
                        TrackingCode=item.TrackingCode,
                        TypeofUse=item.TypeofUse,
                        TypeofUseID=item.TypeofUseID,
                        VehicleCard=item.VehicleCard,
                        VehicleType=item.VehicleType,
                        VehicleTypeID=item.VehicleTypeID,
                        VIN=item.VIN,
                        Workshop=item.Workshop,
                        WorkshopID=item.WorkshopID,
                        HealthCertificate=item.HealthCertificate
                    });
                }

            };
            //if (isAdmin.Equals("true"))
            //{
            //    var tbl_VehicleRegistrations = db.tbl_VehicleRegistrations.Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);
            //    return View(tbl_VehicleRegistrations.ToList());
            //}
            //else
            //{
            //    var tbl_VehicleRegistrations = db.tbl_VehicleRegistrations.Where(v=>v.Creator==userId).Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);
            //    return View(tbl_VehicleRegistrations.ToList());
            //}

            return View(tbl_VehicleRegistrations.ToList());
        }

        // GET: VehicleRegistrations / ExportData
        public ActionResult ExportData()
        {
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var rolId = db.tbl_Roles.Where(r => r.IsSysAdmin == true).ToList();
            //string isAdmin = "false";
            foreach (var item in rolId)
            {
                var itemuserId = item.Users.SingleOrDefault().UserID;

                if (itemuserId == userId)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    //if (role.Users.Contains(user))
                    //{
                    //    isAdmin = "true";
                    //}
                    //else
                    //{
                    //    isAdmin = "false";
                    //}
                }

            }

            List<VehicleRegistration> tbl_VehicleRegistrations = new List<VehicleRegistration>();
            var VehicleRegistrations = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true).Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);

            foreach (var item in VehicleRegistrations)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    tbl_VehicleRegistrations.Add(new VehicleRegistration
                    {
                        Address = item.Address,
                        AlphaPlate = item.AlphaPlate,
                        ChassisNumber = item.ChassisNumber,
                        ConstructionYear = item.ConstructionYear,
                        CreateDate = item.CreateDate,
                        Creator = item.Creator,
                        Description = item.Description,
                        EditDate = item.EditDate,
                        Editor = item.Editor,
                        EngineNumber = item.EngineNumber,
                        FuelCard = item.FuelCard,
                        ID = item.ID,
                        InstallationStatus = item.InstallationStatus,
                        IranNumberPlate = item.IranNumberPlate,
                        LeftNumberPlate = item.LeftNumberPlate,
                        License = item.License,
                        LicenseImage = item.LicenseImage,
                        MobileNumber = item.MobileNumber,
                        NationalCard = item.NationalCard,
                        NationalCode = item.NationalCode,
                        OwnerFamily = item.OwnerFamily,
                        OwnerName = item.OwnerName,
                        PhoneNumber = item.PhoneNumber,
                        RefuelingLable = item.RefuelingLable,
                        RightNumberPlate = item.RightNumberPlate,
                        SerialKey = item.SerialKey,
                        SerialKit = item.SerialKit,
                        SerialRefuelingValve = item.SerialRefuelingValve,
                        SerialSparkPreview = item.SerialSparkPreview,
                        System = item.System,
                        TrackingCode = item.TrackingCode,
                        TypeofUse = item.TypeofUse,
                        TypeofUseID = item.TypeofUseID,
                        VehicleCard = item.VehicleCard,
                        VehicleType = item.VehicleType,
                        VehicleTypeID = item.VehicleTypeID,
                        VIN = item.VIN,
                        Workshop = item.Workshop,
                        WorkshopID = item.WorkshopID
                    });
                }

            };
            //if (isAdmin.Equals("true"))
            //{
            //    var tbl_VehicleRegistrations = db.tbl_VehicleRegistrations.Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);
            //    return View(tbl_VehicleRegistrations.ToList());
            //}
            //else
            //{
            //    var tbl_VehicleRegistrations = db.tbl_VehicleRegistrations.Where(v=>v.Creator==userId).Include(v => v.VehicleType).Include(v => v.TypeofUse).Include(v => v.Workshop);
            //    return View(tbl_VehicleRegistrations.ToList());
            //}

            return View(tbl_VehicleRegistrations.ToList());
        }

        // GET: VehicleRegistrations/Workshop User View
        public ActionResult WorkshopUserView()
        {
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var rolId = db.tbl_Roles.Where(r => r.RoleName== "مرکز خدمات (کارگاه)").ToList();
            foreach (var item in rolId)
            {
                Role role = db.tbl_Roles.Find(item.Role_Id);
                User user = db.tbl_Users.Find(userId);
                if (role.Users.Contains(user))
                {
                    ViewBag.WorkshopUser = "true";
                }
                else
                {
                    ViewBag.WorkshopUser = "false";
                }
            }

            var tbl_VehicleRegistrations = db.tbl_VehicleRegistrations.Where(v =>v.RegisterStatus==true && v.Creator == userId).Include(v => v.VehicleType).Include(v => v.TypeofUse);
            return View(tbl_VehicleRegistrations.ToList());

        }

        // GET: VehicleRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            //
            List<VehicleRegistrationList> registrationList = new List<VehicleRegistrationList>();
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            List<Workshop> list = new List<Workshop>();

            Workshop workshop = db.tbl_Workshops.Find(vehicleRegistration.Workshop.ID);
            User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());
           
            //
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var rolId = db.tbl_Roles.Where(r => r.IsSysAdmin == true).ToList();
            foreach (var item in rolId)
            {
                var itemuserId = item.Users.FirstOrDefault().UserID;

                if (itemuserId == userId)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    if (role.Users.Contains(user))
                    {
                        ViewBag.isAdmin = true;
                    }
                    else
                    {
                        ViewBag.isAdmin = false;
                    }
                }

            }
            //
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);

            //VehicleTank vehicletank = db.tbl_VehicleTanks.Where(t=>t.VehicleRegistrationID==id);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", id);
            //
            string ID = "";
            string VehicleRegistrationID = "";
            string Number = "";
            string Amount = "";
            string Type = "";
            string Date = "";
            string InvoiceFile = "";
            string Description = "";
            string Serial = "";
            string Volume = "";
            string TankConstractorID = "";
            string ProductYear = "";
            string ProductMonth = "";
            string ExpirationYear = "";
            string ExpirationMonth = "";
            string SerialTankValve = "";
            string TypeTankValve = "";
            string ConstractorTankValve = "";
            string Constractor = "";
            string RegulatorConstractorID = "";
            string RegulatorSerial = "";
            int InvoiceCount = 0;
            int TankCount = 0;

            List<VehicleRegistrationsRelatedList> InvoiceTableOuts = new List<VehicleRegistrationsRelatedList>();
            List<VehicleRegistrationsRelatedList> TankTableOuts = new List<VehicleRegistrationsRelatedList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[SP_VehicleRegistrationsRelatedData]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@VehicleRegistrationID", SqlDbType.VarChar).Value = id;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            VehicleRegistrationID = reader["VehicleRegistrationID"].ToString();
                            Number = reader["Number"].ToString();
                            Amount = reader["Amount"].ToString();
                            Type = reader["Type"].ToString();
                            Date = reader["Date"].ToString();
                            InvoiceFile = reader["InvoiceFile"].ToString();
                            Description = reader["Description"].ToString();
                            InvoiceCount += 1;
                            InvoiceTableOuts.Add(new VehicleRegistrationsRelatedList
                            {
                                ID = ID,
                                VehicleRegistrationID = VehicleRegistrationID,
                                Number = Number,
                                Amount = Amount,
                                Type = Type,
                                Date = Date,
                                InvoiceFile = InvoiceFile,
                                Description = Description,
                                InvoiceCount = InvoiceCount.ToString()
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            VehicleRegistrationID = reader["VehicleRegistrationID"].ToString();
                            Serial = reader["Serial"].ToString();
                            Volume = reader["Volume"].ToString();
                            TankConstractorID = reader["TankConstractorID"].ToString();
                            ProductYear = reader["ProductYear"].ToString();
                            ProductMonth = reader["ProductMonth"].ToString();
                            ExpirationYear = reader["ExpirationYear"].ToString();
                            ExpirationMonth = reader["ExpirationMonth"].ToString();
                            SerialTankValve = reader["SerialTankValve"].ToString();
                            TypeTankValve = reader["TypeTankValve"].ToString();
                            ConstractorTankValve = reader["ConstractorTankValve"].ToString();
                            Constractor = reader["Constractor"].ToString();
                            RegulatorSerial = reader["RegulatorSerial"].ToString();
                            RegulatorConstractorID = reader["RegulatorConstractorID"].ToString();
                            TankCount += 1;

                            TankTableOuts.Add(new VehicleRegistrationsRelatedList
                            {
                                ID = ID,
                                VehicleRegistrationID = VehicleRegistrationID,
                                Serial = Serial,
                                Volume = Volume,
                                TankConstractorID = TankConstractorID,
                                ProductYear = ProductYear,
                                ProductMonth = ProductMonth,
                                ExpirationYear = ExpirationYear,
                                ExpirationMonth = ExpirationMonth,
                                SerialTankValve = SerialTankValve,
                                TypeTankValve = TypeTankValve,
                                ConstractorTankValve = ConstractorTankValve,
                                Constractor = Constractor,
                                TankCount = TankCount.ToString(),
                                RegulatorConstractorID=RegulatorConstractorID,
                                RegulatorSerial=RegulatorSerial
                            });


                        }
                    }
                }//end using
                ViewBag.InvoiceTableOut = InvoiceTableOuts;
                ViewBag.TankTableOut = TankTableOuts;
            }
            catch
            {
                ViewBag.TableOut = null;
            }

            if (workshop.Users.Contains(_user))
            {
                return View(vehicleRegistration);
                //registrationList.Add(new VehicleRegistrationList
                //{

                //});
            }
            else
            {
                return RedirectToAction("Index", "Unauthorised");
            }           
           
        }
        //
        // GET: VehicleRegistrations/DetailsPrint/5
        public ActionResult DetailsPrint()
        {
            return View();
        }
        //
        [HttpPost]
        public JsonResult CheckeChassisNumber(string ChassisNumber)
        {
            string message = "";

            if (!string.IsNullOrEmpty(ChassisNumber))
            {
                var chassisnumber = dbStatic.tbl_VehicleRegistrations
                .Where(c =>c.RegisterStatus==true && c.ChassisNumber.Equals(ChassisNumber))
                .ToList();

                if (chassisnumber.Count() > 0)
                    message = "شماره شاسی وارد شده تکراری می باشد!";
                else
                    message = "true";

            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckeEngineNumber(string EngineNumber)
        {
            string message = "";

            if (!string.IsNullOrEmpty(EngineNumber))
            {
                var enginenumber = dbStatic.tbl_VehicleRegistrations
                .Where(c => c.RegisterStatus == true && c.EngineNumber.Equals(EngineNumber))
                .ToList();

                if (!string.IsNullOrEmpty(EngineNumber))
                {
                    if (enginenumber.Count() > 0)
                        message = "شماره موتور وارد شده تکراری می باشد!";
                    else
                        message = "true";
                }
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckeSerialNumber(string SerialNumber)
        {
            string message = "";

            if (!string.IsNullOrEmpty(SerialNumber))
            {
                var serialnumber = dbStatic.tbl_VehicleTanks
                .Where(c => c.Serial.Equals(SerialNumber))
                .ToList();

                if (serialnumber.Count() > 0)
                    message = "شماره سریال مخزن وارد شده تکراری می باشد!";
                else
                    message = "true";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckeSerialTankValve(string SerialTankValve)
        {
            string message = "";

            if (!string.IsNullOrEmpty(SerialTankValve))
            {
                var serialnumber = dbStatic.tbl_VehicleTanks
                .Where(c => c.SerialTankValve.Equals(SerialTankValve))
                .ToList();

                if (serialnumber.Count() > 0)
                    message = "شماره سریال شیر مخزن وارد شده تکراری می باشد!";
                else
                    message = "true";
            }            

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckeSerialRegulator(string SerialRegulator)
        {
            string message = "";
            string genaration = "";

            if (!string.IsNullOrEmpty(SerialRegulator))
            {
                var serialnumber = dbStatic.tbl_VehicleTanks
                .Where(c => c.RegulatorSerial.Equals(SerialRegulator))
                .ToList();

                if (serialnumber.Count() > 0)
                    message = "شماره سریال کیت (رگلاتور) وارد شده تکراری می باشد!";
                else
                    message = "true";
            }
            //
            if (!string.IsNullOrEmpty(SerialRegulator))
            {
                var Regulators = dbStatic.tbl_Kits
                .Where(c => c.serialNumber.Equals(SerialRegulator))
                .ToList();

                if (Regulators.Count() > 0)
                    genaration = Regulators.FirstOrDefault().generation;
                else
                    genaration = "false";
            }

            return Json(new { message=message, genaration=genaration}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckeGenaration(string SerialRegulator)
        {
            string message = "";

            if (!string.IsNullOrEmpty(SerialRegulator))
            {
                var Regulators = dbStatic.tbl_Kits
                .Where(c => c.serialNumber.Equals(SerialRegulator))
                .ToList();

                if (Regulators.Count() > 0)
                    message = Regulators.FirstOrDefault().generation;
                else
                    message = "False";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult cylinderBulkType(int VehicleTypeID)
        {
            string result = "";

            if (!string.IsNullOrEmpty(VehicleTypeID.ToString()))
            {
                var cylinderBulk = dbStatic.tbl_TypeofTanks.Where(c => c.VehicleTypeId == VehicleTypeID)
                .ToList();

                if (cylinderBulk.Count() > 0)
                    result = cylinderBulk.FirstOrDefault().Type;
                else
                    result = "false";
            }

            return Json(result, JsonRequestBehavior.AllowGet);            
        }
        public JsonResult cylinderBulkId(int VehicleTypeID)
        {
            string result = "";

            if (!string.IsNullOrEmpty(VehicleTypeID.ToString()))
            {
                var cylinderBulk = dbStatic.tbl_TypeofTanks.Where(c => c.VehicleTypeId == VehicleTypeID)
                .ToList();

                if (cylinderBulk.Count() > 0)
                    result = cylinderBulk.FirstOrDefault().ID.ToString();
                else
                    result = "false";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: VehicleRegistrations/Create
        public ActionResult Create()
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
            ViewBag.VehicleTypeID= new SelectList(list, "Value", "Text");
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type");
            ViewBag.ValveConstractorID= new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            return View();
        }

        // POST: VehicleRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VehicleTypeID,System,TypeofUseID,OwnerName,OwnerFamily,PhoneNumber,MobileNumber,NationalCode,Address,ConstructionYear,LeftNumberPlate,AlphaPlate,RightNumberPlate,IranNumberPlate,VIN,EngineNumber,ChassisNumber,SerialSparkPreview,SerialKit,SerialKey,RefuelingLable,SerialRefuelingValve,TrackingCode,License,InstallationStatus,Description,FuelCard")] VehicleRegistration vehicleRegistration, HttpPostedFileBase NationalCard, HttpPostedFileBase VehicleCard,HttpPostedFileBase LicenseImage, HttpPostedFileBase HealthCertificate,
            string[] Number,string[] Amount,string[] Type,string[] Date,string[] InvoiceDescription, HttpPostedFileBase[] InvoiceFile,string[] Serial, string[] Volume, string[] TankConstractorID, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth,string[] ExpirationYear, string[] SerialTankValve,string[] TypeTankValve,int[] ValveConstractorID, int[] RegulatorConstractorID, string[] RegulatorSerial)
        {

            if (NationalCard != null)
            {
                if (vehicleRegistration.NationalCard != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + vehicleRegistration.NationalCard));
                }

                vehicleRegistration.NationalCard = vehicleRegistration.NationalCode + "_"  + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + NationalCard.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + vehicleRegistration.NationalCard);
                NationalCard.SaveAs(ImagePath);
                ViewBag.NationalCardExtention= Path.GetExtension(NationalCard.FileName);
            }

            if (VehicleCard != null)
            {
                if (vehicleRegistration.VehicleCard != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + vehicleRegistration.VehicleCard));
                }

                vehicleRegistration.VehicleCard = vehicleRegistration.NationalCode + "_" + vehicleRegistration.EngineNumber.Replace("/","") + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + VehicleCard.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + vehicleRegistration.VehicleCard);
                VehicleCard.SaveAs(ImagePath);
                ViewBag.VehicleCardExtention = Path.GetExtension(VehicleCard.FileName);
            }

            if (LicenseImage != null)
            {
                if (vehicleRegistration.LicenseImage != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + vehicleRegistration.LicenseImage));
                }

                vehicleRegistration.LicenseImage = vehicleRegistration.NationalCode + "_" + vehicleRegistration.License + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + LicenseImage.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + vehicleRegistration.LicenseImage);
                LicenseImage.SaveAs(ImagePath);
            }

            if (HealthCertificate != null)
            {
                if (vehicleRegistration.HealthCertificate != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + vehicleRegistration.HealthCertificate));
                }

                vehicleRegistration.HealthCertificate = vehicleRegistration.NationalCode + "_" + vehicleRegistration.License + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + HealthCertificate.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + vehicleRegistration.HealthCertificate);
                HealthCertificate.SaveAs(ImagePath);
            }
            //
            VehicleInvoice vehicleinvoice = new VehicleInvoice();
            VehicleTank vehicletank = new VehicleTank();
            Insurance insurance = new Insurance();
            var insuranceNumber = db.tbl_Insurances.OrderByDescending(i => i.ID).Take(1).SingleOrDefault();
            var ChassisNumber = db.tbl_VehicleRegistrations.Where(v => v.ChassisNumber.Equals(vehicleRegistration.ChassisNumber.TrimStart().TrimEnd())).ToList();

            if (ModelState.IsValid)
            {
                if(ChassisNumber.Count()==0)
                {
                    vehicleRegistration.CreateDate = DateTime.Now;
                    vehicleRegistration.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                    vehicleRegistration.WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;

                    if (vehicleRegistration.InstallationStatus == null)
                        vehicleRegistration.InstallationStatus = false;

                    //vehicleRegistration.VIN = "IR" + vehicleRegistration.VIN;
                    vehicleRegistration.VIN = vehicleRegistration.VIN.ToUpper();
                    vehicleRegistration.ChassisNumber = vehicleRegistration.ChassisNumber.ToUpper().TrimStart().TrimEnd();
                    vehicleRegistration.EngineNumber = vehicleRegistration.EngineNumber.ToUpper().TrimStart().TrimEnd();
                    vehicleRegistration.CreatorIPAddress = IP;
                    vehicleRegistration.OwnerFamily = vehicleRegistration.OwnerFamily.TrimStart().TrimEnd();
                    vehicleRegistration.OwnerName = vehicleRegistration.OwnerName.TrimStart().TrimEnd();

                    //db.tbl_VehicleRegistrations.Add(vehicleRegistration); 14030320 edited
                    // db.SaveChanges(); 14030320 edited
                    //for insert and get Insurance Number
                    //-------14030320 edited------------------------------------
                    //insurance.CreateDate = DateTime.Now;
                    //if (insuranceNumber == null)
                    //    insurance.Number = "100";
                    //else
                    //    insurance.Number = (Convert.ToDouble(insuranceNumber.Number) + 1).ToString();

                    //insurance.Creator = User.Identity.Name;
                    //insurance.VehicleRegistrationID = vehicleRegistration.ID;
                    //db.tbl_Insurances.Add(insurance);
                    //db.SaveChanges();

                    //
                    try
                    {
                        for (int i = 0; i < Amount.Count(); i++)
                        {
                            if (Number[i] != null && Number[i] != "")
                            {
                                vehicleinvoice.Number = Number[i];
                                vehicleinvoice.Amount = Convert.ToDouble(Amount[i]);
                                vehicleinvoice.Type = Type[i];
                                vehicleinvoice.CreateDate = DateTime.Now;
                                vehicleinvoice.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                vehicleinvoice.Date = Convert.ToDateTime(Date[i]);
                                vehicleinvoice.Description = InvoiceDescription[i];
                                vehicleinvoice.VehicleRegistrationID = vehicleRegistration.ID;
                                if (InvoiceFile[i] != null)
                                {
                                    if (vehicleinvoice.InvoiceFile != null)
                                    {
                                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile));
                                    }

                                    vehicleinvoice.InvoiceFile = vehicleinvoice.Number[i] + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + InvoiceFile[i].FileName;// + Path.GetExtension(CommissionImage.FileName);

                                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile);
                                    InvoiceFile[i].SaveAs(ImagePath);
                                }

                                db.tbl_VehicleInvoices.Add(vehicleinvoice);
                                db.SaveChanges();


                            }
                        }
                    }
                    catch { }
                    //
                    try
                    {
                        for (int i = 0; i < Serial.Count(); i++)
                        {
                            if (Serial[i] != null && Serial[i] != "")
                            {
                                vehicletank.ValveConstractorID = ValveConstractorID[i];
                                vehicletank.ExpirationDate = ExpirationYear[i] + "-" + ExpirationMonth[i];
                                vehicletank.ProductDate = ProductYear[i] + "-" + ProductMonth[i];
                                vehicletank.Serial = Serial[i];
                                vehicletank.CreateDate = DateTime.Now;
                                vehicletank.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                vehicletank.SerialTankValve = SerialTankValve[i];
                                vehicletank.TankConstractorID = Convert.ToInt32(TankConstractorID[i]);
                                vehicletank.TypeTankValve = TypeTankValve[i];
                                vehicletank.VehicleRegistrationID = vehicleRegistration.ID;
                                vehicletank.Volume = Convert.ToDouble(Volume[i]);
                                vehicletank.RegulatorConstractorID = RegulatorConstractorID[i];
                                vehicletank.RegulatorSerial = RegulatorSerial[i].ToUpper();

                                db.tbl_VehicleTanks.Add(vehicletank);
                                db.SaveChanges();

                            }

                        }
                    }
                    catch { }
                    //
                    return RedirectToAction("Index",new { WorkshopID = vehicleRegistration.WorkshopID });                   
                }

                ViewBag.message = "اطلاعات وارد شده تکراری می باشد!";
                //if (ChassisNumber.Count()>0)
                //{

                //}
                //else
                //{
                //    return Json("شماره شاسی وارد شده تکراری می باشد!", JsonRequestBehavior.AllowGet);
                //}

                //

            }

            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--انتخاب نوع خودرو--", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", vehicletank.ValveConstractorID);
            //ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", vehicleRegistration.VehicleTypeID);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            return View(vehicleRegistration);
        }
        //
        public ActionResult CreateNew()
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
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text");
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type");
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            return View();
        }
        //
        public ActionResult VehicleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);           
            //
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", id);

            return View(vehicleRegistration);
        }
        // GET: VehicleRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            ViewBag.InvoiceStatus = db.tbl_VehicleInvoices.Where(i => i.VehicleRegistrationID == vehicleRegistration.ID).Count();
            ViewBag.TankStatus = db.tbl_VehicleTanks.Where(i => i.VehicleRegistrationID == vehicleRegistration.ID).Count();
            var vehicleTank = db.tbl_VehicleTanks.Where(i => i.VehicleRegistrationID == vehicleRegistration.ID).ToList();
            //
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);
            if (vehicleTank.Count() > 0)
            {
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", vehicleTank.SingleOrDefault().ValveConstractorID);
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator", vehicleTank.SingleOrDefault().RegulatorConstractorID);
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", vehicleTank.SingleOrDefault().TankConstractorID);
                ViewBag.Volume = new SelectList(db.tbl_TypeofTanks, "ID", "Type", vehicleTank.SingleOrDefault().Volume);
            }
            else
            {
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator");
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
                ViewBag.Volume = new SelectList(db.tbl_TypeofTanks, "ID", "Type");
            }

            //VehicleTank vehicletank = db.tbl_VehicleTanks.Where(t=>t.VehicleRegistrationID==id);

            //
            string ID = "";
            string VehicleRegistrationID = "";
            string Number = "";
            string Amount = "";
            string Type = "";
            string Date = "";
            string InvoiceFile = "";
            string Description = "";
            string Serial = "";
            string Volume = "";
            string TankConstractorID = "";
            string ProductYear = "";
            string ProductMonth = "";
            string ExpirationYear = "";
            string ExpirationMonth = "";
            string SerialTankValve = "";
            string TypeTankValve = "";
            string ConstractorTankValve = "";
            string Constractor = "";
            string RegulatorConstractorID = "";
            string RegulatorSerial = "";
            int InvoiceCount = 0;
            int TankCount = 0;

            List<VehicleRegistrationsRelatedList> InvoiceTableOuts = new List<VehicleRegistrationsRelatedList>();
            List<VehicleRegistrationsRelatedList> TankTableOuts = new List<VehicleRegistrationsRelatedList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[SP_VehicleRegistrationsRelatedData]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@VehicleRegistrationID", SqlDbType.VarChar).Value = id;                        

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                             VehicleRegistrationID = reader["VehicleRegistrationID"].ToString();
                             Number = reader["Number"].ToString();
                             Amount = reader["Amount"].ToString();
                             Type = reader["Type"].ToString();
                             Date = reader["Date"].ToString();
                             InvoiceFile = reader["InvoiceFile"].ToString();
                             Description = reader["Description"].ToString();
                            InvoiceCount += 1;
                            InvoiceTableOuts.Add(new VehicleRegistrationsRelatedList
                            {
                                ID = ID,
                                VehicleRegistrationID = VehicleRegistrationID,
                                Number = Number,
                                Amount = Amount,
                                Type = Type,
                                Date = Date,
                                InvoiceFile = InvoiceFile,
                                Description = Description,
                                InvoiceCount=InvoiceCount.ToString()
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            VehicleRegistrationID = reader["VehicleRegistrationID"].ToString();                            
                            Serial = reader["Serial"].ToString();
                            Volume = reader["Volume"].ToString();
                            TankConstractorID = reader["TankConstractorID"].ToString();
                            ProductYear = reader["ProductYear"].ToString();
                            ProductMonth = reader["ProductMonth"].ToString();
                            ExpirationYear = reader["ExpirationYear"].ToString();
                            ExpirationMonth = reader["ExpirationMonth"].ToString();
                            SerialTankValve = reader["SerialTankValve"].ToString();
                            TypeTankValve = reader["TypeTankValve"].ToString();
                            ConstractorTankValve = reader["ConstractorTankValve"].ToString();
                            Constractor = reader["Constractor"].ToString();
                            RegulatorSerial= reader["RegulatorSerial"].ToString();
                            RegulatorConstractorID = reader["RegulatorConstractorID"].ToString();
                            TankCount += 1;

                            TankTableOuts.Add(new VehicleRegistrationsRelatedList
                            {
                                ID = ID,
                                VehicleRegistrationID = VehicleRegistrationID,
                                Serial = Serial,
                                Volume = Volume,
                                TankConstractorID = TankConstractorID,
                                ProductYear = ProductYear,
                                ProductMonth = ProductMonth,
                                ExpirationYear = ExpirationYear,
                                ExpirationMonth = ExpirationMonth,
                                SerialTankValve = SerialTankValve,
                                TypeTankValve = TypeTankValve,
                                ConstractorTankValve = ConstractorTankValve,
                                Constractor = Constractor,
                                TankCount=TankCount.ToString(),
                                RegulatorConstractorID=RegulatorConstractorID,
                                RegulatorSerial=RegulatorSerial
                            });
                        }
                    }
                }//end using
                ViewBag.InvoiceTableOut = InvoiceTableOuts;
                ViewBag.TankTableOut = TankTableOuts;
                //ViewBag.TankTableOutCount = TankTableOuts.Count;
            }
            catch
            {
                ViewBag.TableOut = null;
            }
            
            return View(vehicleRegistration);
        }
        //
            // POST: VehicleRegistrations/Edit/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NationalCard,VehicleCard,LicenseImage,VehicleTypeID,System,TypeofUseID,OwnerName,OwnerFamily,PhoneNumber,MobileNumber,NationalCode,Address,ConstructionYear,LeftNumberPlate,AlphaPlate,RightNumberPlate,IranNumberPlate,VIN,EngineNumber,ChassisNumber,SerialSparkPreview,SerialKit,SerialKey,RefuelingLable,SerialRefuelingValve,TrackingCode,License,Description,FuelCard,HealthCertificate,CreateDate,Creator,WorkshopID")] VehicleRegistration vehicleRegistration, HttpPostedFileBase NationalCard, HttpPostedFileBase VehicleCard, HttpPostedFileBase LicenseImage,HttpPostedFileBase HealthCertificate,
            string[] Number, string[] Amount, string[] Type, string[] Date, string[] InvoiceDescription, HttpPostedFileBase[] InvoiceFile, string[] Serial, string[] Volume,string[] hajm, string[] TankConstractorID, string[] ProductMonth, string[] ProductYear, string[] ExpirationMonth, string[] ExpirationYear, string[] SerialTankValve, string[] TypeTankValve, int[] ValveConstractorID, int[] InvoiceID,int[] TankrowID,int[] RegulatorConstractorID, string[] RegulatorSerial)
        {
            if (NationalCard != null)
            {
                if (vehicleRegistration.NationalCard != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + vehicleRegistration.NationalCard));
                }

                vehicleRegistration.NationalCard = vehicleRegistration.NationalCode + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + NationalCard.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + vehicleRegistration.NationalCard);
                NationalCard.SaveAs(ImagePath);
                ViewBag.NationalCardExtention = Path.GetExtension(NationalCard.FileName);
            }

            if (VehicleCard != null)
            {
                if (vehicleRegistration.VehicleCard != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + vehicleRegistration.VehicleCard));
                }

                vehicleRegistration.VehicleCard = vehicleRegistration.NationalCode + "_" + vehicleRegistration.EngineNumber.Replace("/", "") + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + VehicleCard.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + vehicleRegistration.VehicleCard);
                VehicleCard.SaveAs(ImagePath);
                ViewBag.VehicleCardExtention = Path.GetExtension(VehicleCard.FileName);
            }

            if (LicenseImage != null)
            {
                if (vehicleRegistration.LicenseImage != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + vehicleRegistration.LicenseImage));
                }

                vehicleRegistration.LicenseImage = vehicleRegistration.NationalCode + "_" + vehicleRegistration.License + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + LicenseImage.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + vehicleRegistration.LicenseImage);
                LicenseImage.SaveAs(ImagePath);
            }

            if (HealthCertificate != null)
            {
                if (vehicleRegistration.HealthCertificate != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + vehicleRegistration.HealthCertificate));
                }

                vehicleRegistration.HealthCertificate = vehicleRegistration.NationalCode + "_" + vehicleRegistration.License + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + HealthCertificate.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + vehicleRegistration.HealthCertificate);
                HealthCertificate.SaveAs(ImagePath);
            }
            //
            //
            //if (ModelState.IsValid)
            //{
            vehicleRegistration.EditDate = DateTime.Now;
            vehicleRegistration.Editor = cngfapco.Helper.Helpers.GetCurrentUserId();            

            if (!string.IsNullOrEmpty(vehicleRegistration.Description))
                vehicleRegistration.InstallationStatus = true;
            //
            if (ModelState.IsValid)
            {
                //if (vehicleRegistration.CreateDate == null)
                //    vehicleRegistration.CreateDate = DateTime.Now;
                vehicleRegistration.VIN = vehicleRegistration.VIN.ToUpper();
                vehicleRegistration.ChassisNumber = vehicleRegistration.ChassisNumber.ToUpper();
                vehicleRegistration.EngineNumber = vehicleRegistration.EngineNumber.ToUpper();
                vehicleRegistration.EditorIPAddress = IP;

                db.Entry(vehicleRegistration).State = EntityState.Modified;
                db.SaveChanges();
                //
                try
                {
                    if (Amount != null && Amount[0] != "")
                    {
                        for (int i = 0; i < Amount.Count(); i++)
                        {
                            if(InvoiceID != null)
                            {
                                VehicleInvoice vehicleinvoice = db.tbl_VehicleInvoices.Find(InvoiceID[i]);
                                if (Number[i] != null || Number[i] != "")
                                {
                                    vehicleinvoice.Number = Number[i];
                                    vehicleinvoice.Amount = Convert.ToDouble(Amount[i]);
                                    vehicleinvoice.Type = Type[i];
                                    vehicleinvoice.CreateDate = DateTime.Now;
                                    vehicleinvoice.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                    vehicleinvoice.Date = Convert.ToDateTime(Date[i]);
                                    vehicleinvoice.Description = InvoiceDescription[i];
                                    vehicleinvoice.VehicleRegistrationID = vehicleRegistration.ID;
                                    if (InvoiceFile[i] != null)
                                    {
                                        if (vehicleinvoice.InvoiceFile != null)
                                        {
                                            System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile));
                                        }

                                        vehicleinvoice.InvoiceFile = vehicleinvoice.Number[i] + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + InvoiceFile[i].FileName;// + Path.GetExtension(CommissionImage.FileName);

                                        string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile);
                                        InvoiceFile[i].SaveAs(ImagePath);
                                    }

                                    db.Entry(vehicleinvoice).State = EntityState.Modified;
                                    db.SaveChanges();


                                }
                            }
                            else
                            {
                                VehicleInvoice vehicleinvoice = new VehicleInvoice();
                                if (Number[i] != null || Number[i] != "")
                                {
                                    vehicleinvoice.Number = Number[i];
                                    vehicleinvoice.Amount = Convert.ToDouble(Amount[i]);
                                    vehicleinvoice.Type = Type[i];
                                    vehicleinvoice.CreateDate = DateTime.Now;
                                    vehicleinvoice.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                    vehicleinvoice.Date = Convert.ToDateTime(Date[i]);
                                    vehicleinvoice.Description = InvoiceDescription[i];
                                    vehicleinvoice.VehicleRegistrationID = vehicleRegistration.ID;
                                    if (InvoiceFile[i] != null)
                                    {
                                        if (vehicleinvoice.InvoiceFile != null)
                                        {
                                            System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile));
                                        }

                                        vehicleinvoice.InvoiceFile = vehicleinvoice.Number[i] + "_" + vehicleRegistration.OwnerFamily + "_" + vehicleRegistration.OwnerName + "_" + InvoiceFile[i].FileName;// + Path.GetExtension(CommissionImage.FileName);

                                        string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/InvoiceFile/" + vehicleinvoice.InvoiceFile);
                                        InvoiceFile[i].SaveAs(ImagePath);
                                    }

                                    db.tbl_VehicleInvoices.Add(vehicleinvoice);
                                    db.SaveChanges();


                                }
                            }

                        }
                    }
                }
                catch { }                
                //
                try
                {
                    if (Serial != null && Serial[0] != "")
                    {
                        for (int i = 0; i < Serial.Count(); i++)
                        {
                            if (TankrowID != null)
                            {
                                VehicleTank vehicletank = db.tbl_VehicleTanks.Find(TankrowID[i]);
                                if (Serial[i] != null || Serial[i] != "")
                                {
                                    vehicletank.ValveConstractorID = ValveConstractorID[i];
                                    vehicletank.ExpirationDate = ExpirationYear[i] + "-" + ExpirationMonth[i];
                                    vehicletank.ProductDate = ProductYear[i] + "-" + ProductMonth[i];
                                    vehicletank.Serial = Serial[i];
                                    vehicletank.CreateDate = DateTime.Now;
                                    vehicletank.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                    vehicletank.SerialTankValve = SerialTankValve[i];
                                    vehicletank.TankConstractorID = Convert.ToInt32(TankConstractorID[i]);
                                    vehicletank.TypeTankValve = TypeTankValve[i];
                                    vehicletank.VehicleRegistrationID = vehicleRegistration.ID;
                                    vehicletank.Volume = Convert.ToDouble(Volume[i]);
                                    vehicletank.RegulatorSerial = RegulatorSerial[i].ToUpper();
                                    vehicletank.RegulatorConstractorID = RegulatorConstractorID[i];

                                    db.Entry(vehicletank).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", vehicletank.ValveConstractorID);
                            }
                            else
                            {
                                VehicleTank vehicletank = new VehicleTank();
                                if (Serial[i] != null || Serial[i] != "")
                                {
                                    vehicletank.ValveConstractorID = ValveConstractorID[i];
                                    vehicletank.ExpirationDate = ExpirationYear[i] + "-" + ExpirationMonth[i];
                                    vehicletank.ProductDate = ProductYear[i] + "-" + ProductMonth[i];
                                    vehicletank.Serial = Serial[i];
                                    vehicletank.CreateDate = DateTime.Now;
                                    vehicletank.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                                    vehicletank.SerialTankValve = SerialTankValve[i];
                                    vehicletank.TankConstractorID = Convert.ToInt32(TankConstractorID[i]);
                                    vehicletank.TypeTankValve = TypeTankValve[i];
                                    vehicletank.VehicleRegistrationID = vehicleRegistration.ID;
                                    vehicletank.Volume = Convert.ToDouble(Volume[i]);
                                    vehicletank.RegulatorSerial = RegulatorSerial[i].ToUpper();
                                    vehicletank.RegulatorConstractorID = RegulatorConstractorID[i];

                                    db.tbl_VehicleTanks.Add(vehicletank);
                                    db.SaveChanges();

                                }
                                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
                            }

                        }
                    }
                }
                catch { }               
                //
                return RedirectToAction("Index",new { WorkshopID = vehicleRegistration.WorkshopID });
            }
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", vehicleRegistration.ID);
            return View(vehicleRegistration);
        }
        //
        public ActionResult UploadImages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            //
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            return View(vehicleRegistration);
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImages(int? id ,HttpPostedFileBase[] NationalCard, HttpPostedFileBase[] VehicleCard, HttpPostedFileBase[] LicenseImage, HttpPostedFileBase[] HealthCertificate, HttpPostedFileBase[] TechnicalDiagnosis)
        {
            var vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            VehicleAttachment attach = new VehicleAttachment();
            var attachExist = db.tbl_VehicleAttachments.Where(a => a.VehicleRegistrationID == id).ToList();
            attach.Creator = User.Identity.Name;
            attach.CreateDate = DateTime.Now;

            if (NationalCard.Count() > 0 && NationalCard[0] != null)
            {     
                foreach(var item in attachExist.Where(a=>a.Folder.Equals("NationalCard")))
                {
                    if (item.Image != null && !string.IsNullOrEmpty(item.Image))
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + item.Image));
                        db.tbl_VehicleAttachments.Remove(item);
                        db.SaveChanges();
                    }
                }
                foreach(var file in NationalCard)
                {                    
                    attach.VehicleRegistrationID = id;
                    //attach.Image = vehicleRegistration.NationalCode.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerName.TrimStart().TrimEnd() + "_" + file.FileName;// + Path.GetExtension(CommissionImage.FileName);
                    attach.Image = vehicleRegistration.VIN.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + file.FileName;

                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/NationalCard/" + attach.Image.Trim());
                    file.SaveAs(ImagePath);
                    attach.Folder = "NationalCard";

                    db.tbl_VehicleAttachments.Add(attach);
                    db.SaveChanges();
                }
            }
            //
            if (VehicleCard.Count() > 0 && VehicleCard[0] != null)
            {
                foreach (var item in attachExist.Where(a => a.Folder.Equals("VehicleCard")))
                {
                    if (item.Image != null && !string.IsNullOrEmpty(item.Image))
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + item.Image.Trim()));
                        db.tbl_VehicleAttachments.Remove(item);
                        db.SaveChanges();
                    }
                }

                foreach (var file in VehicleCard)
                {
                    attach.VehicleRegistrationID = id;
                    //attach.Image = vehicleRegistration.NationalCode.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerName.TrimStart().TrimEnd() + "_" + file.FileName;// + Path.GetExtension(CommissionImage.FileName);
                    attach.Image = vehicleRegistration.VIN.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + file.FileName;

                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/VehicleCard/" + attach.Image.Trim());
                    file.SaveAs(ImagePath);

                    attach.Folder = "VehicleCard";

                    db.tbl_VehicleAttachments.Add(attach);
                    db.SaveChanges();
                }
            }
            //
            if (LicenseImage.Count() > 0 && LicenseImage[0]!=null)
            {
                foreach (var item in attachExist.Where(a => a.Folder.Equals("LicenseImage")))
                {
                    if (item.Image != null && !string.IsNullOrEmpty(item.Image))
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + item.Image));
                        db.tbl_VehicleAttachments.Remove(item);
                        db.SaveChanges();
                    }
                }

                foreach (var file in LicenseImage)
                {
                    attach.VehicleRegistrationID = id;
                    //attach.Image = vehicleRegistration.NationalCode.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerName.TrimStart().TrimEnd() + "_" + file.FileName;// + Path.GetExtension(CommissionImage.FileName);
                    attach.Image = vehicleRegistration.VIN.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + file.FileName;

                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/LicenseImage/" + attach.Image.Trim());
                    file.SaveAs(ImagePath);

                    attach.Folder = "LicenseImage";

                    db.tbl_VehicleAttachments.Add(attach);
                    db.SaveChanges();
                }
            }
            //
            if (HealthCertificate.Count() > 0 && HealthCertificate[0] != null)
            {
                foreach (var item in attachExist.Where(a => a.Folder.Equals("HealthCertificate")))
                {
                    if (item.Image != null && !string.IsNullOrEmpty(item.Image))
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + item.Image));
                        db.tbl_VehicleAttachments.Remove(item);
                        db.SaveChanges();
                    }
                }

                foreach (var file in HealthCertificate)
                {
                    attach.VehicleRegistrationID = id;
                    //attach.Image = vehicleRegistration.NationalCode.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerName.TrimStart().TrimEnd() + "_" + file.FileName;// + Path.GetExtension(CommissionImage.FileName);
                    attach.Image = vehicleRegistration.VIN.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + file.FileName;

                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/HealthCertificate/" + attach.Image.Trim());
                    file.SaveAs(ImagePath);

                    attach.Folder = "HealthCertificate";

                    db.tbl_VehicleAttachments.Add(attach);
                    db.SaveChanges();
                }
            }
            //
            if (TechnicalDiagnosis.Count() > 0 && TechnicalDiagnosis[0] != null)
            {
                foreach (var item in attachExist.Where(a => a.Folder.Equals("TechnicalDiagnosis")))
                {
                    if (item.Image != null && !string.IsNullOrEmpty(item.Image))
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Vehicle/TechnicalDiagnosis/" + item.Image));
                        db.tbl_VehicleAttachments.Remove(item);
                        db.SaveChanges();
                    }
                }

                foreach (var file in TechnicalDiagnosis)
                {
                    attach.VehicleRegistrationID = id;
                    //attach.Image = vehicleRegistration.NationalCode.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerName.TrimStart().TrimEnd() + "_" + file.FileName;// + Path.GetExtension(CommissionImage.FileName);
                    attach.Image = vehicleRegistration.VIN.TrimStart().TrimEnd() + "_" + vehicleRegistration.OwnerFamily.TrimStart().TrimEnd() + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + file.FileName;

                    string ImagePath = Server.MapPath("/UploadedFiles/Vehicle/TechnicalDiagnosis/" + attach.Image.Trim());
                    file.SaveAs(ImagePath);

                    attach.Folder = "TechnicalDiagnosis";

                    db.tbl_VehicleAttachments.Add(attach);
                    db.SaveChanges();
                }
            }
            //

            return RedirectToAction("DetailsSection3", new { id=id});
        }
        //
        public ActionResult UploadImageList(int? id)
        {
            var attachList = db.tbl_VehicleAttachments.Include(v=>v.VehicleRegistration).Where(v => v.VehicleRegistrationID == id);
            ViewBag.RegistrationID = id;
            return PartialView(attachList.ToList());
        }
        //
        [RBAC]
        // GET: VehicleRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);            
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            return View(vehicleRegistration);
        }

        // POST: VehicleRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            var workshopId = vehicleRegistration.WorkshopID;
            var vehicletank = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == vehicleRegistration.ID).ToList();
            foreach(var item in vehicletank)
            {
                db.tbl_VehicleTanks.Remove(item);
                db.SaveChanges();
            }
            //
            var vehicleinvoice = db.tbl_VehicleInvoices.Where(v => v.VehicleRegistrationID == vehicleRegistration.ID).ToList();
            foreach (var item in vehicleinvoice)
            {
                db.tbl_VehicleInvoices.Remove(item);
                db.SaveChanges();
            }
            //
            var insurance = db.tbl_Insurances.Where(v => v.VehicleRegistrationID == vehicleRegistration.ID).ToList();
            foreach (var item in insurance)
            {
                db.tbl_Insurances.Remove(item);
                db.SaveChanges();
            }
            //
            var attachments = db.tbl_VehicleAttachments.Where(v => v.VehicleRegistrationID == vehicleRegistration.ID).ToList();
            foreach (var item in attachments)
            {
                db.tbl_VehicleAttachments.Remove(item);
                db.SaveChanges();
            }
            //
            var invoice = db.tbl_Invoices.Where(v => v.OwnersID == vehicleRegistration.ID).ToList();
            foreach (var item in invoice)
            {
                db.tbl_Invoices.Remove(item);
                db.SaveChanges();
            }
            //
            db.tbl_VehicleRegistrations.Remove(vehicleRegistration);
            db.SaveChanges();

            return RedirectToAction("Index",new { WorkshopID = workshopId });
        }

        //GetVehicleType
        public JsonResult GetVehicleType()
        {
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--انتخاب نوع خودرو--", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //GetTypeofTanks
        public JsonResult GetTypeofTanks(int? id)
        {
            //string countrystring = "select ID,Type,Description from dbo.tbl_TypeofTanks where VehicleTypeId='" + id + "'";

            //DataTable dt = new DataTable();
            //dt = objdal.MyMethod(countrystring);
            //List<SelectListItem> list = new List<SelectListItem>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " " + Convert.ToString(row.ItemArray[2]) + "لیتری", Value = Convert.ToString(row.ItemArray[0]) });
            //}
            var typeofTanks = db.tbl_TypeofTanks.Where(t=>t.VehicleTypeId==id).SingleOrDefault();
            //return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
            return Json(new { success = true, responseText = "نوع خودرو مرتبط", responseId = typeofTanks.ID }, JsonRequestBehavior.AllowGet);
        }
        //
        public class VehicleRegistrationsRelatedList
        {
            public string ID { get; set; }
            public string VehicleRegistrationID { get; set; }
            public string Number { get; set; }
            public string InvoiceCount { get; set; }
            public string Amount { get; set; }
            public string Type { get; set; }
            public string Date { get; set; }
            public string InvoiceFile { get; set; }
            public string Description { get; set; }
            public string TankCount { get; set; }
            public string Serial { get; set; }
            public string Volume { get; set; }
            public string TankConstractorID { get; set; }
            public string ProductYear { get; set; }
            public string ProductMonth { get; set; }
            public string ExpirationYear { get; set; }
            public string ExpirationMonth { get; set; }
            public string SerialTankValve { get; set; }
            public string TypeTankValve { get; set; }
            public string ConstractorTankValve { get; set; }
            public string Constractor { get; set; }
            public string RegulatorSerial { get; set; }
            public string RegulatorConstractorID { get; set; }

        }
        //
        public ActionResult AddInsuranceCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleregistration = db.tbl_VehicleRegistrations.Find(id);

            if (vehicleregistration == null)
            {
                return HttpNotFound();
            }           
            return PartialView(vehicleregistration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInsuranceCode(string Number, int? VehicleRegistrationID)
        {
            Insurance insurance = new Insurance();

            insurance.VehicleRegistrationID = VehicleRegistrationID;
            insurance.Number = Number;
            insurance.CreateDate = DateTime.Now;
            insurance.Creator = User.Identity.Name;

            db.tbl_Insurances.Add(insurance);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        public static string GetInsuranceCode(int? id)
        {
            Insurance insurance = dbStatic.tbl_Insurances
                .Include(i => i.VehicleRegistration)
                .Where(c => c.VehicleRegistrationID == id)
                .SingleOrDefault();

            if (insurance != null)
                return insurance.Number;
            else
                return "+";
        }
        //
        public static string GetWorkshopUser(int? id)
        {
            User user = dbStatic.tbl_Users
                .Include(i => i.Workshop)
                .Where(c => c.UserID == id)
                .FirstOrDefault();

            if (user != null)
                return user.Workshop.Title + " - " + user.Workshop.City.Title;
            else
                return "+";
        }
        //
        public ActionResult InsuranceList()
        {
            var insurancelist = db.tbl_Insurances.Include(i=>i.VehicleRegistration).ToList();
            List<Insurance> TableOuts = new List<Insurance>();

            foreach (var item in insurancelist)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.VehicleRegistration.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    TableOuts.Add(new Insurance
                    {
                        CreateDate=item.CreateDate,
                        VehicleRegistration=item.VehicleRegistration,
                        Creator=item.Creator,
                        ID=item.ID,
                        Number=item.Number,
                        VehicleRegistrationID=item.VehicleRegistrationID
                    });
                }

            };

            ViewBag.TableOut = TableOuts;

            return View(TableOuts.ToList());
        }
        //
        public ActionResult InsurancePreview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleregistration = db.tbl_VehicleRegistrations.Find(id);

            if (vehicleregistration == null)
            {
                return HttpNotFound();
            }
            return View(vehicleregistration);
        }
        //
        public static string GetTankVolum(int? id)
        {
            TypeofTank volum = dbStatic.tbl_TypeofTanks
                .Where(c => c.ID == id)
                .SingleOrDefault();

            if (volum != null)
                return volum.Type+" " + "لیتری";
            else
                return "";
        }
        //
        public ActionResult RegistrationCount2(int?[] WorkshopID,DateTime fromDate, DateTime toDate,bool? Post)
        {            
            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            var VehicleRegistrationList = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true).Include(v => v.VehicleType).Include(v => v.Workshop).ToList();            

            //string fDate = fromDate.Year + "-" + fromDate.Month + "-" + fromDate.Day;
            //string tDate = toDate.Year + "-" + toDate.Month + "-" + toDate.Day;

            foreach (var item in tbl_Workshops.Where(w => w.isServices == true).ToList())
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
            }

            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            //

            try
            {                
                //var VehicleRegistrationList = db.tbl_VehicleRegistrations.Include(v => v.VehicleType).Include(v => v.Workshop);
                List<VehicleRegistration> TableOuts = new List<VehicleRegistration>();

                if(WorkshopID==null || WorkshopID[0] == null)
                {
                    foreach (var item in VehicleRegistrationList)
                    {
                        Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                        User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                        if (workshop.Users.Contains(_user))
                        {
                            TableOuts.Add(new VehicleRegistration
                            {
                                CreateDate = item.CreateDate,
                                Workshop = item.Workshop,
                                VehicleType = item.VehicleType,
                                VehicleTypeID=item.VehicleTypeID,
                                TypeofUse = item.TypeofUse,
                                VIN = item.VIN,
                                NationalCode = item.NationalCode,
                                FuelCard=item.FuelCard                                
                            });
                        }

                    };
                    ViewBag.typeDistinct = TableOuts.Select(v => v.VehicleTypeID).Distinct();
                    ViewBag.TableOut = TableOuts;
                }

                else
                {                    
                    foreach(var _workshop in WorkshopID)
                    {
                        if(_workshop!=null)
                        {
                            var results = VehicleRegistrationList.Where(v => v.RegisterStatus==true && v.CreateDate <= toDate && v.CreateDate >= fromDate).ToList();

                            foreach (var item in results.Where(v => v.WorkshopID == _workshop.Value))
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                                if (workshop.Users.Contains(_user))
                                {
                                    TableOuts.Add(new VehicleRegistration
                                    {
                                        CreateDate = item.CreateDate,
                                        Workshop = item.Workshop,
                                        VehicleType = item.VehicleType,
                                        VehicleTypeID=item.VehicleTypeID,
                                        TypeofUse = item.TypeofUse,
                                        VIN = item.VIN,
                                        NationalCode = item.NationalCode,
                                        FuelCard = item.FuelCard
                                    });
                                }

                            };
                        }                        
                    }
                    ViewBag.typeDistinct = TableOuts.Select(v => v.VehicleTypeID).Distinct();
                    ViewBag.TableOut = TableOuts;
                }

            }
            catch
            {
                ViewBag.TableOut = null;
            }
            try
            {
                if (Post == false || Post == null)
                    fromDate = db.tbl_VehicleRegistrations.FirstOrDefault().CreateDate;
            }
            catch
            {
                if (Post == false || Post == null)
                    fromDate = DateTime.Now;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public static int GetVehicleTypeCount(int? id)
        {
            var vehicleCount = dbStatic.tbl_VehicleRegistrations
                .Include(i => i.VehicleType)
                .Where(c => c.VehicleTypeID == id)
                .ToList();
            int count = 0;
            foreach (var item in vehicleCount)
            {
                Workshop workshop = dbStatic.tbl_Workshops.Find(item.WorkshopID);
                User _user = dbStatic.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    count += 1; 
                }

            };
            

            //if (vehicleCount != 0)
            //    return vehicleCount;
            //else
                return count;
        }
        //
        public static string GetVehicleTypeTitle(int? id)
        {
            var vehicleTitle = dbStatic.tbl_VehicleTypes
                .Where(c => c.ID == id)
                .SingleOrDefault();

            if (vehicleTitle != null)
                return vehicleTitle.Type;
            else
                return "";
        }
        //
        public ActionResult RegistrationCount(int?[] WorkshopID,int? VehicleType, DateTime fromDate, DateTime toDate, bool? Post,string count,string RegistrationTypeID)
        {
            ViewBag.RegistrationTypeID = new SelectList(db.tbl_RegistrationTypes, "ID", "Type");
            if (Post != true)
                fromDate = DateTime.Now.AddDays(-7); //Convert.ToDateTime("1399/01/01"); //
            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission =""; 
            string VehicleTypes = "";
            //
            foreach (var item in workshops)
            {
                Workshop workshop = dbStatic.tbl_Workshops.Find(item.ID);
                User _user = dbStatic.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            if (WorkshopID==null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop workshop = dbStatic.tbl_Workshops.Find(item.ID);
                    User _user = dbStatic.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                        //tableOuts.Add(new Workshop
                        //{
                        //    ID=item.ID,
                        //    Title=item.Title
                        //});
                    }

                };

                //ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");

            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop workshop = dbStatic.tbl_Workshops.Find(item);
                    User _user = dbStatic.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                        //tableOuts.Add(new Workshop
                        //{
                        //    //ID =Convert.ToInt32(item),
                        //    //Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item)).Title
                        //    ID = item.ID,
                        //    Title = item.Title
                        //});
                    }

                };
               // ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title",WorkshopID[0]);

            }
            //
            if (VehicleType == null)
            {
                foreach (var item in Vehicle)
                {
                    VehicleTypes += item.ID + ",";
                };
                ViewBag.Type = "";
            }
            else
            {
                VehicleTypes = VehicleType + ",";
                ViewBag.Type = db.tbl_VehicleTypes.Find(VehicleType).Type + " - " + db.tbl_VehicleTypes.Find(VehicleType).Description;
            }

            if (RegistrationTypeID == null)
                RegistrationTypeID = "1,2";

            //برای بخش تعداد و نوع ماشین های تبدیل شده
            string VehicleTypeID = "";
            string Count = "";
            string Title = "";
            string Image = "~/img/car/car_cng_2.png";
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string Type = "";
            string CreateDate = "";
            string WorkshopTitle = "";
            string NationalCode = "";
            string FuelCard = "";
            string VIN = "";
            int Counter = 0;
            //
            List<VehicleType> vehicletype = new List<VehicleType>();
            List<VehicleRegistrations> vehicleregistration = new List<VehicleRegistrations>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrations]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@VehicleTypeID", SqlDbType.VarChar).Value = VehicleTypes.TrimEnd(',');
                cmd.Parameters.Add("@RegistrationTypeID", SqlDbType.VarChar).Value = RegistrationTypeID;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VehicleTypeID = reader["VehicleTypeID"].ToString();
                    Count = reader["Count"].ToString();
                    Image = reader["Image"].ToString();
                    Title = reader["Title"].ToString();                    
                    //
                    vehicletype.Add(new VehicleType
                    {
                        ID=Convert.ToInt32(VehicleTypeID),
                        Image=Image,
                        Type=Count,
                        Description=Title
                    });
                }
                reader.NextResult();
                while (reader.Read())
                {
                    Type = reader["Type"].ToString()+ " " + reader["vtDescription"].ToString();
                    CreateDate = reader["CreateDate"].ToString();
                    WorkshopTitle= reader["Title"].ToString();
                    NationalCode = reader["NationalCode"].ToString();
                    FuelCard = reader["FuelCard"].ToString();
                    VIN = reader["VIN"].ToString();
                    //if (string.IsNullOrEmpty(count))
                    //    count = "0";
                    Counter += 1;
                    ViewBag.count = Counter;
                    //
                    vehicleregistration.Add(new VehicleRegistrations
                    {
                        Type = Type,
                        CreateDate=CreateDate,
                        WorkshopTitle=WorkshopTitle,
                        NationalCode= NationalCode,
                        VIN=VIN,
                        FuelCard=FuelCard

                    });
                }

                conn.Close();
            }
            
            ViewBag.vehicletype = vehicletype;
            ViewBag.vehicleregistration = vehicleregistration;
            //
            //try
            //{
            //    if (Post == false || Post == null)
            //        fromDate = db.tbl_VehicleRegistrations.FirstOrDefault().CreateDate;
            //}
            //catch
            //{
            //    if (Post == false || Post == null)
            //        fromDate = DateTime.Now;
            //}
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            //

            return View();
        }
        /// <summary>
        /// Lazy Load
        /// </summary>
        /// <returns></returns>
        public ActionResult LazyLoadPage()
        {
            return RedirectToAction("GetProjects");
        }
        //
        public ActionResult GetProjects(int? pageNum)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            bool? Post = false;
            int?[] WorkshopID = { 7 };
            pageNum = pageNum ?? 0;
            ViewBag.IsEndOfRecords = false;
            if (Request.IsAjaxRequest())
            {
                var projects = GetRecordsForPage(pageNum.Value);
                ViewBag.IsEndOfRecords = (projects.Any());
                return PartialView("_ProjectData", projects);
            }
            else
            {
                var projectRep = new ProjectRepository();
                ProjectData = projectRep.GetProjectList(WorkshopID, fromDate, toDate, Post);

                ViewBag.TotalNumberProjects = ProjectData.Count;
                ViewBag.Projects = GetRecordsForPage(pageNum.Value);

                return View("LazyLoadPage");
            }
        }

        public List<Registration> GetRecordsForPage(int pageNum)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            bool? Post = false;
            int?[] WorkshopID = { 7 };

            var projectRep = new ProjectRepository();
            ProjectData = projectRep.GetProjectList(WorkshopID, fromDate, toDate, Post);

            int from = (pageNum * RecordsPerPage);

            var tempList = (from rec in ProjectData
                            select rec).Skip(from).Take(20).ToList<Registration>();

            return tempList;
        }
        //
        public ActionResult Index(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            string permission = ""; //db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault()

            var userRole = Helper.Helpers.GetCurrentUserRoles();
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var role in userRole)
                {
                    if (role.RoleName.Contains("مدیر تبدیل ناوگان") || role.RoleName.Contains("admin"))
                    {
                        foreach (var item in workshops)
                        {
                            permission += item.ID + ",";
                        }
                    }

                    if (role.RoleName.Contains("مرکز خدمات (کارگاه)"))
                    {
                        permission = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                    }

                    if (!role.RoleName.Contains("مرکز خدمات (کارگاه)") && !role.RoleName.Contains("مدیر تبدیل ناوگان") && !role.RoleName.Contains("admin"))
                    {
                        if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
                        {
                            foreach (var item in workshops)
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                                if (workshop.Users.Contains(_user))
                                {
                                    permission += item.ID + ",";
                                }

                            };
                        }
                        else
                        {
                            foreach (var item in WorkshopID)
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item);
                                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                                if (workshop.Users.Contains(_user))
                                {
                                    permission += item + ",";
                                }

                            };

                        }
                    }
                }
            }

            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };

            }
            ViewBag.workshopId = permission.TrimEnd(',');
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string ID = "";
            string WorkshopTitle = "";
            string VehicleType = "";
            string TypeofUse = "";
            string FullName = "";
            string MobileNumber = "";
            string NationalCode = "";
            //LeftNumberPlate, AlphaPlate, RightNumberPlate, IranNumberPlate, EngineNumber, ChassisNumber,
            string Plate = "";
            string EngineNumber = "";
            string ChassisNumber = "";
            string VIN = "";
            string InsuranceNumber = "";
            DateTime? CreateDate = null;
            int rowNum = 0;
            string CylinderBulk = "";
            string CylinderSerial = "";
            string ValveSerial = "";
            string RegistrationTypeID = "";
            string RegistrationType = "";
            //
            List<VehicleRegistrationList> vehicleregistration = new List<VehicleRegistrationList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
                try
                {
                    using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationTable]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            CreateDate=Convert.ToDateTime(reader["CreateDate"].ToString());
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            if (Convert.ToInt32((reader["VehicleTypeID"].ToString().Length)) > 0)
                                VehicleType = reader["VehicleType"].ToString();
                            else
                                VehicleType = "-";
                            if (Convert.ToInt32((reader["TypeofUseID"].ToString().Length)) > 0)
                                TypeofUse = reader["TypeofUse"].ToString();
                            else
                                TypeofUse = "-";

                            if (Convert.ToInt32((reader["OwnerName"].ToString().Length)) > 0)
                                FullName = reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                            else
                                FullName = "-";

                            if (Convert.ToInt32((reader["NationalCode"].ToString().Length)) > 0)
                                NationalCode = reader["NationalCode"].ToString();
                            else
                                NationalCode = "-";

                            if (Convert.ToInt32((reader["EngineNumber"].ToString().Length)) > 0)
                                EngineNumber = reader["EngineNumber"].ToString();
                            else
                                EngineNumber = "-";

                            if (Convert.ToInt32((reader["ChassisNumber"].ToString().Length)) > 0)
                                ChassisNumber = reader["ChassisNumber"].ToString();
                            else
                                ChassisNumber = "-";

                            MobileNumber = reader["MobileNumber"].ToString();
                            Plate = "ایران " + reader["IranNumberPlate"].ToString() + " - " + reader["RightNumberPlate"].ToString() + " " + reader["AlphaPlate"].ToString()+ " " + reader["LeftNumberPlate"].ToString();

                            VIN = reader["VIN"].ToString();
                            InsuranceNumber = reader["InsuranceNumber"].ToString();
                            CylinderBulk = reader["CylinderBulk"].ToString();
                            CylinderSerial = reader["Serial"].ToString();
                            ValveSerial = reader["SerialTankValve"].ToString();
                            RegistrationType = reader["RegistrationType"].ToString();
                            RegistrationTypeID = reader["RegistrationTypeID"].ToString();
                            //string typeofuse = "";
                            //string vehicleType = "";
                            //if (!string.IsNullOrEmpty(TypeofUse))
                            //    typeofuse = Helper.Helpers.GetTypeofUse(Convert.ToInt32(TypeofUse)).Type;
                            //if (!string.IsNullOrEmpty(VehicleType))
                            //    vehicleType = Helper.Helpers.GetVehicleType(Convert.ToInt32(VehicleType)).Type;
                            rowNum += 1;
                            //
                            vehicleregistration.Add(new VehicleRegistrationList
                            {
                                ID = ID,
                                rowNum = rowNum.ToString(),
                                CreateDate= CreateDate,
                                ChassisNumber = ChassisNumber,
                                TypeofUse = TypeofUse,
                                WorkshopTitle = WorkshopTitle.Replace("مرکز خدمات CNG", " کارگاه"),// WorkshopTitle, //Helper.Helpers.GetWorkshops(Convert.ToInt32(WorkshopTitle)).Title,
                                EngineNumber = EngineNumber,
                                FullName = FullName,
                                NationalCode = NationalCode,
                                MobileNumber = MobileNumber,
                                Plate = Plate,
                                VehicleType = VehicleType,
                                VIN = VIN,
                                //InsuranceNumber = cngfapco.Controllers.VehicleRegistrationsController.GetInsuranceCode(Convert.ToInt32(ID))
                                InsuranceNumber = InsuranceNumber,
                                Details = "1",
                                CylinderBulk=CylinderBulk,
                                CylinderNumber=CylinderSerial,
                                ValveNumber=ValveSerial,
                                RegistrationType= RegistrationType,
                                RegistrationTypeID= RegistrationTypeID
                            });
                        }

                        conn.Close();
                    }
                }
                catch { }


            ViewBag.vehicleregistration = vehicleregistration;
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            var rolName = cngfapco.Helper.Helpers.GetCurrentUserRole();
            if (rolName.Contains("مرکز خدمات (کارگاه)"))
            {
                var workshopId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().WorkshopID;
                var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.WorkshopID == workshopId).Include(r => r.DivisionPlan).Include(r => r.User);
                int count = 0;

                foreach (var item in tbl_Remittances)
                {
                    if ((DateTime.Now - item.CreateDate).Days > 0)
                    {
                        count++;
                    }
                }

                if (count > 0)
                    return RedirectToAction("Lockscreen", "Home");
                else
                    return View();
            }
            else
                return View();
        }
        //
        public ActionResult AllVehicle(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            string permission = ""; //db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault()

            var userRole = Helper.Helpers.GetCurrentUserRoles();
            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var role in userRole)
                {
                    if (role.RoleName.Contains("مدیر تبدیل ناوگان") || role.RoleName.Contains("admin"))
                    {
                        foreach (var item in workshops)
                        {
                            permission += item.ID + ",";
                        }
                    }

                    if (role.RoleName.Contains("مرکز خدمات (کارگاه)"))
                    {
                        permission = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                    }

                    if (!role.RoleName.Contains("مرکز خدمات (کارگاه)") && !role.RoleName.Contains("مدیر تبدیل ناوگان") && !role.RoleName.Contains("admin"))
                    {
                        if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
                        {
                            foreach (var item in workshops)
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                                if (workshop.Users.Contains(_user))
                                {
                                    permission += item.ID + ",";
                                }

                            };
                        }
                        else
                        {
                            foreach (var item in WorkshopID)
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item);
                                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                                if (workshop.Users.Contains(_user))
                                {
                                    permission += item + ",";
                                }

                            };

                        }
                    }
                }
            }

            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };

            }
            ViewBag.workshopId = permission.TrimEnd(',');
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string ID = "";
            string WorkshopTitle = "";
            string VehicleType = "";
            string TypeofUse = "";
            string FullName = "";
            string MobileNumber = "";
            //LeftNumberPlate, AlphaPlate, RightNumberPlate, IranNumberPlate, EngineNumber, ChassisNumber,
            string Plate = "";
            string EngineNumber = "";
            string ChassisNumber = "";
            string VIN = "";
            string InsuranceNumber = "";
            string SerialKit = "";
            string KitConstractor = "";
            string CylinderNumber = "";
            string CylinderBulk = "";
            string CylinderCostractor = "";
            string ValveNumber = "";
            string ValveCostractor = "";
            string SerialCutofValve = "";
            string CutofValveConstractor = "";
            string SerialFillingValve = "";
            string FillingValveConstractor = "";

            DateTime? CreateDate = null;
            int rowNum = 0;
            //
            List<VehicleRegistrationList> vehicleregistration = new List<VehicleRegistrationList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
                try
                {
                    using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationTable]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            if (Convert.ToInt32((reader["VehicleTypeID"].ToString().Length)) > 0)
                                VehicleType = reader["VehicleType"].ToString();
                            else
                                VehicleType = "-";
                            if (Convert.ToInt32((reader["TypeofUseID"].ToString().Length)) > 0)
                                TypeofUse = reader["TypeofUse"].ToString();
                            else
                                TypeofUse = "-";

                            if (Convert.ToInt32((reader["OwnerName"].ToString().Length)) > 0)
                                FullName = reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                            else
                                FullName = "-";

                            if (Convert.ToInt32((reader["EngineNumber"].ToString().Length)) > 0)
                                EngineNumber = reader["EngineNumber"].ToString();
                            else
                                EngineNumber = "-";

                            if (Convert.ToInt32((reader["ChassisNumber"].ToString().Length)) > 0)
                                ChassisNumber = reader["ChassisNumber"].ToString();
                            else
                                ChassisNumber = "-";

                            MobileNumber = reader["MobileNumber"].ToString();
                            Plate = "ایران " + reader["IranNumberPlate"].ToString() + " - " + reader["RightNumberPlate"].ToString() + " " + reader["AlphaPlate"].ToString() + " " + reader["LeftNumberPlate"].ToString();

                            VIN = reader["VIN"].ToString();
                            InsuranceNumber = " "; //reader["InsuranceNumber"].ToString();
                            if (Convert.ToInt32((reader["RegulatorSerial"].ToString().Length)) > 0)
                                SerialKit = reader["RegulatorSerial"].ToString();
                            else
                                SerialKit = "-";
                            if (Convert.ToInt32((reader["KitConstractor"].ToString().Length)) > 0)
                                KitConstractor = reader["KitConstractor"].ToString();
                            else
                                KitConstractor = "-";
                            if (Convert.ToInt32((reader["Valve"].ToString().Length)) > 0)
                                ValveCostractor = reader["Valve"].ToString();
                            else
                                ValveCostractor = "-";
                            if (Convert.ToInt32((reader["SerialTankValve"].ToString().Length)) > 0)
                                ValveNumber = reader["SerialTankValve"].ToString();
                            else
                                ValveNumber = "-";
                            if (Convert.ToInt32((reader["Constractor"].ToString().Length)) > 0)
                                CylinderCostractor = reader["Constractor"].ToString();
                            else
                                CylinderCostractor = "-";
                            if (Convert.ToInt32((reader["Serial"].ToString().Length)) > 0)
                                CylinderNumber = reader["Serial"].ToString();
                            else
                                CylinderNumber = "-";
                            if (Convert.ToInt32((reader["CylinderBulk"].ToString().Length)) > 0)
                                CylinderBulk = reader["CylinderBulk"].ToString();
                            else
                                CylinderBulk = "-";
                            //
                            if (Convert.ToInt32((reader["CutofValveSerial"].ToString().Length)) > 0)
                                SerialCutofValve = reader["CutofValveSerial"].ToString();
                            else
                                SerialCutofValve = "-";
                            if (Convert.ToInt32((reader["CutofValveConstractor"].ToString().Length)) > 0)
                                CutofValveConstractor = reader["CutofValveConstractor"].ToString();
                            else
                                CutofValveConstractor = "-";
                            //
                            if (Convert.ToInt32((reader["FillingValveSerial"].ToString().Length)) > 0)
                                SerialFillingValve = reader["FillingValveSerial"].ToString();
                            else
                                SerialFillingValve = "-";
                            if (Convert.ToInt32((reader["FillingValveConstractor"].ToString().Length)) > 0)
                                FillingValveConstractor = reader["FillingValveConstractor"].ToString();
                            else
                                FillingValveConstractor = "-";
                            //string typeofuse = "";
                            //string vehicleType = "";
                            //if (!string.IsNullOrEmpty(TypeofUse))
                            //    typeofuse = Helper.Helpers.GetTypeofUse(Convert.ToInt32(TypeofUse)).Type;
                            //if (!string.IsNullOrEmpty(VehicleType))
                            //    vehicleType = Helper.Helpers.GetVehicleType(Convert.ToInt32(VehicleType)).Type;
                            rowNum += 1;

                            //
                            vehicleregistration.Add(new VehicleRegistrationList
                            {
                                ID = ID,
                                rowNum = rowNum.ToString(),
                                CreateDate = CreateDate,
                                ChassisNumber = ChassisNumber.ToUpper(),
                                TypeofUse = TypeofUse,
                                WorkshopTitle = WorkshopTitle.Replace("مرکز خدمات CNG", " کارگاه"),// WorkshopTitle, //Helper.Helpers.GetWorkshops(Convert.ToInt32(WorkshopTitle)).Title,
                                EngineNumber = EngineNumber.ToUpper(),
                                FullName = FullName,
                                MobileNumber = MobileNumber,
                                Plate = Plate,
                                VehicleType = VehicleType,
                                VIN = VIN.ToUpper(),
                                //InsuranceNumber = cngfapco.Controllers.VehicleRegistrationsController.GetInsuranceCode(Convert.ToInt32(ID))
                                InsuranceNumber = InsuranceNumber,
                                Details = "1",
                                SerialKit= SerialKit.ToUpper(),
                                CylinderBulk=CylinderBulk,
                                CylinderNumber=CylinderNumber.ToUpper(),
                                CylinderCostractor=CylinderCostractor,
                                ValveNumber=ValveNumber.ToUpper(),
                                ValveCostractor=ValveCostractor,
                                KitConstractor=KitConstractor,
                                CutofValveConstractor=CutofValveConstractor,
                                FillingValveConstractor=FillingValveConstractor,
                                SerialCutofValve=SerialCutofValve,
                                SerialFillingValve=SerialFillingValve
                            });
                        }

                        conn.Close();
                    }
                }
                catch { }


            ViewBag.vehicleregistration = vehicleregistration;
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            var rolName = cngfapco.Helper.Helpers.GetCurrentUserRole();
            if (rolName.Contains("مرکز خدمات (کارگاه)"))
            {
                var workshopId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().WorkshopID;
                var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.WorkshopID == workshopId).Include(r => r.DivisionPlan).Include(r => r.User);
                int count = 0;

                foreach (var item in tbl_Remittances)
                {
                    if ((DateTime.Now - item.CreateDate).Days > 0)
                    {
                        count++;
                    }
                }

                if (count > 0)
                    return RedirectToAction("Lockscreen", "Home");
                else
                    return View();
            }
            else
                return View();
        }
        //
        public ActionResult Index2()
        {
            var vehicleList = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true).Include(v => v.TypeofUse).Include(v => v.Workshop).Include(v => v.VehicleType);
            var workshops = db.tbl_Workshops.ToList();
            string permission = ""; //db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault()

            var userRole = Helper.Helpers.GetCurrentUserRoles();

            foreach (var role in userRole)
            {
                if (role.RoleName.Contains("مدیر تبدیل ناوگان") || role.RoleName.Contains("admin"))
                {
                    permission = "0";
                }

                if (role.RoleName.Contains("مرکز خدمات (کارگاه)"))
                {
                    permission = Helper.Helpers.GetWorkshopCurrentUser().ID.ToString();
                }

                if (!role.RoleName.Contains("مرکز خدمات (کارگاه)") && !role.RoleName.Contains("مدیر تبدیل ناوگان") && !role.RoleName.Contains("admin"))
                {
                    foreach (var item in workshops)
                    {
                        Workshop workshop = dbStatic.tbl_Workshops.Find(item.ID);
                        User _user = dbStatic.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                        if (workshop.Users.Contains(_user))
                        {
                            permission += item.ID + ",";
                        }

                    };
                }
            }
            if(permission.Equals("0"))
                return View( vehicleList.ToList());
            if(Convert.ToDouble(permission)>0)
                return View(vehicleList.Where(v=>v.WorkshopID.Value.ToString().Equals(permission)).ToList());
            else
                return View(vehicleList.Where(v => v.WorkshopID.Value.ToString().Contains(permission)).ToList());
        }
        //
        //[HttpPost]
        public JsonResult AddData(int?[] WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Workshops.ToList();
            string permission = ""; //db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault()

            var userRole = Helper.Helpers.GetCurrentUserRoles();

            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.ID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item.ID + ",";
                    }

                };
            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item + ",";
                    }

                };

            }

            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string ID = "";
            string WorkshopTitle = "";
            string VehicleType = "";
            string TypeofUse = "";
            string FullName = "";
            string MobileNumber = "";
            string NationalCode = "";
            //LeftNumberPlate, AlphaPlate, RightNumberPlate, IranNumberPlate, EngineNumber, ChassisNumber,
            string Plate = "";
            string EngineNumber = "";
            string ChassisNumber = "";
            string VIN = "";
            string InsuranceNumber = "";
            string CreateDate = null;
            int rowNum = 0;
            string CylinderBulk = "";
            string CylinderSerial = "";
            string ValveSerial = "";
            //
            List<VehicleRegistrationList> vehicleregistration = new List<VehicleRegistrationList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
                try
                {
                    using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationTable]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString()).ToShortDateString();
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            if (Convert.ToInt32((reader["VehicleTypeID"].ToString().Length)) > 0)
                                VehicleType = reader["VehicleType"].ToString();
                            else
                                VehicleType = "-";
                            if (Convert.ToInt32((reader["TypeofUseID"].ToString().Length)) > 0)
                                TypeofUse = reader["TypeofUse"].ToString();
                            else
                                TypeofUse = "-";

                            if (Convert.ToInt32((reader["OwnerName"].ToString().Length)) > 0)
                                FullName = reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                            else
                                FullName = "-";

                            if (Convert.ToInt32((reader["NationalCode"].ToString().Length)) > 0)
                                NationalCode = reader["NationalCode"].ToString();
                            else
                                NationalCode = "-";

                            if (Convert.ToInt32((reader["EngineNumber"].ToString().Length)) > 0)
                                EngineNumber = reader["EngineNumber"].ToString();
                            else
                                EngineNumber = "-";

                            if (Convert.ToInt32((reader["ChassisNumber"].ToString().Length)) > 0)
                                ChassisNumber = reader["ChassisNumber"].ToString();
                            else
                                ChassisNumber = "-";

                            MobileNumber = reader["MobileNumber"].ToString();
                            Plate = "ایران " + reader["IranNumberPlate"].ToString() + " - " + reader["RightNumberPlate"].ToString() + " " + reader["AlphaPlate"].ToString() + " " + reader["LeftNumberPlate"].ToString();                           
                            
                            VIN = reader["VIN"].ToString();
                            InsuranceNumber= reader["InsuranceNumber"].ToString();
                            CylinderBulk = reader["CylinderBulk"].ToString();
                            CylinderSerial = reader["Serial"].ToString();
                            ValveSerial = reader["SerialTankValve"].ToString();
                            //string typeofuse = "";
                            //string vehicleType = "";
                            //if (!string.IsNullOrEmpty(TypeofUse))
                            //    typeofuse = Helper.Helpers.GetTypeofUse(Convert.ToInt32(TypeofUse)).Type;
                            //if (!string.IsNullOrEmpty(VehicleType))
                            //    vehicleType = Helper.Helpers.GetVehicleType(Convert.ToInt32(VehicleType)).Type;
                            rowNum += 1;
                            //
                            vehicleregistration.Add(new VehicleRegistrationList
                            {
                                ID = ID,
                                rowNum=rowNum.ToString(),
                                Date= CreateDate,
                                ChassisNumber = ChassisNumber,
                                TypeofUse = TypeofUse,
                                WorkshopTitle = WorkshopTitle.Replace("مرکز خدمات CNG", " کارگاه"),// WorkshopTitle, //Helper.Helpers.GetWorkshops(Convert.ToInt32(WorkshopTitle)).Title,
                                EngineNumber = EngineNumber,
                                FullName = FullName,
                                NationalCode = NationalCode,
                                MobileNumber = MobileNumber,
                                Plate = Plate,
                                VehicleType = VehicleType,
                                VIN = VIN,
                                //InsuranceNumber = cngfapco.Controllers.VehicleRegistrationsController.GetInsuranceCode(Convert.ToInt32(ID))
                                InsuranceNumber= InsuranceNumber,
                                Details= "1",
                                CylinderBulk = CylinderBulk,
                                CylinderNumber = CylinderSerial,
                                ValveNumber = ValveSerial

                            });
                        }

                        conn.Close();
                    }
                }
                catch { }


            ViewBag.vehicleregistration = vehicleregistration;
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return Json(new { data = vehicleregistration, MaxJsonLength = 86753090}, JsonRequestBehavior.AllowGet);
        }
        public class VehicleRegistrationList
        {
            public string ID { get; set; }
            public string rowNum { get; set; }
            public DateTime? CreateDate { get; set; }
            public string Date { get; set; }
            public string WorkshopTitle { get; set; }
            public string VehicleType { get; set; }
            public string TypeofUse { get; set; }
            public string FullName { get; set; }
            public string NationalCode { get; set; }
            public string MobileNumber { get; set; }
            public string Plate { get; set; }
            public string EngineNumber { get; set; }
            public string ChassisNumber { get; set; }
            public string VIN { get; set; }
            public string  InsuranceNumber { get; set; }
            public string Details { get; set; }
            public string SerialKit { get; set; }
            public string KitConstractor { get; set; }
            public string CylinderNumber { get; set; }
            public string CylinderCostractor { get; set; }
            public string CylinderBulk { get; set; }
            public string ValveNumber { get; set; }
            public string ValveCostractor { get; set; }
            public string SerialCutofValve { get; set; }
            public string CutofValveConstractor { get; set; }
            public string SerialFillingValve { get; set; }
            public string FillingValveConstractor { get; set; }
            public string RegistrationType { get; set; }
            public string RegistrationTypeID { get; set; }
        }
        /// <summary>
        /// مشاهده لیست کسری پیوست های خودروهای ثبت شده
        /// </summary>
        /// <returns></returns>
        // GET: VehicleRegistrations VehicleRegistrationDefects
        [HttpPost]
        public ActionResult VehicleRegistrationDefects(string Workshop, DateTime fromDate, DateTime toDate, bool? Post,int? workshopId,int? InvoiceCode,string RegistrationTypeID)
        {
            RegistrationTypeID = "1";
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string VehicleTypes = "";

            foreach (var item in Vehicle)
            {
                VehicleTypes += item.ID + ",";
            };
            string permission = db.tbl_Workshops.Where(w => w.Title.Equals(Workshop)).SingleOrDefault().ID.ToString();
            if (workshopId != null)
                workshopId = Convert.ToInt32(permission);
            ViewBag.workshopId = permission;

            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string Type = "";
            string ID = "";
            string WorkshopTitle = "";
            string FullName = "";
            string CreateDate = "";
            string ChassisNumber = "";
            string NationalCard = "";
            string VehicleCard = "";
            string LicenseImage = "";
            string HealthCertificate = "";
            string TechnicalDiagnosis = "";
            //
            int isExistNationalCard = 0;
            int isExistHealthCertificate = 0;
            int isExistLicenseImage = 0;
            int isExistVehicleCard = 0;
            //
            //List<VehicleType> vehicletype = new List<VehicleType>();
            List<VehicleRegistrations> vehicleregistration = new List<VehicleRegistrations>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrations]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@VehicleTypeID", SqlDbType.VarChar).Value = VehicleTypes.TrimEnd(',');
                cmd.Parameters.Add("@RegistrationTypeID", SqlDbType.VarChar).Value = RegistrationTypeID;

                conn.Open();
                reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                    
                //}
                reader.NextResult();
                while (reader.Read())
                {
                    ID = reader["ID"].ToString();
                    Type = reader["Type"].ToString() +" - "+ reader["vtDescription"].ToString();
                    WorkshopTitle = reader["Title"].ToString();
                    CreateDate = reader["CreateDate"].ToString();
                    FullName= reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                    ChassisNumber = reader["ChassisNumber"].ToString();
                    NationalCard = reader["N_NationalCard"].ToString();
                    if (NationalCard.Equals("noExist"))
                        isExistNationalCard += 1;
                    VehicleCard = reader["N_VehicleCard"].ToString();
                    if (VehicleCard.Equals("noExist"))
                        isExistVehicleCard += 1;
                    //FuelCard = reader["FuelCard"].ToString();
                    //VIN = reader["VIN"].ToString();
                    LicenseImage = reader["N_LicenseImage"].ToString();
                    if (LicenseImage.Equals("noExist"))
                        isExistLicenseImage += 1;
                    HealthCertificate = reader["N_HealthCertificate"].ToString();
                    if (HealthCertificate.Equals("noExist"))
                        isExistHealthCertificate += 1;
                    TechnicalDiagnosis = reader["N_TechnicalDiagnosis"].ToString();

                    //
                    vehicleregistration.Add(new VehicleRegistrations
                    {
                        ID=ID,
                        Type=Type,
                        WorkshopTitle=WorkshopTitle,
                        CreateDate=CreateDate,
                        FullName=FullName,
                        ChassisNumber=ChassisNumber,
                        NationalCard = NationalCard,
                        VehicleCard=VehicleCard,
                        LicenseImage=LicenseImage,
                        HealthCertificate=HealthCertificate,
                        TechnicalDiagnosis= TechnicalDiagnosis
                    });
                }

                conn.Close();
            }
            ViewBag.vehicleregistration = vehicleregistration;
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            //
            ViewBag.isExistAttachments = isExistNationalCard + isExistHealthCertificate + isExistLicenseImage + isExistVehicleCard;
            ViewBag.InvoiceCode = InvoiceCode;
            //
            return View();
        }

        //
        public ActionResult VehicleRegistrationDefects2(string Workshop, DateTime fromDate, DateTime toDate, bool? Post, string RegistrationTypeID)
        {
            RegistrationTypeID = "1";
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string VehicleTypes = "";

            foreach (var item in Vehicle)
            {
                VehicleTypes += item.ID + ",";
            };
            string permission = db.tbl_Workshops.Where(w => w.Title.Equals(Workshop)).SingleOrDefault().ID.ToString();
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string Type = "";
            string Checked = "";
            string ID = "";
            string WorkshopTitle = "";
            string FullName = "";
            string CreateDate = "";
            string ChassisNumber = "";
            string NationalCard = "";
            string VehicleCard = "";
            string LicenseImage = "";
            string HealthCertificate = "";
            //
            //List<VehicleType> vehicletype = new List<VehicleType>();
            List<VehicleRegistrations> vehicleregistration = new List<VehicleRegistrations>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrations]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@VehicleTypeID", SqlDbType.VarChar).Value = VehicleTypes.TrimEnd(',');
                cmd.Parameters.Add("@RegistrationTypeID", SqlDbType.VarChar).Value = RegistrationTypeID;

                conn.Open();
                reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{

                //}
                reader.NextResult();
                while (reader.Read())
                {
                    ID = reader["ID"].ToString();
                    Checked = reader["Checked"].ToString();
                    Type = reader["Type"].ToString() + " - " + reader["vtDescription"].ToString();
                    WorkshopTitle = reader["Title"].ToString();
                    CreateDate = reader["CreateDate"].ToString();
                    FullName = reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                    ChassisNumber = reader["ChassisNumber"].ToString();
                    NationalCard = reader["N_NationalCard"].ToString();
                    VehicleCard = reader["N_VehicleCard"].ToString();
                    //FuelCard = reader["FuelCard"].ToString();
                    //VIN = reader["VIN"].ToString();
                    LicenseImage = reader["N_LicenseImage"].ToString();
                    HealthCertificate = reader["N_HealthCertificate"].ToString();

                    //
                    vehicleregistration.Add(new VehicleRegistrations
                    {
                        ID=ID,
                        Type = Type,
                        Checked= Checked,
                        WorkshopTitle = WorkshopTitle,
                        CreateDate = CreateDate,
                        FullName = FullName,
                        ChassisNumber = ChassisNumber,
                        NationalCard = NationalCard,
                        VehicleCard = VehicleCard,
                        LicenseImage = LicenseImage,
                        HealthCertificate = HealthCertificate
                    });
                }

                conn.Close();
            }
            ViewBag.vehicleregistration = vehicleregistration;
            //
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            //

            return View();
        }

        //
        public class VehicleRegistrations
        {
            public string ID { get; set; }
            public string Checked { get; set; }
            public string Type { get; set; }
            public string CreateDate { get; set; }
            public string WorkshopTitle { get; set; }
            public string FullName { get; set; }
            public string ChassisNumber { get; set; }
            public string VehicleCard { get; set; }
            public string LicenseImage { get; set; }
            public string HealthCertificate { get; set; }
            public string NationalCode { get; set; }
            public string NationalCard { get; set; }
            public string FuelCard { get; set; }
            public string VIN { get; set; }
            public string TechnicalDiagnosis { get; set; }
        }
        //       
        public ActionResult CreateSection1(string Type)
        {
            ViewBag.Type = Type;
            var vehicleTypes = db.tbl_VehicleTypes.ToList();
            List<VehicleType> type = new List<VehicleType>();
            foreach(var item in vehicleTypes)
            {
                type.Add(new VehicleType
                {
                    Type = item.Type + " " + item.Description,
                    ID=item.ID
                });
            }
            ViewBag.VehicleTypeID = new SelectList(type, "ID", "Type");
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type");
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            ViewBag.GenerationofRegulatorID = new SelectList(db.tbl_GenerationofRegulators.Where(g=>g.ID != 5), "ID", "Title",1);

            return View();
        }

        //for type 2 tarhe taavize ghataat
        public ActionResult CreateSection1_Type2(string Type)
        {
            ViewBag.Type = Type;
            var vehicleTypes = db.tbl_VehicleTypes.ToList();
            List<VehicleType> type = new List<VehicleType>();
            foreach (var item in vehicleTypes)
            {
                type.Add(new VehicleType
                {
                    Type = item.Type + " " + item.Description,
                    ID = item.ID
                });
            }
            ViewBag.VehicleTypeID = new SelectList(type, "ID", "Type");
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type",1);
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            ViewBag.GenerationofRegulatorID = new SelectList(db.tbl_GenerationofRegulators.Where(g => g.ID != 5), "ID", "Title", 1);

            return View();
        }
        public JsonResult SaveSection1_Old(int? VehicleTypeID,string System, int? TypeofUseID, string OwnerName, 
                                                string OwnerFamily, string NationalCode, string PhoneNumber, string MobileNumber,
                                                string Address,string ConstructionYear, string LeftNumberPlate, string AlphaPlate,
                                                string RightNumberPlate,string IranNumberPlate, string VIN, string EngineNumber, string ChassisNumber, string FuelCard)
        {
            FuelCard = "00";//1402-05-20 is changed
            Insurance insurance = new Insurance();
            var insuranceNumber = db.tbl_Insurances.OrderByDescending(i => i.ID).Take(1).SingleOrDefault();
            var checkChassisNumber = db.tbl_VehicleRegistrations.Where(v => v.ChassisNumber.Trim().Replace(" ", "").Equals(ChassisNumber.Trim().Replace(" ",""))).ToList();
            VehicleRegistration section1 = new VehicleRegistration();
            int? redirectId = null;
            string status = "error";            
            //
            try
            {
                if (checkChassisNumber.Count() > 0)
                    ViewBag.message = "خودرو با این شماره شاسی تکراری می باشد!";
                else
                {
                    int? WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                    int? regcount = db.tbl_VehicleRegistrations.Where(V=>V.WorkshopID == WorkshopID).Count();
                    string FapCode = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().FapCode;
                    string RegisterUniqueCode = "";
                    string Year = pc.GetYear(DateTime.Now).ToString();
                    string Month = pc.GetMonth(DateTime.Now).ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;
                    string YearMonth = Year + Month;
                    int len = regcount.HasValue?regcount.Value.ToString().Length:1;
                    if (len == 1)
                        RegisterUniqueCode = FapCode + YearMonth + "0000" + (regcount + 1);
                    if (len == 2)
                        RegisterUniqueCode = FapCode + YearMonth + "000" + (regcount + 1);
                    if (len == 3)
                        RegisterUniqueCode = FapCode + YearMonth + "00" + (regcount + 1);
                    if (len == 4)
                        RegisterUniqueCode = FapCode + YearMonth + "0" + (regcount + 1);
                    if (len == 5)
                        RegisterUniqueCode = FapCode + YearMonth + (regcount + 1);

                    if (section1.InstallationStatus == null)
                        section1.InstallationStatus = false;
                    section1.Address = Address;
                    section1.VIN = VIN.ToUpper();
                    section1.AlphaPlate = AlphaPlate;
                    section1.RightNumberPlate = RightNumberPlate;
                    section1.LeftNumberPlate = LeftNumberPlate;
                    section1.IranNumberPlate = IranNumberPlate;
                    section1.ChassisNumber = ChassisNumber.ToUpper();
                    section1.ConstructionYear = ConstructionYear;
                    section1.CreateDate = DateTime.Now;
                    section1.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                    section1.WorkshopID = WorkshopID;
                    section1.CreatorIPAddress = IP;
                    section1.EngineNumber = EngineNumber.ToUpper();
                    section1.FuelCard = FuelCard;
                    section1.System = System;
                    section1.VehicleTypeID = VehicleTypeID;
                    section1.TypeofUseID = TypeofUseID;
                    section1.OwnerName = OwnerName;
                    section1.OwnerFamily = OwnerFamily;
                    section1.NationalCode = NationalCode;
                    section1.PhoneNumber = PhoneNumber;
                    section1.MobileNumber = MobileNumber;
                    section1.RegisterStatus = true;
                    section1.RegisterStatusDate = DateTime.Now;
                    section1.RegisterStatusUser = User.Identity.Name;
                    section1.RegisterUniqueCode = RegisterUniqueCode;
                    db.tbl_VehicleRegistrations.Add(section1);
                    db.SaveChanges();
                    ViewBag.status = "disabled";
                    //
                    insurance.CreateDate = DateTime.Now;
                    if (insuranceNumber == null)
                        insurance.Number = "100";
                    else
                        insurance.Number = (Convert.ToDouble(insuranceNumber.Number) + 1).ToString();

                    insurance.Creator = User.Identity.Name;
                    insurance.VehicleRegistrationID = section1.ID;
                    db.tbl_Insurances.Add(insurance);
                    db.SaveChanges();
                    redirectId = section1.ID;
                    status = "success";
                    //                    
                }

            }
            catch
            {
                var vehicleTypes = db.tbl_VehicleTypes.ToList();
                List<VehicleType> type = new List<VehicleType>();
                foreach (var item in vehicleTypes)
                {
                    type.Add(new VehicleType
                    {
                        Type = item.Type + " " + item.Description,
                        ID = item.ID
                    });
                }
                ViewBag.VehicleTypeID = new SelectList(type, "ID", "Type",VehicleTypeID);
                ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type",TypeofUseID);
                status = "error";
                //return View(section1);
            }
            //
            return Json(new { response = status, redirectId = redirectId }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DetailsSection1",new { id = redirectId});
        }
        //
        [HttpPost]
        public JsonResult SaveSection1(int? VehicleTypeID, string System, int? TypeofUseID, string OwnerName,
                                                string OwnerFamily, string NationalCode, string PhoneNumber, string MobileNumber,
                                                string Address, string ConstructionYear, string LeftNumberPlate, string AlphaPlate,
                                                string RightNumberPlate, string IranNumberPlate, string VIN, string EngineNumber, 
                                                string ChassisNumber, string FuelCard, string BulkId, string Serial,
                                                string SerialTankValve, string RegulatorSerial, string CutofValveSerial,
                                                string FillingValveSerial, string FuelRelaySerial, string GasECUSerial, string GenarationID,string RegistrationType)
        {
            string message = "";
            int? redirectId = null;
            string status = "error";

            #region(for registartion type 1- tarhe tabdil cng )
            if(RegistrationType=="1")
            {
                var checkExist = db.tbl_VehicleRegistrations.Where(v => v.ChassisNumber.Trim().Replace(" ", "").Equals(ChassisNumber.Trim().Replace(" ", "")) && v.EngineNumber.Trim().Replace(" ", "").Equals(EngineNumber.Trim().Replace(" ", "")) && v.VIN.Trim().Replace(" ", "").Equals(VIN.Trim().Replace(" ", ""))).ToList();
                if (checkExist.Count == 0)
                {

                    if (String.IsNullOrEmpty(FuelCard))
                    FuelCard = "0000-0000-0000-0000";
                    double? bulkId = Convert.ToDouble(BulkId);
                    #region(insert value for vehicle_14020714)
                    Insurance insurance = new Insurance();
                    var insuranceNumber = db.tbl_Insurances.OrderByDescending(i => i.ID).Take(1).SingleOrDefault();
                    VehicleRegistration section1 = new VehicleRegistration();
                    int? WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                    int? regcount = db.tbl_VehicleRegistrations.Where(V => V.WorkshopID == WorkshopID).Count();
                    string FapCode = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().FapCode;
                    string RegisterUniqueCode = "";
                    string Year = pc.GetYear(DateTime.Now).ToString();
                    string Month = pc.GetMonth(DateTime.Now).ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;
                    string YearMonth = Year + Month;
                    int len = regcount.HasValue ? regcount.Value.ToString().Length : 1;
                    if (len == 1)
                        RegisterUniqueCode = FapCode + YearMonth + "0000" + (regcount + 1);
                    if (len == 2)
                        RegisterUniqueCode = FapCode + YearMonth + "000" + (regcount + 1);
                    if (len == 3)
                        RegisterUniqueCode = FapCode + YearMonth + "00" + (regcount + 1);
                    if (len == 4)
                        RegisterUniqueCode = FapCode + YearMonth + "0" + (regcount + 1);
                    if (len == 5)
                        RegisterUniqueCode = FapCode + YearMonth + (regcount + 1);

                    if (section1.InstallationStatus == null)
                        section1.InstallationStatus = false;
                    section1.Address = Address;
                    section1.VIN = VIN.ToUpper();
                    section1.AlphaPlate = AlphaPlate;
                    section1.RightNumberPlate = RightNumberPlate;
                    section1.LeftNumberPlate = LeftNumberPlate;
                    section1.IranNumberPlate = IranNumberPlate;
                    section1.ChassisNumber = ChassisNumber.ToUpper();
                    section1.ConstructionYear = ConstructionYear;
                    section1.CreateDate = DateTime.Now;
                    section1.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                    section1.WorkshopID = WorkshopID;
                    section1.CreatorIPAddress = IP;
                    section1.EngineNumber = EngineNumber.ToUpper();
                    section1.FuelCard = FuelCard;
                    section1.System = System;
                    section1.VehicleTypeID = VehicleTypeID;
                    section1.TypeofUseID = TypeofUseID;
                    section1.OwnerName = OwnerName;
                    section1.OwnerFamily = OwnerFamily;
                    section1.NationalCode = NationalCode;
                    section1.PhoneNumber = PhoneNumber;
                    section1.MobileNumber = MobileNumber;
                    section1.RegisterStatus = true;
                    section1.RegisterStatusDate = DateTime.Now;
                    section1.RegisterStatusUser = User.Identity.Name;
                    section1.RegisterUniqueCode = RegisterUniqueCode;
                    section1.RegistrationTypeID = Convert.ToInt32(RegistrationType);

                    #endregion
                    #region(insert value for vehicle cylinder and otehr parts_14020714)
                    VehicleTank section2 = new VehicleTank();

                    section2.CreateDate = DateTime.Now;
                    section2.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                    section2.ExpirationDate = "----";// ExpirationYear + "-" + ExpirationMonth;
                    section2.ProductDate = "----";// ProductYear + "-" + ProductMonth;
                    section2.RegulatorConstractorID = null;// RegulatorConstractorID;
                    section2.RegulatorSerial = RegulatorSerial.ToUpper();
                    section2.Serial = Serial.ToUpper();
                    section2.SerialTankValve = SerialTankValve.ToUpper();
                    section2.TankConstractorID = null;// TankConstractorID;
                    section2.TypeTankValve = "دستی";// TypeTankValve;
                    section2.ValveConstractorID = 1;// ValveConstractorID;
                    section2.Volume = bulkId.Value;
                    section2.CutofValveConstractorID = null;// CutofValveConstractorID;
                    section2.CutofValveSerial = CutofValveSerial.ToUpper();
                    section2.FillingValveConstractorID = null;// FillingValveConstractorID;
                    section2.FillingValveSerial = FillingValveSerial.ToUpper();
                    section2.FuelRelayConstractorID = null;// FuelRelayConstractorID;
                    section2.FuelRelaySerial = FuelRelaySerial.ToUpper();
                    section2.GasECUConstractorID = null;// GasECUConstractorID;
                    section2.GasECUSerial = GasECUSerial.ToUpper();
                    section2.GenarationID = Convert.ToInt32(GenarationID);
                    // after true all data section2 save section1
                    db.tbl_VehicleRegistrations.Add(section1);
                    db.SaveChanges();
                    ViewBag.status = "disabled";
                    message = "اطلاعات خودرو در طرح تبدیل دوگانه سوز با موفقیت ذخیره شد.";
                    // after true all data section2 save section1 then save section2
                    section2.VehicleRegistrationID = section1.ID;
                    db.tbl_VehicleTanks.Add(section2);
                    db.SaveChanges();
                    message = message + "," + "اطلاعات سریال مخزن و متعلقات با موفقیت ذخیره شد.";
                    #endregion
                    //
                    //
                    insurance.CreateDate = DateTime.Now;
                    if (insuranceNumber == null)
                        insurance.Number = "100";
                    else
                        insurance.Number = (Convert.ToDouble(insuranceNumber.Number) + 1).ToString();

                    insurance.Creator = User.Identity.Name;
                    insurance.VehicleRegistrationID = section1.ID;
                    db.tbl_Insurances.Add(insurance);
                    db.SaveChanges();
                    //
                    message += message + "," + "بیمه نامه صادر شد.";
                    redirectId = section1.ID;
                    status = "success";

                }
                else
                {
                    message = "خودرو با این شماره شاسی یا موتور و یا vin تکراری می باشد!";
                }
            }
            #endregion

            #region(for registartion type 2-tarhe taavize ghataat )
            if (RegistrationType == "2")
            {
                if (String.IsNullOrEmpty(FuelCard))
                    FuelCard = "0000-0000-0000-0000";
                double? bulkId = Convert.ToDouble(BulkId);
                #region(insert value for vehicle_14020714)
                Insurance insurance = new Insurance();
                var insuranceNumber = db.tbl_Insurances.OrderByDescending(i => i.ID).Take(1).SingleOrDefault();
                VehicleRegistration section1 = new VehicleRegistration();
                int? WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                int? regcount = db.tbl_VehicleRegistrations.Where(V => V.WorkshopID == WorkshopID).Count();
                string FapCode = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().FapCode;
                string RegisterUniqueCode = "";
                string Year = pc.GetYear(DateTime.Now).ToString();
                string Month = pc.GetMonth(DateTime.Now).ToString();
                if (Month.Length == 1)
                    Month = "0" + Month;
                string YearMonth = Year + Month;
                int len = regcount.HasValue ? regcount.Value.ToString().Length : 1;
                if (len == 1)
                    RegisterUniqueCode = FapCode + YearMonth + "0000" + (regcount + 1);
                if (len == 2)
                    RegisterUniqueCode = FapCode + YearMonth + "000" + (regcount + 1);
                if (len == 3)
                    RegisterUniqueCode = FapCode + YearMonth + "00" + (regcount + 1);
                if (len == 4)
                    RegisterUniqueCode = FapCode + YearMonth + "0" + (regcount + 1);
                if (len == 5)
                    RegisterUniqueCode = FapCode + YearMonth + (regcount + 1);

                if (section1.InstallationStatus == null)
                    section1.InstallationStatus = false;
                section1.Address = Address;
                section1.VIN = VIN.ToUpper();
                section1.AlphaPlate = AlphaPlate;
                section1.RightNumberPlate = RightNumberPlate;
                section1.LeftNumberPlate = LeftNumberPlate;
                section1.IranNumberPlate = IranNumberPlate;
                section1.ChassisNumber = ChassisNumber.ToUpper();
                section1.ConstructionYear = ConstructionYear;
                section1.CreateDate = DateTime.Now;
                section1.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                section1.WorkshopID = WorkshopID;
                section1.CreatorIPAddress = IP;
                section1.EngineNumber = EngineNumber.ToUpper();
                section1.FuelCard = FuelCard;
                section1.System = System;
                section1.VehicleTypeID = VehicleTypeID;
                section1.TypeofUseID = TypeofUseID;
                section1.OwnerName = OwnerName;
                section1.OwnerFamily = OwnerFamily;
                section1.NationalCode = NationalCode;
                section1.PhoneNumber = PhoneNumber;
                section1.MobileNumber = MobileNumber;
                section1.RegisterStatus = true;
                section1.RegisterStatusDate = DateTime.Now;
                section1.RegisterStatusUser = User.Identity.Name;
                section1.RegisterUniqueCode = RegisterUniqueCode;
                section1.RegistrationTypeID = Convert.ToInt32(RegistrationType);

                #endregion
                #region(insert value for vehicle cylinder and otehr parts_14020714)
                VehicleTank section2 = new VehicleTank();

                section2.CreateDate = DateTime.Now;
                section2.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                section2.ExpirationDate = "----";// ExpirationYear + "-" + ExpirationMonth;
                section2.ProductDate = "----";// ProductYear + "-" + ProductMonth;
                section2.RegulatorConstractorID = null;// RegulatorConstractorID;
                section2.RegulatorSerial = RegulatorSerial.ToUpper();
                section2.Serial = Serial.ToUpper();
                section2.SerialTankValve = SerialTankValve.ToUpper();
                section2.TankConstractorID = null;// TankConstractorID;
                section2.TypeTankValve = "دستی";// TypeTankValve;
                section2.ValveConstractorID = 1;// ValveConstractorID;
                section2.Volume = bulkId.Value;
                section2.CutofValveConstractorID = null;// CutofValveConstractorID;
                section2.CutofValveSerial = CutofValveSerial.ToUpper();
                section2.FillingValveConstractorID = null;// FillingValveConstractorID;
                section2.FillingValveSerial = FillingValveSerial.ToUpper();
                section2.FuelRelayConstractorID = null;// FuelRelayConstractorID;
                section2.FuelRelaySerial = FuelRelaySerial.ToUpper();
                section2.GasECUConstractorID = null;// GasECUConstractorID;
                section2.GasECUSerial = GasECUSerial.ToUpper();
                section2.GenarationID = Convert.ToInt32(GenarationID);
                // after true all data section2 save section1
                db.tbl_VehicleRegistrations.Add(section1);
                db.SaveChanges();
                ViewBag.status = "disabled";
                message = "اطلاعات خودرو در طرح تعویض قطعات با موفقیت ذخیره شد.";
                // after true all data section2 save section1 then save section2
                section2.VehicleRegistrationID = section1.ID;
                db.tbl_VehicleTanks.Add(section2);
                db.SaveChanges();
                message = message + "," + "اطلاعات سریال مخزن و متعلقات با موفقیت ذخیره شد.";
                #endregion
                //
                //
                insurance.CreateDate = DateTime.Now;
                if (insuranceNumber == null)
                    insurance.Number = "100";
                else
                    insurance.Number = (Convert.ToDouble(insuranceNumber.Number) + 1).ToString();

                insurance.Creator = User.Identity.Name;
                insurance.VehicleRegistrationID = section1.ID;
                db.tbl_Insurances.Add(insurance);
                db.SaveChanges();
                //
                message += message + "," + "بیمه نامه صادر شد.";
                redirectId = section1.ID;
                status = "success";
            }
            #endregion
            //
            return Json (new { response = status, message= message, redirectId = redirectId }, JsonRequestBehavior.AllowGet);
            //RedirectToAction ("DetailsSection1", new { response = status, id = redirectId });
        }
        //
        //
        public ActionResult DetailsSection1(int? id, bool? Goback, int? existvehicleTank)
        {
            //ViewBag.existvehicleTank = existvehicleTank;
            //ViewBag.icon = "icon-chevron-up";
            //ViewBag.style = "display:none";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            var cylinderBulk = db.tbl_TypeofTanks.Where(c => c.VehicleTypeId == vehicleRegistration.VehicleTypeID).ToList();
            if (cylinderBulk == null)
            {
                return HttpNotFound();
            }
            var existCylinder = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == id).ToList();
            if (existCylinder.Count() > 0)
                ViewBag.existCylinder = true;
            else
                ViewBag.existCylinder = false;

            ViewBag.existvehicleTank = cylinderBulk.SingleOrDefault().ID;
            ViewBag.cylinderBulk = cylinderBulk.SingleOrDefault().Type;
            ViewBag.cylindertypeId = cylinderBulk.SingleOrDefault().ID;
            ViewBag.selectedVehicleId = id;// vehicleRegistration.ID;

            //if (Goback == null)
            //    Goback = false;
            ViewBag.Goback = Goback;

            return View(vehicleRegistration);
        }
        //
        public ActionResult CreateSection2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.selectedVehicleId = vehicleRegistration.ID;

            var cylinderBulk = db.tbl_TypeofTanks.Where(c => c.VehicleTypeId == vehicleRegistration.VehicleTypeID).ToList();
            if (cylinderBulk == null)
            {
                return HttpNotFound();
            }

            ViewBag.cylinderBulk = cylinderBulk.SingleOrDefault().Type;
            ViewBag.cylindertypeId = cylinderBulk.SingleOrDefault().ID;
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
            ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve");
            ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
            ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator");
            ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay");
            ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU");

            return View();
        }
        //
        public JsonResult SaveSection2(int? VehicleRegistrationID, double? cylindertypeId, string Serial, int? TankConstractorID, string ProductMonth,
                                               string ProductYear, string ExpirationMonth, string ExpirationYear, string SerialTankValve,
                                               string TypeTankValve, int? ValveConstractorID, string RegulatorSerial, int? RegulatorConstractorID,
                                               string CutofValveSerial, int? CutofValveConstractorID, string FillingValveSerial, int? FillingValveConstractorID,
                                               string FuelRelaySerial, int? FuelRelayConstractorID, string GasECUSerial, int? GasECUConstractorID, int? GenarationID)
        {
            var vehicleCylinder = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == VehicleRegistrationID);
            bool checkedExist = false;
            if (vehicleCylinder.Count() > 0)
                checkedExist = true;

            VehicleTank section2 = new VehicleTank();
            int? redirectId = null;
            string status = "error";
            //
            try
            {
                if (checkedExist == false)
                {
                    section2.VehicleRegistrationID = VehicleRegistrationID;
                    section2.CreateDate = DateTime.Now;
                    section2.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                    section2.ExpirationDate = ExpirationYear + "-" + ExpirationMonth;
                    section2.ProductDate = ProductYear + "-" + ProductMonth;
                    section2.RegulatorConstractorID = RegulatorConstractorID;
                    section2.RegulatorSerial = RegulatorSerial.ToUpper();
                    section2.Serial = Serial.ToUpper();
                    section2.SerialTankValve = SerialTankValve.ToUpper();
                    section2.TankConstractorID = TankConstractorID;
                    section2.TypeTankValve = TypeTankValve;
                    section2.ValveConstractorID = ValveConstractorID;
                    section2.Volume = cylindertypeId.Value;
                    section2.CutofValveConstractorID = CutofValveConstractorID;
                    section2.CutofValveSerial = CutofValveSerial.ToUpper();
                    section2.FillingValveConstractorID = FillingValveConstractorID;
                    section2.FillingValveSerial = FillingValveSerial.ToUpper();
                    section2.FuelRelayConstractorID = FuelRelayConstractorID;
                    section2.FuelRelaySerial = FuelRelaySerial.ToUpper();
                    section2.GasECUConstractorID = GasECUConstractorID;
                    section2.GasECUSerial = GasECUSerial.ToUpper();
                    section2.GenarationID = GenarationID;

                    db.tbl_VehicleTanks.Add(section2);
                    db.SaveChanges();
                    status = "success";
                    //
                    redirectId = VehicleRegistrationID;
                    //
                }
                else
                {
                    status = "اطلاعات قبلا در سیستم ثبت شده است!";
                }
            }
            catch
            {
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", ValveConstractorID);
                ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve", CutofValveConstractorID);
                ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve", FillingValveConstractorID);
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", TankConstractorID);
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator", RegulatorConstractorID);
                ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay",FuelRelayConstractorID);
                ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU", GasECUConstractorID);
                status = "error";
                //return View(section2);
            }
            //
            return Json(new { response = status, redirectId = redirectId }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DetailsSection2", new { id = redirectId });
        }
        //
        public ActionResult DetailsSection2(int? id, bool? Goback, int? existvehicleTank)
        {
            ViewBag.existvehicleTank = existvehicleTank;
            ViewBag.icon = "icon-chevron-up";
            ViewBag.style = "display:none";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.selectedVehicleId = vehicleRegistration.ID;
            if (Goback == null)
                Goback = false;
            ViewBag.Goback = Goback;

            return View(vehicleRegistration);
        }
        //
        public ActionResult Section2(int? id, bool? Goback, string icon, string style)
        {
            ViewBag.icon = icon;
            ViewBag.style = style;

            //if (Goback == true)
            //{
            //    ViewBag.icon = "icon-chevron-up";
            //    ViewBag.style = "display:none";
            //}
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            ViewBag.Bulk = "";
            var vehicleRegistration = db.tbl_VehicleTanks.Where(v=>v.VehicleRegistrationID==id).ToList();
            if (vehicleRegistration.Count()>0)
            {
                int bulkId = Convert.ToInt32(vehicleRegistration.SingleOrDefault().Volume);
                ViewBag.Bulk = db.tbl_TypeofTanks.Where(t => t.ID == bulkId).SingleOrDefault().Type + " " + "لیتری";
                //return HttpNotFound();
            }

            ViewBag.selectedVehicleId = id;
           
            return PartialView(vehicleRegistration.SingleOrDefault());
        }
        //
        public ActionResult CreateSection3(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.selectedVehicleId = vehicleRegistration.ID;
            
            return View();
        }
        //
        public JsonResult SaveSection3(int? VehicleRegistrationID, string SerialSparkPreview, string SerialKey, string RefuelingLable, string SerialRefuelingValve, 
                                        string TrackingCode, string License, bool? InstallationStatus, string Description)
        {
            string status = "";
            VehicleRegistration section3 = db.tbl_VehicleRegistrations.Find(VehicleRegistrationID);
            //
            try
            {
                section3.SerialKey = SerialKey.ToUpper();
                section3.SerialSparkPreview = SerialSparkPreview.ToUpper();
                section3.RefuelingLable = RefuelingLable.ToUpper();
                section3.SerialRefuelingValve = SerialRefuelingValve.ToUpper();
                section3.TrackingCode = TrackingCode.ToUpper();
                section3.License = License.ToUpper();
                if (InstallationStatus == null)
                    InstallationStatus = false;
                section3.Description = Description;

                db.Entry(section3).State = EntityState.Modified;
                db.SaveChanges();
                status = "success";
                //
            }
            catch
            {
                status = "error";
                //return View(section3);
            }
            //
            var existvehicleTank = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == VehicleRegistrationID).ToList();
            return Json(new { response = status, redirectId = VehicleRegistrationID, existvehicleTank = existvehicleTank.Count() }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DetailsSection3", new { id = VehicleRegistrationID, icon= "icon-chevron-down",style= "", existvehicleTank= existvehicleTank.Count() });
        }
        //
        public ActionResult DetailsSection3(int? id, string icon, string style, int? existvehicleTank,string Type)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var workshopId = db.tbl_Users.Where(u => u.UserID == userId).SingleOrDefault().WorkshopID;
            var existInRegisteredList = db.tbl_VehicleRegistrations.Where(v => v.ID == id);
            //int limitedUser = 0;
            List<Workshop> list = new List<Workshop>();

            foreach (var item in existInRegisteredList)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                User _user = db.tbl_Users.Find(userId);

                if (workshop.Users.Contains(_user))
                {
                    list.Add(new Workshop
                    {
                        ID = item.ID
                    });
                }

            };

            if (list.Count()==0)
                return RedirectToAction("Page403", "Home");
            //
            bool userRole = false;
            string Role = Helper.Helpers.GetCurrentUserRole();
            if (Role.Equals("کارشناس ناظر") || Role.Equals("مدیر تبدیل ناوگان"))
                userRole = true;
            ViewBag.userRole = userRole;
            //if (Role.Equals("کارشناس ناظر"))
            //    limitedUser = 1;
            //ViewBag.limitedUser = limitedUser;
            //
            var rolId = db.tbl_Roles.Where(r => r.IsSysAdmin == true).ToList();
            foreach (var item in rolId)
            {
                var itemuserId = item.Users.FirstOrDefault().UserID;

                if (itemuserId == userId)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    if (role.Users.Contains(user))
                    {
                        ViewBag.isAdmin = true;
                    }
                    else
                    {
                        ViewBag.isAdmin = false;
                    }
                }

            }
            //
            var existvehicleTanks = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == id).ToList();
            ViewBag.existvehicleTank = existvehicleTanks.Count();
            if (existvehicleTank != null)
                ViewBag.existvehicleTank = existvehicleTank;
            ViewBag.icon = icon;
            ViewBag.style = style;            

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Type = Type;
            ViewBag.selectedVehicleId = vehicleRegistration.ID;
            return View(vehicleRegistration);
        }
       //
        public ActionResult EditSection3(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.selectedVehicleId = vehicleRegistration.ID;

            return View(vehicleRegistration);
        }
        //
        [HttpPost]
        public ActionResult EditSection3(int? VehicleRegistrationID, string SerialSparkPreview, string SerialKey, string RefuelingLable, string SerialRefuelingValve,
                                        string TrackingCode, string License, bool? InstallationStatus, string Description)
        {
            VehicleRegistration section3 = db.tbl_VehicleRegistrations.Find(VehicleRegistrationID);
            //
            try
            {
                section3.SerialKey = SerialKey.ToUpper();
                section3.SerialSparkPreview = SerialSparkPreview.ToUpper();
                section3.RefuelingLable = RefuelingLable.ToUpper();
                section3.SerialRefuelingValve = SerialRefuelingValve.ToUpper();
                section3.TrackingCode = TrackingCode.ToUpper();
                section3.License = License.ToUpper();
                if (InstallationStatus == null)
                    InstallationStatus = false;
                section3.Description = Description;

                db.Entry(section3).State = EntityState.Modified;
                db.SaveChanges();
                //
            }
            catch
            {
                return View(section3);
            }
            //

            return RedirectToAction("DetailsSection3", new { id = VehicleRegistrationID , Goback = true , icon = "icon-chevron-up", style = "display:none" });
        }
        //
        public ActionResult EditSection2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            ViewBag.selectedVehicleId = id;//vehicleRegistration.ID;
            //
            var cylinderBulk = db.tbl_TypeofTanks.Where(c => c.VehicleTypeId == vehicleRegistration.VehicleTypeID).ToList();
            if (cylinderBulk == null)
            {
                return HttpNotFound();
            }

            ViewBag.cylinderBulk = cylinderBulk.SingleOrDefault().Type;
            ViewBag.cylindertypeId = cylinderBulk.SingleOrDefault().ID;

            var checkVehicleTank = db.tbl_VehicleTanks.Where(v => v.VehicleRegistrationID == id).ToList();
            if (checkVehicleTank.Count() > 0)
            {
                int? selectedRowId = checkVehicleTank.SingleOrDefault().ID;
                VehicleTank vehicleTank = db.tbl_VehicleTanks.Find(selectedRowId);
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", vehicleTank.ValveConstractorID);
                ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve", vehicleTank.CutofValveConstractorID);
                ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve", vehicleTank.FillingValveConstractorID);
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", vehicleTank.TankConstractorID);
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator", vehicleTank.RegulatorConstractorID);
                ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay",vehicleTank.FuelRelayConstractorID);
                ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU",vehicleTank.GasECUConstractorID);
                ViewBag.existvehicleTank = true;
                return View(vehicleTank);
            }
            else
            {
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");
                ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve");
                ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve");
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator");
                ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay");
                ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU");
                ViewBag.existvehicleTank = false;
                return View();
            }           
            
        }
        //
        [HttpPost]
        public ActionResult EditSection2(int? VehicleRegistrationID, double? cylindertypeId, string Serial, int? TankConstractorID, string ProductMonth,
                                               string ProductYear, string ExpirationMonth, string ExpirationYear, string SerialTankValve,
                                               string TypeTankValve, int? ValveConstractorID, string RegulatorSerial, int? RegulatorConstractorID, 
                                               string CutofValveSerial,int? CutofValveConstractorID, string FillingValveSerial, int? FillingValveConstractorID,
                                               string FuelRelaySerial, int? FuelRelayConstractorID, string GasECUSerial, int? GasECUConstractorID, int? GenarationID)
        {
            VehicleTank section2 = db.tbl_VehicleTanks.Where(v=>v.VehicleRegistrationID==VehicleRegistrationID).SingleOrDefault();
            int? redirectId = null;
            //
            try
            {
                section2.CreateDate = DateTime.Now;
                section2.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                section2.ExpirationDate = ExpirationYear + "-" + ExpirationMonth;
                section2.ProductDate = ProductYear + "-" + ProductMonth;
                section2.RegulatorConstractorID = RegulatorConstractorID;
                section2.RegulatorSerial = RegulatorSerial.ToUpper();
                section2.Serial = Serial.ToUpper();
                section2.SerialTankValve = SerialTankValve.ToUpper();
                section2.TankConstractorID = TankConstractorID;
                section2.TypeTankValve = TypeTankValve;
                section2.ValveConstractorID = ValveConstractorID;
                section2.Volume = cylindertypeId.Value;
                section2.FillingValveConstractorID = FillingValveConstractorID;
                section2.FillingValveSerial = FillingValveSerial.ToUpper();
                section2.CutofValveConstractorID = CutofValveConstractorID;
                section2.CutofValveSerial = CutofValveSerial.ToUpper();
                section2.FuelRelaySerial = FuelRelaySerial.ToUpper();
                section2.FuelRelayConstractorID = FuelRelayConstractorID;
                section2.GasECUSerial = GasECUSerial.ToUpper();
                section2.GasECUConstractorID = GasECUConstractorID;
                section2.GenarationID = GenarationID;

                db.Entry(section2).State = EntityState.Modified;
                db.SaveChanges();
                //
                redirectId = VehicleRegistrationID;
                //
            }
            catch
            {
                ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", ValveConstractorID);
                ViewBag.CutofValveConstractorID = new SelectList(db.tbl_CutofValveConstractors, "ID", "CutofValve");
                ViewBag.FillingValveConstractorID = new SelectList(db.tbl_FillingValveConstractors, "ID", "FillingValve");
                ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", TankConstractorID);
                ViewBag.RegulatorConstractorID = new SelectList(db.tbl_RegulatorConstractors, "ID", "Regulator", RegulatorConstractorID);
                ViewBag.FuelRelayConstractorID = new SelectList(db.tbl_FuelRelayConstractors, "ID", "FuelRelay", FuelRelayConstractorID);
                ViewBag.GasECUConstractorID = new SelectList(db.tbl_GasECUConstractors, "ID", "GasECU", GasECUConstractorID);
                return View(section2);
            }
            //

            return RedirectToAction("DetailsSection2", new { id = redirectId, Goback = true });
        }
        //
        public ActionResult EditSection1(int? id, int? existvehicleTank)
        {
            //ViewBag.existvehicleTank = existvehicleTank;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleRegistration vehicleRegistration = db.tbl_VehicleRegistrations.Find(id);
            if (vehicleRegistration == null)
            {
                return HttpNotFound();
            }

            var vehicleTypes = db.tbl_VehicleTypes.ToList();
            List<VehicleType> type = new List<VehicleType>();
            foreach (var item in vehicleTypes)
            {
                type.Add(new VehicleType
                {
                    Type = item.Type + " " + item.Description,
                    ID = item.ID
                });
            }

            ViewBag.selectedVehicleId = id;
            ViewBag.VehicleTypeID = new SelectList(type, "ID", "Type", vehicleRegistration.VehicleTypeID);
            ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", vehicleRegistration.TypeofUseID);
            
            return View(vehicleRegistration);
        }

        [HttpPost]
        public ActionResult EditSection1(int? id,int? VehicleTypeID, string System, int? TypeofUseID, string OwnerName,
                                                string OwnerFamily, string NationalCode, string PhoneNumber, string MobileNumber,
                                                string Address, string ConstructionYear, string LeftNumberPlate, string AlphaPlate,
                                                string RightNumberPlate, string IranNumberPlate, string VIN, string EngineNumber, string ChassisNumber, string FuelCard)
        {           
            VehicleRegistration section1 = db.tbl_VehicleRegistrations.Find(id);
            //
            try
            {
                if (section1.InstallationStatus == null)
                    section1.InstallationStatus = false;
                section1.Address = Address;
                section1.VIN = VIN.ToUpper();
                section1.AlphaPlate = AlphaPlate;
                section1.RightNumberPlate = RightNumberPlate;
                section1.LeftNumberPlate = LeftNumberPlate;
                section1.IranNumberPlate = IranNumberPlate;
                section1.ChassisNumber = ChassisNumber.ToUpper();
                section1.ConstructionYear = ConstructionYear;
                //section1.CreateDate = DateTime.Now;
                //section1.Creator = cngfapco.Helper.Helpers.GetCurrentUserId();
                //section1.WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                //section1.CreatorIPAddress = IP;
                section1.EngineNumber = EngineNumber.ToUpper();
                section1.FuelCard = FuelCard;
                section1.System = System;
                section1.VehicleTypeID = VehicleTypeID;
                section1.TypeofUseID = TypeofUseID;
                section1.OwnerName = OwnerName;
                section1.OwnerFamily = OwnerFamily;
                section1.NationalCode = NationalCode;
                section1.PhoneNumber = PhoneNumber;
                section1.MobileNumber = MobileNumber;
                section1.EditDate = DateTime.Now;
                section1.Editor = cngfapco.Helper.Helpers.GetCurrentUserId();
                section1.EditorIPAddress = IP;

                db.Entry(section1).State = EntityState.Modified;
                db.SaveChanges();
                //                   
            }
            catch
            {
                var vehicleTypes = db.tbl_VehicleTypes.ToList();
                List<VehicleType> type = new List<VehicleType>();
                foreach (var item in vehicleTypes)
                {
                    type.Add(new VehicleType
                    {
                        Type = item.Type + " " + item.Description,
                        ID = item.ID
                    });
                }
                ViewBag.VehicleTypeID = new SelectList(type, "ID", "Type", VehicleTypeID);
                ViewBag.TypeofUseID = new SelectList(db.tbl_TypeofUses, "ID", "Type", TypeofUseID);
                return View(section1);
            }
            //

            return RedirectToAction("DetailsSection1", new { id = id, Goback=true});
        }
        //
        [HttpGet]
        public ActionResult EditPermit(int? id)
        {
            var vehicle = db.tbl_VehicleRegistrations.Where(u => u.ID == id).Include(u => u.Workshop).SingleOrDefault();
            return PartialView(vehicle);
        }
        //
        [HttpPost]
        public ActionResult EditPermit(int? id, bool Status)
        {
            VehicleRegistration vehicle = db.tbl_VehicleRegistrations.Where(u => u.ID == id).SingleOrDefault();

            vehicle.Status = Status;

            db.Entry(vehicle).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DetailsSection3", new { id=id});
        }
        //
        [HttpGet]
        public ActionResult EditRegisterStatus(int? id)
        {
            var vehicle = db.tbl_VehicleRegistrations.Where(u => u.ID == id).Include(u => u.Workshop).SingleOrDefault();
            return PartialView(vehicle);
        }
        //
        [HttpPost]
        public ActionResult EditRegisterStatus(int? id, bool Status)
        {
            VehicleRegistration vehicle = db.tbl_VehicleRegistrations.Where(u => u.ID == id).SingleOrDefault();

            vehicle.RegisterStatus = Status;
            vehicle.RegisterStatusDate = DateTime.Now;
            vehicle.RegisterStatusUser = User.Identity.Name;

            db.Entry(vehicle).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DetailsSection3", new { id = id });
        }
        //
        //[HttpPost]
        public ActionResult ConversionCertificate(int? selectedId)
        {
            ViewBag.selectedId = selectedId;
            //
            string WorkshopCode = "";
            string RegisterUniqueCode = "00001";
            string ConversionYear = "";
            string ConversionMonth = "";
            string CertificateNumber = " ";
            string ConversionDate = "";
            string OwnerFullName = " ";
            string NationalCode = " ";
            string OwnerMobile = " ";
            string VehicleType = " ";
            string TypeofUse = " ";
            string ConstructionYear = " ";
            string RightNumberPlate = " ";
            string LeftNumberPlate = " ";
            string AlphaPlate = " ";
            string IranNumberPlate = " ";
            string EngineNumber = " ";
            string ChassisNumber = " ";
            string VIN = " ";
            string CylinderConstractor = " ";
            string CylinderSerialNumber = " ";
            string CylinderType = " ";
            string CylinderBulk = " ";
            string CylinderPressure = " ";
            string CylinderExpireDate = " ";
            string ValveConstractor = " ";
            string ValveSerialNumber = " ";
            string RegulatorConstractor = " ";
            string RegulatorSerialNumber = " ";
            string FillingValveConstractor = " ";
            string FillingValveSerialNumber = " ";
            string CutoffValveConstractor = " ";
            string CutoffValveSerialNumber = " ";
            string FuelRelayConstractor = " ";
            string FuelRelaySerialNumber = " ";
            string GasECUConstractor = " ";
            string GasECUSerialNumber = " ";
            string Workshop = " ";
            string WorkshopOwner = " ";
            string WorkshopOwnerMobile = " ";
            string WorkshopAddress = " ";
            string WorkshopPhone = " ";
            string rezve = " ";
            string RegulatorGeneration = " ";
            string RegulatorModel = " ";
            string RegulatorType = " ";
            string ValveType = " ";
            string ValveModel = " ";
            string ValveRezve = " ";
            string FillingValveModel = " ";
            string CutoffValveModel = " ";
            string RegistrationType = "";
            //
            List<ConversionCertificateList> conversioncertificate = new List<ConversionCertificateList>();
            //
            if (!string.IsNullOrEmpty(selectedId.ToString()))
            {
                SqlDataReader reader;
                var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("[dbo].[sp_VehicleConversionCertificate]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@selectedId", SqlDbType.VarChar).Value = selectedId;

                    conn.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        WorkshopCode = reader["FapCode"].ToString();
                        ConversionYear = pc.GetYear(Convert.ToDateTime(reader["CreateDate"].ToString())).ToString();
                        ConversionMonth = pc.GetMonth(Convert.ToDateTime(reader["CreateDate"].ToString())).ToString();
                        //CertificateNumber = WorkshopCode + ConversionYear + ConversionMonth + MaxRegistrationCount;
                        ConversionDate = Convert.ToDateTime(reader["CreateDate"].ToString()).ToShortDateString();
                        OwnerFullName = reader["OwnerName"].ToString() + " " + reader["OwnerFamily"].ToString();
                        NationalCode = reader["NationalCode"].ToString();
                        OwnerMobile = reader["MobileNumber"].ToString();
                        VehicleType = reader["VehicleType"].ToString();
                        TypeofUse = reader["TypeofUse"].ToString();
                        ConstructionYear = reader["ConstructionYear"].ToString();
                        //Plate = "ایران " + reader["IranNumberPlate"].ToString() + " | " + reader["RightNumberPlate"].ToString() + " " + reader["AlphaPlate"].ToString() + " " + reader["LeftNumberPlate"].ToString();
                        RightNumberPlate = reader["RightNumberPlate"].ToString();
                        LeftNumberPlate = reader["LeftNumberPlate"].ToString();
                        IranNumberPlate = reader["IranNumberPlate"].ToString();
                        AlphaPlate = reader["AlphaPlate"].ToString();
                        EngineNumber = reader["EngineNumber"].ToString();
                        ChassisNumber = reader["ChassisNumber"].ToString();
                        VIN = reader["VIN"].ToString();
                        CylinderConstractor = reader["CylinderConstractor"].ToString();
                        CylinderSerialNumber = reader["CylinderSerialNumber"].ToString();
                        CylinderType = reader["CylinderType"].ToString();
                        CylinderBulk = reader["CylinderBulk"].ToString();
                        CylinderPressure = reader["CylinderPressure"].ToString();
                        CylinderExpireDate = reader["CylinderExpireDate"].ToString();
                        ValveConstractor = reader["ValveConstractor"].ToString();
                        ValveSerialNumber = reader["ValveSerialNumber"].ToString();
                        RegulatorConstractor = reader["RegulatorConstractor"].ToString();
                        RegulatorSerialNumber = reader["RegulatorSerialNumber"].ToString();
                        FillingValveConstractor = reader["FillingValveConstractor"].ToString();
                        FillingValveSerialNumber = reader["FillingValveSerialNumber"].ToString();
                        CutoffValveConstractor = reader["CutoffValveConstractor"].ToString();
                        CutoffValveSerialNumber = reader["CutoffValveSerialNumber"].ToString();
                        FuelRelayConstractor = " ";
                        FuelRelaySerialNumber = " ";
                        GasECUConstractor = " ";
                        GasECUSerialNumber = " ";
                        Workshop = reader["Workshop"].ToString();
                        WorkshopOwner = reader["WOwnerName"].ToString() + " " + reader["WOwnerFamily"].ToString();
                        WorkshopOwnerMobile = reader["WMobileNumber"].ToString();
                        WorkshopAddress = reader["WAddress"].ToString();
                        WorkshopPhone = reader["WPhoneNumber"].ToString();
                        rezve = reader["rezve"].ToString();
                        RegulatorGeneration = reader["RegulatorGeneration"].ToString();
                        RegulatorType = reader["RegulatorType"].ToString();
                        ValveModel = reader["ValveModel"].ToString();
                        ValveType = reader["ValveType"].ToString();
                        ValveRezve = reader["ValveRezve"].ToString();
                        FillingValveModel = reader["FillingValveModel"].ToString();
                        CutoffValveModel = reader["CutoffValveModel"].ToString();
                        RegisterUniqueCode = reader["RegisterUniqueCode"].ToString();
                        RegistrationType = reader["RegistrationType"].ToString();
                    }
                    conversioncertificate.Add(new ConversionCertificateList
                    {
                        CertificateNumber = CertificateNumber,
                        ConversionDate = ConversionDate,
                        OwnerFullName = OwnerFullName,
                        NationalCode = NationalCode,
                        OwnerMobile = OwnerMobile,
                        VehicleType = VehicleType,
                        TypeofUse = TypeofUse,
                        ConstructionYear = ConstructionYear,
                        AlphaPlate = AlphaPlate,
                        IranNumberPlate = IranNumberPlate,
                        LeftNumberPlate = LeftNumberPlate,
                        RightNumberPlate = RightNumberPlate,
                        EngineNumber = EngineNumber,
                        ChassisNumber = ChassisNumber,
                        VIN = VIN,
                        CylinderConstractor = CylinderConstractor,
                        CylinderSerialNumber = CylinderSerialNumber,
                        CylinderType = CylinderType,
                        CylinderBulk = CylinderBulk,
                        CylinderPressure = CylinderPressure,
                        CylinderExpireDate = CylinderExpireDate,
                        ValveConstractor = ValveConstractor,
                        ValveSerialNumber = ValveSerialNumber,
                        RegulatorConstractor = RegulatorConstractor,
                        RegulatorSerialNumber = RegulatorSerialNumber,
                        FillingValveConstractor = FillingValveConstractor,
                        FillingValveSerialNumber = FillingValveSerialNumber,
                        CutoffValveConstractor = CutoffValveConstractor,
                        CutoffValveSerialNumber = CutoffValveSerialNumber,
                        FuelRelayConstractor = FuelRelayConstractor,
                        FuelRelaySerialNumber = FuelRelaySerialNumber,
                        GasECUConstractor = GasECUConstractor,
                        GasECUSerialNumber = GasECUSerialNumber,
                        Workshop = Workshop,
                        WorkshopOwner = WorkshopOwner,
                        WorkshopOwnerMobile = WorkshopOwnerMobile,
                        WorkshopAddress = WorkshopAddress,
                        WorkshopPhone = WorkshopPhone,
                        rezve = rezve,
                        RegulatorType = RegulatorType,
                        RegulatorGeneration = RegulatorGeneration,
                        ValveType = ValveType,
                        ValveModel = ValveModel,
                        ValveRezve = ValveRezve,
                        CutoffValveModel = CutoffValveModel,
                        FillingValveModel = FillingValveModel,
                        RegisterUniqueCode = RegisterUniqueCode,
                        RegistrationType = RegistrationType
                    });
                    conn.Close();
                }
                //
            }
            //
            ViewBag.tableOut = conversioncertificate;
            if(conversioncertificate.Count()>0)
                ViewBag.RegistrationType = conversioncertificate.SingleOrDefault().RegistrationType;

            return View();
        }
        //
        public class ConversionCertificateList
        {
            public string CertificateNumber { get; set; }
            public string ConversionDate { get; set; }
            public string OwnerFullName { get; set; }
            public string NationalCode { get; set; }
            public string OwnerMobile { get; set; }
            public string VehicleType { get; set; }
            public string TypeofUse { get; set; }
            public string ConstructionYear { get; set; }
            public string RightNumberPlate { get; set; }
            public string LeftNumberPlate { get; set; }
            public string AlphaPlate { get; set; }
            public string IranNumberPlate { get; set; }
            public string EngineNumber { get; set; }
            public string ChassisNumber { get; set; }
            public string VIN { get; set; }
            public string CylinderConstractor { get; set; }
            public string CylinderSerialNumber { get; set; }
            public string CylinderType { get; set; }
            public string CylinderBulk { get; set; }
            public string CylinderPressure { get; set; }
            public string CylinderExpireDate { get; set; }
            public string ValveConstractor { get; set; }
            public string ValveSerialNumber { get; set; }
            public string RegulatorConstractor { get; set; }
            public string RegulatorSerialNumber { get; set; }
            public string FillingValveConstractor { get; set; }
            public string FillingValveSerialNumber { get; set; }
            public string CutoffValveConstractor { get; set; }
            public string CutoffValveSerialNumber { get; set; }
            public string FuelRelayConstractor { get; set; }
            public string FuelRelaySerialNumber { get; set; }
            public string GasECUConstractor { get; set; }
            public string GasECUSerialNumber { get; set; }
            public string Workshop { get; set; }
            public string WorkshopOwner { get; set; }
            public string WorkshopOwnerMobile { get; set; }
            public string WorkshopAddress { get; set; }
            public string WorkshopPhone { get; set; }
            public string rezve { get; set; }
            public string RegulatorGeneration { get; set; }
            public string RegulatorModel { get; set; }
            public string RegulatorType { get; set; }
            public string ValveModel { get; set; }
            public string ValveType { get; set; }
            public string ValveRezve { get; set; }
            public string FillingValveModel { get; set; }
            public string CutoffValveModel { get; set; }
            public string RegisterUniqueCode { get; set; }
            public string RegistrationType { get; set; }
        }
        //
        public ActionResult PrintConversionCertificate(int? selectedId)
        {
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("ConversionCertificate", new { selectedId = selectedId })
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
            };
            //return report;
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
