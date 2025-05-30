using cngfapco.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;
using Highsoft.Web.Mvc.Charts;
using System.Data.SqlClient;
using System.Configuration;
using cngfapco.Helper;

namespace cngfapco.Controllers
{
    [Authorize]
    [RBACAttribute.NoCache]
    public class HomeController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();
        //
        public ActionResult FreeDBCatch()
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_freeproccache]", conn))
                {                    
                    conn.Open();
                    reader = command.ExecuteReader();
                    ViewBag.Status = true;
                }
            }
            //            
            return View();
        }
        public ActionResult Page400()
        {
            return View();
        }
        //
        public ActionResult Page403()
        {
            return View();
        }
        //
        public ActionResult Page404()
        {
            return View();
        }
        //
        public ActionResult Page500()
        {
            return View();
        }

        public ActionResult Page504()
        {
            return View();
        }
        // GET: Home
        public ActionResult Index()
        {
            var userObject = Helpers.GetWorkshopCurrentUser();
            if (userObject == null)
                return RedirectToAction("LogOff", "Users");
            //
            int? userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            ViewBag.unreadmessages = db.tbl_Messages.Where(m => m.ReciverID == userId && m.Priority.Contains("فوری") && (m.ReadStatus == null || m.ReadStatus == false)).Count();

            ViewBag.announce = db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("mainannounce")).OrderBy(s => s.Order).Count();

            ViewBag.divisionplan = db.tbl_DivisionPlans.Where(d => (d.FinalCheck == true) && (d.Confirmation == false || d.Confirmation == null)).Include(d => d.Workshop).OrderByDescending(d => d.CreateDate).Count();

            ViewBag.WarehouseKeeper = db.tbl_DivisionPlans.Where(d => (d.Confirmation == true) && (d.Send == false || d.Send == null)).Include(d => d.Workshop).OrderByDescending(d => d.CreateDate).Count();

            //
            var rolName = cngfapco.Helper.Helpers.GetCurrentUserRole();
            if (rolName.Contains("مرکز خدمات (کارگاه)"))
            {
                var workshopId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().WorkshopID;
                ViewBag.remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null && r.DivisionPlan.Send != false && r.DivisionPlan.WorkshopID == workshopId)).Include(r => r.DivisionPlan).Include(r => r.User).Count();
                var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.WorkshopID == workshopId ).Include(r => r.DivisionPlan).Include(r => r.User);
                int count = 0;

                foreach (var item in tbl_Remittances)
                {
                    if((DateTime.Now - item.CreateDate).Days > 0)
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
            {
                ViewBag.remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null && r.DivisionPlan.Send != false )).Include(r => r.DivisionPlan).Include(r => r.User).Count();
                return View();
            }
        }
        //
        public ActionResult DivisionPlans()
        {
            return View();
        }
        //
        public ActionResult Remittances()
        {
            return View();
        }
        // GET: Home
        public ActionResult Dashboard()
        {
            //var count = db.tbl_VehicleRegistrations.ToList();
            //int countNum = 0;
            //foreach(var item in count)
            //{
            //    Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
            //    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());
            //    if (workshop.Users.Contains(_user))
            //    {
            //        countNum++;
            //    }
            //}
            int workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            string userRole = Helper.Helpers.GetCurrentUserRole();
            if (userRole.Contains("مرکز خدمات (کارگاه)"))
            {
                ViewBag.VehicleCount = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true && (v.RegistrationTypeID == 1 || v.RegistrationTypeID == null) &&  v.WorkshopID == workshopId ).Count();
                ViewBag.VehicleCount2 = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true && v.RegistrationTypeID == 2 && v.WorkshopID == workshopId).Count();
            }
            else
            {
                ViewBag.VehicleCount = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true && (v.RegistrationTypeID == 1 || v.RegistrationTypeID == null)).Count();
                ViewBag.VehicleCount2 = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true && v.RegistrationTypeID == 2).Count();
            }

            ViewBag.divisionCount = db.tbl_DivisionPlans.Count();
            ViewBag.userCount = db.tbl_Users.Where(u=> u.Inactive == true).Count();
            ViewBag.commentCount = db.tbl_Messages.Count();
            ViewBag.workshopCount = db.tbl_Workshops.Where(w=> w.isServices == true).Count();
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Sidebar/5
        public ActionResult Sidebar(int id)
        {
            return PartialView();
        }

        //Get: Home/Block Page
        public ActionResult Lockscreen()
        {
            return View();
        }
        //
        public ActionResult Charts(int?[] WorkshopID, DateTime Date, bool? Post, int? PreDay, int? NextDay,int? RegistrationTypeID)
        {
            if (RegistrationTypeID == null)
                RegistrationTypeID = 1;

            if (RegistrationTypeID!=null)
                ViewBag.RegistrationTypeID = new SelectList(db.tbl_RegistrationTypes, "ID", "Type", RegistrationTypeID);
            else
                ViewBag.RegistrationTypeID = new SelectList(db.tbl_RegistrationTypes, "ID", "Type");

            ViewBag.RegistrationType = RegistrationTypeID;

            if (Post != true)
            {
                ViewBag.Date = DateTime.Now;
                ViewBag.nexDay = "disabled";
                Session["Date"] = ViewBag.Date;
            }
            else
            {
                if (PreDay != null)
                {
                    ViewBag.Date = Date.AddDays(-PreDay.Value);
                    ViewBag.nexDay = "";
                    Session["Date"] = ViewBag.Date;
                }
                else
                {
                    ViewBag.Date = Date.AddDays(NextDay.Value);
                    ViewBag.nexDay = "";
                    if (DateTime.Now.ToShortDateString().Equals(Date.AddDays(NextDay.Value).ToShortDateString()))
                        ViewBag.nexDay = "disabled";
                    Session["Date"] = ViewBag.Date;
                }

            }
            
            double sumValue1 = 0.0;
            double sumValue2 = 0.0;
            List<ChartData> chart = new List<ChartData>();
            List<ChartData> radarchart = new List<ChartData>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationCount_Chart]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = ViewBag.Date;
                cmd.Parameters.Add("@RegistrationTypeID", SqlDbType.NVarChar).Value = RegistrationTypeID;

                conn.Open();
                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    ChartData data = new ChartData();
                    data.Value1 = Convert.ToDouble(reader["Count"].ToString());
                    data.Value2 = Convert.ToDouble(reader["DayCount"].ToString());
                    sumValue2 += data.Value2;
                    data.Category = reader["Title"].ToString();
                    chart.Add(data);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ChartData data = new ChartData();
                    data.Value1 = Convert.ToDouble(reader["Count"].ToString());
                    sumValue1 += data.Value1;
                    data.Value2 = 0;
                    data.Category = reader["Type"].ToString();
                    radarchart.Add(data);
                }
                conn.Close();
            }

            ViewBag.Date = Date.ToShortDateString();
            if (sumValue1 > 0)
                ViewBag.sumValue1 = sumValue1.ToString("#,##");
            else
                ViewBag.sumValue1 = sumValue1;

            if (sumValue2 > 0)
                ViewBag.sumValue2 = sumValue2.ToString("#,##");
            else
                ViewBag.sumValue2 = sumValue2;
            //

            return View(chart);
        }
        //
        public ActionResult ChartAllValues(int?[] WorkshopID, DateTime Date, bool? Post, int? RegistrationTypeID)
        {
            double sumValue2 = 0.0;
            List<ChartData> chart = new List<ChartData>();
            List<ChartData> radarchart = new List<ChartData>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationCount_Chart]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = ViewBag.Date;
                cmd.Parameters.Add("@RegistrationTypeID", SqlDbType.NVarChar).Value = RegistrationTypeID;

                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChartData data = new ChartData();
                    data.Value1 = Convert.ToDouble(reader["Count"].ToString());
                    data.Value2 = Convert.ToDouble(reader["DayCount"].ToString());
                    sumValue2 += data.Value2;
                    data.Category = reader["Title"].ToString();
                    chart.Add(data);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ChartData data = new ChartData();
                    data.Value1 = Convert.ToDouble(reader["Count"].ToString());
                    data.Value2 = 0;
                    data.Category = reader["Type"].ToString();
                    radarchart.Add(data);
                }
                conn.Close();
            }

            ViewBag.Date = Date.ToShortDateString();
            if (sumValue2 > 0)
                ViewBag.sumValue2 = sumValue2.ToString("#,##");
            else
                ViewBag.sumValue2 = sumValue2;
            //

            return View(chart);
        }
        //
        public ActionResult RadarCharts(int?[] WorkshopID, DateTime Date, bool? Post)
        {
            List<ChartData> radarchart = new List<ChartData>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationCount_Chart]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;

                conn.Open();
                reader = cmd.ExecuteReader();
                reader.NextResult();
                while (reader.Read())
                {
                    ChartData data = new ChartData();
                    data.Value1 = Convert.ToDouble(reader["count"].ToString());
                    //data.Value2 = Convert.ToDouble(reader["count"].ToString());
                    data.Category = reader["Type"].ToString();
                    radarchart.Add(data);
                }
                conn.Close();
            }

            ViewBag.Date = Date.ToShortDateString();
            //

            return View(radarchart);
        }
        //
        //
        public ActionResult RadarAreaChart()
        {
            return View(RadarChartData.GetData());
        }
        //
        public ActionResult RadarAreaChart2()
        {
            return View(RadarChartData.GetData2());
        }
        //
        public ActionResult RegistrationOneView()
        {
            var LastModifyDate = db.tbl_ContradictionTotals.OrderByDescending(c => c.ID);
            ViewBag.LastModifyDate = "";
            if (LastModifyDate.Count()>0)
                ViewBag.LastModifyDate = LastModifyDate.FirstOrDefault().CreateDate;
            //
            string ID = "";
            string Title = "";
            string RegistrCount = "";
            string FapCount = "";
            string LisenceCount = "";
            string HealtCount = "";
            string FapPercent = "";
            string LisencePercent = "";
            string HealtPercent = "";
            string HLPercent = "";
            string LisenceDiff = "";
            string HealtDiff = "";
            string FapDiff = "";
            string HLDiff = "";
            string LisenceDiffColor = "";
            string HealtDiffColor = "";
            string FapDiffColor = "";
            string HLDiffColor = "";
            //
            List<OneView> tableOuts = new List<OneView>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_RegistraioninOneView]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission;

                conn.Open();
                reader = cmd.ExecuteReader();                
                while (reader.Read())
                {
                    ID = reader["ID"].ToString();
                    Title = reader["Title"].ToString();
                    RegistrCount = reader["RegistrCount"].ToString();
                    FapCount = reader["FapCount"].ToString();
                    LisenceCount = reader["LisenceCount"].ToString();
                    HealtCount = reader["HealtCount"].ToString();
                    FapPercent = reader["FapPercent"].ToString();
                    LisencePercent = reader["LisencePercent"].ToString();
                    HealtPercent = reader["HealtPercent"].ToString();
                    LisenceDiff = reader["LisenceDiff"].ToString();
                    HealtDiff = reader["HealtDiff"].ToString();
                    LisenceDiffColor = reader["LisenceDiffColor"].ToString();
                    HealtDiffColor = reader["HealtDiffColor"].ToString();
                    FapDiff = reader["FapDiff"].ToString();
                    FapDiffColor = reader["FapDiffColor"].ToString();
                    HLDiff = reader["HLDiff"].ToString();
                    HLDiffColor = reader["HLDiffColor"].ToString();
                    HLPercent = reader["HLPercent"].ToString();
                    //
                    tableOuts.Add(new OneView
                    {
                        ID=ID,
                        HealtPercent=HealtPercent,
                        LisencePercent=LisencePercent,
                        FapPercent=FapPercent,
                        HealtCount=HealtCount,
                        LisenceCount=LisenceCount,
                        FapCount=FapCount,
                        RegistrCount=RegistrCount,
                        Title=Title,
                        HealtDiff=HealtDiff,
                        HealtDiffColor= HealtDiffColor,
                        LisenceDiff=LisenceDiff,
                        LisenceDiffColor=LisenceDiffColor,
                        FapDiff=FapDiff,
                        FapDiffColor=FapDiffColor,
                        HLDiff=HLDiff,
                        HLDiffColor=HLDiffColor,
                        HLPercent = HLPercent
                    });
                }

                conn.Close();
            }
            ViewBag.tableOuts = tableOuts;
            return View();
        }

        public ActionResult UpdateRegistrationInOne()
        {
            var LastModifyDate = db.tbl_ContradictionTotals.OrderByDescending(c => c.ID);
            ViewBag.LastModifyDate = "";
            if (LastModifyDate.Count() > 0)
                ViewBag.LastModifyDate = LastModifyDate.FirstOrDefault().CreateDate;
            //
            string ID = "";
            string Title = "";
            string RegistrCount = "";
            string FapCount = "";
            string LisenceCount = "";
            string HealtCount = "";
            string FapPercent = "";
            string LisencePercent = "";
            string HealtPercent = "";
            //
            List<OneView> tableOuts = new List<OneView>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_RegistraioninOneView]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ID = reader["ID"].ToString();
                    Title = reader["Title"].ToString();
                    RegistrCount = reader["RegistrCount"].ToString();
                    FapCount = reader["FapCount"].ToString();
                    LisenceCount = reader["LisenceCount"].ToString();
                    HealtCount = reader["HealtCount"].ToString();
                    FapPercent = reader["FapPercent"].ToString();
                    LisencePercent = reader["LisencePercent"].ToString();
                    HealtPercent = reader["HealtPercent"].ToString();
                    //
                    tableOuts.Add(new OneView
                    {
                        ID = ID,
                        HealtPercent = HealtPercent,
                        LisencePercent = LisencePercent,
                        FapPercent = FapPercent,
                        HealtCount = HealtCount,
                        LisenceCount = LisenceCount,
                        FapCount = FapCount,
                        RegistrCount = RegistrCount,
                        Title = Title
                    });
                }

                conn.Close();
            }
            ViewBag.tableOuts = tableOuts;
            return View();
        }
        public ActionResult UpdateRegistration(int[] ID, double[] RegistrCount, double[] LisenceCount, double[] HealtCount)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);  
            string query = "TRUNCATE TABLE tbl_ContradictionTotals";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            ContradictionTotal addtoTable = new ContradictionTotal();
            for(int i=0;i<ID.Count();i++)
            {
                if (!string.IsNullOrEmpty(ID[i].ToString()))
                {
                    addtoTable.WorkshopId = ID[i];
                    addtoTable.Count = RegistrCount[i];
                    addtoTable.ContradictionTypeId = 1;
                    addtoTable.Date = DateTime.Now;
                    addtoTable.CreateDate = DateTime.Now;
                    addtoTable.Creator = User.Identity.Name;
                    db.tbl_ContradictionTotals.Add(addtoTable);
                    db.SaveChanges();
                }
                if (!string.IsNullOrEmpty(ID[i].ToString()))
                {
                    addtoTable.WorkshopId = ID[i];
                    addtoTable.Count = LisenceCount[i];
                    addtoTable.ContradictionTypeId = 3;
                    addtoTable.Date = DateTime.Now;
                    addtoTable.CreateDate = DateTime.Now;
                    addtoTable.Creator = User.Identity.Name;
                    db.tbl_ContradictionTotals.Add(addtoTable);
                    db.SaveChanges();
                }
                if (!string.IsNullOrEmpty(ID[i].ToString()))
                {
                    addtoTable.WorkshopId = ID[i];
                    addtoTable.Count = HealtCount[i];
                    addtoTable.ContradictionTypeId = 4;
                    addtoTable.Date = DateTime.Now;
                    addtoTable.CreateDate = DateTime.Now;
                    addtoTable.Creator = User.Identity.Name;
                    db.tbl_ContradictionTotals.Add(addtoTable);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("RegistrationOneView");
        }
        public class OneView
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string RegistrCount { get; set; }
            public string FapCount { get; set; }
            public string LisenceCount { get; set; }
            public string HealtCount { get; set; }
            public string FapPercent { get; set; }
            public string LisencePercent { get; set; }
            public string HealtPercent { get; set; }
            public string HLPercent { get; set; }
            public string LisenceDiff { get; set; }
            public string HealtDiff { get; set; }
            public string FapDiff { get; set; }
            public string HLDiff { get; set; }
            public string LisenceDiffColor { get; set; }
            public string HealtDiffColor { get; set; }
            public string FapDiffColor { get; set; }
            public string HLDiffColor { get; set; }
        }
        //
        public ActionResult Gallery()
        {
            return View();
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
