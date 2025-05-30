using cngfapco.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cngfapco.Controllers
{
    [Authorize]
    [RBACAttribute.NoCache]
    public class SMSPanelController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();

        //
        public ActionResult sendOption(string option)
        {
            ViewBag.sendOption = option;
            return PartialView();
        }
        // GET: SMSPanel
        public ActionResult Index(string[] WorkshopID, string[] VehicleType, DateTime fromDate, DateTime toDate, bool? Post, string count)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            List<VehicleType> vehicleType = new List<VehicleType>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            string VehicleTypes = "";            
            //
            foreach (var item in workshops)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }

            };

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

                ViewBag.selectedWorkshopId = permission.TrimEnd(',');
            }

            else
            {
                foreach (var item in WorkshopID)
                {
                    Workshop workshop = db.tbl_Workshops.Find(Convert.ToInt32(item));
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        permission += item + ",";                        
                    }
                };

                ViewBag.selectedWorkshopId = permission.TrimEnd(',');
            }
            //
            foreach (var item in Vehicle)
            {
                vehicleType.Add(new VehicleType
                {
                    ID = item.ID,
                    Type = item.Type + " " + item.Description
                });
            };

            ViewBag.VehicleType = new SelectList(vehicleType, "ID", "Type");
            //
            if (VehicleType == null)
            {
                foreach (var item in Vehicle)
                {
                    VehicleTypes += item.ID + ",";
                };
                ViewBag.Type = VehicleTypes.TrimEnd(',');
            }

            else
            {
                foreach (var item in VehicleType)
                {
                    VehicleTypes += item + ",";
                };
                ViewBag.Type = VehicleTypes.TrimEnd(',');
                //VehicleTypes = VehicleType[0];
                //ViewBag.Type = VehicleTypes.TrimEnd(',');//db.tbl_VehicleTypes.Find(VehicleType).Type + " - " + db.tbl_VehicleTypes.Find(VehicleType).Description;
            }
            //
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View();
        }
        //
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
            public string MobileNumber { get; set; }
            public string Plate { get; set; }
            public string EngineNumber { get; set; }
            public string ChassisNumber { get; set; }
            public string VIN { get; set; }
            public string InsuranceNumber { get; set; }
            public string Details { get; set; }
            public string ConstructionYear { get; set; }
        }
        //
        public JsonResult AddData(string[] WorkshopID, string[] VehicleType, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            List<Workshop> tableOuts = new List<Workshop>();
            List<VehicleType> vehicleType = new List<VehicleType>();
            var workshops = db.tbl_Workshops.ToList();
            var Vehicle = db.tbl_VehicleTypes.ToList();
            string permission = "";
            string VehicleTypes = "";

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
                //foreach (var item in WorkshopID)
                //{
                //    Workshop workshop = db.tbl_Workshops.Find(item);
                //    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                //    if (workshop.Users.Contains(_user))
                //    {
                //        permission += item + ",";
                //    }

                //};
                permission = WorkshopID[0];

            }
            //
            foreach (var item in vehicleType)
            {
                vehicleType.Add(new VehicleType
                {
                    ID = item.ID,
                    Type = item.Type + " " + item.Description
                });
            };

            ViewBag.VehicleType = new SelectList(vehicleType, "ID", "Type");
            //
            if (VehicleType == null)
            {
                foreach (var item in Vehicle)
                {
                    VehicleTypes += item.ID + ",";
                };
                ViewBag.Type = VehicleTypes;
            }

            else
            {
                foreach (var item in VehicleType)
                {
                    VehicleTypes += item + ",";
                };
                ViewBag.Type = VehicleTypes;
                //VehicleTypes = VehicleType[0];
                //ViewBag.Type = VehicleTypes;//db.tbl_VehicleTypes.Find(VehicleType).Type + " - " + db.tbl_VehicleTypes.Find(VehicleType).Description;
            }
            //
            //برای بخش جدول اطلاعات ماشین های تبدیل شده
            string ID = "";
            string WorkshopTitle = "";
            string Type = "";
            string TypeofUse = "";
            string FullName = "";
            string MobileNumber = "";
            //LeftNumberPlate, AlphaPlate, RightNumberPlate, IranNumberPlate, EngineNumber, ChassisNumber,
            string Plate = "";
            string EngineNumber = "";
            string ChassisNumber = "";
            string VIN = "";
            string InsuranceNumber = "";
            string CreateDate = null;
            string ConstructionYear = "";
            int rowNum = 0;
            //
            List<VehicleRegistrationList> vehicleregistration = new List<VehicleRegistrationList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
                try
                {
                    using (var cmd = new SqlCommand("[dbo].[sp_SMStoVehicleRegistrations]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                        cmd.Parameters.Add("@VehicleTypeID", SqlDbType.VarChar).Value = VehicleTypes.TrimEnd(',');
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
                                Type = reader["VehicleType"].ToString();
                            else
                                Type = "-";
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
                            Plate = " ایران " + reader["IranNumberPlate"].ToString() + " | " + reader["RightNumberPlate"].ToString()+ " " + reader["AlphaPlate"].ToString() + " " + reader["LeftNumberPlate"].ToString();

                            VIN = reader["VIN"].ToString();
                            InsuranceNumber = reader["InsuranceNumber"].ToString();
                            ConstructionYear = reader["ConstructionYear"].ToString();
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
                                Date = CreateDate,
                                ChassisNumber = ChassisNumber,
                                TypeofUse = TypeofUse,
                                WorkshopTitle = WorkshopTitle.Replace("مرکز خدمات CNG", " کارگاه"),// WorkshopTitle, //Helper.Helpers.GetWorkshops(Convert.ToInt32(WorkshopTitle)).Title,
                                EngineNumber = EngineNumber,
                                FullName = FullName,
                                MobileNumber = MobileNumber,
                                Plate = Plate,
                                VehicleType = Type,
                                VIN = VIN,
                                //InsuranceNumber = cngfapco.Controllers.VehicleRegistrationsController.GetInsuranceCode(Convert.ToInt32(ID))
                                InsuranceNumber = InsuranceNumber,
                                Details = "1",
                                ConstructionYear = ConstructionYear

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
            return Json(new { data = vehicleregistration, MaxJsonLength = 86753090 }, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpPost]
        public JsonResult SendVehicleSMS(string[] id, string option, string message,string Url)
        {
            string status = "error";
            string statusResponse = "در هنگام ارسال پیام خطا رخ داده است!";
            //
            SMSPanelResult saveResults = new SMSPanelResult();
            if (message.Equals("undefined"))
                message = "";
            if (Url.Equals("undefined"))
                Url = "";

            string sendMessage = "";
            string result = "";
            for(int i=0;i<id.Count();i++)
            {
                int Ids = Convert.ToInt32(id[i]);
                var registerVehicle = db.tbl_VehicleRegistrations.Where(v=> v.RegisterStatus==true && v.ID == Ids).ToList();

                if (registerVehicle.Count() > 0)
                {
                    string plate = "ایران " + registerVehicle.FirstOrDefault().IranNumberPlate + " | " + registerVehicle.FirstOrDefault().RightNumberPlate + " " + registerVehicle.FirstOrDefault().AlphaPlate + " " + registerVehicle.FirstOrDefault().LeftNumberPlate;
                    string mobile = "98" + registerVehicle.FirstOrDefault().MobileNumber.TrimStart('0');
                    string vehicleType = registerVehicle.FirstOrDefault().VehicleType.Type;
                    string workshop = registerVehicle.FirstOrDefault().Workshop.Title;
                    //
                    if (option=="1")
                    {
                        message = "مالک محترم خودروی " + vehicleType + " به شماره " + plate + " تبدیل شده در " + workshop + "- " + "با سلام و سپاس از انتخاب شرکت فن آوران پارسیان برای دوگانه سوز نمودن خودرو خود، لطفا به منظور بهبود کیفیت ارائه خدمات به مشتریان، به سوالات مندرج در لینک ذیل پاسخ دهید. با تشکر- واحد صدای مشتری";
                        sendMessage = message;
                    }
                    //
                    if (option == "2")
                    {
                        message = "مالک محترم خودروی " + vehicleType + " به شماره " + plate + " تبدیل شده در " + workshop + "- " + "با سلام و سپاس از انتخاب شرکت فن آوران پارسیان برای دوگانه سوز نمودن خودرو خود، لطفا در راستای سنجش میزان رضایت مشتریان و ارتقاء سطح کیفی خدمات مراکز نصب CNG، به سوالات مطرح شده در فرم مندرج در لینک ذیل به دقت پاسخ دهید. با تشکر- واحد صدای مشتری";
                        sendMessage = message;
                    }
                    if (!string.IsNullOrEmpty(message))
                        sendMessage = message;
                    //
                    var client = new RestClient("https://new.payamsms.com:443/services/rest/index.php");
                    var crmUrl = ""; //"http://cng.fapco.ir/CRM/Index?id=" + id[i];
                    if (!string.IsNullOrEmpty(Url))
                        if (Url.Contains("http://cng.fapco.ir/CRM/Index?id="))
                            crmUrl = "http://cng.fapco.ir/CRM/Index?id=" + id[i];
                        else
                            crmUrl = Url + id[i];
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\r\n \"organization\": \"fapco\",\r\n \"username\": \"fapco\",\r\n \"password\": \"123654789\",\r\n \"method\": \"send\",\r\n \"messages\": [\r\n {\r\n    \"sender\": \"98200030033\",\r\n    \"recipient\": \"" + mobile + "\",\r\n    \"body\" : \"" + sendMessage + " :" + crmUrl + "\",\r\n    \r\n    \"customerId\":" + id[i] + " \r\n }\r\n ]\r\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    result += response.Content;
                    if (!string.IsNullOrEmpty(result))
                    {
                        statusResponse = result;
                        status = "success";
                        //Begin -----------------------------------save sended sms results ----------------------------------------------------
                        saveResults.FullName = registerVehicle.FirstOrDefault().OwnerName + " " + registerVehicle.FirstOrDefault().OwnerFamily;
                        saveResults.Workshop = registerVehicle.FirstOrDefault().Workshop.ID.ToString();
                        saveResults.Mobile = registerVehicle.FirstOrDefault().MobileNumber;
                        saveResults.refID = registerVehicle.FirstOrDefault().ID.ToString();
                        saveResults.Section = "1";
                        saveResults.SendDate = DateTime.Now;
                        saveResults.Sender = User.Identity.Name;
                        saveResults.SenderIP = Helper.Helpers.GetIPHelper();
                        saveResults.Result = response.Content;
                        db.tbl_SMSPanelResults.Add(saveResults);
                        db.SaveChanges();
                        //End -----------------------------------save sended sms results ----------------------------------------------------
                    }
                }

            }
            //Console.WriteLine(response.Content);
            //return Json(msg=result, JsonRequestBehavior.AllowGet);
            return Json(new { success = status, responseText = statusResponse }, JsonRequestBehavior.AllowGet);
        }
        public static string SendStatus(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var result = dbStatic.tbl_SMSPanelResults.Where(s => s.refID.Equals(id));
                if (result != null)
                    if (result.SingleOrDefault().Result.Contains("{\"code\":200,") == true)
                        return "ارسال موفق";
                    else
                        return "ارسال ناموفق";
                else
                    return "نامشخص";
            }
            else
            {
                return "نامشخص";
            }
        }
        //
        public ActionResult SMSPanelResults()
        {
            var SMSPanelResults = db.tbl_SMSPanelResults.OrderByDescending(s=>s.ID);
            return View(SMSPanelResults.Take(3000).ToList());
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