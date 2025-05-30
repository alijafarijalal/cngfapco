using cngfapco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Data;
using System.Net;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using cngfapco.Helper;
using System.Text.RegularExpressions;
using Stimulsoft.Report.Export;
using System.IO;
using System.Text;
using Stimulsoft.Report.Web;
using Rotativa;
using System.Web.SessionState;

namespace cngfapco.Controllers
{
    public class ReplacementPlanValveController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        // GET: ReplacementPlanValve/WorkshopPage 
        public ActionResult WorkshopPage(string Year,string Month)
        {
            ViewBag.Year = Year;
            ViewBag.Month = Month;
           //
            switch (Month)
            {
                case "01":
                    ViewBag.MonthTitle = "فروردین";
                    break;
                case "02":
                    ViewBag.MonthTitle = "ادیبهشت";
                    break;
                case "03":
                    ViewBag.MonthTitle = "خرداد";
                    break;
                case "04":
                    ViewBag.MonthTitle = "تیر";
                    break;
                case "05":
                    ViewBag.MonthTitle = "مرداد";
                    break;
                case "06":
                    ViewBag.MonthTitle = "شهریور";
                    break;
                case "07":
                    ViewBag.MonthTitle = "مهر";
                    break;
                case "08":
                    ViewBag.MonthTitle = "آبان";
                    break;
                case "09":
                    ViewBag.MonthTitle = "آذر";
                    break;
                case "10":
                    ViewBag.MonthTitle = "دی";
                    break;
                case "11":
                    ViewBag.MonthTitle = "بهمن";
                    break;
                case "12":
                    ViewBag.MonthTitle = "اسفند";
                    break;
                default:
                    ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(ViewBag.Month));
                    break;
            }
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true && w.IRNGVCod != null).ToList();
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
                        City = item.City
                    });
                }

            };
            return View(list.ToList());
        }

        // GET: ReplacementPlanValve/WorkshopPageWithFilter 
        public ActionResult WorkshopPageWithFilter(string fromYear, string fromMonth, string toYear, string toMonth)
        {
            string WorkshopsID = "";
            string WorkshopTitle = "";
            string Number = "";
            string TotalAmount = "";
            string ServiceDesc = "";
            //            
            if (string.IsNullOrEmpty(fromYear))
                fromYear = "1402";
            if (string.IsNullOrEmpty(fromMonth))
                fromMonth = "01";
            if (string.IsNullOrEmpty(toYear))
                toYear = pc.GetYear(DateTime.Now).ToString();
            if (string.IsNullOrEmpty(toMonth))
                toMonth = pc.GetMonth(DateTime.Now).ToString();
            //
            List<ShowResults> ResultsTable = new List<ShowResults>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesDamagesFilterwithfromtoDate]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@table", SqlDbType.VarChar).Value = "[CNGFAPCO].[dbo].[tbl_InvoicesValveDamages]";
                        command.Parameters.Add("@fromYear", SqlDbType.VarChar).Value = fromYear;
                        command.Parameters.Add("@fromMonth", SqlDbType.VarChar).Value = fromMonth;
                        command.Parameters.Add("@toYear", SqlDbType.VarChar).Value = toYear;
                        command.Parameters.Add("@toMonth", SqlDbType.VarChar).Value = toMonth;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            WorkshopsID = reader["WorkshopID"].ToString();
                            ServiceDesc = reader["Owner"].ToString();
                            Number = reader["Number"].ToString();
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]).ToString("#,#");

                            ResultsTable.Add(new ShowResults
                            {
                                WorkshopID = WorkshopsID,
                                WorkshopTitle = WorkshopTitle,
                                Number = Number,
                                TotalAmount = TotalAmount,
                                ServiceDesc = ServiceDesc
                            });
                        }
                        conn.Close();
                    }
                }
                //
                if (ResultsTable.Count() == 0)
                    ViewBag.errorMesage = "در بازه مورد نظر شما داده‌ای برای نمایش وجود ندارد!";
                //
            }
            //end using
            catch (SqlException ex) when (ex.Number == 50000) // خطاهای سفارشی با RAISERROR
            {
                // این کد زمانی اجرا می‌شود که خطای RAISERROR با سطح 16 رخ دهد
                //return BadRequest(new
                //{
                //    Message = ex.Message,
                //    ErrorCode = ex.Number                    
                //});
                ViewBag.errorMesage = ex.Message;
            }
            catch (Exception ex)
            {
                // سایر خطاهای سیستمی
                //_logger.LogError(ex, "خطا در دریافت داده‌ها");
                //return StatusCode(500, new
                //{
                //    Message = "خطای داخلی سرور",
                //    Details = ex.Message                    
                //});
                ViewBag.errorMesage = ex.Message;
            }


            ViewBag.ShowResults = ResultsTable;
            //
            ViewBag.fromYear = fromYear;
            ViewBag.fromMonth = fromMonth;
            ViewBag.fMonth = Helper.Helpers.GetLongMonth(Convert.ToInt32(fromMonth)); ;

            ViewBag.toYear = toYear;
            if (Convert.ToInt32(toMonth) < 10)
                ViewBag.toMonth = "0" + toMonth;
            else
                ViewBag.toMonth = toMonth;
            ViewBag.tMonth = Helper.Helpers.GetLongMonth(Convert.ToInt32(toMonth)); ;
            //
            
            return View();
        }

        public ActionResult InvoiceValveDamagedResultFilterView(string WorkshopId, string Year, string Month)
        {
            //string WorkshopsID = "";
            string WorkshopTitle = "";
            string CreatedDate = "";
            string ServiceDesc = "";
            string Number = "";
            string UnitofMeasurement = "";
            string UnitAmount = "";
            string TotalAmount = "";
            string DiscountAmount = "";
            string TotalAmountafterDiscount = "";
            string AmountTaxandComplications = "";
            string TotalAmountTaxandComplications = "";
            string Years = "";
            string Months = "";
            //
            if (string.IsNullOrEmpty(WorkshopId))
                WorkshopId = "0";
            if (string.IsNullOrEmpty(Year))
                Year = "0";
            if (string.IsNullOrEmpty(Month))
                Month = "0";
            List<ShowResults> ResultsTable = new List<ShowResults>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceValveDamagedResultFilterView]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@workshopId", SqlDbType.VarChar).Value = WorkshopId;
                    command.Parameters.Add("@Year", SqlDbType.VarChar).Value = Year;
                    command.Parameters.Add("@Month", SqlDbType.VarChar).Value = Month;

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        WorkshopTitle = reader["WorkshopTitle"].ToString();
                        CreatedDate = reader["CreatedDate"].ToString();
                        ServiceDesc = reader["ServiceDesc"].ToString();
                        Number = reader["Number"].ToString();
                        UnitofMeasurement = reader["UnitofMeasurement"].ToString();
                        UnitAmount = reader["UnitAmount"].ToString();
                        TotalAmount = reader["TotalAmount"].ToString();
                        DiscountAmount = reader["DiscountAmount"].ToString();
                        TotalAmountafterDiscount = reader["TotalAmountafterDiscount"].ToString();
                        AmountTaxandComplications = reader["AmountTaxandComplications"].ToString();
                        TotalAmountTaxandComplications = reader["TotalAmountTaxandComplications"].ToString();
                        Years = reader["Year"].ToString();
                        Months = reader["Month"].ToString();

                        ResultsTable.Add(new ShowResults
                        {
                            WorkshopTitle = WorkshopTitle,
                            AmountTaxandComplications = AmountTaxandComplications,
                            CreatedDate = CreatedDate,
                            DiscountAmount = DiscountAmount,
                            Months = Months,
                            Years = Years,
                            Number = Number,
                            ServiceDesc = ServiceDesc,
                            TotalAmount = TotalAmount,
                            TotalAmountafterDiscount = TotalAmountafterDiscount,
                            TotalAmountTaxandComplications = TotalAmountTaxandComplications,
                            UnitAmount = UnitAmount,
                            UnitofMeasurement = UnitofMeasurement
                        });
                    }
                    conn.Close();
                }
            }//end using

            ViewBag.ShowResults = ResultsTable;

            //
            return View();
        }
        //
        public class ShowResults
        {
            public string WorkshopID { get; set; }
            public string WorkshopTitle { get; set; }
            public string CreatedDate { get; set; }
            public string ServiceDesc { get; set; }
            public string Number { get; set; }
            public string UnitofMeasurement { get; set; }
            public string UnitAmount { get; set; }
            public string TotalAmount { get; set; }
            public string DiscountAmount { get; set; }
            public string TotalAmountafterDiscount { get; set; }
            public string AmountTaxandComplications { get; set; }
            public string TotalAmountTaxandComplications { get; set; }
            public string Years { get; set; }
            public string Months { get; set; }
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