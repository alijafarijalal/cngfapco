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
    public class ReplacementPlanController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();
        private object _logger;

        // GET: ReplacementPlan/WorkshopPage 
        public ActionResult WorkshopPage(string Year,string Month)
        {
            //if (!string.IsNullOrEmpty(Year))
                ViewBag.Year = Year;
            //else
            //    ViewBag.Year = pc.GetYear(DateTime.Now).ToString() ;
            //
            //if (!string.IsNullOrEmpty(Month))
                ViewBag.Month = Month;
            //else
            //    ViewBag.Month = pc.GetMonth(DateTime.Now).ToString();
            //
            //if (!string.IsNullOrEmpty(Month) && Convert.ToInt32(Month) < 10)
            //{
            //    ViewBag.Month = "0" + Month;
            //}
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

        // GET: ReplacementPlan/WorkshopPageWithFilter 
        public ActionResult WorkshopPageWithFilter(string fromYear, string fromMonth, string toYear, string toMonth)
        {           
            string WorkshopsID = "";
            string WorkshopTitle = "";            
            string Number = "";            
            string TotalAmount = "";
            string ServiceDesc = "";
            string errorMesage = "";
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
                        command.Parameters.Add("@table", SqlDbType.VarChar).Value = "[CNGFAPCO].[dbo].[tbl_InvoicesDamages]";
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
            
            //
            ViewBag.ShowResults = ResultsTable;
            //
            ViewBag.fromYear = fromYear;
            ViewBag.fromMonth = fromMonth;
            ViewBag.fMonth = Helper.Helpers.GetLongMonth(Convert.ToInt32(fromMonth)); ;

            ViewBag.toYear = toYear;
            if(Convert.ToInt32(toMonth) < 10)
                ViewBag.toMonth ="0"+toMonth;
            else
                ViewBag.toMonth = toMonth;
            ViewBag.tMonth = Helper.Helpers.GetLongMonth(Convert.ToInt32(toMonth)); ;
            //
            
            return View();
        }

        private ActionResult StatusCode(int v, object p)
        {
            throw new NotImplementedException();
        }

        private ActionResult BadRequest(object p)
        {
            throw new NotImplementedException();
        }

        // GET: ReplacementPlan
        /// <summary>
        /// ثبت فاکتورفروش اقلام طرح تعویض
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesInvoice(string InvoiceCode, int? WorkshopId)
        {
            string countrystring = "SELECT * FROM[CNGFAPCO].[dbo].[tbl_EquipmentList] where RegistrationTypeID ='2'";
            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]) + '-' + Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            ViewBag.ServicesID = new SelectList(list, "Value", "Text"); //new SelectList(db.tbl_EquipmentList.Where(e=>e.Value>0 || e.Value2>0).OrderBy(e=>e.Title), "ID", "Title");
            ViewBag.WorkshopId = WorkshopId;
            var FapCode = db.tbl_Workshops.Where(w => w.ID == WorkshopId).SingleOrDefault().FapCode;
            int Count = db.tbl_RequestFreeSales.Where(v => v.WorkshopsID == WorkshopId).GroupBy(v => v.InvoiceCode).Count() + 1;
            string MaxofRow = "1";
            //
            if (Count < 10)
                MaxofRow = "000" + Count;
            if (Count >= 10 && Count < 100)
                MaxofRow = "00" + Count;
            if (Count >= 100 && Count < 1000)
                MaxofRow = "0" + Count;
            if (Count >= 1000)
                MaxofRow = Count.ToString();
            //           
            int CurrYear = pc.GetYear(DateTime.Now);

            if (InvoiceCode == null || InvoiceCode == "")
            {
                var invoicelist = db.tbl_RequestFreeSales.OrderByDescending(i => i.InvoiceID).ToList();
                //int maxcode = 1;

                if (invoicelist.Count() > 0)
                {
                    //maxcode = Convert.ToInt32(invoicelist.Take(1).Max(i => i.InvoiceCode)) + 1;
                    //ViewBag.invoiceCode = (maxcode).ToString();
                    ViewBag.invoiceCode = FapCode + "-" + MaxofRow;
                }
                else
                {
                    //ViewBag.invoiceCode = "1";
                    ViewBag.invoiceCode = FapCode + "-" + "0001";
                }

                return View(invoicelist.Where(i => i.InvoiceCode == "1000000000").ToList());
            }
            else
            {
                var invoice = db.tbl_RequestFreeSales.Where(i => i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
                var eid = invoice.OrderByDescending(i => i.InvoiceID).FirstOrDefault().WorkshopsID;
                var coid = invoice.OrderByDescending(i => i.EquipmentsID).FirstOrDefault().EquipmentsID;
                ViewBag.invoiceCode = FapCode + "-" + "0001"; //invoice.FirstOrDefault().InvoiceCode;
                ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                return View(invoice.ToList());
            }
        }

        /// <summary>
        /// ثبت اطلاعات درخواست خرید اقلام فروش طرح تعویض
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SalesInvoice(List<RequestFreeSale> invoices)
        {
            var nasle4 = db.tbl_EquipmentList.Where(e => e.Pid == 82).ToList();
            var Workshop = invoices.FirstOrDefault().WorkshopsID; /*cngfapco.Helper.Helpers.GetWorkshopCurrentUser();*/
            var FapCode = db.tbl_Workshops.Where(w => w.ID == Workshop).SingleOrDefault().FapCode;
            int Count = db.tbl_Invoices.Where(v => v.WorkshopsID == Workshop).GroupBy(v => v.InvoiceCode).Count() + 1;
            string MaxofRow = "1";
            //
            if (Count < 10)
                MaxofRow = "000" + Count;
            if (Count >= 10 && Count < 100)
                MaxofRow = "00" + Count;
            if (Count >= 100 && Count < 1000)
                MaxofRow = "0" + Count;
            if (Count >= 1000)
                MaxofRow = Count.ToString();
            //           
            using (ContextDB entities = new ContextDB())
            {
                string invoiceparam = "";
                int CurrYear = pc.GetYear(DateTime.Now);
                var departmentid = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                //Check for NULL.
                if (invoices == null || invoices.Count == 0)
                {
                    Thread.Sleep(1000);
                    return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var invoicelist = db.tbl_RequestFreeSales.OrderByDescending(i => i.InvoiceID).ToList();
                    var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    //int maxcode = 1;

                    if (invoicelist.Count() > 0)
                    {
                        //maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                        ViewBag.invoiceCode = FapCode + "-" + MaxofRow;// (maxcode).ToString();
                    }
                    else
                    {
                        ViewBag.invoiceCode = FapCode + "-" + "0001";// "1";
                    }

                    RequestFreeSale additem = new RequestFreeSale();
                    //Loop and insert records.

                    foreach (var invoice in invoices)
                    {
                        //for other value of Nasle4 regulator
                        if (invoice.ServiceCode != "82")
                        {
                            if (invoice.EquipmentsID != null)
                            {
                                additem.EquipmentsID = db.tbl_EquipmentList.Where(e => e.FinancialCode.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                            }
                            else
                            {
                                additem.EquipmentsID = null;
                            }
                            //
                            additem.WorkshopsID = Workshop; //db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                            additem.CreatedDate = DateTime.Now;
                            additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                            additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                            if (invoice.ServiceDesc == null)
                                additem.ServiceDesc = "مقداری ثبت نشده!";
                            else
                                additem.ServiceDesc = invoice.ServiceDesc;
                            if (invoice.UnitAmount < 1)
                                additem.UnitAmount = 1;
                            else
                                additem.UnitAmount = Convert.ToDouble(invoice.UnitAmount);
                            if (invoice.Number == 0)
                                additem.Number = 1;
                            else
                            additem.Number = invoice.Number;
                            additem.EmployerEconomicalnumber = invoice.EmployerEconomicalnumber;
                            additem.Employerregistrationnumber = invoice.Employerregistrationnumber;
                            additem.EmployerPostalcode = invoice.EmployerPostalcode;
                            additem.EmployerPhone = invoice.EmployerPhone;
                            additem.EmployerState = invoice.EmployerState;
                            additem.UnitofMeasurement = invoice.UnitofMeasurement;
                            additem.EmployerAddress = invoice.EmployerAddress;
                            additem.EmployerFax = invoice.EmployerFax;
                            additem.Description = invoice.Description;
                            additem.SaleCondition = invoice.SaleCondition;
                            additem.Comment = invoice.Comment;
                            additem.Status = false;
                            additem.ServiceCode = invoice.ServiceCode;
                            additem.Owners = invoice.Owners;
                            additem.FinalStatus = true;
                            if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                                additem.CurrencyTypeID = invoice.CurrencyTypeID;
                            else
                                additem.CurrencyTypeID = 6;
                            //
                            additem.InvoiceCode = FapCode + "-" + MaxofRow;// maxcode.ToString();
                            additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                            additem.RegistrationTypeID = 2;
                            //
                            db.tbl_RequestFreeSales.Add(additem);
                            db.SaveChanges();
                            //
                            invoiceparam = invoice.InvoiceCode;
                        }
                    }
                  
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "فاکتور فروش کالا و خدمات شما با موفقیت ثبت شد!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ReplacementPlanList(int? WorkshopId)
        {
            var resualts = db.tbl_RequestFreeSales.Include(r => r.RegistrationType)
                .Include(r => r.CurrencyType).Include(r => r.Equipments)
                .Include(r => r.RegistrationType).Include(r => r.Workshops)
                .Where(r => r.WorkshopsID == WorkshopId && r.RegistrationTypeID == 2);
            return View(resualts.ToList());
        }
        //
        public ActionResult InvoiceDamagedResultFilterView(string WorkshopId, string Year, string Month)
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
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceDamagedResultFilterView]", conn))
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
                            AmountTaxandComplications=AmountTaxandComplications,
                            CreatedDate=CreatedDate,
                            DiscountAmount=DiscountAmount,
                            Months=Months,
                            Years=Years,
                            Number=Number,
                            ServiceDesc=ServiceDesc,
                            TotalAmount=TotalAmount,
                            TotalAmountafterDiscount=TotalAmountafterDiscount,
                            TotalAmountTaxandComplications=TotalAmountTaxandComplications,
                            UnitAmount=UnitAmount,
                            UnitofMeasurement=UnitofMeasurement                            
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
            public string CreatedDate  { get; set; }
            public string ServiceDesc  { get; set; }
            public string Number  { get; set; }
            public string UnitofMeasurement  { get; set; }
            public string UnitAmount  { get; set; }
            public string TotalAmount  { get; set; }
            public string DiscountAmount  { get; set; }
            public string TotalAmountafterDiscount  { get; set; }
            public string AmountTaxandComplications  { get; set; }
            public string TotalAmountTaxandComplications  { get; set; }
            public string Years  { get; set; }
            public string Months  { get; set; }
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