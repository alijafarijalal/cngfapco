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
using System.ComponentModel;
using System.Data.Common;
using System.Dynamic;

namespace cngfapco.Controllers
{
    [Authorize]
    [RBACAttribute.NoCache]
    //[SessionState(SessionStateBehavior.ReadOnly)]
    public class FinancialsController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        // GET: Financials /WorkshopPage 
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
                        City = item.City,
                        closedServices = item.closedServices,
                        closedDate = item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }

        // GET: Financials /WorkshopPage 
        public ActionResult WorkshopPage2()
        {
            //var workshops = db.tbl_Workshops.Where(w => w.isServices == true);
            //return View(workshops.ToList());
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
                        City = item.City,
                        closedServices = item.closedServices,
                        closedDate = item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }
        //
        public ActionResult WorkshopPage3()
        {
            //var workshops = db.tbl_Workshops.Where(w => w.isServices == true);
            //return View(workshops.ToList());
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
                        City = item.City,
                        closedServices = item.closedServices,
                        closedDate = item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }
        //
        public ActionResult WorkshopPage4()
        {
            //var workshops = db.tbl_Workshops.Where(w => w.isServices == true);
            //return View(workshops.ToList());
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
                        City = item.City,
                        closedServices = item.closedServices,
                        closedDate = item.closedDate
                    });
                }

            };
            return View(list.ToList());
        }
        //
        public ActionResult WorkshopPage5()
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
                        City = item.City,
                        closedServices = item.closedServices,
                        closedDate = item.closedDate
                    });
                }

            };
            
            return View(list.ToList());
        }
        //
        public static string GetInvoiceFapaValue(int? id)
        {
            var value = dbStatic.tbl_InvoicesFapa
                .Where(c => c.WorkshopsID == id)
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmountTaxandComplications.HasValue? c.TotalAmountTaxandComplications.Value:0).ToString("#,##");
            else
                return "0";
        }
        //
        public static string GetInvoiceFapaValueTotal()
        {
            var value = dbStatic.tbl_InvoicesFapa
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
            else
                return "0";
        }
        //
        public static string GetInvoiceValue(int? id)
        {
            var value = dbStatic.tbl_Invoices
                .Where(c => c.WorkshopsID == id)
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmount.HasValue ? c.TotalAmount.Value : 0).ToString("#,##");
            else
                return "0";
        }
        //
        public static string GetInvoicValueTotal()
        {
            var value = dbStatic.tbl_Invoices
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmount.HasValue ? c.TotalAmount.Value : 0).ToString("#,##");
            else
                return "0";
        }
        // GET: Financials
        public ActionResult Index(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            ViewBag.Post = Post;
            ViewBag.workshop = WorkshopID;
            string existOfferedDate = "";
            string existOfferedSerial = "";
            string number = "";
            var OfferedDate = db.tbl_OfferedPrices.OrderByDescending(o => o.Date).ToList();
            if (OfferedDate.Count() > 0)
            {
                existOfferedDate = OfferedDate.FirstOrDefault().Date.ToShortDateString();
                existOfferedSerial = OfferedDate.FirstOrDefault().Serial;
                number = OfferedDate.FirstOrDefault().Number.ToString();
            }
            ViewBag.existOfferedDate = existOfferedDate;
            ViewBag.existOfferedSerial = existOfferedSerial;
            ViewBag.number = number;
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();

            foreach (var item in tbl_Workshops.Where(w => w.isServices == true).ToList())
            {
                Workshop _workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (_workshop.Users.Contains(_user))
                {
                    //permission = item.ID.ToString().ToLower().Split(',');
                    tableOuts.Add(new Workshop
                    {
                        ID = item.ID,
                        Title = item.Title
                    });
                }
            }
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            //string filterItems="";
            //
            if (WorkshopID == null)
                if (cngfapco.Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
                    WorkshopID = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId()).WorkshopID;
                else
                    WorkshopID = 0;

            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            string Type = "";
            int Count = 0;
            double Price = 0.0;
            double Salary = 0.0;
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;
            //
            string PreInvoiceCode = "";
            string WorkshopsID = "";
            string SaleCondition = "";
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            string OfferedSerial = "";
            //
            List<PreInvoicesHint> PreInvoicesHint = new List<PreInvoicesHint>();
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            List<FinancialDebitCredit> DebitCreditOneView = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();

                        //reader = command.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //    ID = reader["ID"].ToString();
                        //    Title = reader["Title"].ToString();
                        //    Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                        //    sumCredit += Creditor;
                        //    Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                        //    sumDebit += Debtor;
                        //    Rem = Convert.ToDouble(reader["Rem"].ToString());
                        //    sumRem += Rem;

                        //    TableDebitCredit.Add(new FinancialDebitCredit
                        //    {
                        //        ID=ID,
                        //        Title=Title,
                        //        Creditor=Creditor,
                        //        Debtor=Debtor,
                        //        Rem=Rem                                
                        //    });
                        //}

                        //111111111111
                        //reader.NextResult();
                        //while (reader.Read())
                        //{                            
                        //    Type = reader["Type"].ToString();
                        //    Count = Convert.ToInt32(reader["Count"].ToString());
                        //    sumCount += Count;
                        //    Price = Convert.ToDouble(reader["Price"].ToString());
                        //    sumPrice += Price;
                        //    Salary = Convert.ToDouble(reader["Salary"].ToString());
                        //    sumSalary += Salary;

                        //    TableOuts.Add(new Models.Financial
                        //    {
                        //        Type=Type,
                        //        Count=Count,
                        //        Price=Price,
                        //        Salary=Salary
                        //    });
                        //}
                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    ID = reader["ID"].ToString();
                        //    Title = reader["Title"].ToString();
                        //    Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                        //    sumTCredit += Creditor;
                        //    Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                        //    sumTDebit += Debtor;
                        //    Rem = Convert.ToDouble(reader["Rem"].ToString());
                        //    sumTRem += Rem;
                        //    NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                        //    SumNonCash += NonCash;
                        //    Amount = Convert.ToDouble(reader["Amount"].ToString());
                            
                        //    Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                        //    SumDeductions += Deductions;
                        //    RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                        //    SumRemWithDeductions += RemWithDeductions;
                        //    NetPercent = reader["NetPercent"].ToString();
                        //    GrossPercent = reader["GrossPercent"].ToString();
                        //    PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                        //    SumPreInvoiceHint += PreInvoiceHint;
                        //    OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                        //    SumOfferedPrice += OfferedPrice;
                        //    OfferedSerial = reader["OfferedSerial"].ToString();

                        //    try
                        //    {
                        //        DebitCreditOneView.Add(new FinancialDebitCredit
                        //        {
                        //            ID = ID,
                        //            Title = Title,
                        //            Creditor = Creditor,
                        //            Debtor = Debtor,
                        //            Rem = Rem,
                        //            NonCash = NonCash,
                        //            Amount = Amount,
                        //            Deductions = Deductions,
                        //            GrossPercent = GrossPercent,
                        //            NetPercent = NetPercent,
                        //            RemWithDeductions = RemWithDeductions,
                        //            PreInvoiceHint = PreInvoiceHint,
                        //            OfferedPrice = OfferedPrice,
                        //            OfferedSerial=OfferedSerial
                        //        });

                        //    }
                        //    catch
                        //    {
                        //        DebitCreditOneView.Add(new FinancialDebitCredit
                        //        {
                        //            ID = "0",
                        //            Title = "",
                        //            Creditor = 0,
                        //            Debtor = 0,
                        //            Rem = 0,
                        //            NonCash = 0,
                        //            Amount = 0,
                        //            Deductions = 0,
                        //            GrossPercent = "0",
                        //            NetPercent = "0",
                        //            RemWithDeductions = 0,
                        //            PreInvoiceHint=0,
                        //            OfferedPrice=0,
                        //            OfferedSerial=""
                        //        });

                        //    }
                        //}
                        //111111111111


                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    SaleCondition = reader["SaleCondition"].ToString();
                        //    Amount = Convert.ToDouble(reader["Amount"].ToString());

                        //    PreInvoicesHint.Add(new PreInvoicesHint
                        //    {
                        //        Amount=Amount,
                        //        SaleCondition=SaleCondition
                        //    });
                        //}
                        //
                        conn.Close();
                    }
                }//end using
                ViewBag.TableDebitCredit = TableDebitCredit;
                ViewBag.DebitCreditOneView = DebitCreditOneView;
                //ViewBag.PreInvoicesHint = PreInvoicesHint;
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //
                ViewBag.TableOuts = TableOuts;
                ViewBag.sumCount = sumCount;
                if (sumPrice > 0)
                {
                    ViewBag.sumPrice = sumPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.sumPrice = sumPrice;
                }

                if (sumSalary > 0)
                {
                    ViewBag.sumSalary = sumSalary.ToString("#,##");
                }
                else
                {
                    ViewBag.sumSalary = sumSalary;
                }
                if (SumNonCash > 0)
                {
                    ViewBag.sumNonCash = SumNonCash.ToString("#,##");
                }
                else
                {
                    ViewBag.sumNonCash = SumNonCash;
                }
                //
                if (SumDeductions > 0)
                {
                    ViewBag.sumDeductions = SumDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDeductions = SumDeductions;
                }
                //
                if (SumRemWithDeductions > 0)
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions;
                }
                //                
                if (SumOfferedPrice > 0)
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice;
                }
            }
            catch
            {
                ViewBag.TankValveTableOuts = null;
            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        //
        public ActionResult TopSection(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01");
            ViewBag.fromDate = fromDate.ToShortDateString();
            //
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            //
            if (WorkshopID == null)
                if (cngfapco.Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
                    WorkshopID = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId()).WorkshopID;
                else
                    WorkshopID = 0;
            ViewBag.workshop = WorkshopID;
            
            //begin top section
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            //end top section

            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();

            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ID = reader["ID"].ToString();
                        Title = reader["Title"].ToString();
                        Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                        sumCredit += Creditor;
                        Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                        sumDebit += Debtor;
                        Rem = Convert.ToDouble(reader["Rem"].ToString());
                        sumRem += Rem;

                        TableDebitCredit.Add(new FinancialDebitCredit
                        {
                            ID = ID,
                            Title = Title,
                            Creditor = Creditor,
                            Debtor = Debtor,
                            Rem = Rem
                        });
                    }
                }
            }
            //
            ViewBag.TableDebitCredit = TableDebitCredit;
            //
            if (sumCredit > 0)
            {
                ViewBag.sumCredit = sumCredit.ToString("#,##");
            }
            else
            {
                ViewBag.sumCredit = sumCredit;
            }

            if (sumDebit > 0)
            {
                ViewBag.sumDebit = sumDebit.ToString("#,##");
            }
            else
            {
                ViewBag.sumDebit = sumDebit;
            }

            if (sumRem > 0)
            {
                ViewBag.sumRem = sumRem.ToString("#,##");
            }
            else
            {
                ViewBag.sumRem = sumRem;
            }
            //

            return PartialView();
        }
        //
        public ActionResult DetailsSection(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            ViewBag.workshop = WorkshopID;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //begin details section
            string Type = "";
            int Count = 0;
            double Price = 0.0;
            double Salary = 0.0;
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            //end details section

            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    conn.Open();
                    reader = command.ExecuteReader();
                    reader.NextResult();
                    while (reader.Read())
                    {
                        Type = reader["Type"].ToString();
                        Count = Convert.ToInt32(reader["Count"].ToString());
                        sumCount += Count;
                        Price = Convert.ToDouble(reader["Price"].ToString());
                        sumPrice += Price;
                        Salary = Convert.ToDouble(reader["Salary"].ToString());
                        sumSalary += Salary;

                        TableOuts.Add(new Models.Financial
                        {
                            Type = Type,
                            Count = Count,
                            Price = Price,
                            Salary = Salary
                        });
                    }

                }
            }
            //
            ViewBag.TableOuts = TableOuts;

            ViewBag.sumCount = sumCount;

            if (sumSalary > 0)
            {
                ViewBag.sumSalary = sumSalary.ToString("#,##");
            }
            else
            {
                ViewBag.sumSalary = sumSalary;
            }

            return View();
        }
        //Compare with invoice
        public ActionResult AnalyseTableSection(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (WorkshopID == null)
                WorkshopID = 0;
            ViewBag.workshop = WorkshopID;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //begin analyse table section
            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;            
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;            
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;            
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            string OfferedSerial = "";
            //end analyse table section

            List<FinancialDebitCredit> TableOuts = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    conn.Open();
                    reader = command.ExecuteReader();
                    reader.NextResult();
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ID = reader["ID"].ToString();
                        Title = reader["Title"].ToString();
                        Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                        sumTCredit += Creditor;
                        Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                        sumTDebit += Debtor;
                        Rem = Convert.ToDouble(reader["Rem"].ToString());
                        sumTRem += Rem;
                        NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                        SumNonCash += NonCash;
                        Amount = Convert.ToDouble(reader["Amount"].ToString());
                        Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                        SumDeductions += Deductions;
                        RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                        SumRemWithDeductions += RemWithDeductions;
                        NetPercent = reader["NetPercent"].ToString();
                        GrossPercent = reader["GrossPercent"].ToString();
                        PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                        SumPreInvoiceHint += PreInvoiceHint;
                        OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                        SumOfferedPrice += OfferedPrice;
                        OfferedSerial = reader["OfferedSerial"].ToString();

                        TableOuts.Add(new FinancialDebitCredit
                        {
                            ID = ID,
                            Title = Title,
                            Creditor = Creditor,
                            Debtor = Debtor,
                            Rem = Rem,
                            NonCash = NonCash,
                            Amount = Amount,
                            Deductions = Deductions,
                            GrossPercent = GrossPercent,
                            NetPercent = NetPercent,
                            RemWithDeductions = RemWithDeductions,
                            PreInvoiceHint = PreInvoiceHint,
                            OfferedPrice = OfferedPrice,
                            OfferedSerial = OfferedSerial
                        });
                    }

                }
            }
            //
            ViewBag.TableOuts = TableOuts;
            //
            if (SumNonCash > 0)
            {
                ViewBag.sumNonCash = SumNonCash.ToString("#,##");
            }
            else
            {
                ViewBag.sumNonCash = SumNonCash;
            }
            //
            if (SumDeductions > 0)
            {
                ViewBag.sumDeductions = SumDeductions.ToString("#,##");
            }
            else
            {
                ViewBag.sumDeductions = SumDeductions;
            }
            //
            if (SumRemWithDeductions > 0)
            {
                ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
            }
            else
            {
                ViewBag.SumRemWithDeductions = SumRemWithDeductions;
            }
            //
            if (sumTCredit > 0)
            {
                ViewBag.sumTCredit = sumTCredit.ToString("#,##");
            }
            else
            {
                ViewBag.sumTCredit = sumTCredit;
            }
            //
            if (sumTDebit > 0)
            {
                ViewBag.sumTDebit = sumTDebit.ToString("#,##");
            }
            else
            {
                ViewBag.sumTDebit = sumTDebit;
            }
            //
            return PartialView();
        }  
        //
        public class FinancialDebitCredit
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public double Creditor { get; set; }
            public double Debtor { get; set; }
            public double Rem { get; set; }
            public double NonCash { get; set; }
            public double Amount { get; set; }
            public double Deductions { get; set; }
            public double RemWithDeductions { get; set; }
            public string NetPercent { get; set; }
            public string GrossPercent { get; set; }
            public double PreInvoiceHint { get; set; }
            public double OfferedPrice { get; set; }
            public string OfferedSerial { get; set; }
            public string OfferedID { get; set; }
        }
        public class PreInvoicesHint
        {
            public string PreInvoiceCode { get; set; }
            public string WorkshopsID { get; set; }
            public string SaleCondition { get; set; }
            public double Amount { get; set; }            
        }
        public class ApprovedInvoice
        {
            public string InvoiceCode { get; set; }
            public string ServiceDesc { get; set; }
            public string Number { get; set; }
            public string UnitofMeasurement { get; set; }
            public string Description { get; set; }
            public double UnitAmount { get; set; }
            public double TotalAmount { get; set; }
            public double AmountTaxandComplications { get; set; }
            public double TotalAmountTaxandComplications { get; set; }
        }
        public class TotalAmount
        {
            public string SumCount { get; set; }
            public string SumTotalAmount { get; set; }
            public string SumAmountTax { get; set; }
            public string SumTotalAmountTax { get; set; }
        }
        // GET: Financials/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Financials/InvoiceList/5
        public ActionResult InvoiceList(int? workshopId, string InvoiceCode)
        {
            ViewBag.workshopId = workshopId;
            ViewBag.InvoiceCode = InvoiceCode;
            var invoices = db.tbl_Invoices.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode));

            return View(invoices.ToList());
        }
        // GET: Financials/Create
        public ActionResult Create(int? workshopId, string InvoiceCode)
        {
            if (workshopId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoice = db.tbl_Invoices.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode));
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnersID = new SelectList(db.tbl_VehicleRegistrations.Where(v=>v.RegisterStatus==true), "ID", "OwnerFamily", invoice.FirstOrDefault().OwnersID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", invoice.FirstOrDefault().WorkshopsID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", invoice.FirstOrDefault().EquipmentsID);
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", invoice.FirstOrDefault().CurrencyTypeID);
            return View(invoice.FirstOrDefault());
        }

        // POST: Financials/Create
        [HttpPost]
        public ActionResult Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {                
                db.tbl_Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("InvoiceList", new { workshopId = invoice.WorkshopsID, InvoiceCode = invoice.InvoiceCode });
            }
            ViewBag.OwnersID = new SelectList(db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true), "ID", "OwnerFamily", invoice.OwnersID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", invoice.WorkshopsID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", invoice.EquipmentsID);
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", invoice.CurrencyTypeID);
            return View(invoice);
        }

        // GET: Financials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.tbl_Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnersID= new SelectList(db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true), "ID", "OwnerFamily", invoice.OwnersID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", invoice.WorkshopsID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", invoice.EquipmentsID);
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", invoice.CurrencyTypeID);
            return View(invoice);
        }

        // POST: Financials/Edit/5
        [HttpPost]
        public ActionResult Edit(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InvoiceList",new { workshopId=invoice.WorkshopsID, InvoiceCode=invoice.InvoiceCode });
            }
            ViewBag.OwnersID = new SelectList(db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus == true), "ID", "OwnerFamily", invoice.OwnersID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", invoice.WorkshopsID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", invoice.EquipmentsID);
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", invoice.CurrencyTypeID);
            return View(invoice);
        }

        // GET: Financials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.tbl_Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Financials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.tbl_Invoices.Find(id);
            int? WorkshopsID = invoice.WorkshopsID;
            string InvoiceCode = invoice.InvoiceCode;
            db.tbl_Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("InvoiceList", new { workshopId = WorkshopsID, InvoiceCode = InvoiceCode });
        }

        // GET: Financials/Remittance/5
        public ActionResult Remittance(int? id, string url)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RemittanceDetails remittancedetails = db.tbl_RemittanceDetails.Find(id);
            if (url.Equals("Remittance"))
            {
                return RedirectToAction("Remittance_Old", new { id = id,url=url });
            }
            else
            {
                var remittancedetails = db.tbl_RemittanceDetails.Include(r => r.Remittances).Where(r => r.Remittances.DivisionPlanID == id).SingleOrDefault();

                if (remittancedetails == null)
                {
                    return HttpNotFound();
                }
                //
                //var divisionId = remittancedetails.Remittances.DivisionPlanID;

                List<RemittanceList> remittanceList = new List<RemittanceList>();
                //
                SqlDataReader reader;
                var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("[dbo].[sp_remittance]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;
                    cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

                    conn.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        remittanceList.Add(new RemittanceList
                        {
                            NumberofSend = reader["NumberofSend"].ToString(),
                            Description = reader["Description"].ToString(),
                            Unit = reader["Unit"].ToString(),
                            Title = reader["Title"].ToString()
                        });
                    }
                    ViewBag.RemittanceList = remittanceList;
                    conn.Close();
                }
                return View(remittancedetails);
            }
        }
        //
        public ActionResult Remittance_Old(int? id, string url)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var remittancedetails = db.tbl_RemittanceDetails.Find(id);
            int? divisionId = remittancedetails.Remittances.DivisionPlanID;
            if (remittancedetails == null)
            {
                return HttpNotFound();
            }
            //
            List<RemittanceList> remittanceList = new List<RemittanceList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_remittance]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    remittanceList.Add(new RemittanceList
                    {
                        NumberofSend = reader["NumberofSend"].ToString(),
                        Description = reader["Description"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        Title = reader["Title"].ToString()
                    });
                }
                ViewBag.RemittanceList = remittanceList;
                conn.Close();
            }
            return View(remittancedetails);
        }

        // GET: Financials/Remittance/5
        public ActionResult RemittanceDetailsWithBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            RemittanceDetails remittancedetails = db.tbl_RemittanceDetails.Find(id);
            if (remittancedetails == null)
            {
                return HttpNotFound();
            }
            //
            var divisionId = remittancedetails.Remittances.DivisionPlanID;

            List<RemittanceList> remittanceList = new List<RemittanceList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_remittancewithBOM]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    remittanceList.Add(new RemittanceList
                    {
                        FinancialCode = reader["FinancialCode"].ToString(),
                        NumberofSend = reader["NumberofSend"].ToString(),
                        Description = reader["Description"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        Title = reader["Title"].ToString()
                    });
                }
                ViewBag.RemittanceList = remittanceList;
                conn.Close();
            }
            return View(remittancedetails);
        }
        //
        public ActionResult RemittanceWithBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            Remittances remittancedetails = db.tbl_Remittances.Where(r => r.ID == id).SingleOrDefault();
            if (remittancedetails == null)
            {
                return HttpNotFound();
            }
            //
            var divisionId = remittancedetails.DivisionPlanID;

            List<RemittanceList> remittanceList = new List<RemittanceList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_remittancewithBOM]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    remittanceList.Add(new RemittanceList
                    {
                        FinancialCode = reader["FinancialCode"].ToString(),
                        NumberofSend = reader["NumberofSend"].ToString(),
                        Description = reader["Description"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        Title = reader["Title"].ToString()
                    });
                }
                ViewBag.RemittanceList = remittanceList;
                conn.Close();
            }
            return View(remittancedetails);
        }
        public ActionResult RemittanceWithBOMFixed(int? id, string level)
        {            
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            if (level == "0")
            {
                return new ActionAsPdf("RemittanceDetailsWithBOM", new { id = id, level = level })
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    CustomSwitches = footer
                };
            }
            else
            {
                //
                return new ActionAsPdf("RemittanceWithBOM", new { id = id, level = level })
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    CustomSwitches = footer
                };
            }
            //return report;
        }
        //
        // GET: Financials/Remittance/5
        public ActionResult RemittanceForPay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemittanceDetails remittancedetails = db.tbl_RemittanceDetails.Find(id);
            if (remittancedetails == null)
            {
                return HttpNotFound();
            }
            //
            var divisionId = remittancedetails.Remittances.DivisionPlanID;

            List<RemittanceList> remittanceList = new List<RemittanceList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_remittance]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = divisionId;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    remittanceList.Add(new RemittanceList
                    {
                        NumberofSend = reader["NumberofSend"].ToString(),
                        Description = reader["Description"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        Title = reader["Title"].ToString()
                    });
                }
                ViewBag.RemittanceList = remittanceList;
                conn.Close();
            }
            return View(remittancedetails);
        }
        //
        public JsonResult GetServices(int id)
        {
            var listofServices = db.tbl_ListofServices.Where(l => l.ID == id).SingleOrDefault();
            List<ListofServices> services = new List<ListofServices>();
            services.Add(new ListofServices
            {
                ID = listofServices.ID,
                ServiceRent = listofServices.ServiceRent,
                Title = listofServices.Title,
                Unit = listofServices.Unit
            });

            return Json(services, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult GetFreeSaleEquipment(int id,int? type)
        {
            double? groupValue = null;
            if (id == 82)
            {
                if(type==1)
                    groupValue = db.tbl_EquipmentList.Where(l => l.Pid == id).Sum(l => l.Value);
                else
                    groupValue = db.tbl_EquipmentList.Where(l => l.Pid == id).Sum(l => l.Value2);
            }
            //
            var listofServices = db.tbl_EquipmentList.Where(l => l.ID == id).SingleOrDefault();
            List<EquipmentList> services = new List<EquipmentList>();
            //
            if (id != 82)
            {
                if (type == 1)
                {
                    services.Add(new EquipmentList
                    {
                        ID = listofServices.ID,
                        Title = listofServices.Title,
                        FinancialCode = listofServices.FinancialCode,
                        Value = listofServices.Value
                    });
                }
                else
                {
                    services.Add(new EquipmentList
                    {
                        ID = listofServices.ID,
                        Title = listofServices.Title,
                        FinancialCode = listofServices.FinancialCode,
                        Value = listofServices.Value2
                    });
                }
            }
            //
            else
            {
                if (type == 1)
                {
                    services.Add(new EquipmentList
                    {
                        ID = listofServices.ID,
                        Title = listofServices.Title,
                        FinancialCode = "82",
                        Value = groupValue
                    });
                }
                else
                {
                    services.Add(new EquipmentList
                    {
                        ID = listofServices.ID,
                        Title = listofServices.Title,
                        FinancialCode = "82",
                        Value = groupValue
                    });
                }
            }
            //
            return Json(services, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult GetVehicleRegistration(int id)
        {
            var workshop = Helper.Helpers.GetWorkshopCurrentUser();
            var listofRegistration = db.tbl_VehicleRegistrations.Where(l => l.VehicleTypeID == id && l.WorkshopID== workshop.ID).Count();
            var vtype = db.tbl_VehicleTypes.Find(id);
            List<VehicleTypeInfo> type = new List<VehicleTypeInfo>();
            if (vtype.Type.Contains("وانت"))
            {
                type.Add(new VehicleTypeInfo
                {
                    Amount = "2500000",
                    Count = listofRegistration.ToString(),
                    Unit = "دستگاه",
                    Description = vtype.Type +" - "+vtype.Description
                });

            }
            else
            {
                type.Add(new VehicleTypeInfo
                {
                    Amount = "2000000",
                    Count = listofRegistration.ToString(),
                    Unit = "دستگاه",
                    Description = vtype.Type + " - " + vtype.Description
                });

            }

            return Json(type, JsonRequestBehavior.AllowGet);
        }
        //
        public class VehicleTypeInfo
        {
            public string Description { get; set; }
            public string Count { get; set; }
            public string Amount { get; set; }
            public string Unit { get; set; }
        }
        //
        public class RemittanceList
        {
            public string Title { get; set; }
            public string Unit { get; set; }
            public string NumberofSend { get; set; }            
            public string Description { get; set; }
            public string FinancialCode { get; set; }
        }

        /// <summary>
        /// صدور فاکتور فروش کالا و خدمات غیر مصوب توسط کارگاه های به مشتریان
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Invoice(string InvoiceCode)
        {            
            ViewBag.ServicesID = new SelectList(db.tbl_ListofServices, "ID", "Title");
            ViewBag.ContractID = new SelectList(db.tbl_Workshops, "ID", "Title");
            var Workshop = cngfapco.Helper.Helpers.GetWorkshopCurrentUser();
            ViewBag.WorkshopId = Workshop.ID;
            ViewBag.State = Workshop.City.State.Title;
            ViewBag.City = Workshop.City.Title;
            //           
            int CurrYear = pc.GetYear(DateTime.Now);

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_VehicleRegistrations.Where(v=> v.RegisterStatus==true && v.WorkshopID == Workshop.ID), "ID", "NationalCode", null);
                //ViewBag.ContractID = new SelectList(db.Contracts, "ContractID", "Title",null);
                var invoicelist = db.tbl_Invoices.Where(i=> i.WorkshopsID == Workshop.ID).ToList();
                //var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                var invoicelist2 = invoicelist.OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                int maxcode = 1;

                if (invoicelist2.Count() > 0)
                {
                    maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                    ViewBag.invoiceCode = (maxcode).ToString();
                }
                else
                {
                    ViewBag.invoiceCode = "1";
                }

                return View(invoicelist2.Where(i => i.InvoiceCode == "1000000000").ToList());
            }
            else
            {
                var invoice = db.tbl_Invoices.Where(i => i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
                var eid = invoice.OrderByDescending(i => i.InvoiceID).FirstOrDefault().WorkshopsID;
                var coid = invoice.OrderByDescending(i => i.EquipmentsID).FirstOrDefault().EquipmentsID;
                ViewBag.ContractID = new SelectList(db.tbl_Workshops.Where(e => e.ID == eid), "ID", "Title");
                ViewBag.EmployerID = new SelectList(db.tbl_VehicleRegistrations.Where(e => e.RegisterStatus==true && e.ID == eid), "ID", "NationalCode");
                ViewBag.invoiceCode = invoice.FirstOrDefault().InvoiceCode;
                ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                //ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                return View(invoice.ToList());
            }
        }
        //
        /// <summary>
        /// صدور فاکتور فروش کالا و خدمات توسط کارگاه ها برای شرکت فن آوران پارسیان
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InvoiceFapa(string InvoiceCode)
        {
            var vehicleTypes = db.tbl_VehicleTypes.ToList();
            List<VehicleType> list = new List<VehicleType>();
            foreach(var item in vehicleTypes)
            {
                list.Add(new VehicleType
                {
                    ID=item.ID,
                    Type=item.Type + " - " + item.Description
                });
            }

            ViewBag.ServicesID = new SelectList(list, "ID", "Type");
            ViewBag.ContractID = new SelectList(db.tbl_Workshops, "ID", "Title");
            var Workshop = cngfapco.Helper.Helpers.GetWorkshopCurrentUser();
            ViewBag.WorkshopId = Workshop.ID;
            ViewBag.State = Workshop.City.State.Title;
            ViewBag.City = Workshop.City.Title;
            //           
            int CurrYear = pc.GetYear(DateTime.Now);

            if (InvoiceCode == null || InvoiceCode == "")
            {
                //ViewBag.EmployerID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", null);
                //ViewBag.ContractID = new SelectList(db.Contracts, "ContractID", "Title",null);
                var invoicelist = db.tbl_InvoicesFapa.ToList();
                var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                int maxcode = 1;

                if (invoicelist2.Count() > 0)
                {
                    maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                    ViewBag.invoiceCode = (maxcode).ToString();
                }
                else
                {
                    ViewBag.invoiceCode = "1";
                }

                return View(invoicelist2.Where(i => i.InvoiceCode == "1000000000").ToList());
            }
            else
            {
                var invoice = db.tbl_InvoicesFapa.Where(i => i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
                var eid = invoice.OrderByDescending(i => i.InvoiceID).FirstOrDefault().WorkshopsID;
                var coid = invoice.OrderByDescending(i => i.EquipmentsID).FirstOrDefault().EquipmentsID;
                ViewBag.ContractID = new SelectList(db.tbl_Workshops.Where(e => e.ID == eid), "ID", "Title");
                ViewBag.EmployerID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", null);
                ViewBag.invoiceCode = invoice.FirstOrDefault().InvoiceCode;
                ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                //ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                return View(invoice.ToList());
            }
        }
        //
        public ActionResult InvoiceSimplePrint(string InvoiceCode, int? workshopId)
        {
            bool isVanet = false;
            string vt1 = "";
            string vt2 = "";
            if (workshopId==null)
                workshopId = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
            var invoice = db.tbl_Invoices.Where(i => i.InvoiceCode == InvoiceCode && i.WorkshopsID == workshopId).Include(i => i.Owners).Include(i => i.Workshops);
            if(invoice.Count()==0)
                return RedirectToAction("Page403", "Home");

            try
            {
                vt1 = invoice.FirstOrDefault().Owners.VehicleType.Type;
            }
            catch { }
            try
            {
                vt2 = invoice.FirstOrDefault().EmployerPostalcode;
            }
            catch { }

            if (invoice.Count() > 0)
            {
                if (!string.IsNullOrEmpty(vt1))
                    if (vt1.Contains("وانت"))
                        isVanet = true;
                if (!string.IsNullOrEmpty(vt2))
                    if (vt2.Contains("وانت"))
                        isVanet = true;
            }
            ViewBag.isVanet = isVanet;
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.TotalAmount = invoice.Sum(i=>i.TotalAmount).HasValue? invoice.Sum(i => i.TotalAmount).Value.ToString("#,##"):"0";
            ViewBag.Date = invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString();
            ViewBag.Time = invoice.FirstOrDefault().CreatedDate.Value.ToShortTimeString();
            ViewBag.Address = invoice.FirstOrDefault().Workshops.Address;
            ViewBag.PhoneNumber = invoice.FirstOrDefault().Workshops.PhoneNumber;
            return View(invoice.Where(i=>i.InvoiceCode == InvoiceCode && i.WorkshopsID == workshopId).ToList());
            //return RedirectToAction("InvoicetoPrint",new { InvoiceCode= InvoiceCode, Type= Type });
        }
        //
        public ActionResult InvoiceSimplePrintFixed(string InvoiceCode, int? workshopId)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            if (workshopId == null)
                workshopId = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("InvoiceSimplePrint", new { InvoiceCode = InvoiceCode, workshopId = workshopId, Type = 1 })
            {
                //FileName = Server.MapPath("~/Content/Relato.pdf"),
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
            };
            //return report;
        }
        //
        public ActionResult VehicleanOwner(string InvoiceCode, int? workshopId)
        {
            if (workshopId == null)
                workshopId = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
            
            var OwnersID = db.tbl_Invoices.Where(i=>i.InvoiceCode==InvoiceCode && i.WorkshopsID== workshopId).FirstOrDefault().OwnersID;
            var Owner = db.tbl_VehicleRegistrations.Find(OwnersID);
            //List<Invoice> invoiceList = new List<Invoice>();
            var list = db.tbl_Invoices.Where(i => i.InvoiceCode == InvoiceCode && i.WorkshopsID == workshopId).Include(i=>i.Workshops).Take(1).ToList();
            //foreach (var item in list)
            //{
            //    invoiceList.Add(new Invoice
            //    {
            //        InvoiceID = item.InvoiceID,
            //        EmployerEconomicalnumber = item.EmployerEconomicalnumber,
            //        Employerregistrationnumber = item.Employerregistrationnumber,
            //        EmployerState = item.EmployerState,
            //        EmployerAddress = item.EmployerAddress,
            //        EmployerFax = item.EmployerFax,
            //        EmployerPhone = item.EmployerPhone,
            //        EmployerPostalcode = item.EmployerPostalcode
            //    });
            //}
            ViewBag.tableOut = list;
            return PartialView(Owner);
        }
        //
        public ActionResult GetWorkshopInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.tbl_Workshops.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }
        //
        public ActionResult GetCustomerInfo(int? id, string invoiceCode)
        {
            var Workshop = cngfapco.Helper.Helpers.GetWorkshopCurrentUser();
            //ViewBag.EmployerID = new SelectList(db.tbl_VehicleRegistrations, "ID", "OwnerFamily", null);
           
            if (id != null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                VehicleRegistration owner = db.tbl_VehicleRegistrations.Find(id);
                if (owner == null)
                {
                    return HttpNotFound();
                }
                return View(owner);
            }
            else
            {               
                return View();
            }
            
        }
        //
        /// <summary>
        /// ثبت و صدور صورتحساب فروش کالا/ خدمات
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertInvoices(List<Invoice> invoices)
        {
            using (ContextDB entities = new ContextDB())
            {
                string invoiceparam = "";
                int CurrYear = pc.GetYear(DateTime.Now);
                //Truncate Table to delete all old records.
                // entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [Customers]");
                var departmentid = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                //Check for NULL.
                if (invoices == null || invoices.Count == 0)
                {
                    Thread.Sleep(1000);
                    return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var invoicelist = db.tbl_Invoices.Where(i => i.WorkshopsID == departmentid).ToList();
                    //var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    var invoicelist2 = invoicelist.OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    int maxcode = 1;

                    if (invoicelist2.Count() > 0)
                    {
                        maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                        ViewBag.invoiceCode = (maxcode).ToString();
                    }
                    else
                    {
                        ViewBag.invoiceCode = "1";
                    }

                    Invoice additem = new Invoice();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
                    {
                        //var list = db.Invoices.OrderByDescending(i => i.InvoiceID).Where(i => i.InvoiceCode == invoice.InvoiceCode).ToList();
                        //if(list.Count>0)
                        //{
                        //    var result = list.FirstOrDefault().DepartmentID;

                        //    if (list != null && result != departmentid)
                        //    {
                        //        additem.InvoiceCode = (Convert.ToInt32(list.OrderByDescending(i => i.InvoiceID).FirstOrDefault().InvoiceCode) + 1).ToString();
                        //    }
                        //}

                        //else
                        //{
                        //    additem.InvoiceCode = invoice.InvoiceCode;
                        //}

                        //
                        if (invoice.OwnersID != null)
                        {
                            additem.OwnersID = db.tbl_VehicleRegistrations.Find(invoice.OwnersID).ID;
                        }
                        else
                        {
                            additem.OwnersID = null;
                        }
                        if (invoice.EquipmentsID != null)
                        {
                            additem.EquipmentsID = db.tbl_EquipmentList.Find(invoice.EquipmentsID).ID;
                        }
                        else
                        {
                            additem.EquipmentsID = null;
                        }
                        //
                        invoice.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;

                        //if (invoice.CreatedDate == null)
                            invoice.CreatedDate = DateTime.Now;
                        //else
                        //    additem.CreatedDate =Convert.ToDateTime(invoice.CreatedDate +" "+ DateTime.Now.ToShortTimeString());
                        additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                        additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                        additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                        additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                        additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
                        if (invoice.ServiceDesc == null)
                            invoice.ServiceDesc = "مقداری ثبت نشده!";
                        else
                            additem.ServiceDesc = invoice.ServiceDesc;
                        if (invoice.UnitAmount < 1)
                            invoice.UnitAmount = 1;
                        else
                            additem.UnitAmount = Convert.ToDouble(invoice.UnitAmount);
                        if (invoice.Number == null)
                            invoice.Number = "1";
                        else
                            additem.Number = invoice.Number;
                        additem.EmployerEconomicalnumber = invoice.EmployerEconomicalnumber;
                        additem.EmployerPostalcode = invoice.EmployerPostalcode;
                        additem.EmployerPhone = invoice.EmployerPhone;
                        additem.EmployerFax = invoice.EmployerFax;
                        additem.Description = invoice.Description;
                        additem.SaleCondition = invoice.SaleCondition;
                        additem.Comment = invoice.Comment;
                        invoice.Tax = 0;
                        invoice.Complications = 0;
                        invoice.Status = true;
                        if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            additem.CurrencyTypeID = invoice.CurrencyTypeID;
                        else
                            additem.CurrencyTypeID = 6;
                        //
                        //if (invoice.WorkshopsID == 5 || invoice.DepartmentID == 41)
                        //{
                        invoice.AcceptedAmount = invoice.TotalAmount;
                        invoice.AcceptedDate = invoice.CreatedDate;
                        //}
                        //
                        invoice.InvoiceCode = maxcode.ToString();
                        invoice.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                        //
                        db.tbl_Invoices.Add(invoice);
                        db.SaveChanges();
                        //
                        invoiceparam = invoice.InvoiceCode;
                    }
                    //string cnnString = ConfigurationManager.ConnectionStrings["RBAC_Model"].ConnectionString;

                    //SqlConnection cnn = new SqlConnection(cnnString);
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.Connection = cnn;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.CommandText = "[dbo].[InsertInvoicestoStatements]";
                    //cmd.Parameters.Add(new SqlParameter("@InvoiceCode", invoiceparam));
                    ////add any parameters the stored procedure might require
                    //cnn.Open();
                    //object o = cmd.ExecuteScalar();
                    //cnn.Close();
                    //
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "اطلاعات ارسالی شما با موفقیت ثبت شد" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        /// <summary>
        ///  ثبت و صدور صورتحساب فروش کالا/ خدمات کارگاه به شرکت فن آوران پارسیان
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertInvoicesFapa(List<InvoiceFapa> invoices)
        {
            using (ContextDB entities = new ContextDB())
            {
                string invoiceparam = "";
                int CurrYear = pc.GetYear(DateTime.Now);
                //Truncate Table to delete all old records.
                // entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [Customers]");
                var departmentid = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                //Check for NULL.
                if (invoices == null || invoices.Count == 0)
                {
                    Thread.Sleep(1000);
                    return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var invoicelist = db.tbl_InvoicesFapa.ToList();
                    var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    int maxcode = 1;

                    if (invoicelist2.Count() > 0)
                    {
                        maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                        ViewBag.invoiceCode = (maxcode).ToString();
                    }
                    else
                    {
                        ViewBag.invoiceCode = "1";
                    }

                    InvoiceFapa additem = new InvoiceFapa();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
                    {
                        //var list = db.Invoices.OrderByDescending(i => i.InvoiceID).Where(i => i.InvoiceCode == invoice.InvoiceCode).ToList();
                        //if(list.Count>0)
                        //{
                        //    var result = list.FirstOrDefault().DepartmentID;

                        //    if (list != null && result != departmentid)
                        //    {
                        //        additem.InvoiceCode = (Convert.ToInt32(list.OrderByDescending(i => i.InvoiceID).FirstOrDefault().InvoiceCode) + 1).ToString();
                        //    }
                        //}

                        //else
                        //{
                        //    additem.InvoiceCode = invoice.InvoiceCode;
                        //}

                        //
                        if (invoice.VehicleTypesID != null)
                        {
                            additem.VehicleTypesID = db.tbl_VehicleTypes.Find(invoice.VehicleTypesID).ID;
                        }
                        else
                        {
                            additem.VehicleTypesID = null;
                        }
                        if (invoice.EquipmentsID != null)
                        {
                            additem.EquipmentsID = db.tbl_EquipmentList.Find(invoice.EquipmentsID).ID;
                        }
                        else
                        {
                            additem.EquipmentsID = null;
                        }
                        //
                        invoice.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;

                        if (invoice.CreatedDate == null)
                            invoice.CreatedDate = DateTime.Now;
                        else
                            additem.CreatedDate = invoice.CreatedDate;
                        additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                        additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                        additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                        additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                        additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
                        if (invoice.ServiceDesc == null)
                            invoice.ServiceDesc = "مقداری ثبت نشده!";
                        else
                            additem.ServiceDesc = invoice.ServiceDesc;
                        if (invoice.UnitAmount < 1)
                            invoice.UnitAmount = 1;
                        else
                            additem.UnitAmount = Convert.ToDouble(invoice.UnitAmount);
                        if (invoice.Number == null)
                            invoice.Number = "1";
                        else
                            additem.Number = invoice.Number;
                        additem.EmployerEconomicalnumber = invoice.EmployerEconomicalnumber;
                        additem.EmployerPostalcode = invoice.EmployerPostalcode;
                        additem.EmployerPhone = invoice.EmployerPhone;
                        additem.EmployerFax = invoice.EmployerFax;
                        additem.Description = invoice.Description;
                        additem.SaleCondition = invoice.SaleCondition;
                        additem.Comment = invoice.Comment;
                        invoice.Tax = 0;
                        invoice.Complications = 0;
                        invoice.Status = true;
                        if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            additem.CurrencyTypeID = invoice.CurrencyTypeID;
                        else
                            additem.CurrencyTypeID = 6;
                        //
                        //if (invoice.WorkshopsID == 5 || invoice.DepartmentID == 41)
                        //{
                        invoice.AcceptedAmount = invoice.TotalAmount;
                        invoice.AcceptedDate = invoice.CreatedDate;
                        //}
                        //
                        invoice.InvoiceCode = maxcode.ToString();
                        invoice.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                        //
                        db.tbl_InvoicesFapa.Add(invoice);
                        db.SaveChanges();
                        //
                        invoiceparam = invoice.InvoiceCode;
                    }
                    //string cnnString = ConfigurationManager.ConnectionStrings["RBAC_Model"].ConnectionString;

                    //SqlConnection cnn = new SqlConnection(cnnString);
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.Connection = cnn;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.CommandText = "[dbo].[InsertInvoicestoStatements]";
                    //cmd.Parameters.Add(new SqlParameter("@InvoiceCode", invoiceparam));
                    ////add any parameters the stored procedure might require
                    //cnn.Open();
                    //object o = cmd.ExecuteScalar();
                    //cnn.Close();
                    //
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "اطلاعات ارسالی شما با موفقیت ثبت شد" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        /// <summary>
        /// مشاهده لیست صورتحسابهای فروش کالا/ خدمات صادر شده
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        public ActionResult InvoicesList(int? Year)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            ViewBag.Permission = false;
            //var invoice = db.tbl_Invoices.Include(i => i.Owners).Include(i => i.Workshops).Include(i => i.Equipments);
            //return View(invoice.ToList());
            var userName = Helper.Helpers.GetCurrentUser();
            string permission = db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault().WorkshopID.ToString();
            string InvoiceCode = "";
            string CreatedDate = "";
            string EmployerTitle = "";
            //string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            //
            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = permission;
                    //command.Parameters.Add("@year", SqlDbType.VarChar).Value = Year;

                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = reader["CreatedDate"].ToString();
                        EmployerTitle = reader["EmployerTitle"].ToString();
                        //DepartmentTitle = reader["Title"].ToString();
                        Title = reader["Title"].ToString();
                        Status = reader["Status"].ToString();
                        TotalAmount = reader["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = Convert.ToDateTime(CreatedDate).ToShortDateString(),
                            EmployerTitle = EmployerTitle,
                            //DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##"))
                        });


                    }
                }
                conn.Close();
            }//end using
            ViewBag.TableOut = TableOuts;
            ViewBag.workshopId = permission;

            return View();
        }
        //
        /// <summary>
        /// مشاهده لیست صورتحسابهای فروش کالا/ خدمات صادر شده
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        public ActionResult InvoicesFapaList()
        {
            int Year = pc.GetYear(DateTime.Now);
            var workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            ViewBag.Permission = false;
            //var invoice = db.tbl_Invoices.Include(i => i.Owners).Include(i => i.Workshops).Include(i => i.Equipments);
            //return View(invoice.ToList());
            var userName = Helper.Helpers.GetCurrentUser();
            string permission = db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault().WorkshopID.ToString();
            string InvoiceCode = "";
            string CreatedDate = "";
            string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            string FromDate = "";
            //
            string invoiceCode = "1";
            DateTime dt = DateTime.Now;
            //int diffDay=0;
            DateTime ToDate = DateTime.Now;
            DateTime oneDueDate = Convert.ToDateTime("1399/04/31");
            string fromdateInString = "1399/03/01";
            string todateInString = DateTime.Now.ToShortDateString();
            string Month = "01";
            string Day = "30";
            string Month2 = "01";
            string Day2 = "01";
            string DefectsCount = "0";
            //Begin------------------------------------------------new code for 30 day-------------------------------------------------------------------
            int currYear = pc.GetYear(DateTime.Now);
            int currMonth = pc.GetMonth(DateTime.Now);
            int currDay = pc.GetDayOfMonth(DateTime.Now);

            int? existYear = null;
            int? existMonth = null;
            int? existDay = null;

            var list = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == workshopId).OrderByDescending(i => i.InvoiceID).ToList();
            var Registrationlist = db.tbl_VehicleRegistrations.Where(i => i.RegisterStatus == true && i.WorkshopID == workshopId).OrderBy(i => i.CreateDate).ToList();
            //End------------------------------------------------new code for 30 day-------------------------------------------------------------------


            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                SqlDataReader reader;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    //
                    if (list.Count() > 0)
                    {
                        invoiceCode = (Convert.ToDouble(list.FirstOrDefault().InvoiceCode) + 1).ToString();
                        existYear = pc.GetYear(list.FirstOrDefault().CreatedDate.Value);
                        existMonth = pc.GetMonth(list.FirstOrDefault().CreatedDate.Value);
                        existDay = pc.GetDayOfMonth(list.FirstOrDefault().CreatedDate.Value);

                        //--------------begin code for added invoice---------------------------
                        //if (!list.FirstOrDefault().Description.Contains("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                        //if (string.IsNullOrEmpty(list.FirstOrDefault().Description))
                        //{
                        //    for (int i = 0; i < 2; i++)
                        //    {
                        //        invoiceCode = (Convert.ToDouble(list.FirstOrDefault().InvoiceCode) + 1).ToString();
                        //        existYear = pc.GetYear(list.FirstOrDefault().CreatedDate.Value);
                        //        existMonth = pc.GetMonth(list.FirstOrDefault().CreatedDate.Value);
                        //        existDay = pc.GetDayOfMonth(list.FirstOrDefault().CreatedDate.Value);
                        //        //
                        //        if (i == 0)
                        //        {
                        //            fromdateInString = "2020-03-20";
                        //            todateInString = "2020-09-21";
                        //        }
                        //        else
                        //        {
                        //            invoiceCode = (Convert.ToInt32(invoiceCode) + 1).ToString();
                        //            fromdateInString = "2020-09-22";
                        //            todateInString = "2020-11-20";
                        //        }

                        //        SqlConnection cnn = new SqlConnection(connStr);
                        //        SqlCommand cmd = new SqlCommand();
                        //        cmd.Connection = cnn;
                        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //        cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                        //        cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                        //        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                        //        cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                        //        cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                        //        cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                        //        //add any parameters the stored procedure might require
                        //        cnn.Open();
                        //        object o = cmd.ExecuteScalar();
                        //        cnn.Close();
                        //    }

                        //}
                        //--------------end code for added invoice---------------------------

                        //--------------------for curr year is equal exist year--------------------------------------
                        if (currYear == existYear && currMonth > existMonth)
                        {
                            if (existMonth != currMonth - 1)
                            {
                                Month = (existMonth + 1).ToString();

                                if (currMonth <= 6)
                                {
                                    Year = currYear;
                                    //Month = (currMonth - 1).ToString();
                                    Day = "31";
                                }
                                if (currMonth > 6 && currMonth < 12)
                                {
                                    Year = currYear;
                                    //Month = (currMonth - 1).ToString();
                                    Day = "30";
                                }
                                if (currMonth == 12 && currDay == 29)
                                {
                                    Year = currYear;
                                    Month = "12";
                                    Day = "29";
                                }
                                //---------------------------------------------from date-----------------------------------------------------------
                                if (existDay < 30)
                                {
                                    if (existMonth < 10)
                                        Month2 = "0" + existMonth.ToString();
                                    if (existMonth >= 10 && existMonth < 12)
                                        Month2 = existMonth.ToString();
                                    if (existMonth == 12)
                                        Month2 = existMonth.ToString();
                                    //
                                    if (existDay < 10)
                                        Day2 = "0" + (Convert.ToInt32(existDay) + 1).ToString();
                                    if (existDay >= 10 && existDay < 31)
                                        Day2 = (Convert.ToInt32(existDay) + 1).ToString();
                                    if (existMonth == 12)
                                        Day2 = "29";

                                }
                                else
                                {
                                    if (existMonth < 10)
                                        Month2 = "0" + (Convert.ToInt32(existMonth) + 1).ToString();
                                    if (existMonth >= 10 && existMonth < 12)
                                        Month2 = (Convert.ToInt32(existMonth) + 1).ToString();
                                    if (existMonth == 12)
                                        Month2 = existMonth.ToString();
                                    //
                                    //if (existDay < 10)
                                    Day2 = "01";//+ existDay.ToString();
                                                //if (existDay >= 10 && existDay < 31)
                                                //    Day2 = existDay.ToString();
                                                //if (existMonth == 12)
                                                //    Day2 = "29";

                                }

                                if (Convert.ToInt32(Month2) > 9)
                                    fromdateInString = existYear + "/" + Month2.TrimStart('0') + "/" + Day2;
                                else
                                    fromdateInString = existYear + "/" + Month2 + "/" + Day2;

                                DateTime dt1 = new DateTime(Convert.ToInt32(existYear), Convert.ToInt32(Month2), Convert.ToInt32(Day2), pc);
                                fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                //string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                //DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                //string dt1 = FormatDateTimeAsGregorian(dtt1);
                                //DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                                //fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                //fromdateInString = fromdateInString.Replace("/", "-");
                                //---------------------------------------------to date-----------------------------------------------------------
                                if (Convert.ToInt32(Month) < 10)
                                    Month = "0" + Month;
                                else
                                    Month = Month.ToString();
                                //////////////////////////////////////////////////////
                                if (Convert.ToInt32(Month) <= 6)
                                    Day = "31";
                                if (Convert.ToInt32(Month) >= 7 && Convert.ToInt32(Month) <= 11)
                                    Day = "30";
                                if (Convert.ToInt32(Month) == 12)
                                    Day = "29";

                                if (Convert.ToInt32(Month) > 9)
                                    todateInString = Year + "/" + Month.TrimStart('0') + "/" + Day;
                                else
                                    todateInString = Year + "/" + Month + "/" + Day;

                                DateTime dt2 = new DateTime(Year, Convert.ToInt32(Month), Convert.ToInt32(Day), pc);
                                todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                                //string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                //DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                //string dttt = FormatDateTimeAsGregorian(dtt);
                                //DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                                //todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                //todateInString = todateInString.Replace("/", "-");

                                //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                                SqlConnection cnn = new SqlConnection(connStr);
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cnn;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                                cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                                cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                                //add any parameters the stored procedure might require
                                cnn.Open();
                                object o = cmd.ExecuteScalar();
                                cnn.Close();
                                //----------------------
                            }
                        }

                        //--------------------for curr year is great than exist year--------------------------------------
                        if (currYear > existYear && existMonth == 12)
                        {
                            Year = currYear;
                            Month = "01";
                            Day = "1";

                            fromdateInString = Year + "/" + Month + "/" + "01";
                            string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                            DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            string dt1 = FormatDateTimeAsGregorian(dtt1);
                            DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                            fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            fromdateInString = fromdateInString.Replace("/", "-");
                            //---------------------------------------------to date-----------------------------------------------------------
                            todateInString = Year + "/" + Month + "/" + "31";

                            string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                            DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            string dttt = FormatDateTimeAsGregorian(dtt);
                            DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                            todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            todateInString = todateInString.Replace("/", "-");

                            //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                            cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                        }
                    }
                    //
                    else if (list.Count() == 0)
                    {
                        if (existYear == null)
                            existYear = 1399;
                        if (currMonth <= 6)
                        {
                            Year = currYear;
                            Month = (currMonth - 1).ToString();
                            Day = "31";
                        }
                        if (currMonth > 6 && currMonth <= 12)
                        {
                            Year = currYear;
                            Month = (currMonth - 1).ToString();
                            Day = "30";
                        }
                        if (currMonth == 12 && currDay == 29)
                        {
                            Year = currYear;
                            Month = "12";
                            Day = "29";
                        }
                        DateTime dt1 = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32("01"), pc);
                        fromdateInString = dt1.ToString(CultureInfo.InvariantCulture);

                        DateTime dt2 = new DateTime(Year, Convert.ToInt32(Month), Convert.ToInt32(Day), pc);
                        todateInString = dt2.ToString(CultureInfo.InvariantCulture);

                        //string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                        //DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        //string dt1 = FormatDateTimeAsGregorian(dtt1);
                        //DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                        fromdateInString = dt1.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                        fromdateInString = fromdateInString.Replace("/", "-");

                        //string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                        //DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        //string dttt = FormatDateTimeAsGregorian(dtt);
                        //DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                        todateInString = dt2.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                        todateInString = todateInString.Replace("/", "-");

                        //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                        SqlConnection cnn = new SqlConnection(connStr);
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                        cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                        cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                        cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                        cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                        //add any parameters the stored procedure might require
                        cnn.Open();
                        object o = cmd.ExecuteScalar();
                        cnn.Close();
                        //----------------------
                    }
                    conn.Close(); 
                }//end using
            }
            catch
            {

            }
            //
            var connStr2 = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader2;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesFapaList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    //command.Parameters.Add("@year", SqlDbType.VarChar).Value = Year;
                    conn.Open();
                    reader2 = command.ExecuteReader();

                    while (reader2.Read())
                    {
                        InvoiceCode = reader2["InvoiceCode"].ToString();
                        CreatedDate = Convert.ToDateTime(reader2["CreatedDate"].ToString()).ToShortDateString();
                        FromDate = Convert.ToDateTime(reader2["EmployerEconomicalnumber"].ToString()).ToShortDateString();
                        // EmployerTitle = reader["EmployerTitle"].ToString();
                        DepartmentTitle = reader2["Description"].ToString();
                        Title = reader2["Title"].ToString();
                        Status = reader2["Status"].ToString();
                        TotalAmount = reader2["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";
                        DefectsCount = reader2["DefectsCount"].ToString();

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            FromDate = FromDate,
                            //EmployerTitle = EmployerTitle,
                            DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##")),
                            DefectsCount = DefectsCount
                        });
                    }
                    conn.Close();
                }
            }
           
            //
            ViewBag.TableOut = TableOuts;

            return View();
        }
        public ActionResult InvoicesListTotal(int? workshopId, string workshopTitle)
        {
            ViewBag.workshopTitle = workshopTitle;
            ViewBag.workshopId = workshopId;
            var userName = Helper.Helpers.GetCurrentUser();
            string permission = db.tbl_Users.Where(u => u.Username == userName).SingleOrDefault().WorkshopID.ToString();
            string InvoiceCode = "";
            string CreatedDate = "";
            string EmployerTitle = "";
            //string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            //
            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    //command.Parameters.Add("@year", SqlDbType.VarChar).Value = Year;

                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = reader["CreatedDate"].ToString();
                        EmployerTitle = reader["EmployerTitle"].ToString();
                        //DepartmentTitle = reader["Title"].ToString();
                        Title = reader["Title"].ToString();
                        Status = reader["Status"].ToString();
                        TotalAmount = reader["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            EmployerTitle = EmployerTitle,
                            //DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##"))
                        });


                    }
                }
                conn.Close();
            }//end using
            ViewBag.TableOut = TableOuts;

            return View();
        }
        //
        public static string FormatDateTimeAsGregorian(DateTime input)
        {
            return input.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss", CultureInfo.InvariantCulture);
        }
        //
        //
        public ActionResult InvoicesFapaListTotal(int? workshopId, string workshopTitle)
        {
            //var users = db.tbl_Users.Where(u => u.WorkshopID==workshopId).ToList();
            //int userId = 0;
            //if (users.Count() > 1)
            //    userId = users.FirstOrDefault().UserID;
            //else
            //    userId = users.SingleOrDefault().UserID;
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            //
            ViewBag.workshopTitle = workshopTitle;
            ViewBag.workshopId = workshopId;
            var userName = Helper.Helpers.GetCurrentUser();
            string InvoiceCode = "";
            string CreatedDate = "";
            string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            string TotalAmount2 = "";
            string FromDate = "";
            string CheckStatus = "در حال بررسی";
            string FinancialStatus = "در حال بررسی";
            string ReciveStatus = "در حال بررسی";
            //
            int Year = pc.GetYear(DateTime.Now);
            DateTime dt = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            DateTime oneDueDate = Convert.ToDateTime("1399/04/31");
            string fromdateInString = "1399/03/01";
            string todateInString = DateTime.Now.ToShortDateString();
            string DefectsCount = "0";
            //
            string invoiceCode = "1";
            //int diffDay = 0;            
            string Month = "01";
            string Day = "30";
            string Month2 = "01";
            string Day2 = "01";
            //Begin------------------------------------------------new code for 30 day-------------------------------------------------------------------
            int currYear = pc.GetYear(DateTime.Now) - 1;
            int currMonth = pc.GetMonth(DateTime.Now) - 1;
            int currDay = pc.GetDayOfMonth(DateTime.Now);

            int? existYear = null;
            int? existMonth = null;
            int? existDay = null;
            int TotalCount = 0;
            int SumTotalCount = 0;
            double SumTotalAmount = 0.0;
            double SumTotalAmount2 = 0.0;
            double SumDoubledInvoice = 0.0;

            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();

            var list = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == workshopId).OrderByDescending(i => i.InvoiceID).ToList();
            var Registrationlist = db.tbl_VehicleRegistrations.Where(i => i.RegisterStatus == true && i.WorkshopID == workshopId).OrderBy(i => i.CreateDate).ToList();
            //End------------------------------------------------new code for 30 day-------------------------------------------------------------------
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                SqlDataReader reader;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    try
                    {
                        if (list.Count() > 0)
                        {
                            invoiceCode = (Convert.ToDouble(list.FirstOrDefault().InvoiceCode) + 1).ToString();
                            existYear = pc.GetYear(list.FirstOrDefault().CreatedDate.Value);
                            existMonth = pc.GetMonth(list.FirstOrDefault().CreatedDate.Value);
                            existDay = pc.GetDayOfMonth(list.FirstOrDefault().CreatedDate.Value);

                            //--------------begin code for added invoice---------------------------
                            //if (!list.FirstOrDefault().Description.Contains("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                            //if (string.IsNullOrEmpty(list.FirstOrDefault().Description))
                            //{
                            //    for (int i = 0; i < 2; i++)
                            //    {
                            //        invoiceCode = (Convert.ToDouble(list.FirstOrDefault().InvoiceCode) + 1).ToString();
                            //        existYear = pc.GetYear(list.FirstOrDefault().CreatedDate.Value);
                            //        existMonth = pc.GetMonth(list.FirstOrDefault().CreatedDate.Value);
                            //        existDay = pc.GetDayOfMonth(list.FirstOrDefault().CreatedDate.Value);
                            //        //
                            //        if (i == 0)
                            //        {
                            //            fromdateInString = "2020-03-20";
                            //            todateInString = "2020-09-21";
                            //        }
                            //        else
                            //        {
                            //            invoiceCode = (Convert.ToInt32(invoiceCode) + 1).ToString();
                            //            fromdateInString = "2020-09-22";
                            //            todateInString = "2020-11-20";
                            //        }

                            //        SqlConnection cnn = new SqlConnection(connStr);
                            //        SqlCommand cmd = new SqlCommand();
                            //        cmd.Connection = cnn;
                            //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            //        cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                            //        cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                            //        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            //        cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            //        cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                            //        cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                            //        //add any parameters the stored procedure might require
                            //        cnn.Open();
                            //        object o = cmd.ExecuteScalar();
                            //        cnn.Close();
                            //    }

                            //}
                            //--------------end code for added invoice---------------------------

                            //--------------------for curr year is equal exist year--------------------------------------
                            if (currYear == existYear && currMonth > existMonth)
                            {
                                if (existMonth != currMonth - 1)
                                {
                                    Month = (existMonth + 1).ToString();

                                    if (currMonth <= 6)
                                    {
                                        Year = currYear;
                                        //Month = (currMonth - 1).ToString();
                                        Day = "31";
                                    }
                                    if (currMonth > 6 && currMonth < 12)
                                    {
                                        Year = currYear;
                                        //Month = (currMonth - 1).ToString();
                                        Day = "30";
                                    }
                                    if (currMonth == 12 && currDay == 29)
                                    {
                                        Year = currYear;
                                        Month = "12";
                                        Day = "29";
                                    }
                                    //---------------------------------------------from date-----------------------------------------------------------
                                    if (existDay < 30)
                                    {
                                        if (existMonth < 10)
                                            Month2 = "0" + existMonth.ToString();
                                        if (existMonth >= 10 && existMonth < 12)
                                            Month2 = existMonth.ToString();
                                        if (existMonth == 12)
                                            Month2 = existMonth.ToString();
                                        //
                                        if (existDay < 10)
                                            Day2 = "0" + (Convert.ToInt32(existDay) + 1).ToString();
                                        if (existDay >= 10 && existDay < 31)
                                            Day2 = (Convert.ToInt32(existDay) + 1).ToString();
                                        if (existMonth == 12)
                                            Day2 = "29";

                                    }
                                    else
                                    {
                                        if (existMonth < 10)
                                            Month2 = "0" + (Convert.ToInt32(existMonth) + 1).ToString();
                                        if (existMonth >= 10 && existMonth < 12)
                                            Month2 = (Convert.ToInt32(existMonth) + 1).ToString();
                                        if (existMonth == 12)
                                            Month2 = existMonth.ToString();
                                        //
                                        //if (existDay < 10)
                                        Day2 = "01";//+ existDay.ToString();
                                                    //if (existDay >= 10 && existDay < 31)
                                                    //    Day2 = existDay.ToString();
                                                    //if (existMonth == 12)
                                                    //    Day2 = "29";

                                    }

                                    if (Convert.ToInt32(Month2) > 9)
                                        fromdateInString = existYear + "/" + Month2.TrimStart('0') + "/" + Day2;
                                    else
                                        fromdateInString = existYear + "/" + Month2 + "/" + Day2;

                                    DateTime dt1 = new DateTime(Convert.ToInt32(existYear), Convert.ToInt32(Month2), Convert.ToInt32(Day2), pc);
                                    fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                    //DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string dt1 = FormatDateTimeAsGregorian(dtt1);
                                    //DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                                    //fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                    //fromdateInString = fromdateInString.Replace("/", "-");
                                    //---------------------------------------------to date-----------------------------------------------------------
                                    if (Convert.ToInt32(Month) < 10)
                                        Month = "0" + Month;
                                    else
                                        Month = Month.ToString();
                                    //////////////////////////////////////////////////////
                                    if (Convert.ToInt32(Month) <= 6)
                                        Day = "31";
                                    if (Convert.ToInt32(Month) >= 7 && Convert.ToInt32(Month) <= 11)
                                        Day = "30";
                                    if (Convert.ToInt32(Month) == 12)
                                        Day = "29";

                                    if (Convert.ToInt32(Month) > 9)
                                        todateInString = Year + "/" + Month.TrimStart('0') + "/" + Day;
                                    else
                                        todateInString = Year + "/" + Month + "/" + Day;

                                    DateTime dt2 = new DateTime(Year, Convert.ToInt32(Month), Convert.ToInt32(Day), pc);
                                    todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                                    //string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                    //DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string dttt = FormatDateTimeAsGregorian(dtt);
                                    //DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                                    //todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                    //todateInString = todateInString.Replace("/", "-");

                                    //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                                    SqlConnection cnn = new SqlConnection(connStr);
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cnn;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                                    cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                    cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                                    cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                                    cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                                    //add any parameters the stored procedure might require
                                    cnn.Open();
                                    object o = cmd.ExecuteScalar();
                                    cnn.Close();
                                    //----------------------
                                }
                            }

                            //--------------------for curr year is great than exist year--------------------------------------
                            if (currYear > existYear && existMonth == 12)
                            {
                                Year = currYear;
                                Month = "01";
                                Day = "1";

                                fromdateInString = Year + "/" + Month + "/" + "01";
                                string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                string dt1 = FormatDateTimeAsGregorian(dtt1);
                                DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                                fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                fromdateInString = fromdateInString.Replace("/", "-");
                                //---------------------------------------------to date-----------------------------------------------------------
                                todateInString = Year + "/" + Month + "/" + "31";

                                string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                string dttt = FormatDateTimeAsGregorian(dtt);
                                DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                                todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                todateInString = todateInString.Replace("/", "-");

                                //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                                SqlConnection cnn = new SqlConnection(connStr);
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cnn;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                                cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                                cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                                //add any parameters the stored procedure might require
                                cnn.Open();
                                object o = cmd.ExecuteScalar();
                                cnn.Close();
                                //----------------------
                            }

                        }
                        //
                        if (list.Count() == 0)
                        {
                            invoiceCode = "1";
                            existYear = pc.GetYear(Registrationlist.FirstOrDefault().CreateDate);
                            existMonth = pc.GetMonth(Registrationlist.FirstOrDefault().CreateDate);
                            //--------------------for curr year is equal exist year--------------------------------------
                            if (currYear == existYear && currMonth > existMonth)
                            {
                                if (existMonth != currMonth - 1)
                                {
                                    if (list.Count() == 0)
                                        Month = existMonth.Value.ToString();
                                    else
                                        Month = (existMonth + 1).ToString();

                                    if (Convert.ToInt32(Month) <= 6)
                                    {
                                        Year = currYear;
                                        //Month = (currMonth - 1).ToString();
                                        Day = "31";
                                    }
                                    if (Convert.ToInt32(Month) > 6 && Convert.ToInt32(Month) <= 12)
                                    {
                                        Year = currYear;
                                        //Month = (currMonth - 1).ToString();
                                        Day = "30";
                                    }
                                    if (Convert.ToInt32(Month) == 12 && currDay == 29)
                                    {
                                        Year = currYear;
                                        Month = "12";
                                        Day = "29";
                                    }
                                    //---------------------------------------------from date-----------------------------------------------------------
                                    if (existDay < 30)
                                    {
                                        if (existMonth < 10)
                                            Month2 = "0" + existMonth.ToString();
                                        if (existMonth >= 10 && existMonth < 12)
                                            Month2 = existMonth.ToString();
                                        if (existMonth == 12)
                                            Month2 = existMonth.ToString();
                                        //
                                        if (existDay < 10)
                                            Day2 = "0" + (Convert.ToInt32(existDay) + 1).ToString();
                                        if (existDay >= 10 && existDay < 31)
                                            Day2 = (Convert.ToInt32(existDay) + 1).ToString();
                                        if (existMonth == 12)
                                            Day2 = "29";

                                    }
                                    else
                                    {
                                        if (list.Count() == 0)
                                        {
                                            if (existMonth < 10)
                                                Month2 = "0" + existMonth.ToString();
                                            if (existMonth >= 10 && existMonth < 12)
                                                Month2 = existMonth.ToString();
                                            if (existMonth == 12)
                                                Month2 = existMonth.ToString();
                                        }
                                        else
                                        {
                                            if (existMonth < 10)
                                                Month2 = "0" + (Convert.ToInt32(existMonth) + 1).ToString();
                                            if (existMonth >= 10 && existMonth < 12)
                                                Month2 = (Convert.ToInt32(existMonth) + 1).ToString();
                                            if (existMonth == 12)
                                                Month2 = existMonth.ToString();
                                        }

                                        //
                                        //if (existDay < 10)
                                        Day2 = "01";//+ existDay.ToString();
                                                    //if (existDay >= 10 && existDay < 31)
                                                    //    Day2 = existDay.ToString();
                                                    //if (existMonth == 12)
                                                    //    Day2 = "29";

                                    }

                                    if (Convert.ToInt32(Month2) > 9)
                                        fromdateInString = existYear + "/" + Month2.TrimStart('0') + "/" + Day2;
                                    else
                                        fromdateInString = existYear + "/" + Month2 + "/" + Day2;

                                    DateTime dt1 = new DateTime(Convert.ToInt32(existYear), Convert.ToInt32(Month2), Convert.ToInt32(Day2), pc);
                                    fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                    //DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string dt1 = FormatDateTimeAsGregorian(dtt1);
                                    //DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                                    //fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                    //fromdateInString = fromdateInString.Replace("/", "-");
                                    //---------------------------------------------to date-----------------------------------------------------------
                                    if (Convert.ToInt32(Month) < 10)
                                        Month = "0" + Month;
                                    else
                                        Month = Month.ToString();

                                    if (Convert.ToInt32(Month) <= 6)
                                        Day = "31";
                                    else
                                        Day = "30";

                                    if (Convert.ToInt32(Month) > 9)
                                        todateInString = Year + "/" + Month.TrimStart('0') + "/" + Day;
                                    else
                                        todateInString = Year + "/" + Month + "/" + Day;

                                    DateTime dt2 = new DateTime(Year, Convert.ToInt32(Month), Convert.ToInt32(Day), pc);
                                    todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                                    //string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                    //DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                    //string dttt = FormatDateTimeAsGregorian(dtt);
                                    //DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                                    //todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                    //todateInString = todateInString.Replace("/", "-");

                                    //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                                    SqlConnection cnn = new SqlConnection(connStr);
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cnn;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                                    cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                    cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                                    cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                                    cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                                    //add any parameters the stored procedure might require
                                    cnn.Open();
                                    object o = cmd.ExecuteScalar();
                                    cnn.Close();
                                    //----------------------
                                }
                            }

                            //--------------------for curr year is great than exist year--------------------------------------
                            if (currYear > existYear && existMonth == 12)
                            {
                                Year = currYear;
                                Month = "01";
                                Day = "1";

                                fromdateInString = Year + "/" + Month + "/" + "01";
                                string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                string dt1 = FormatDateTimeAsGregorian(dtt1);
                                DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                                fromdateInString = dt221.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                fromdateInString = fromdateInString.Replace("/", "-");
                                //---------------------------------------------to date-----------------------------------------------------------
                                todateInString = Year + "/" + Month + "/" + "31";

                                string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                                DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                                string dttt = FormatDateTimeAsGregorian(dtt);
                                DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                                todateInString = dt22.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                                todateInString = todateInString.Replace("/", "-");

                                //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                                SqlConnection cnn = new SqlConnection(connStr);
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cnn;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                                cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                                cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                                cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                                //add any parameters the stored procedure might require
                                cnn.Open();
                                object o = cmd.ExecuteScalar();
                                cnn.Close();
                                //----------------------
                            }

                        }
                        //
                        else if (list.Count() == 0)
                        {
                            if (existYear == null)
                                existYear = 1399;
                            if (currMonth <= 6)
                            {
                                Year = currYear;
                                Month = (currMonth - 1).ToString();
                                Day = "31";
                            }
                            if (currMonth > 6 && currMonth <= 12)
                            {
                                Year = currYear;
                                Month = (currMonth - 1).ToString();
                                Day = "30";
                            }
                            if (currMonth == 12 && currDay == 29)
                            {
                                Year = currYear;
                                Month = "12";
                                Day = "29";
                            }
                            DateTime dt1 = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32("01"), pc);
                            fromdateInString = dt1.ToString(CultureInfo.InvariantCulture);

                            DateTime dt2 = new DateTime(Year, Convert.ToInt32(Month), Convert.ToInt32(Day), pc);
                            todateInString = dt2.ToString(CultureInfo.InvariantCulture);

                            //string date = Regex.Replace(fromdateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                            //DateTime dtt1 = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            //string dt1 = FormatDateTimeAsGregorian(dtt1);
                            //DateTime dt221 = pc.ToDateTime(dtt1.Year, dtt1.Month, dtt1.Day, dtt1.Hour, dtt1.Minute, dtt1.Second, dtt1.Millisecond);
                            fromdateInString = dt1.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            fromdateInString = fromdateInString.Replace("/", "-");

                            //string date2 = Regex.Replace(todateInString, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
                            //DateTime dtt = DateTime.ParseExact(date2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            //string dttt = FormatDateTimeAsGregorian(dtt);
                            //DateTime dt22 = pc.ToDateTime(dtt.Year, dtt.Month, dtt.Day, dtt.Hour, dtt.Minute, dtt.Second, dtt.Millisecond);
                            todateInString = dt2.ToString("yyyy/MM/dd hh:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-us", "en"));
                            todateInString = todateInString.Replace("/", "-");

                            //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString));
                            cmd.Parameters.Add(new SqlParameter("@todate", todateInString));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                        }
                        //
                    }
                    catch
                    {
                        
                    }                   
                    conn.Close();
                }//end using

            }
            catch
            {
                //TableOuts = null;                
            }
            //
            var connStr2 = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader2;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesFapaList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    //command.Parameters.Add("@year", SqlDbType.VarChar).Value = Year;
                    conn.Open();
                    reader2 = command.ExecuteReader();

                    while (reader2.Read())
                    {
                        InvoiceCode = reader2["InvoiceCode"].ToString();
                        CreatedDate = Convert.ToDateTime(reader2["CreatedDate"].ToString()).ToShortDateString();
                        FromDate = Convert.ToDateTime(reader2["EmployerEconomicalnumber"].ToString()).ToShortDateString();
                        // EmployerTitle = reader2["EmployerTitle"].ToString();
                        DepartmentTitle = reader2["Description"].ToString();
                        Title = reader2["Title"].ToString();
                        Status = reader2["Status"].ToString();
                        TotalAmount = reader2["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";
                        SumTotalAmount += Convert.ToDouble(TotalAmount);
                        TotalAmount2 = reader2["TotalAmount2"].ToString();
                        if (TotalAmount2 == "")
                            TotalAmount2 = "0";
                        SumTotalAmount2 += Convert.ToDouble(TotalAmount2);
                        DefectsCount = reader2["DefectsCount"].ToString();
                        TotalCount = Convert.ToInt32(reader2["TotalCount"].ToString());
                        if (!DepartmentTitle.Equals("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                            SumTotalCount += TotalCount;
                        CheckStatus = reader2["CheckStatus"].ToString();
                        FinancialStatus = reader2["FinancialStatus"].ToString();
                        ReciveStatus = reader2["ReciveStatus"].ToString();

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            FromDate = FromDate,
                            DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##")),
                            TotalAmount2 = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount2)).ToString("#,##.##")),
                            DefectsCount = DefectsCount,
                            TotalCount = TotalCount,
                            CheckStatus = CheckStatus,
                            FinancialStatus = FinancialStatus,
                            ReciveStatus = ReciveStatus
                        });
                    }
                }
            }
            //
            ViewBag.TableOut = TableOuts;
            ViewBag.SumTotalCount = SumTotalCount;
            ViewBag.SumTotalAmount = SumTotalAmount.ToString("#,##");
            ViewBag.SumTotalAmount2 = SumTotalAmount2.ToString("#,##");
            return View();
        }
        //
        public ActionResult InvoicesDamagesList(int? workshopId, string workshopTitle)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            ViewBag.workshopTitle = workshopTitle;
            ViewBag.workshopId = workshopId;
            var userName = Helper.Helpers.GetCurrentUser();
            string InvoiceCode = "";
            string CreatedDate = "";
            string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            string TotalAmount2 = "";
            string FromDate = "";
            string CheckStatus = "در حال بررسی";
            string FinancialStatus = "در حال بررسی";
            string ReciveStatus = "در حال بررسی";
            //
            int Year = pc.GetYear(DateTime.Now);
            DateTime dt = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            DateTime oneDueDate = Convert.ToDateTime("1399/04/31");
            string todateInString = DateTime.Now.ToShortDateString();
            string DefectsCount = "0";
            //
            int TotalCount = 0;
            int SumTotalCount = 0;
            double SumTotalAmount = 0.0;
            double SumTotalAmount2 = 0.0;

            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();
            //
            var connStr2 = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesDamagesList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = reader["Year"].ToString();
                        FromDate = reader["Month"].ToString();
                        DepartmentTitle = reader["Description"].ToString();
                        Title = reader["Title"].ToString();
                        Status = reader["Status"].ToString();
                        TotalAmount = reader["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";
                        SumTotalAmount += Convert.ToDouble(TotalAmount);
                        TotalAmount2 = reader["TotalAmount2"].ToString();
                        if (TotalAmount2 == "")
                            TotalAmount2 = "0";
                        SumTotalAmount2 += Convert.ToDouble(TotalAmount2);
                        DefectsCount = reader["DefectsCount"].ToString();
                        TotalCount = Convert.ToInt32(reader["TotalCount"].ToString());
                        if (!DepartmentTitle.Equals("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                            SumTotalCount += TotalCount;
                        CheckStatus = reader["CheckStatus"].ToString();
                        FinancialStatus = reader["FinancialStatus"].ToString();
                        ReciveStatus = reader["ReciveStatus"].ToString();

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            FromDate = FromDate,
                            DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##")),
                            TotalAmount2 = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount2)).ToString("#,##.##")),
                            DefectsCount = DefectsCount,
                            TotalCount = TotalCount,
                            CheckStatus = CheckStatus,
                            FinancialStatus = FinancialStatus,
                            ReciveStatus = ReciveStatus
                        });
                    }
                }
            }
            //
            ViewBag.TableOut = TableOuts;
            ViewBag.SumTotalCount = SumTotalCount;
            ViewBag.SumTotalAmount = SumTotalAmount.ToString("#,##");
            ViewBag.SumTotalAmount2 = SumTotalAmount2.ToString("#,##");
            return View();
        }
        public class MyList
        {
            public int WorkshopId { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
        }//
        public class InvoicesTableList
        {
            public string InvoiceCode { get; set; }
            public string CreatedDate { get; set; }
            public string FromDate { get; set; }
            public string EmployerTitle { get; set; }
            public string Title { get; set; }
            public string Status { get; set; }
            public string TotalAmount { get; set; }
            public string TotalAmount2 { get; set; }
            public string DepartmentTitle { get; set; }
            public string DefectsCount { get; set; }
            public int TotalCount { get; set; }
            public string CheckStatus { get; set; }
            public string FinancialStatus { get; set; }
            public string ReciveStatus { get; set; }
        }
        //
        // //تغییر وضعیت فاکتورهای صادر شده
        public ActionResult InvoiceStatus(string id, int? Year)
        {
            ViewBag.InvoiceCode = id;
            ViewBag.Year = Year;
            return PartialView();
        }
        public JsonResult ChangeInvoiceStatus(string InvoiceCode, int? Year, bool? Status)
        {
            var changeStatus = db.tbl_Invoices.Where(i => i.InvoiceCode.Equals(InvoiceCode)).ToList();
            foreach (var item in changeStatus)
            {
                item.Status = Status;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json("فاکتور شما ابطال شد", JsonRequestBehavior.AllowGet);
        }
        //
        public ActionResult SelectPrintType(string InvoiceCode)
        {
            ViewBag.InvoiceCode = Convert.ToInt32(InvoiceCode);
            return PartialView();
        }
        //
        public ActionResult InvoicetoPrint(int InvoiceCode, int? WorkshopID, int Type)
        {            
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.WorkshopId = WorkshopID;
            ViewBag.Type = Type;
            int? workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            if (WorkshopID == null)
                WorkshopID = workshopId;
            var invoice = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == WorkshopID && i.InvoiceCode.Equals(InvoiceCode.ToString())).OrderBy(i=>i.CreatedDate);
            string fromdateInString = invoice.FirstOrDefault().EmployerEconomicalnumber;
            DateTime todateInString = invoice.FirstOrDefault().CreatedDate.Value;
            int fYear = pc.GetYear(Convert.ToDateTime(fromdateInString));
            int fMonth = pc.GetMonth(Convert.ToDateTime(fromdateInString));
            int fDay = pc.GetDayOfMonth(Convert.ToDateTime(fromdateInString));

            DateTime Pfromdate = new DateTime(fYear, fMonth, fDay);
            string p2fromdate = Pfromdate.ToString(CultureInfo.InvariantCulture);
            string p2fMonth = "01";
            string p2fDay = "01";
            string p2tMonth = "01";
            string p2tDay = "01";
            if (Pfromdate.Month < 10)
                p2fMonth = "0" + Pfromdate.Month;
            else
                p2fMonth = Pfromdate.Month.ToString();

            if (Pfromdate.Day < 10)
                p2fDay = "0" + Pfromdate.Day;
            else
                p2fDay = (Pfromdate.Day).ToString();

            string p3fromdate = Pfromdate.Year + "-" + p2fMonth + "-" + p2fDay;
            //
            if (todateInString.Month < 10)
                p2tMonth = "0" + todateInString.Month;
            else
                p2tMonth = todateInString.Month.ToString();

            if (todateInString.Day < 10)
                p2tDay = "0" + todateInString.Day;
            else
                p2tDay = (todateInString.Day).ToString();
            string todate = todateInString.Year + "-" + p2tMonth + "-" + p2tDay;
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[sp_LockedVehicleEdit]";
            cmd.Parameters.Add(new SqlParameter("@invoiceCode", InvoiceCode));
            cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
            cmd.Parameters.Add(new SqlParameter("@fromdate",p3fromdate ));
            cmd.Parameters.Add(new SqlParameter("@todate",todate ));
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            //
            return View();
            //return RedirectToAction("InvoicetoPrint",new { InvoiceCode= InvoiceCode, Type= Type });
        }
        //
        public ActionResult PrintInvoice(int InvoiceCode, int? workshopId, string fromDate,string toDate)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            //int? workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            var invoice = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode.ToString())).OrderBy(i => i.CreatedDate);
            string fromdateInString = invoice.FirstOrDefault().EmployerEconomicalnumber;
            DateTime todateInString = invoice.FirstOrDefault().CreatedDate.Value;
            int fYear = pc.GetYear(Convert.ToDateTime(fromdateInString));
            int fMonth = pc.GetMonth(Convert.ToDateTime(fromdateInString));
            int fDay = pc.GetDayOfMonth(Convert.ToDateTime(fromdateInString));

            DateTime Pfromdate = new DateTime(fYear, fMonth, fDay);
            string p2fromdate = Pfromdate.ToString(CultureInfo.InvariantCulture);
            string p2fMonth = "01";
            string p2fDay = "01";
            string p2tMonth = "01";
            string p2tDay = "01";
            if (Pfromdate.Month < 10)
                p2fMonth = "0" + Pfromdate.Month;
            else
                p2fMonth = Pfromdate.Month.ToString();

            if (Pfromdate.Day < 10)
                p2fDay = "0" + Pfromdate.Day;
            else
                p2fDay = (Pfromdate.Day).ToString();

            string p3fromdate = Pfromdate.Year + "-" + p2fMonth + "-" + p2fDay;
            //
            if (todateInString.Month < 10)
                p2tMonth = "0" + todateInString.Month;
            else
                p2tMonth = todateInString.Month.ToString();

            if (todateInString.Day < 10)
                p2tDay = "0" + todateInString.Day;
            else
                p2tDay = (todateInString.Day).ToString();
            string todate = todateInString.Year + "-" + p2tMonth + "-" + p2tDay;
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[sp_LockedVehicleEdit]";
            cmd.Parameters.Add(new SqlParameter("@invoiceCode", InvoiceCode));
            cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
            cmd.Parameters.Add(new SqlParameter("@fromdate", p3fromdate));
            cmd.Parameters.Add(new SqlParameter("@todate", todate));
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            //
            //var report = new ActionAsPdf("InvoiceFapaPage", new { InvoiceCode = InvoiceCode, workshopId = workshopId, Type = 1 });
            //string 
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("InvoiceFapaPage", new { InvoiceCode = InvoiceCode, workshopId = workshopId, fromDate= fromDate, toDate= toDate, Type = 1 })
            {
                //FileName = Server.MapPath("~/Content/Relato.pdf"),
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches=footer
            };
            //return report;
        }
        //
        public ActionResult GetReport(int InvoiceCode, int Type, int? WorkshopID)
        {
            if (WorkshopID == null)
                WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
            if (InvoiceCode < 1)
            {
                InvoiceCode = 0;
            }
            StiReport report = new StiReport();
            string Path = Server.MapPath("~/StimulSoftReports/InvoiceReport.mrt");
            if (Type == 1)
                Path = Server.MapPath("~/StimulSoftReports/InvoiceReport.mrt");
            if (Type == 2)
                Path = Server.MapPath("~/StimulSoftReports/InvoiceReport2.mrt");
            report.Load(Path);
            report.Dictionary.DataSources["RBAC"].Parameters["InvoiceCode"].Value = InvoiceCode.ToString();
            report.Dictionary.DataSources["RBAC"].Parameters["WorkshopID"].Value = WorkshopID.ToString();
            StiPdfExportService pdfexport = new StiPdfExportService();
            report.Compile();
            //pdfexport.ExportPdf(report, "MyReport.Pdf");
            //StiPdfExportSettings pdfSettings = new StiPdfExportSettings();
            //report.ExportDocument(StiExportFormat.Pdf, "MyReport.Pdf", pdfSettings);
            return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
            //return View();
        }
        //
        // Print to PDF
        public ActionResult GetReport_Old(int InvoiceCode, int Type, int? WorkshopID)
        {
            if (WorkshopID == null)
                WorkshopID = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
            if (InvoiceCode < 1)
            {
                InvoiceCode = 0;
            }
            StiReport report = new StiReport();
            string Path = Server.MapPath("~/StimulSoftReports/InvoiceReport.mrt");
            if (Type == 1)
                Path = Server.MapPath("~/StimulSoftReports/InvoiceReport.mrt");
            if (Type == 2)
                Path = Server.MapPath("~/StimulSoftReports/InvoiceReport2.mrt");
            report.Load(Path);
            report.Render(false);

            MemoryStream stream = new MemoryStream();

            StiPdfExportSettings settings = new StiPdfExportSettings();
            settings.AutoPrintMode = StiPdfAutoPrintMode.Dialog;

            StiPdfExportService service = new StiPdfExportService();
            service.ExportPdf(report, stream, settings);

            this.Response.Buffer = true;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            this.Response.ContentType = "application/pdf";
            //this.Response.AddHeader("Content-Disposition", "attachment; filename=\"report.pdf\"");
            this.Response.ContentEncoding = Encoding.UTF8;
            this.Response.AddHeader("Content-Length", stream.Length.ToString());
            this.Response.BinaryWrite(stream.ToArray());
            this.Response.End();

            return View();
        }

        //
        //ایجاد پرینت
        public ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(this.HttpContext);
        }
        //ایجاد خروجی
        public ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult(this.HttpContext);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(this.HttpContext);
        }
        //
        [HttpGet]
        public JsonResult CurrencyType()
        {
            return Json(db.tbl_CurrencyTypes.Select(d => new { ID = d.ID, Title = d.Title }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        //
        //             
        public ActionResult InvoicePage(string InvoiceCode,int? workshopId)
        {
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_Workshops, "ID", "Title");
                var invoice = db.tbl_Invoices.Include(c => c.Workshops).Include(c=>c.Owners).Include(c=>c.Equipments);
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
            else
            {
                ViewBag.InvoiceCode = InvoiceCode;
                var invoice = db.tbl_Invoices.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode == InvoiceCode).Include(c => c.Workshops).Include(c => c.Owners).Include(c => c.Equipments);
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.CustomerId = invoice.FirstOrDefault().OwnersID;
                ViewBag.Description = invoice.FirstOrDefault().Description;
                ViewBag.Comment = invoice.FirstOrDefault().Comment;
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                ViewBag.date = invoice.FirstOrDefault().CreatedDate;
                ViewBag.CurrencyType = "ریال";
                if (invoice.FirstOrDefault().CurrencyTypeID != null)
                    ViewBag.CurrencyType = db.tbl_CurrencyTypes.Where(c => c.ID == invoice.FirstOrDefault().CurrencyTypeID).SingleOrDefault().Title;
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
        }
        //
        public ActionResult InvoiceFapaPage(string InvoiceCode, int? workshopId, string fromDate, string toDate)
        {
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //
            if (workshopId==null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_Workshops, "ID", "Title");
                var invoice = db.tbl_InvoicesFapa.Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments);
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
            else
            {
                ViewBag.InvoiceCode = InvoiceCode;
                var invoice = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID==workshopId && i.InvoiceCode == InvoiceCode).Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments);
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.CustomerId = invoice.FirstOrDefault().VehicleTypesID;
                ViewBag.Description = invoice.FirstOrDefault().Description;
                ViewBag.Comment = invoice.FirstOrDefault().Comment;
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);               
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                string totalSum = Math.Round(invoice.Sum(i => i.TotalAmountTaxandComplications).Value).ToString();
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalSum);
                ViewBag.date = invoice.FirstOrDefault().CreatedDate.HasValue? invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString():"";
                ViewBag.CurrencyType = "ریال";
                if (invoice.FirstOrDefault().CurrencyTypeID != null)
                    ViewBag.CurrencyType = db.tbl_CurrencyTypes.Where(c => c.ID == invoice.FirstOrDefault().CurrencyTypeID).SingleOrDefault().Title;
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
        }

        /// <summary>
        /// ثبت درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestFreeSale(string InvoiceCode)
        {
            //--------------changed in 14031029-------------------------------------------------
            //string countrystring = "SELECT * FROM [CNGFAPCO].[dbo].[tbl_EquipmentList] where RegistrationTypeID is null and Pid<>'82' and (Value > 0 or Value2 > 0) union all SELECT ID, Title, '82' as FinancialCode, Address, Pid, Presentable, Creator, CreateDate,[dbo].[GetEquipmentGroupValue] ('82','1') as Value,[dbo].[GetEquipmentGroupValue] ('82','2') as Value2,'1' as RegistrationType FROM[CNGFAPCO].[dbo].[tbl_EquipmentList] where id = '82'";
            string countrystring = "SELECT * FROM [CNGFAPCO].[dbo].[tbl_EquipmentList] where Pid<>'82' and (Value > 0 or Value2 > 0) union all SELECT ID, Title, '82' as FinancialCode, Address, Pid, Presentable, Creator, CreateDate,[dbo].[GetEquipmentGroupValue] ('82','1') as Value,[dbo].[GetEquipmentGroupValue] ('82','2') as Value2,'1' as RegistrationType FROM[CNGFAPCO].[dbo].[tbl_EquipmentList] where id = '82'";
            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]) + '-' + Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            ViewBag.ServicesID = new SelectList(list, "Value", "Text"); //new SelectList(db.tbl_EquipmentList.Where(e=>e.Value>0 || e.Value2>0).OrderBy(e=>e.Title), "ID", "Title");
            var Workshop = cngfapco.Helper.Helpers.GetWorkshopCurrentUser();
            ViewBag.WorkshopId = Workshop.ID;
            var FapCode = db.tbl_Workshops.Where(w => w.ID == Workshop.ID).SingleOrDefault().FapCode;
            int Count = db.tbl_RequestFreeSales.Where(v => v.WorkshopsID == Workshop.ID).GroupBy(v => v.InvoiceCode).Count() + 1;
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
                int maxcode = 1;

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
        /// ثبت اطلاعات درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequestFreeSale(List<RequestFreeSale> invoices)
        {
            var nasle4 = db.tbl_EquipmentList.Where(e => e.Pid == 82).ToList();
            var Workshop = cngfapco.Helper.Helpers.GetWorkshopCurrentUser();
            var FapCode = db.tbl_Workshops.Where(w => w.ID == Workshop.ID).SingleOrDefault().FapCode;
            int Count = db.tbl_RequestFreeSales.Where(v => v.WorkshopsID == Workshop.ID).GroupBy(v=>v.InvoiceCode).Count() + 1;
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
                    int maxcode = 1;

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
                        if(invoice.ServiceCode != "82")
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
                            additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
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
                            additem.ViewStatus = false;
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
                            //
                            db.tbl_RequestFreeSales.Add(additem);
                            db.SaveChanges();
                            //
                            invoiceparam = invoice.InvoiceCode;
                        }
                    }
                    //for value of Nasle4 regulator
                    var invoiceNasle4 = invoices.Where(i => i.ServiceCode == "82").ToList();
                    if (invoiceNasle4.Count() > 0)
                    {
                        foreach (var invoice in nasle4)
                        {
                            additem.EquipmentsID = invoiceNasle4.SingleOrDefault().EquipmentsID;
                            additem.WorkshopsID = invoiceNasle4.SingleOrDefault().WorkshopsID;
                            additem.CreatedDate = DateTime.Now;
                            additem.DiscountAmount = Convert.ToDouble(invoiceNasle4.SingleOrDefault().DiscountAmount);
                            additem.TotalAmount = Convert.ToDouble(invoiceNasle4.SingleOrDefault().Number) * invoice.Value;
                            additem.ServiceDesc = invoice.Title;
                            additem.UnitAmount = invoice.Value.HasValue ? invoice.Value.Value : 0;
                            additem.Number = invoiceNasle4.SingleOrDefault().Number;
                            additem.EmployerEconomicalnumber = invoiceNasle4.SingleOrDefault().EmployerEconomicalnumber;
                            additem.Employerregistrationnumber = invoiceNasle4.SingleOrDefault().Employerregistrationnumber;
                            additem.EmployerPostalcode = invoiceNasle4.SingleOrDefault().EmployerPostalcode;
                            additem.EmployerPhone = invoiceNasle4.SingleOrDefault().EmployerPhone;
                            additem.EmployerState = invoiceNasle4.SingleOrDefault().EmployerState;
                            additem.UnitofMeasurement = invoiceNasle4.SingleOrDefault().UnitofMeasurement;
                            additem.EmployerAddress = invoiceNasle4.SingleOrDefault().EmployerAddress;
                            additem.EmployerFax = invoiceNasle4.SingleOrDefault().EmployerFax;
                            additem.Description = invoiceNasle4.SingleOrDefault().Description;
                            additem.SaleCondition = invoiceNasle4.SingleOrDefault().SaleCondition;
                            additem.Comment = invoiceNasle4.SingleOrDefault().Comment;
                            additem.Status = false;
                            additem.ViewStatus = false;
                            additem.ServiceCode = invoice.FinancialCode;
                            additem.Owners = invoiceNasle4.SingleOrDefault().Owners;
                            additem.FinalStatus = true;
                            if (!string.IsNullOrEmpty(invoiceNasle4.SingleOrDefault().CurrencyTypeID.ToString()))
                                additem.CurrencyTypeID = invoiceNasle4.SingleOrDefault().CurrencyTypeID;
                            else
                                additem.CurrencyTypeID = 6;
                            //
                            additem.InvoiceCode = FapCode + "-" + MaxofRow;// maxcode.ToString();
                            additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                            //
                            db.tbl_RequestFreeSales.Add(additem);
                            db.SaveChanges();
                            //
                            invoiceparam = invoiceNasle4.SingleOrDefault().InvoiceCode;
                        }
                    }
                    
                    //
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "درخواست خرید اقلام شما با موفقیت ثبت و ارسال شد!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// نمایش اطلاعات کارگاه درخواست کننده اقلام در طرح فروش آزاد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public ActionResult Workshops(int? id)
        {
            var workshops = db.tbl_Workshops.Find(id);
            return PartialView(workshops);
        }
        /// <summary>
        /// لیست درخواست خرید اقلام توسط کارگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestFreeSaleList()
        {            
            string Owners = "";
            string Workshops = "";
            string InvoiceCode = "";
            DateTime? CreatedDate = null;
            int Number = 0;
            string UnitofMeasurement = "";
            double TotalAmount = 0;
            double DiscountAmount = 0;
            bool Status = false;
            string ViewStatusColor = "#f7818c";
            string PreSaleCondition = "";
            string RequestSaleCondition = "";
            string PreCount="";
            string PreCode = "";
            string FinalCode = "";
            string PaymentStatus = "";
            string PayerCode = "";

            List<RequestList> TableOuts = new List<RequestList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_RequestFreeSaleList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;                    

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Owners = reader["Owners"].ToString();
                        Workshops = reader["Workshops"].ToString();
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate =Convert.ToDateTime(reader["CreatedDate"].ToString());
                        Number = Convert.ToInt32(reader["Number"].ToString());
                        UnitofMeasurement = reader["UnitofMeasurement"].ToString();
                        TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
                        DiscountAmount = Convert.ToDouble(reader["DiscountAmount"].ToString());
                        Status = Convert.ToBoolean(reader["Status"].ToString());
                        ViewStatusColor = reader["ViewStatusColor"].ToString();
                        PreSaleCondition = reader["PreSaleCondition"].ToString();
                        RequestSaleCondition = reader["RequestSaleCondition"].ToString();
                        PreCode = reader["PreCode"].ToString();
                        PreCount = reader["PreCount"].ToString();
                        FinalCode = reader["FinalCode"].ToString();
                        PaymentStatus = reader["PaymentStatus"].ToString();
                        PayerCode = reader["PayerCode"].ToString();
                        //var preInvoice = db.tbl_FreeSaleInvoices.Where(f => f.RequestInvoiceCode == InvoiceCode).ToList();
                        //if (preInvoice.Count > 0)
                        //    preInvoiceCode = preInvoice.FirstOrDefault().InvoiceCode;
                        //else
                        //    preInvoiceCode = "";

                        TableOuts.Add(new RequestList
                        {
                            CreatedDate=CreatedDate.HasValue? CreatedDate.Value.ToShortDateString():null,
                            DiscountAmount=DiscountAmount.ToString("#,##"),
                            FinalCode=FinalCode,
                            InvoiceCode=InvoiceCode,
                            Number=Number.ToString(),
                            Owners=Owners,
                            PaymentStatus=PaymentStatus,
                            PreCode=PreCode,
                            PreCount=PreCount,
                            PreSaleCondition=PreSaleCondition,
                            RequestSaleCondition=RequestSaleCondition,
                            Status=Status,
                            TotalAmount=TotalAmount.ToString("#,##"),
                            UnitofMeasurement=UnitofMeasurement,
                            ViewStatusColor=ViewStatusColor,
                            Workshops=Workshops,
                            PayerCode=PayerCode
                        });
                    }
                   
                    //
                    conn.Close();
                }
            }//end using
            //
            bool userRole = false;
            string Role = Helper.Helpers.GetCurrentUserRole();
            if (Role.Equals("مدیر فروش"))
                userRole = true;
            //
            var requestList = db.tbl_RequestFreeSales.Where(r => r.ViewStatus == false).ToList();
            if (requestList.Count() > 0)
            {
                if (userRole == true)
                {
                    foreach (var item in requestList)
                    {
                        item.ViewStatus = true;
                        item.ViewDate = DateTime.Now;
                        item.Viewer = User.Identity.Name;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            //
            ViewBag.TableOuts = TableOuts.ToList();
            return View();
        }
        //
        public class RequestList
        {
            public string Owners { get; set; }
            public string PayerCode { get; set; }
            public string Workshops { get; set; }            //Workshops = reader["Workshops"].ToString();
            public string InvoiceCode { get; set; }//InvoiceCode = reader["InvoiceCode"].ToString();
            public string CreatedDate { get; set; }//CreatedDate =Convert.ToDateTime(reader["CreatedDate"].ToString());
            public string Number { get; set; }//            Number = Convert.ToInt32(reader["Number"].ToString());
            public string UnitofMeasurement { get; set; }//            UnitofMeasurement = reader["UnitofMeasurement"].ToString();
            public string TotalAmount { get; set; }//TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
            public string DiscountAmount { get; set; }//            DiscountAmount = Convert.ToDouble(reader["DiscountAmount"].ToString());
            public bool Status { get; set; }//            Status = Convert.ToBoolean(reader["Status"].ToString());
            public string ViewStatusColor { get; set; }//            ViewStatusColor = reader["ViewStatusColor"].ToString();
            public string PreSaleCondition { get; set; }//PreSaleCondition = reader["PreSaleCondition"].ToString();
            public string RequestSaleCondition { get; set; } //RequestSaleCondition = reader["RequestSaleCondition"].ToString();
            public string PreCode { get; set; }//PreCode = reader["PreCode"].ToString();
            public string PreCount { get; set; }//PreCount = reader["PreCount"].ToString();
            public string FinalCode { get; set; }//FinalCode = reader["FinalCode"].ToString();
            public string PaymentStatus { get; set; }//PaymentStatus = Convert.ToBoolean(reader["PaymentStatus"].ToString());
        }
        /// <summary>
        /// لیست درخواست خرید اقلام توسط کارگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestFreeSaleDetails(string InvoiceCode)
        {
            //int currYear = DateTime.Now.Year;
            var detailsList = db.tbl_RequestFreeSales.Where(r => r.InvoiceCode.Equals(InvoiceCode)).OrderByDescending(r=>r.InvoiceID);
            return View(detailsList.ToList());
        }

        /// <summary>
        /// مشاهده و چاپ پیش فاکتور بر اساس درخواست های  خرید اقلام در طرح فروش آزاد
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewPreFreeSaleInvoice(string InvoiceCode, int Year)
        {
            ViewBag.ServicesID = new SelectList(db.tbl_EquipmentList, "ID", "Title");
            var invoice = db.tbl_FreeSaleInvoices.Where(i => i.RequestInvoiceCode.Equals(InvoiceCode)).Include(c => c.Workshops);
            string inCode = invoice.FirstOrDefault().InvoiceCode;
            var checkExist = db.tbl_FinallFreeSaleInvoices.Where(i => i.PreInvoiceCode.Equals(inCode)).ToList();
            if (checkExist.Where(i => pc.GetYear(i.CreatedDate.Value) == Year).ToList().Count > 0)
                ViewBag.Exist = true;
            else
                ViewBag.Exist = false;
            //
            var checkPaied = db.tbl_Payments.Where(i => i.PreInvoiceCode.Equals(inCode)).ToList();
            if (checkPaied.Where(i=>i.Status.Equals("پرداخت شده")).Where(i => pc.GetYear(i.PayDate) == Year).ToList().Count() > 0)
                ViewBag.checkPaied = true;
            else
                ViewBag.checkPaied = false;
            //
            if (invoice != null)
            {               
                ViewBag.PreinvoiceCode = InvoiceCode;
                ViewBag.Nationalcode = invoice.FirstOrDefault().Employerregistrationnumber + "_"+ invoice.FirstOrDefault().EmployerEconomicalnumber;
                ViewBag.FullName = invoice.FirstOrDefault().Workshops.Title;
                ViewBag.Phone = invoice.FirstOrDefault().EmployerPhone;
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.InvoiceCode = invoice.FirstOrDefault().InvoiceCode;
                ViewBag.CreateDate = invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString();
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                string totalSum = Math.Round(invoice.Sum(i => i.TotalAmountTaxandComplications).Value).ToString();
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalSum);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(invoice.ToList());
        }
        /// <summary>
        /// مشاهده و صدور پیش فاکتور بر اساس درخواست های  خرید اقلام در طرح فروش آزاد
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreFreeSaleInvoice(string InvoiceCode)
        {
            int currYear = pc.GetYear(DateTime.Now);
            double ValueAdded = db.tbl_TaxValueAdded.Where(i => i.Year == currYear).SingleOrDefault().ValueAdded.HasValue? db.tbl_TaxValueAdded.Where(i => i.Year == currYear).SingleOrDefault().ValueAdded.Value:0;
            double TotalAmountafterDiscount = 0;
            double AmountTaxandComplications = 0;
            double TotalAmountTaxandComplications = 0;

            double sumTotalAmountafterDiscount = 0;
            double sumAmountTaxandComplications = 0;
            double sumTotalAmountTaxandComplications = 0;
            //
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.ServicesID = new SelectList(db.tbl_EquipmentList.Where(e => e.Value > 0 || e.Value2 > 0), "ID", "Title");
            var invoice = db.tbl_RequestFreeSales.Where(i => i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
            if(invoice!=null)
            {
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.PreinvoiceCode = invoice.FirstOrDefault().InvoiceCode;
                ViewBag.TotalAmount = invoice.Sum(r=>r.TotalAmount).HasValue? invoice.Sum(r => r.TotalAmount).Value.ToString("#,##") : "0";
                foreach(var item in invoice)
                {
                    TotalAmountafterDiscount = item.TotalAmount.Value - item.DiscountAmount.Value;
                    AmountTaxandComplications = TotalAmountafterDiscount * ValueAdded;// 0.09
                    TotalAmountTaxandComplications = TotalAmountafterDiscount + AmountTaxandComplications;
                    //
                    sumTotalAmountafterDiscount += TotalAmountafterDiscount;
                    sumAmountTaxandComplications += AmountTaxandComplications;
                    sumTotalAmountTaxandComplications += TotalAmountTaxandComplications;
                }
                //
                ViewBag.TotalAmountafterDiscount = sumTotalAmountafterDiscount.ToString("#,##");
                ViewBag.AmountTaxandComplications = sumAmountTaxandComplications.ToString("#,##");
                ViewBag.TotalAmountTaxandComplications = sumTotalAmountTaxandComplications.ToString("#,##");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(invoice.ToList());
        }

        [HttpGet]
        public ActionResult FreeSaleInvoice(string InvoiceCode, bool checkExist, string checkPaied, string SaleCondition,int Year)
        {            
            int CurrYear = pc.GetYear(DateTime.Now);
            string invoiceparam = "";
            bool finallExist = false;
            double SumTotalAmount = 0;

            if (checkPaied == "0")
                ViewBag.checkPaied = false;
            else
                ViewBag.checkPaied = true;

            var paiedList = db.tbl_Payments.Where(p => p.PreInvoiceCode == InvoiceCode && p.Status.Equals("پرداخت شده")).ToList();
            if(string.IsNullOrEmpty(checkPaied))
                if(paiedList.Where(i => pc.GetYear(i.PayDate) == Year).ToList().Count>0)
                    ViewBag.checkPaied = true;
            else
                    ViewBag.checkPaied = false;

            ViewBag.Exist = checkExist;
            var refreshList = db.tbl_FinallFreeSaleInvoices.Where(f => f.PreInvoiceCode == InvoiceCode).ToList();
            if (refreshList.Where(i=> pc.GetYear(i.CreatedDate.Value) == Year).ToList().Count > 0)
                finallExist = true;
            
            if (checkExist == false && finallExist==false)
            {
                if(SaleCondition.Equals("نقدی") && paiedList.Count > 0)
                {
                    var preinvoice = db.tbl_FreeSaleInvoices.Where(f => f.InvoiceCode == InvoiceCode).ToList();
                    if (preinvoice.Where(i => pc.GetYear(i.CreatedDate.Value) == Year).ToList().Count > 0)
                    {
                        var invoicelist = db.tbl_FinallFreeSaleInvoices.ToList();
                        var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => i.InvoiceID);
                        int maxcode = 1;
                        string finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";

                        if (invoicelist2.Count() > 0)
                        {
                            var invoicecount = invoicelist2.Take(1).Max(i => i.InvoiceCode).Replace(pc.GetYear(DateTime.Now) + "-" + "109" + "-", "");
                            maxcode = Convert.ToInt32(invoicecount) + 1;
                            finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + maxcode;
                        }
                        else
                        {
                            //ViewBag.finallinvoiceCode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";
                        }
                        FinallFreeSaleInvoice additem = new FinallFreeSaleInvoice();
                        //Loop and insert records.
                        foreach (var invoice in preinvoice)
                        {
                            FreeSaleInvoice updateStatus = db.tbl_FreeSaleInvoices.Find(invoice.InvoiceID);
                            updateStatus.Status = true;
                            updateStatus.AcceptedDate = DateTime.Now;
                            updateStatus.AcceptedAmount = invoice.TotalAmountTaxandComplications;
                            //
                            if (invoice.EquipmentsID != null)
                            {
                                additem.EquipmentsID = invoice.EquipmentsID; //db.tbl_EquipmentList.Where(e => e.ID.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                            }
                            else
                            {
                                additem.EquipmentsID = null;
                            }
                            //
                            additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                            additem.Comment = invoice.Comment;

                            if (invoice.CreatedDate == null)
                                additem.CreatedDate = DateTime.Now;
                            else
                                additem.CreatedDate = invoice.CreatedDate;
                            additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                            additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                            additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                            additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                            additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
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
                            additem.Tax = 0;
                            additem.Complications = 0;
                            additem.Status = false;
                            additem.ViewStatus = false;
                            additem.ServiceCode = invoice.ServiceCode;
                            additem.CustomersID = invoice.CustomersID;
                            if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                                additem.CurrencyTypeID = invoice.CurrencyTypeID;
                            else
                                additem.CurrencyTypeID = 6;
                            //
                            additem.InvoiceCode = finallmaxcode;
                            additem.PreInvoiceCode = invoice.InvoiceCode;
                            additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                            //
                            db.tbl_FinallFreeSaleInvoices.Add(additem);
                            db.SaveChanges();
                            //برای بروزرسانی شماره فاکتور نهایی در جدول پرداخت آنلاین- نقدی فاکتورهای خرید
                            foreach (var item in paiedList)
                            {
                                Payment updatePay = db.tbl_Payments.Find(item.ID);
                                updatePay.InvoiceCode = finallmaxcode;
                                db.Entry(updatePay).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //
                            //برای کسر کردن از لیست موجودی انبار فروش
                            var update = db.tbl_SaleWarehouses.Where(s => s.FinancialCode == additem.ServiceCode).SingleOrDefault();
                            update.CurrentRem -= Convert.ToDouble(additem.Number);
                            db.Entry(update).State = EntityState.Modified;
                            db.SaveChanges();
                            //
                            invoiceparam = additem.InvoiceCode;
                            var totalList = db.tbl_FinallFreeSaleInvoices.Where(f => f.InvoiceCode == invoiceparam).ToList();
                            ViewBag.finallinvoiceCode = additem.InvoiceCode;
                            ViewBag.CreateDate = additem.CreatedDate.Value.ToShortDateString();
                            ViewBag.PreinvoiceCode = additem.PreInvoiceCode;
                            ViewBag.Nationalcode = additem.Employerregistrationnumber + "_" + additem.EmployerEconomicalnumber;
                            ViewBag.FullName = additem.Workshops.Title;
                            ViewBag.Phone = additem.EmployerPhone;
                            ViewBag.WorkshopId = additem.WorkshopsID;
                            ViewBag.TotalAmount = totalList.Sum(i => i.TotalAmount);
                            ViewBag.DiscountAmount = totalList.Sum(i => i.DiscountAmount);
                            ViewBag.TotalAmountafterDiscount = totalList.Sum(i => i.TotalAmountafterDiscount);
                            ViewBag.AmountTaxandComplications = totalList.Sum(i => i.AmountTaxandComplications);
                            ViewBag.TotalAmountTaxandComplications = totalList.Sum(i => i.TotalAmountTaxandComplications);
                            Helper.PNumberTString AmounttoLetter = new PNumberTString();
                            ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalList.Sum(i => i.TotalAmountTaxandComplications).ToString());
                        }
                    }
                    //
                    var finallinvoice = db.tbl_FinallFreeSaleInvoices.Where(f => f.InvoiceCode == invoiceparam);
                    return View(finallinvoice.ToList());
                }
                
                else
                {
                    var preinvoice = db.tbl_FreeSaleInvoices.Where(f => f.InvoiceCode == InvoiceCode).ToList();
                    if (preinvoice.Where(i => pc.GetYear(i.CreatedDate.Value) == Year).ToList().Count > 0)
                    {
                        var invoicelist = db.tbl_FinallFreeSaleInvoices.ToList();
                        var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => i.InvoiceID);
                        int maxcode = 1;
                        string finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";

                        if (invoicelist2.Count() > 0)
                        {
                            var invoicecount = invoicelist2.Take(1).Max(i => i.InvoiceCode).Replace(pc.GetYear(DateTime.Now) + "-" + "109" + "-", "");
                            maxcode = Convert.ToInt32(invoicecount) + 1;
                            finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + maxcode;
                        }
                        else
                        {
                            //ViewBag.finallinvoiceCode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";
                        }
                        FinallFreeSaleInvoice additem = new FinallFreeSaleInvoice();
                        //Loop and insert records.
                        foreach (var invoice in preinvoice.Where(i => pc.GetYear(i.CreatedDate.Value) == Year).ToList())
                        {
                            FreeSaleInvoice updateStatus = db.tbl_FreeSaleInvoices.Find(invoice.InvoiceID);
                            updateStatus.Status = true;
                            updateStatus.AcceptedDate = DateTime.Now;
                            updateStatus.AcceptedAmount = invoice.TotalAmountTaxandComplications;
                            //
                            if (invoice.EquipmentsID != null)
                            {
                                additem.EquipmentsID = invoice.EquipmentsID; //db.tbl_EquipmentList.Where(e => e.ID.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                            }
                            else
                            {
                                additem.EquipmentsID = null;
                            }
                            //
                            additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                            additem.Comment = invoice.Comment;

                            if (invoice.CreatedDate == null)
                                additem.CreatedDate = DateTime.Now;
                            else
                                additem.CreatedDate = invoice.CreatedDate;
                            additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                            additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                            additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                            additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                            additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
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
                            additem.Tax = 0;
                            additem.Complications = 0;
                            additem.Status = false;
                            additem.ViewStatus = false;
                            additem.ServiceCode = invoice.ServiceCode;
                            additem.CustomersID = invoice.CustomersID;
                            if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                                additem.CurrencyTypeID = invoice.CurrencyTypeID;
                            else
                                additem.CurrencyTypeID = 6;
                            //
                            additem.InvoiceCode = finallmaxcode;
                            additem.PreInvoiceCode = invoice.InvoiceCode;
                            additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;                           
                            //
                            db.tbl_FinallFreeSaleInvoices.Add(additem);
                            db.SaveChanges();
                            //
                            //برای افزودن جمع مبلغ فاکتور غیر نقدی در بخش بدهی کارگاه ها در گزارش مالی
                            SumTotalAmount += (double)additem.TotalAmountTaxandComplications;                           
                            //.

                            //برای کسر کردن از لیست موجودی انبار فروش
                            try
                            {
                                var update = db.tbl_SaleWarehouses.Where(s => s.FinancialCode == additem.ServiceCode).SingleOrDefault();
                                update.CurrentRem -= Convert.ToDouble(additem.Number);
                                db.Entry(update).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch { }
                           
                            //
                            invoiceparam = additem.InvoiceCode;
                            var totalList = db.tbl_FinallFreeSaleInvoices.Where(f => f.InvoiceCode == invoiceparam).ToList();
                            ViewBag.finallinvoiceCode = additem.InvoiceCode;
                            ViewBag.CreateDate = additem.CreatedDate.Value.ToShortDateString();
                            ViewBag.PreinvoiceCode = additem.PreInvoiceCode;
                            ViewBag.Nationalcode = additem.Employerregistrationnumber + "_" + additem.EmployerEconomicalnumber;
                            ViewBag.FullName = additem.Workshops.Title;
                            ViewBag.Phone = additem.EmployerPhone;
                            ViewBag.WorkshopId = additem.WorkshopsID;
                            ViewBag.TotalAmount = totalList.Sum(i => i.TotalAmount);
                            ViewBag.DiscountAmount = totalList.Sum(i => i.DiscountAmount);
                            ViewBag.TotalAmountafterDiscount = totalList.Sum(i => i.TotalAmountafterDiscount);
                            ViewBag.AmountTaxandComplications = totalList.Sum(i => i.AmountTaxandComplications);
                            ViewBag.TotalAmountTaxandComplications = totalList.Sum(i => i.TotalAmountTaxandComplications);
                            Helper.PNumberTString AmounttoLetter = new PNumberTString();
                            ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalList.Sum(i => i.TotalAmountTaxandComplications).ToString());
                        }
                        //
                        //برای افزودن جمع مبلغ فاکتور غیر نقدی در بخش بدهی کارگاه ها در گزارش مالی
                        FinancialPayment addtoPayment = new FinancialPayment();
                        addtoPayment.Value = SumTotalAmount;
                        addtoPayment.WorkshopID = additem.WorkshopsID;
                        addtoPayment.FinancialDescID = 6;
                        addtoPayment.CreateDate = DateTime.Now;
                        addtoPayment.Creator = User.Identity.Name;
                        addtoPayment.Date = DateTime.Now;
                        addtoPayment.Description = "بابت خرید غیر نقدی کالا و کسر از محل طلب دستمزد تبدیل";
                        db.tbl_FinancialPayments.Add(addtoPayment);
                        db.SaveChanges();
                        //.
                    }
                    //
                    var finallinvoice = db.tbl_FinallFreeSaleInvoices.Where(f => f.InvoiceCode == invoiceparam);
                    return View(finallinvoice.ToList());
                }
            }
            else
            {
                var List = db.tbl_FinallFreeSaleInvoices.Where(f => f.PreInvoiceCode==InvoiceCode).ToList();
                var totalList = List.Where(i => pc.GetYear(i.CreatedDate.Value) == Year).ToList();
                ViewBag.finallinvoiceCode = totalList.FirstOrDefault().InvoiceCode;
                ViewBag.CreateDate = totalList.FirstOrDefault().CreatedDate.Value.ToShortDateString();
                ViewBag.PreinvoiceCode = totalList.FirstOrDefault().PreInvoiceCode;
                ViewBag.Nationalcode = totalList.FirstOrDefault().Employerregistrationnumber + "_" + totalList.FirstOrDefault().EmployerEconomicalnumber;
                ViewBag.FullName = totalList.FirstOrDefault().Workshops.Title;
                ViewBag.Phone = totalList.FirstOrDefault().EmployerPhone;
                ViewBag.WorkshopId = totalList.FirstOrDefault().WorkshopsID;
                ViewBag.TotalAmount = totalList.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = totalList.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = totalList.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = totalList.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = totalList.Sum(i => i.TotalAmountTaxandComplications);
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalList.Sum(i => i.TotalAmountTaxandComplications).ToString());
                return View(totalList);
            }
        }

        /// <summary>
        ///  ثبت و صدور پیش فاکتور فروش کالا/ خدمات فروش آزاد قطعات
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PreFreeSaleInvoice(List<FreeSaleInvoice> invoices)
        {
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
                    var invoicelist = db.tbl_FreeSaleInvoices.ToList();
                    var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    int maxcode = 1;

                    if (invoicelist2.Count() > 0)
                    {
                        maxcode = Convert.ToInt32(invoicelist2.Take(1).Max(i => i.InvoiceCode)) + 1;
                        ViewBag.invoiceCode = (maxcode).ToString();
                    }
                    else
                    {
                        ViewBag.invoiceCode = "1";
                    }

                    FreeSaleInvoice additem = new FreeSaleInvoice();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
                    {
                        RequestFreeSale updateStatus = db.tbl_RequestFreeSales.Find(invoice.InvoiceID);
                        updateStatus.Status = true;
                        //
                        if (invoice.EquipmentsID != null)
                        {
                            additem.EquipmentsID = invoice.EquipmentsID; //db.tbl_EquipmentList.Where(e => e.ID.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                        }
                        else
                        {
                            additem.EquipmentsID = null;
                        }
                        //
                        additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                        additem.Comment = invoice.Comment;

                        if (invoice.CreatedDate == null)
                            additem.CreatedDate = DateTime.Now;
                        else
                            additem.CreatedDate = invoice.CreatedDate;
                        additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                        additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                        additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                        additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                        additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
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
                        additem.Tax = 0;
                        additem.Complications = 0;
                        additem.Status = false;
                        additem.ViewStatus = false;
                        additem.ServiceCode = invoice.ServiceCode;
                        additem.CustomersID = invoice.CustomersID;
                        if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            additem.CurrencyTypeID = invoice.CurrencyTypeID;
                        else
                            additem.CurrencyTypeID = 6;
                        //
                        additem.InvoiceCode = maxcode.ToString();
                        additem.RequestInvoiceCode = invoice.InvoiceCode;
                        additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                        //
                        db.tbl_FreeSaleInvoices.Add(additem);
                        db.SaveChanges();
                        //
                        invoiceparam = invoice.InvoiceCode;
                    }
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "پیش فاکتور با موفقیت صادر و ارسال شد!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        /// <summary>
        ///  ثبت و صدور فاکتور نهایی فروش کالا/ خدمات فروش آزاد قطعات
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FreeSaleInvoice(List<FreeSaleInvoice> invoices)
        {
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
                    var invoicelist = db.tbl_FinallFreeSaleInvoices.ToList();
                    var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                    int maxcode = 1;
                    string finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";

                    if (invoicelist2.Count() > 0)
                    {
                        var invoicecount = invoicelist2.Take(1).Max(i => i.InvoiceCode).Replace(pc.GetYear(DateTime.Now) + "-" + "109" + "-", "");
                        maxcode = Convert.ToInt32(invoicecount) + 1;
                        finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + maxcode;
                        ViewBag.invoiceCode = (maxcode).ToString();
                        ViewBag.finallinvoiceCode = finallmaxcode;
                    }
                    else
                    {
                        ViewBag.invoiceCode = "1";
                        ViewBag.finallinvoiceCode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";
                    }

                    FreeSaleInvoice additem = new FreeSaleInvoice();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
                    {
                        RequestFreeSale updateStatus = db.tbl_RequestFreeSales.Find(invoice.InvoiceID);
                        updateStatus.Status = true;
                        //
                        if (invoice.EquipmentsID != null)
                        {
                            additem.EquipmentsID = invoice.EquipmentsID; //db.tbl_EquipmentList.Where(e => e.ID.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                        }
                        else
                        {
                            additem.EquipmentsID = null;
                        }
                        //
                        additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                        additem.Comment = invoice.Comment;

                        if (invoice.CreatedDate == null)
                            additem.CreatedDate = DateTime.Now;
                        else
                            additem.CreatedDate = invoice.CreatedDate;
                        additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                        additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                        additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                        additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                        additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
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
                        additem.Tax = 0;
                        additem.Complications = 0;
                        additem.Status = false;
                        additem.ViewStatus = false;
                        additem.ServiceCode = invoice.ServiceCode;
                        additem.CustomersID = invoice.CustomersID;
                        if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            additem.CurrencyTypeID = invoice.CurrencyTypeID;
                        else
                            additem.CurrencyTypeID = 6;
                        //
                        additem.InvoiceCode = finallmaxcode;
                        additem.RequestInvoiceCode = invoice.InvoiceCode;
                        additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                        //
                        db.tbl_FreeSaleInvoices.Add(additem);
                        db.SaveChanges();
                        //
                        invoiceparam = invoice.InvoiceCode;
                        //برای کسر کردن از لیست موجودی انبار فروش
                        var update = db.tbl_SaleWarehouses.Where(s => s.FinancialCode == additem.ServiceCode).SingleOrDefault();
                        update.CurrentRem -= Convert.ToDouble(additem.Number);
                        db.Entry(update).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "فاکتور با موفقیت صادر شد!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        public ActionResult CheckSalaryRem(int? WorkshopID)
        {
            DateTime fromDate = Convert.ToDateTime("1399/01/01");
            DateTime toDate = DateTime.Now;
            string value = "0";
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GetCheckSalaryRem]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            value = reader["Rem"].ToString();
                        }
                        //
                        conn.Close();
                    }
                }//end using

            }
            catch
            {

            }
            //
            if (!string.IsNullOrEmpty(value))
                ViewBag.value = Convert.ToDouble(value).ToString("#,##");
            else
                ViewBag.value = "0";
            //
            return PartialView();
        }
        //
        public static string GetCheckSalaryRem(int? WorkshopID)
        {
            DateTime fromDate = Convert.ToDateTime("1399/01/01");
            DateTime toDate = DateTime.Now;
            string value = "0";
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GetCheckSalaryRem]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            value = reader["Rem"].ToString();  
                        }                       
                        //
                        conn.Close();
                    }
                }//end using

            }
            catch
            {
                
            }

            return Convert.ToDouble(value).ToString("#,##");
        }
        //
        public static bool GetCheckFinalInvoice(string PreInvoiceCode)
        {
            var checkExist = dbStatic.tbl_FinallFreeSaleInvoices.Where(f => f.PreInvoiceCode == PreInvoiceCode).ToList();
            if (checkExist.Count > 0)
                return true;
            else
                return false;
        }
        //
        public static bool GetCheckPreInvoice(string PreInvoiceCode)
        {
            //bool status = false;
            var checkExist = dbStatic.tbl_FreeSaleInvoices.Where(f => f.RequestInvoiceCode == PreInvoiceCode).ToList();
            if (checkExist.Count > 0)
                return true;
            else
                return false;
        }
        //
        public static bool GetCheckInvoicePaied(string PreInvoiceCode, DateTime Year)
        {
            string g_date = Year.ToShortDateString();
            //DateTime dt1 = new DateTime(Convert.ToInt32(existYear), Convert.ToInt32(Month2), Convert.ToInt32(Day2), pc);
            g_date = Year.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            string g_Year = g_date.Substring(0, 4);
            bool status = false;
            var checkPaied = dbStatic.tbl_Payments.Where(f => f.PreInvoiceCode == PreInvoiceCode && f.PayDate.Year.ToString() == g_Year && f.Status.Equals("پرداخت شده")).ToList();
            //foreach (var item in checkPaied)
            //{
            //    if (item.PreInvoiceCode == PreInvoiceCode && item.PayDate.Year.ToString() == g_Year)
            //        if (checkPaied.Count > 0)
            //            //return true;
            //            status = true;
            //        else
            //            //return false;
            //            status = false;
            //}

            if (checkPaied.Count() > 0)
                status = true;

            return status;
        }
        //
        public ActionResult FreeSaleInvoicePaied(string FinalCode,int? PayerCode)
        {
            try
            {
                var freeSaleInvoice = db.tbl_Payments.Where(p => p.InvoiceCode == FinalCode && p.PayerCode== PayerCode);
                return View(freeSaleInvoice.ToList());
            }
            catch
            {
                var freeSaleInvoice = db.tbl_Payments.Where(p => p.InvoiceCode == "000");
                return View(freeSaleInvoice.ToList());
            }
           
        }
        /// <summary>
        /// لیست درخواست خرید اقلام توسط کارگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CallBackRequestFreeSale(int? WorkshopID)
        {
            if (WorkshopID == null)
                WorkshopID = db.tbl_Users.Where(u=>u.Username==User.Identity.Name).SingleOrDefault().WorkshopID;
            string Owners = "";
            string Workshops = "";
            string InvoiceCode = "";
            DateTime? CreatedDate = null;
            int Number = 0;
            string UnitofMeasurement = "";
            double TotalAmount = 0;
            double DiscountAmount = 0;
            bool Status = false;
            string ViewStatusColor = "#f7818c";
            string preInvoiceCode = "";
            string SaleCondition = "";

            List<RequestFreeSale> TableOuts = new List<RequestFreeSale>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_CallBackRequestFreeSaleList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@workshop", SqlDbType.VarChar).Value = WorkshopID;

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Owners = reader["Owners"].ToString();
                        Workshops = reader["Workshops"].ToString();
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                        Number = Convert.ToInt32(reader["Number"].ToString());
                        UnitofMeasurement = reader["UnitofMeasurement"].ToString();
                        TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
                        DiscountAmount = Convert.ToDouble(reader["DiscountAmount"].ToString());
                        Status = Convert.ToBoolean(reader["Status"].ToString());
                        ViewStatusColor = reader["ViewStatusColor"].ToString();
                        SaleCondition = reader["SaleCondition"].ToString();
                        var preInvoice = db.tbl_FreeSaleInvoices.Where(f => f.RequestInvoiceCode == InvoiceCode).ToList();
                        if (preInvoice.Count > 0)
                            preInvoiceCode = preInvoice.FirstOrDefault().InvoiceCode;
                        else
                            preInvoiceCode = "";

                        TableOuts.Add(new RequestFreeSale
                        {
                            Owners = Owners,
                            EmployerState = Workshops,
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate.Value,
                            Number = Number,
                            UnitofMeasurement = UnitofMeasurement,
                            TotalAmount = TotalAmount,
                            DiscountAmount = DiscountAmount,
                            Status = Status,
                            Comment = ViewStatusColor,
                            Description = preInvoiceCode,
                            SaleCondition = SaleCondition
                        });
                    }

                    //
                    conn.Close();
                }
            }//end using
             //
            bool userRole = false;
            string Role = Helper.Helpers.GetCurrentUserRole();
            if (Role.Equals("مرکز خدمات (کارگاه)"))
                userRole = true;
            //
            var requestList = db.tbl_FreeSaleInvoices.Where(r =>r.WorkshopsID==WorkshopID && r.ViewStatus == false).ToList();
            if (requestList.Count() > 0)
            {
                if (userRole == true)
                {
                    foreach (var item in requestList)
                    {
                        item.ViewStatus = true;
                        item.ViewDate = DateTime.Now;
                        item.Viewer = User.Identity.Name;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
            }
            //
            return View(TableOuts.ToList());
        }

        /// <summary>
        /// لیست درخواست خرید اقلام نهایی شده و فاکتور شده جهت ارسال به کارگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FinallFreeSaleList()
        {
            string Workshops = "";
            string InvoiceCode = "";
            DateTime? CreatedDate = null;
            int Number = 0;
            string UnitofMeasurement = "";
            double TotalAmount = 0;
            double DiscountAmount = 0;
            bool Status = false;
            string ViewStatusColor = "#f7818c";
            string PreInvoiceCode = "";
            string SaleCondition = "";
            int Count = 0;

            List<FinallFreeSaleInvoice> TableOuts = new List<FinallFreeSaleInvoice>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_FinallFreeSaleInvoiceList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Workshops = reader["Workshops"].ToString();
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                        Number = Convert.ToInt32(reader["Number"].ToString());
                        UnitofMeasurement = reader["UnitofMeasurement"].ToString();
                        TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
                        Status = Convert.ToBoolean(reader["Status"].ToString());
                        ViewStatusColor = reader["ViewStatusColor"].ToString();
                        SaleCondition = reader["SaleCondition"].ToString();
                        PreInvoiceCode = reader["PreInvoiceCode"].ToString();
                        if (Status == false)
                            Count += 1;

                        TableOuts.Add(new FinallFreeSaleInvoice
                        {
                            EmployerState = Workshops,
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate.Value,
                            Number = Number,
                            UnitofMeasurement = UnitofMeasurement,
                            TotalAmount = TotalAmount,
                            Status = Status,
                            Comment = ViewStatusColor,
                            Description = PreInvoiceCode,
                            SaleCondition = SaleCondition
                        });
                    }

                    //
                    conn.Close();
                }
            }//end using
            //
            ViewBag.Count = Count;
            return View(TableOuts.ToList());
        }

        /// <summary>
        /// لیست خرید اقلام آزاد کارگاه ها در انتظار صدور حواله برای نمایش در صفحه اصلی به عنوان آلارم
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FinallFreeSaleListAlarm()
        {
            bool Status = false;
            int Count = 0;

            List<FinallFreeSaleInvoice> TableOuts = new List<FinallFreeSaleInvoice>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_FinallFreeSaleInvoiceList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {                        
                        Status = Convert.ToBoolean(reader["Status"].ToString());
                        
                        if (Status == false)
                        {
                            Count += 1;                           
                        }                            
                    }
                    //
                    conn.Close();
                }
            }//end using
            //
            ViewBag.Count = Count;
            return PartialView();
        }
        //
        public ActionResult DeleteInvoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequestFreeSale invoice = db.tbl_RequestFreeSales.Find(id);
            string InvoiceCode = invoice.InvoiceCode;
            var invoicecount = db.tbl_RequestFreeSales.Where(i => i.InvoiceCode == InvoiceCode).Count();
            if (invoice == null || invoicecount < 2)
            {
                return HttpNotFound();
            }
            db.tbl_RequestFreeSales.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("PreFreeSaleInvoice", new { InvoiceCode = InvoiceCode });
        }
        /// <summary>
        /// افزودن ردیف جدید به درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddNewItems(string InvoiceCode,int? WorkshopId)
        {
            ViewBag.ServicesID = new SelectList(db.tbl_EquipmentList.Where(e => e.Value > 0 || e.Value2 > 0).OrderBy(e => e.Title), "ID", "Title");
            ViewBag.WorkshopId = WorkshopId;            
            //           
            int CurrYear = pc.GetYear(DateTime.Now);
            var invoice = db.tbl_RequestFreeSales.Where(i => i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
            var eid = invoice.OrderByDescending(i => i.InvoiceID).FirstOrDefault().WorkshopsID;
            var coid = invoice.OrderByDescending(i => i.EquipmentsID).FirstOrDefault().EquipmentsID;
            ViewBag.invoiceCode = InvoiceCode;
            ViewBag.date = invoice.FirstOrDefault().CreatedDate;
            return View(invoice.ToList());
        }

        /// <summary>
        /// ثبت اطلاعات افزودن ردیف جدید به درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddNewItems(List<RequestFreeSale> invoices, string InvoiceCode)
        {
            var requestfreesale = db.tbl_RequestFreeSales.Where(r => r.InvoiceCode.Equals(InvoiceCode)).ToList();
            string SaleCondition = "";
            using (ContextDB entities = new ContextDB())
            {
                string invoiceparam = "";
                int count = 0;
                //Check for NULL.
                if (invoices == null || invoices.Count == 0)
                {
                    Thread.Sleep(1000);
                    return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                }
                else
                {                     
                    RequestFreeSale additem = new RequestFreeSale();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
                    {
                        if (invoice.InvoiceID == 0)
                        {
                            count += 1;
                            if (count == 1)
                                SaleCondition = invoice.SaleCondition;
                            if (invoice.EquipmentsID != null)
                            {
                                additem.EquipmentsID = db.tbl_EquipmentList.Where(e => e.FinancialCode.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                            }
                            else
                            {
                                additem.EquipmentsID = null;
                            }
                            //
                            additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;

                           // additem.CreatedDate = Convert.ToDateTime(invoice.CreatedDate); //DateTime.Now;
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
                            additem.ViewStatus = false;
                            additem.ServiceCode = invoice.ServiceCode;
                            additem.Owners = invoice.Owners;
                            additem.FinalStatus = true;
                            if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                                additem.CurrencyTypeID = invoice.CurrencyTypeID;
                            else
                                additem.CurrencyTypeID = 6;
                            //
                            additem.InvoiceCode = InvoiceCode;
                            additem.CreatedDate = invoice.CreatedDate;
                            additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                            //
                            db.tbl_RequestFreeSales.Add(additem);
                            db.SaveChanges();
                            //
                            invoiceparam = invoice.InvoiceCode;
                        }
                        
                    }
                    //
                    foreach(var item in requestfreesale)
                    {
                        item.SaleCondition = SaleCondition;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "تعداد : " + count + "  ردیف جدید با موفقیت به اطلاعات قبلی پیش فاکتور افزوده شدند!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        [HttpGet]
        public ActionResult OfferedPrice(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);

            //
            if (WorkshopID == null)
                if (cngfapco.Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
                    WorkshopID = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId()).WorkshopID;
                else
                    WorkshopID = 0;

            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            string OfferedSerial = "";
            //

            List<PreInvoicesHint> PreInvoicesHint = new List<PreInvoicesHint>();
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            List<FinancialDebitCredit> DebitCreditOneView = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        reader.NextResult();
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumTCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumTDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumTRem += Rem;
                            NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                            SumNonCash += NonCash;
                            Amount = Convert.ToDouble(reader["Amount"].ToString());

                            Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                            SumDeductions += Deductions;
                            RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                            SumRemWithDeductions += RemWithDeductions;
                            NetPercent = reader["NetPercent"].ToString();
                            GrossPercent = reader["GrossPercent"].ToString();
                            PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                            SumPreInvoiceHint += PreInvoiceHint;
                            OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                            SumOfferedPrice += OfferedPrice;
                            OfferedSerial = reader["OfferedSerial"].ToString();
                            ViewBag.OfferedSerial = OfferedSerial;
                            try
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = ID,
                                    Title = Title,
                                    Creditor = Creditor,
                                    Debtor = Debtor,
                                    Rem = Rem,
                                    NonCash = NonCash,
                                    Amount = Amount,
                                    Deductions = Deductions,
                                    GrossPercent = GrossPercent,
                                    NetPercent = NetPercent,
                                    RemWithDeductions = RemWithDeductions,
                                    PreInvoiceHint = PreInvoiceHint,
                                    OfferedPrice= OfferedPrice,
                                    OfferedSerial=OfferedSerial
                                });

                            }
                            catch
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = "0",
                                    Title = "",
                                    Creditor = 0,
                                    Debtor = 0,
                                    Rem = 0,
                                    NonCash = 0,
                                    Amount = 0,
                                    Deductions = 0,
                                    GrossPercent = "0",
                                    NetPercent = "0",
                                    RemWithDeductions = 0,
                                    PreInvoiceHint = 0,
                                    OfferedPrice=0,
                                    OfferedSerial=""
                                });

                            }
                        }                        
                        conn.Close();
                    }
                }//end using
                ViewBag.DebitCreditOneView = DebitCreditOneView;
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //
                ViewBag.TableOuts = TableOuts;
                ViewBag.sumCount = sumCount;
                if (sumPrice > 0)
                {
                    ViewBag.sumPrice = sumPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.sumPrice = sumPrice;
                }

                if (sumSalary > 0)
                {
                    ViewBag.sumSalary = sumSalary.ToString("#,##");
                }
                else
                {
                    ViewBag.sumSalary = sumSalary;
                }
                if (SumNonCash > 0)
                {
                    ViewBag.sumNonCash = SumNonCash.ToString("#,##");
                }
                else
                {
                    ViewBag.sumNonCash = SumNonCash;
                }
                //
                if (SumDeductions > 0)
                {
                    ViewBag.sumDeductions = SumDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDeductions = SumDeductions;
                }
                //
                if (SumRemWithDeductions > 0)
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions;
                }
                //                
                if (SumOfferedPrice > 0)
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice;
                }
            }
            catch
            {
                
            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View();
        }
        //
        [HttpPost]
        public JsonResult OfferedPrice(List<OfferedPrice> OfferedPrice)
        {
            string responseText = "";
            int count = 0;
            bool status = false;
            string number = "1";
            //string Serial = "";
            //
            var OfferedPriceList = db.tbl_OfferedPrices.OrderByDescending(o => o.Date).ToList();

            foreach (var item in OfferedPrice)
            {
                //var OfferedPriceList = db.tbl_OfferedPrices.Where(o => o.WorkshopID == item.WorkshopID).OrderByDescending(o => o.Date).ToList();
                if (OfferedPriceList.Count() == 0)
                {
                    OfferedPrice additem = new OfferedPrice();
                    additem.Number = 1;
                    additem.Serial = pc.GetYear(DateTime.Now).ToString() + "/" + pc.GetMonth(DateTime.Now).ToString() + "/" + pc.GetDayOfMonth(DateTime.Now).ToString() + "-" + number;
                    additem.CreateDate = DateTime.Now;
                    additem.Creator = User.Identity.Name;
                    additem.Date = DateTime.Now;
                    additem.Value = Convert.ToDouble(item.Value.ToString().Replace(",", ""));
                    additem.WorkshopID = item.WorkshopID;
                    db.tbl_OfferedPrices.Add(additem);
                    db.SaveChanges();
                    count++;
                    responseText = "مبلغ پیشنهادی دستور پرداخت در وجه "+ count + " کارگاه با موفقیت ذخیره شد.";
                    status = true;
                    //return RedirectToAction("Index", new { message = "مبلغ پیشنهادی دستور پرداخت در وجه " + Helper.Helpers.GetWorkshops(item.WorkshopID).Title + " " + "با موفقیت ذخیره شد.", color = "green" });
                }
                else
                {
                    string existDate = OfferedPriceList.FirstOrDefault().Date.ToShortDateString();
                    number = (OfferedPriceList.Max(o => o.Number) + 1).ToString();
                    if (!existDate.Equals(DateTime.Now.ToShortDateString()))
                    {
                        OfferedPrice additem = new OfferedPrice();
                        additem.Number = Convert.ToInt32(number);
                        //if(count==0)
                        additem.Serial = pc.GetYear(DateTime.Now).ToString() + "/" + pc.GetMonth(DateTime.Now).ToString() + "/" + pc.GetDayOfMonth(DateTime.Now).ToString() + "-" + number;
                        //Serial = additem.Serial;
                        //if (count > 0)
                        //additem.Serial = Serial;
                        additem.CreateDate = DateTime.Now;
                        additem.Creator = User.Identity.Name;
                        additem.Date = DateTime.Now;
                        additem.Value = Convert.ToDouble(item.Value.ToString().Replace(",", ""));
                        additem.WorkshopID = item.WorkshopID;
                        db.tbl_OfferedPrices.Add(additem);
                        db.SaveChanges();
                        count++;
                        responseText = "مبلغ پیشنهادی دستور پرداخت در وجه " + count + " کارگاه با موفقیت ذخیره شد.";
                        status = true;
                        //return RedirectToAction("Index", new { message = "مبلغ پیشنهادی دستور پرداخت در وجه " + Helper.Helpers.GetWorkshops(item.WorkshopID).Title + " " + "با موفقیت ذخیره شد.", color = "green" });
                    }
                    else
                    {
                        count++;
                        responseText = "مبلغ پیشنهادی دستور پرداخت در وجه " + count + " " + "کارگاه در تاریخ جاری تکراری بوده و لذا ثبت نگردید!";
                        status = false;
                        //return RedirectToAction("Index", new { message = "مبلغ پیشنهادی دستور پرداخت در وجه " + Helper.Helpers.GetWorkshops(item.WorkshopID).Title + " " + "در تاریخ جاری تکراری بوده و لذا ثبت نگردید!", color = "red" });
                    }
                }
            }
            //responseText = "مبلغ پیشنهادی دستور پرداخت در وجه " + count + " کارگاه با موفقیت ذخیره شد.";
            Thread.Sleep(1000);
            return Json(new { success = status, responseText = responseText }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        //Clearing the account
        public ActionResult ClearingtheAccount(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {            
            ViewBag.WorkshopID = WorkshopID;
            ViewBag.Workshop = Helper.Helpers.GetWorkshops(WorkshopID).Title + " - " + Helper.Helpers.GetWorkshops(WorkshopID).City.Title;
            string existOfferedDate = "";
            var OfferedDate = db.tbl_OfferedPrices.OrderByDescending(o => o.Date).ToList();
            if (OfferedDate.Count() > 0)
                existOfferedDate = OfferedDate.FirstOrDefault().Date.ToShortDateString();
            ViewBag.existOfferedDate = existOfferedDate;
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);            
            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            string Type = "";
            int Count = 0;
            double Price = 0.0;
            double Salary = 0.0;
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;
            //
            string PreInvoiceCode = "";
            string WorkshopsID = "";
            string SaleCondition = "";
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            //

            List<PreInvoicesHint> PreInvoicesHint = new List<PreInvoicesHint>();
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            List<FinancialDebitCredit> DebitCreditOneView = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                        
                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumRem += Rem;

                            TableDebitCredit.Add(new FinancialDebitCredit
                            {
                                ID = ID,
                                Title = Title,
                                Creditor = Creditor,
                                Debtor = Debtor,
                                Rem = Rem
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Count = Convert.ToInt32(reader["Count"].ToString());
                            sumCount += Count;
                            Price = Convert.ToDouble(reader["Price"].ToString());
                            sumPrice += Price;
                            Salary = Convert.ToDouble(reader["Salary"].ToString());
                            sumSalary += Salary;

                            TableOuts.Add(new Models.Financial
                            {
                                Type = Type,
                                Count = Count,
                                Price = Price,
                                Salary = Salary
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumTCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumTDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumTRem += Rem;
                            NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                            SumNonCash += NonCash;
                            Amount = Convert.ToDouble(reader["Amount"].ToString());

                            Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                            SumDeductions += Deductions;
                            RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                            SumRemWithDeductions += RemWithDeductions;
                            NetPercent = reader["NetPercent"].ToString();
                            GrossPercent = reader["GrossPercent"].ToString();
                            PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                            SumPreInvoiceHint += PreInvoiceHint;
                            OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                            SumOfferedPrice += OfferedPrice;

                            try
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = ID,
                                    Title = Title,
                                    Creditor = Creditor,
                                    Debtor = Debtor,
                                    Rem = Rem,
                                    NonCash = NonCash,
                                    Amount = Amount,
                                    Deductions = Deductions,
                                    GrossPercent = GrossPercent,
                                    NetPercent = NetPercent,
                                    RemWithDeductions = RemWithDeductions,
                                    PreInvoiceHint = PreInvoiceHint,
                                    OfferedPrice = OfferedPrice
                                });

                            }
                            catch
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = "0",
                                    Title = "",
                                    Creditor = 0,
                                    Debtor = 0,
                                    Rem = 0,
                                    NonCash = 0,
                                    Amount = 0,
                                    Deductions = 0,
                                    GrossPercent = "0",
                                    NetPercent = "0",
                                    RemWithDeductions = 0,
                                    PreInvoiceHint = 0,
                                    OfferedPrice = 0
                                });

                            }
                        }
                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    SaleCondition = reader["SaleCondition"].ToString();
                        //    Amount = Convert.ToDouble(reader["Amount"].ToString());

                        //    PreInvoicesHint.Add(new PreInvoicesHint
                        //    {
                        //        Amount=Amount,
                        //        SaleCondition=SaleCondition
                        //    });
                        //}
                        //
                        conn.Close();
                    }
                }//end using
                ViewBag.TableDebitCredit = TableDebitCredit;
                ViewBag.DebitCreditOneView = DebitCreditOneView;
                //ViewBag.PreInvoicesHint = PreInvoicesHint;
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //
                ViewBag.TableOuts = TableOuts;
                ViewBag.sumCount = sumCount;
                if (sumPrice > 0)
                {
                    ViewBag.sumPrice = sumPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.sumPrice = sumPrice;
                }

                if (sumSalary > 0)
                {
                    ViewBag.sumSalary = sumSalary.ToString("#,##");
                }
                else
                {
                    ViewBag.sumSalary = sumSalary;
                }
                if (SumNonCash > 0)
                {
                    ViewBag.sumNonCash = SumNonCash.ToString("#,##");
                }
                else
                {
                    ViewBag.sumNonCash = SumNonCash;
                }
                //
                if (SumDeductions > 0)
                {
                    ViewBag.sumDeductions = SumDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDeductions = SumDeductions;
                }
                //
                if (SumRemWithDeductions > 0)
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions;
                }
                //                
                if (SumOfferedPrice > 0)
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice;
                }
            }
            catch
            {
                ViewBag.TankValveTableOuts = null;
            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }

        //Clearing the account
        public ActionResult FinancialAssesment(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            ViewBag.WorkshopID = WorkshopID;
            ViewBag.Workshop = Helper.Helpers.GetWorkshops(WorkshopID).Title + " - " + Helper.Helpers.GetWorkshops(WorkshopID).City.Title;
            string existOfferedDate = "";
            var OfferedDate = db.tbl_OfferedPrices.OrderByDescending(o => o.Date).ToList();
            if (OfferedDate.Count() > 0)
                existOfferedDate = OfferedDate.FirstOrDefault().Date.ToShortDateString();
            ViewBag.existOfferedDate = existOfferedDate;
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);            
            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            string Type = "";
            int Count = 0;
            double Price = 0.0;
            double Salary = 0.0;
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;
            //
            string PreInvoiceCode = "";
            string WorkshopsID = "";
            string SaleCondition = "";
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            //

            List<PreInvoicesHint> PreInvoicesHint = new List<PreInvoicesHint>();
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            List<FinancialDebitCredit> DebitCreditOneView = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_FinancialAssesment]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumRem += Rem;

                            TableDebitCredit.Add(new FinancialDebitCredit
                            {
                                ID = ID,
                                Title = Title,
                                Creditor = Creditor,
                                Debtor = Debtor,
                                Rem = Rem
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Count = Convert.ToInt32(reader["Count"].ToString());
                            sumCount += Count;
                            Price = Convert.ToDouble(reader["Price"].ToString());
                            sumPrice += Price;
                            Salary = Convert.ToDouble(reader["Salary"].ToString());
                            sumSalary += Salary;

                            TableOuts.Add(new Models.Financial
                            {
                                Type = Type,
                                Count = Count,
                                Price = Price,
                                Salary = Salary
                            });
                        }
                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    ID = reader["ID"].ToString();
                        //    Title = reader["Title"].ToString();
                        //    Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                        //    sumTCredit += Creditor;
                        //    Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                        //    sumTDebit += Debtor;
                        //    Rem = Convert.ToDouble(reader["Rem"].ToString());
                        //    sumTRem += Rem;
                        //    NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                        //    SumNonCash += NonCash;
                        //    Amount = Convert.ToDouble(reader["Amount"].ToString());

                        //    Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                        //    SumDeductions += Deductions;
                        //    RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                        //    SumRemWithDeductions += RemWithDeductions;
                        //    NetPercent = reader["NetPercent"].ToString();
                        //    GrossPercent = reader["GrossPercent"].ToString();
                        //    PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                        //    SumPreInvoiceHint += PreInvoiceHint;
                        //    OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                        //    SumOfferedPrice += OfferedPrice;

                        //    try
                        //    {
                        //        DebitCreditOneView.Add(new FinancialDebitCredit
                        //        {
                        //            ID = ID,
                        //            Title = Title,
                        //            Creditor = Creditor,
                        //            Debtor = Debtor,
                        //            Rem = Rem,
                        //            NonCash = NonCash,
                        //            Amount = Amount,
                        //            Deductions = Deductions,
                        //            GrossPercent = GrossPercent,
                        //            NetPercent = NetPercent,
                        //            RemWithDeductions = RemWithDeductions,
                        //            PreInvoiceHint = PreInvoiceHint,
                        //            OfferedPrice = OfferedPrice
                        //        });

                        //    }
                        //    catch
                        //    {
                        //        DebitCreditOneView.Add(new FinancialDebitCredit
                        //        {
                        //            ID = "0",
                        //            Title = "",
                        //            Creditor = 0,
                        //            Debtor = 0,
                        //            Rem = 0,
                        //            NonCash = 0,
                        //            Amount = 0,
                        //            Deductions = 0,
                        //            GrossPercent = "0",
                        //            NetPercent = "0",
                        //            RemWithDeductions = 0,
                        //            PreInvoiceHint = 0,
                        //            OfferedPrice = 0
                        //        });

                        //    }
                        //}
                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    SaleCondition = reader["SaleCondition"].ToString();
                        //    Amount = Convert.ToDouble(reader["Amount"].ToString());

                        //    PreInvoicesHint.Add(new PreInvoicesHint
                        //    {
                        //        Amount=Amount,
                        //        SaleCondition=SaleCondition
                        //    });
                        //}
                        //
                        conn.Close();
                    }
                }//end using
                ViewBag.TableDebitCredit = TableDebitCredit;
                ViewBag.DebitCreditOneView = DebitCreditOneView;
                //ViewBag.PreInvoicesHint = PreInvoicesHint;
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //
                ViewBag.TableOuts = TableOuts;
                ViewBag.sumCount = sumCount;
                if (sumPrice > 0)
                {
                    ViewBag.sumPrice = sumPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.sumPrice = sumPrice;
                }

                if (sumSalary > 0)
                {
                    ViewBag.sumSalary = sumSalary.ToString("#,##");
                }
                else
                {
                    ViewBag.sumSalary = sumSalary;
                }
                if (SumNonCash > 0)
                {
                    ViewBag.sumNonCash = SumNonCash.ToString("#,##");
                }
                else
                {
                    ViewBag.sumNonCash = SumNonCash;
                }
                //
                if (SumDeductions > 0)
                {
                    ViewBag.sumDeductions = SumDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDeductions = SumDeductions;
                }
                //
                if (SumRemWithDeductions > 0)
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions;
                }
                //                
                if (SumOfferedPrice > 0)
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice;
                }
            }
            catch
            {
                ViewBag.TankValveTableOuts = null;
            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View(TableOuts.ToList());
        }
        //
        //
        //
        [HttpGet]
        public ActionResult EditOfferedPrice(int? WorkshopID,int? number, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);

            //
            if (WorkshopID == null)
                if (cngfapco.Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
                    WorkshopID = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId()).WorkshopID;
                else
                    WorkshopID = 0;

            //
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            int sumCount = 0;
            double sumPrice = 0.0;
            double sumSalary = 0.0;
            double NonCash = 0.0;
            double SumNonCash = 0.0;
            double Deductions = 0.0;
            double RemWithDeductions = 0.0;
            string NetPercent = "0";
            string GrossPercent = "0";
            double SumDeductions = 0.0;
            double SumRemWithDeductions = 0.0;
            double PreInvoiceHint = 0.0;
            double SumPreInvoiceHint = 0.0;
            double Amount = 0.0;
            double OfferedPrice = 0.0;
            double SumOfferedPrice = 0.0;
            string OfferedSerial = "";
            string OfferedID = "";
            //

            List<PreInvoicesHint> PreInvoicesHint = new List<PreInvoicesHint>();
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<cngfapco.Models.Financial> TableOuts = new List<cngfapco.Models.Financial>();
            List<FinancialDebitCredit> DebitCreditOneView = new List<FinancialDebitCredit>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_VehicleRegistrationSalary]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        reader.NextResult();
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumTCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumTDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumTRem += Rem;
                            NonCash = Convert.ToDouble(reader["NonCash"].ToString());
                            SumNonCash += NonCash;
                            Amount = Convert.ToDouble(reader["Amount"].ToString());

                            Deductions = Convert.ToDouble(reader["Deductions"].ToString());
                            SumDeductions += Deductions;
                            RemWithDeductions = Convert.ToDouble(reader["RemWithDeductions"].ToString());
                            SumRemWithDeductions += RemWithDeductions;
                            NetPercent = reader["NetPercent"].ToString();
                            GrossPercent = reader["GrossPercent"].ToString();
                            PreInvoiceHint = Convert.ToDouble(reader["PreInvoiceHint"].ToString());
                            SumPreInvoiceHint += PreInvoiceHint;
                            OfferedPrice = Convert.ToDouble(reader["OfferedPrice"].ToString());
                            SumOfferedPrice += OfferedPrice;
                            OfferedSerial = reader["OfferedSerial"].ToString();
                            ViewBag.OfferedSerial = OfferedSerial;
                            OfferedID = reader["OfferedID"].ToString();

                            try
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = ID,
                                    Title = Title,
                                    Creditor = Creditor,
                                    Debtor = Debtor,
                                    Rem = Rem,
                                    NonCash = NonCash,
                                    Amount = Amount,
                                    Deductions = Deductions,
                                    GrossPercent = GrossPercent,
                                    NetPercent = NetPercent,
                                    RemWithDeductions = RemWithDeductions,
                                    PreInvoiceHint = PreInvoiceHint,
                                    OfferedPrice = OfferedPrice,
                                    OfferedSerial = OfferedSerial,
                                    OfferedID = OfferedID
                                });

                            }
                            catch
                            {
                                DebitCreditOneView.Add(new FinancialDebitCredit
                                {
                                    ID = "0",
                                    Title = "",
                                    Creditor = 0,
                                    Debtor = 0,
                                    Rem = 0,
                                    NonCash = 0,
                                    Amount = 0,
                                    Deductions = 0,
                                    GrossPercent = "0",
                                    NetPercent = "0",
                                    RemWithDeductions = 0,
                                    PreInvoiceHint = 0,
                                    OfferedPrice = 0,
                                    OfferedSerial = "",
                                    OfferedID=""
                                });

                            }
                        }
                        conn.Close();
                    }
                }//end using
                ViewBag.DebitCreditOneView = DebitCreditOneView;
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //
                ViewBag.TableOuts = TableOuts;
                ViewBag.sumCount = sumCount;
                if (sumPrice > 0)
                {
                    ViewBag.sumPrice = sumPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.sumPrice = sumPrice;
                }

                if (sumSalary > 0)
                {
                    ViewBag.sumSalary = sumSalary.ToString("#,##");
                }
                else
                {
                    ViewBag.sumSalary = sumSalary;
                }
                if (SumNonCash > 0)
                {
                    ViewBag.sumNonCash = SumNonCash.ToString("#,##");
                }
                else
                {
                    ViewBag.sumNonCash = SumNonCash;
                }
                //
                if (SumDeductions > 0)
                {
                    ViewBag.sumDeductions = SumDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDeductions = SumDeductions;
                }
                //
                if (SumRemWithDeductions > 0)
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions.ToString("#,##");
                }
                else
                {
                    ViewBag.SumRemWithDeductions = SumRemWithDeductions;
                }
                //                
                if (SumOfferedPrice > 0)
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice.ToString("#,##");
                }
                else
                {
                    ViewBag.SumOfferedPrice = SumOfferedPrice;
                }
            }
            catch
            {

            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            ViewBag.number = number;


            return View();
        }
        //
        [HttpPost]
        public JsonResult EditOfferedPrice(List<OfferedPrice> OfferedPrice)
        {
            string responseText = "";
            int count = 0;
            bool status = false;
            //string number = "1";
            //
            foreach (var item in OfferedPrice)
            {
                OfferedPrice additem = db.tbl_OfferedPrices.Find(Convert.ToInt32(item.ID));
                additem.CreateDate = DateTime.Now;
                additem.Creator = User.Identity.Name;
                additem.Value = Convert.ToDouble(item.Value.ToString().Replace(",", ""));
                db.Entry(additem).State = EntityState.Modified;
                db.SaveChanges();
                count++;
                responseText = "مبلغ پیشنهادی دستور پرداخت در وجه " + count + " کارگاه با موفقیت ویرایش شد.";
                status = true;
            }
            Thread.Sleep(1000);
            return Json(new { success = status, responseText = responseText }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        //
        [HttpGet]
        public ActionResult PayofOfferedList()
        {
            string Date = "";
            double Sumvalue = 0.0;
            double PaiedValue = 0.0;
            string Serial = "";
            string Number = "";
            //
            List<OfferedList> TableOuts = new List<OfferedList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_PayofOfferedList]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                       // command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;

                        conn.Open();
                        reader = command.ExecuteReader();                        
                        while (reader.Read())
                        {
                            Date = Convert.ToDateTime(reader["Date"].ToString()).ToShortDateString();
                            Sumvalue = Convert.ToDouble(reader["Sumvalue"].ToString());
                            PaiedValue = Convert.ToDouble(reader["PaiedValue"].ToString());
                            Serial = reader["Serial"].ToString();
                            Number = reader["Number"].ToString();

                            try
                            {
                                TableOuts.Add(new OfferedList
                                {
                                    Date=Date,
                                    Number=Number,
                                    PaiedValue=PaiedValue.ToString("#,##"),
                                    Serial=Serial,
                                    Sumvalue= Sumvalue.ToString("#,##")

                                });

                            }
                            catch
                            {
                                TableOuts.Add(new OfferedList
                                {
                                    Date = "",
                                    Number = "",
                                    PaiedValue = "",
                                    Serial = "",
                                    Sumvalue = ""

                                });

                            }
                        }
                        conn.Close();
                    }
                }//end using
                ViewBag.tableOut = TableOuts;
            }
            catch
            {

            }
            return View();
        }
        //
        public class OfferedList
        {
            public string Date { get; set; }
            public string Sumvalue { get; set; }
            public string PaiedValue { get; set; }
            public string Serial { get; set; }
            public string Number { get; set; }

        }
        [HttpGet]
        public ActionResult PayofOffered(int? number)
        {
            ViewBag.serial = "";
            var offeredList = db.tbl_OfferedPrices.Where(o => o.Number == number).ToList();
            if (offeredList.Count() > 0)
                ViewBag.serial = offeredList.OrderByDescending(o => o.ID).Take(1).SingleOrDefault().Serial;
            return View(offeredList);
        }
        //
        [HttpGet]
        public ActionResult PayofOfferedCheck(int? number)
        {
            ViewBag.serial = "";
            var offeredList = db.tbl_OfferedPrices.Where(o => o.Number == number).ToList();
            if (offeredList.Count() > 0)
                ViewBag.serial = offeredList.OrderByDescending(o => o.ID).Take(1).SingleOrDefault().Serial;
            return View(offeredList);
        }
        //
        [HttpPost]
        public JsonResult PayofOffered(List<FinancialPayment> PayofOffered, string serial)
        {
            string responseText = "";
            int count = 0;
            bool status = false;
            //
            foreach (var item in PayofOffered)
            {
                if (item.Value > 0)
                {
                    FinancialPayment additem = new FinancialPayment();
                    additem.CreateDate = DateTime.Now;
                    additem.Creator = User.Identity.Name;
                    additem.Value = Convert.ToDouble(item.Value.ToString().Replace(",", ""));
                    additem.FinancialDescID = item.FinancialDescID;
                    additem.Description = item.Description + "-" + "بر اساس دستور پرداخت شماره: " + serial;
                    if(item.Date!=null)
                        additem.Date = item.Date;
                    additem.WorkshopID = item.WorkshopID;

                    db.tbl_FinancialPayments.Add(additem);
                    db.SaveChanges();
                    count++;
                    responseText = "مبلغ پیشنهادی دستور پرداخت در وجه " + count + " کارگاه با موفقیت ویرایش شد.";
                    status = true;
                    //
                    var updateoffered = db.tbl_OfferedPrices.Where(o => o.Serial.Equals(serial) && o.WorkshopID==item.WorkshopID).ToList();
                    foreach(var items in updateoffered)
                    {
                        items.PayDate = item.Date;
                        items.PaiedValue = additem.Value;
                        items.StatusPay = true;
                        db.Entry(items).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }                
            }
            Thread.Sleep(1000);
            return Json(new { success = status, responseText = responseText }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        //
        //
        [HttpGet]
        public ActionResult FinalStatus(string InvoiceCode)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            var request = db.tbl_RequestFreeSales.Where(u => u.InvoiceCode.Equals(InvoiceCode)).ToList();
            return View(request);
        }
        //
        [HttpPost]
        public ActionResult FinalStatus(string InvoiceCode, bool Status)
        {
            var request = db.tbl_RequestFreeSales.Where(u => u.InvoiceCode.Equals(InvoiceCode)).ToList();
            foreach(var item in request)
            {
                item.FinalStatus = false;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("RequestFreeSaleList");
        }
        //
        // GET: RequestFreeSales/Edit/5
        public ActionResult RequestFreeSaleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            if (requestFreeSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", requestFreeSale.CurrencyTypeID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", requestFreeSale.EquipmentsID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", requestFreeSale.WorkshopsID);
            return View(requestFreeSale);
        }

        // POST: RequestFreeSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestFreeSaleEdit([Bind(Include = "InvoiceID,CreatedDate,Owners,WorkshopsID,EquipmentsID,ServiceCode,InvoiceCode,EmployerEconomicalnumber,Employerregistrationnumber,EmployerState,EmployerAddress,EmployerPostalcode,EmployerPhone,EmployerFax,ServiceDesc,Number,UnitofMeasurement,UnitAmount,TotalAmount,DiscountAmount,Description,SaleCondition,Comment,Status,CurrencyTypeID,CreatorUser,ViewStatus,Viewer,FinalStatus")] RequestFreeSale requestFreeSale)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(requestFreeSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RequestFreeSaleDetails",new { InvoiceCode = requestFreeSale.InvoiceCode});
            }
            ViewBag.CurrencyTypeID = new SelectList(db.tbl_CurrencyTypes, "ID", "Title", requestFreeSale.CurrencyTypeID);
            ViewBag.EquipmentsID = new SelectList(db.tbl_EquipmentList, "ID", "Title", requestFreeSale.EquipmentsID);
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", requestFreeSale.WorkshopsID);
            return View(requestFreeSale);
        }

        // GET: RequestFreeSales/Delete/5
        public ActionResult RequestFreeSaleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            if (requestFreeSale == null)
            {
                return HttpNotFound();
            }
            return View(requestFreeSale);
        }

        // POST: RequestFreeSales/Delete/5
        [HttpPost, ActionName("RequestFreeSaleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RequestFreeSaleDeleteConfirmed(int id)
        {
            RequestFreeSale requestFreeSale = db.tbl_RequestFreeSales.Find(id);
            string InvoiceCode = requestFreeSale.InvoiceCode;
            db.tbl_RequestFreeSales.Remove(requestFreeSale);
            db.SaveChanges();
            return RedirectToAction("RequestFreeSaleDetails", new { InvoiceCode = InvoiceCode });
        }
        //
        [HttpGet]
        public ActionResult CheckStatus(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            //var item = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID==workshopId).Include(u => u.Workshops).ToList();
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult CheckStatus(string InvoiceCode, int? workshopId, bool Status)
        {
            var items = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach(var item in items)
            {
                item.CheckStatus = Status;
                item.CkeckedDate = DateTime.Now;
                item.CkeckedUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            string workshopTitle = items.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapaListTotal", new { InvoiceCode = InvoiceCode, workshopId = workshopId, workshopTitle = workshopTitle });
        }
        //
        [HttpGet]
        public ActionResult FinancialStatus(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            //var item = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID==workshopId).Include(u => u.Workshops).ToList();
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult FinancialStatus(string InvoiceCode, int? workshopId, bool Status)
        {
            string Type = "1";
            var invoice = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach (var item in invoice)
            {
                item.FinancialStatus = Status;
                item.FinancialDate = DateTime.Now;
                item.FinancialUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            //
            string fromdateInString = invoice.FirstOrDefault().EmployerEconomicalnumber;
            DateTime todateInString = invoice.FirstOrDefault().CreatedDate.Value;
            int fYear = pc.GetYear(Convert.ToDateTime(fromdateInString));
            int fMonth = pc.GetMonth(Convert.ToDateTime(fromdateInString));
            int fDay = pc.GetDayOfMonth(Convert.ToDateTime(fromdateInString));

            DateTime Pfromdate = new DateTime(fYear, fMonth, fDay);
            string p2fromdate = Pfromdate.ToString(CultureInfo.InvariantCulture);
            string p2fMonth = "01";
            string p2fDay = "01";
            string p2tMonth = "01";
            string p2tDay = "01";
            if (Pfromdate.Month < 10)
                p2fMonth = "0" + Pfromdate.Month;
            else
                p2fMonth = Pfromdate.Month.ToString();

            if (Pfromdate.Day < 10)
                p2fDay = "0" + Pfromdate.Day;
            else
                p2fDay = (Pfromdate.Day).ToString();

            string p3fromdate = Pfromdate.Year + "-" + p2fMonth + "-" + p2fDay;
            //
            if (todateInString.Month < 10)
                p2tMonth = "0" + todateInString.Month;
            else
                p2tMonth = todateInString.Month.ToString();

            if (todateInString.Day < 10)
                p2tDay = "0" + todateInString.Day;
            else
                p2tDay = (todateInString.Day).ToString();
            string todate = todateInString.Year + "-" + p2tMonth + "-" + p2tDay;
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[sp_FinancialAssesment_Vehicle]";
            cmd.Parameters.Add(new SqlParameter("@invoiceCode", InvoiceCode));
            cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
            cmd.Parameters.Add(new SqlParameter("@fromdate", p3fromdate));
            cmd.Parameters.Add(new SqlParameter("@todate", todate));
            cmd.Parameters.Add(new SqlParameter("@user", User.Identity.Name));
            cmd.Parameters.Add(new SqlParameter("@type", Type));
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            //
            //
            string workshopTitle = invoice.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapaListTotal", new { InvoiceCode = InvoiceCode, workshopId = workshopId, workshopTitle = workshopTitle });
        }
        //
        //Clearing the account
        public ActionResult ApprovedInvoices(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            ViewBag.WorkshopID = WorkshopID;
            ViewBag.Workshop = Helper.Helpers.GetWorkshops(WorkshopID).Title + " - " + Helper.Helpers.GetWorkshops(WorkshopID).City.Title;
            string existOfferedDate = "";
            var OfferedDate = db.tbl_OfferedPrices.OrderByDescending(o => o.Date).ToList();
            if (OfferedDate.Count() > 0)
                existOfferedDate = OfferedDate.FirstOrDefault().Date.ToShortDateString();
            ViewBag.existOfferedDate = existOfferedDate;
            //
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);            
            //           
            string ID = "";
            string Title = "";
            double Creditor = 0.0;
            double Debtor = 0.0;
            double Rem = 0.0;
            double sumDebit = 0.0;
            double sumCredit = 0.0;
            double sumRem = 0.0;
            double sumTDebit = 0.0;
            double sumTCredit = 0.0;
            double sumTRem = 0.0;
            //
            string InvoiceCode = "";
            string ServiceDesc = "";
            string Number = "";
            string UnitofMeasurement = "";
            string Description = "";
            double UnitAmount = 0.0;
            double TotalAmount = 0.0;
            //
            double SumTotalAmount = 0.0;
            double SumCount = 0.0;
            //
            List<FinancialDebitCredit> TableDebitCredit = new List<FinancialDebitCredit>();
            List<ApprovedInvoice> TableOuts = new List<ApprovedInvoice>();
            List<TotalAmount> TableAmount = new List<TotalAmount>();
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_ApprovedInvoices]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ID = reader["ID"].ToString();
                            Title = reader["Title"].ToString();
                            Creditor = Convert.ToDouble(reader["Creditor"].ToString());
                            sumCredit += Creditor;
                            Debtor = Convert.ToDouble(reader["Debtor"].ToString());
                            sumDebit += Debtor;
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            sumRem += Rem;

                            TableDebitCredit.Add(new FinancialDebitCredit
                            {
                                ID = ID,
                                Title = Title,
                                Creditor = Creditor,
                                Debtor = Debtor,
                                Rem = Rem
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            InvoiceCode = reader["InvoiceCode"].ToString();
                            ServiceDesc = reader["ServiceDesc"].ToString();
                            Number = reader["Number"].ToString();
                            UnitAmount = Convert.ToDouble(reader["UnitAmount"].ToString());
                            TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
                            UnitofMeasurement = reader["UnitofMeasurement"].ToString();
                            Description = reader["Description"].ToString();

                            TableOuts.Add(new ApprovedInvoice
                            {
                                InvoiceCode=InvoiceCode,
                                ServiceDesc=ServiceDesc,
                                Number=Number,
                                TotalAmount=TotalAmount,
                                UnitAmount=UnitAmount,
                                UnitofMeasurement= UnitofMeasurement,
                                Description=Description
                            });
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            SumCount = Convert.ToDouble(reader["SumCount"].ToString());
                            SumTotalAmount = Convert.ToDouble(reader["SumTotalAmount"].ToString());

                            TableAmount.Add(new TotalAmount
                            {
                                SumCount=SumCount.ToString(),
                                SumTotalAmount=SumTotalAmount.ToString("#,##")
                            });
                        }
                        //
                        ViewBag.TableDebitCredit = TableDebitCredit;
                        ViewBag.TableOuts = TableOuts;
                        ViewBag.TotalAmount = TableAmount;
                        //
                        conn.Close();
                    }
                }//end using
                
                if (sumCredit > 0)
                {
                    ViewBag.sumCredit = sumCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumCredit = sumCredit;
                }

                if (sumDebit > 0)
                {
                    ViewBag.sumDebit = sumDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumDebit = sumDebit;
                }

                if (sumRem > 0)
                {
                    ViewBag.sumRem = sumRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumRem = sumRem;
                }
                //
                if (sumTCredit > 0)
                {
                    ViewBag.sumTCredit = sumTCredit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTCredit = sumTCredit;
                }

                if (sumTDebit > 0)
                {
                    ViewBag.sumTDebit = sumTDebit.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTDebit = sumTDebit;
                }

                if (sumTRem > 0)
                {
                    ViewBag.sumTRem = sumTRem.ToString("#,##");
                }
                else
                {
                    ViewBag.sumTRem = sumTRem;
                }
                //                
            }
            catch
            {
                ViewBag.TableDebitCredit = null;
                ViewBag.TableOuts = null;
                ViewBag.TotalAmount = null;
            }

            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();

            return View();
        }
        //
        //
        //
        [HttpGet]
        public ActionResult ReciveStatus(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            //var item = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID==workshopId).Include(u => u.Workshops).ToList();
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult ReciveStatus(string InvoiceCode, int? workshopId, bool Status)
        {
            var items = db.tbl_InvoicesFapa.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach (var item in items)
            {
                item.ReciveStatus = Status;
                item.ReciveDate = DateTime.Now;
                item.ReciveUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            string workshopTitle = items.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapaListTotal", new { InvoiceCode = InvoiceCode, workshopId = workshopId, workshopTitle = workshopTitle });
        }

        //
        public ActionResult InvoiceFapaStatusList()
        {
            string WorkshopsID = "";
            string Title = "";
            string Count = "";
            string ReciveCount = "";
            string CheckedCount = "";
            string FinancialCount = "";
            string InCheckedProccess = "";
            string InFinancialProccess = "";
            string Deliver = "";
            //
            List<StatusList> tableOuts = new List<StatusList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            //
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceFapaStatusList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add("@", SqlDbType.VarChar).Value = ;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        WorkshopsID = reader["WorkshopsID"].ToString();
                        Title = reader["Title"].ToString();
                        Count = reader["Count"].ToString();
                        CheckedCount = reader["CheckedCount"].ToString();
                        ReciveCount = reader["ReciveCount"].ToString();
                        FinancialCount = reader["FinancialCount"].ToString();
                        InCheckedProccess = reader["InCheckedProccess"].ToString();
                        InFinancialProccess = reader["InFinancialProccess"].ToString();
                        Deliver = reader["Deliver"].ToString();

                        tableOuts.Add(new StatusList
                        {
                            Title=Title,
                            WorkshopsID=WorkshopsID,
                            CheckedCount=CheckedCount,
                            Count=Count,
                            Deliver=Deliver,
                            FinancialCount=FinancialCount,
                            InCheckedProccess=InCheckedProccess,
                            InFinancialProccess=InFinancialProccess,
                            ReciveCount=ReciveCount
                        });
                    }
                    conn.Close();
                    //
                }
            }//end using
            ViewBag.tableOuts = tableOuts;
            return View();
        }
        //
        public ActionResult InvoiceFapa_DamagesCylinderStatusList()
        {
            string WorkshopsID = "";
            string Title = "";
            string Count = "";
            string ReciveCount = "";
            string CheckedCount = "";
            string FinancialCount = "";
            string InCheckedProccess = "";
            string InFinancialProccess = "";
            string Deliver = "";
            //
            List<StatusList> tableOuts = new List<StatusList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            //
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceFapa_DamagesCylinderStatusList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add("@", SqlDbType.VarChar).Value = ;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        WorkshopsID = reader["WorkshopsID"].ToString();
                        Title = reader["Title"].ToString();
                        Count = reader["Count"].ToString();
                        CheckedCount = reader["CheckedCount"].ToString();
                        ReciveCount = reader["ReciveCount"].ToString();
                        FinancialCount = reader["FinancialCount"].ToString();
                        InCheckedProccess = reader["InCheckedProccess"].ToString();
                        InFinancialProccess = reader["InFinancialProccess"].ToString();
                        Deliver = reader["Deliver"].ToString();

                        tableOuts.Add(new StatusList
                        {
                            Title = Title,
                            WorkshopsID = WorkshopsID,
                            CheckedCount = CheckedCount,
                            Count = Count,
                            Deliver = Deliver,
                            FinancialCount = FinancialCount,
                            InCheckedProccess = InCheckedProccess,
                            InFinancialProccess = InFinancialProccess,
                            ReciveCount = ReciveCount
                        });
                    }
                    conn.Close();
                    //
                }
            }//end using
            ViewBag.tableOuts = tableOuts;
            return View();
        }
        //
        public class StatusList
        {
            public string WorkshopsID { get; set; }
            public string Title { get; set; }
            public string Count { get; set; }
            public string ReciveCount { get; set; }
            public string CheckedCount { get; set; }
            public string FinancialCount { get; set; }
            public string InCheckedProccess { get; set; }
            public string InFinancialProccess { get; set; }
            public string Deliver { get; set; }
        }
        //
        public class DiffWithSalary
        {
            public string Workshop { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string TotalCount { get; set; }
            public string TotalAmount { get; set; }
            public string TotalAmount2 { get; set; }
            public string sumTotalAmount { get; set; }
            public string sumTotalAmount2 { get; set; }
            public string RemTotalAmount { get; set; }
            public string RemTotalAmount2 { get; set; }
        }
        //
        public class DetailList
        {
            public string ServiceDesc { get; set; }
            public string InvoiceCount { get; set; }
            public string RegisterCount { get; set; }
        }
        public ActionResult InvoiceFapaDiffWithSalary(int? WorkshopID, DateTime fromDate, DateTime toDate)
        {
            if (WorkshopID == null)
                WorkshopID = 0;
            string Workshop = "";
            string Description = "";
            string Type = "";
            double TotalCount = 0.0;
            double TotalAmount = 0.0;
            double TotalAmount2 = 0.0;
            double sumTotalAmount = 0.0;
            double sumTotalAmount2 = 0.0;
            double RemTotalAmount = 0.0;
            double RemTotalAmount2 = 0.0;
            //
            string ServiceDesc = "";
            double InvoiceCount = 0.0;
            double RegisterCount = 0.0;
            double SumInvoiceCount = 0.0;
            double SumRegisterCount = 0.0;
            //
            List<DiffWithSalary> TableOuts = new List<DiffWithSalary>();
            List<DetailList> Details = new List<DetailList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceFapaDiffWithSalary]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Workshop = reader["Workshop"].ToString();
                            Type = reader["Type"].ToString();
                            Description = reader["Description"].ToString();
                            TotalCount = Convert.ToDouble(reader["TotalCount"].ToString());
                            if(Convert.ToDouble(reader["TotalAmount"].ToString())>0)
                                TotalAmount = Convert.ToDouble(reader["TotalAmount"].ToString());
                            if (Convert.ToDouble(reader["TotalAmount2"].ToString()) > 0)
                                TotalAmount2 = Convert.ToDouble(reader["TotalAmount2"].ToString());
                            if(Type.Equals("فاکتور دستمزد تبدیل"))
                            {
                                sumTotalAmount += TotalAmount;
                                sumTotalAmount2 += TotalAmount2;
                            }
                            
                            TableOuts.Add(new DiffWithSalary
                            {
                               Description=Description,
                               Workshop=Workshop,
                               Type=Type,
                               TotalCount=TotalCount.ToString("#,##"),
                               TotalAmount=TotalAmount.ToString("#,##"),
                               TotalAmount2=TotalAmount2.ToString("#,##"),
                               sumTotalAmount=sumTotalAmount.ToString("#,##"),
                               sumTotalAmount2=sumTotalAmount2.ToString("#,##")
                            });
                        }
                        //
                        //reader.NextResult();
                        //while (reader.Read())
                        //{
                        //    ServiceDesc = reader["ServiceDesc"].ToString();
                        //    InvoiceCount = Convert.ToDouble(reader["InvoiceCount"].ToString());
                        //    SumInvoiceCount += InvoiceCount;
                        //    RegisterCount = Convert.ToDouble(reader["RegisterCount"].ToString());
                        //    SumRegisterCount += RegisterCount;

                        //    Details.Add(new DetailList
                        //    {
                        //        InvoiceCount = InvoiceCount.ToString(),
                        //        RegisterCount = RegisterCount.ToString(),
                        //        ServiceDesc = ServiceDesc
                        //    });
                        //}
                        //
                        conn.Close();
                    }
                }//end using                

                ViewBag.TableOuts = TableOuts;
                //ViewBag.DetailList = Details;

                //ViewBag.SumInvoiceCount = SumInvoiceCount;
                //ViewBag.SumRegisterCount = SumRegisterCount;
            }
            catch
            {
                //ViewBag.TableOuts = null;
                //ViewBag.DetailList = null;
            }
            
            return PartialView(TableOuts.ToList());
        }
        //
        public ActionResult InvoiceFapaDiffWithRegisterCount(int? WorkshopID, DateTime fromDate, DateTime toDate)
        {
            if (WorkshopID == null)
                WorkshopID = 0;
            string ServiceDesc = "";
            double InvoiceCount = 0.0;
            double RegisterCount = 0.0;
            double SumInvoiceCount = 0.0;
            double SumRegisterCount = 0.0;
            //
            List<DetailList> Details = new List<DetailList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoiceFapaDiffWithRegisterCount]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshops", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@ToDate", SqlDbType.NVarChar).Value = toDate;
                        command.Parameters.Add("@FromDate", SqlDbType.NVarChar).Value = fromDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ServiceDesc = reader["ServiceDesc"].ToString();
                            InvoiceCount = Convert.ToDouble(reader["InvoiceCount"].ToString());
                            SumInvoiceCount += InvoiceCount;
                            RegisterCount = Convert.ToDouble(reader["RegisterCount"].ToString());
                            SumRegisterCount += RegisterCount;

                            Details.Add(new DetailList
                            {
                                InvoiceCount = InvoiceCount.ToString(),
                                RegisterCount = RegisterCount.ToString(),
                                ServiceDesc = ServiceDesc
                            });
                        }
                        //
                        conn.Close();
                    }
                }//end using                

                ViewBag.DetailList = Details;

                ViewBag.SumInvoiceCount = SumInvoiceCount;
                ViewBag.SumRegisterCount = SumRegisterCount;
            }
            catch
            {
                //ViewBag.TableOuts = null;
                //ViewBag.DetailList = null;
            }

            return PartialView();
        }
        
        //CREATE Difference Invoice FOR Workshops
        [HttpGet]
        public ActionResult CreateWorkshopsDifferenceInvoice()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult CreateWorkshopsDifferenceInvoice(int? WorkshopID, String fromDate, String toDate)
        {
            string Message = "خطا";
            String fromdateInString = "";
            int Year = int.Parse(fromDate.Split('/')[0]);
            int Month = int.Parse(fromDate.Split('/')[1]);
            int Day = int.Parse(fromDate.Split('/')[2]);
            DateTime dt1 = new DateTime(Year, Month,Day, pc);
            fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            String todateInString = "";
            int Year2 = int.Parse(toDate.Split('/')[0]);
            int Month2 = int.Parse(toDate.Split('/')[1]);
            int Day2 = int.Parse(toDate.Split('/')[2]);
            DateTime dt2 = new DateTime(Year2, Month2, Day2, pc);
            todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).ToList();
            int? userId = null;
            string invoiceCode = "";
            if (user.Count() > 0)
                userId = user.SingleOrDefault().UserID;
            var workshops = db.tbl_Workshops.Where(w=>w.isServices == true).ToList();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (WorkshopID != null || WorkshopID > 0)
                {
                    var invoices = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == WorkshopID).OrderByDescending(i => i.InvoiceID).ToList();
                    if (invoices.Count() > 0)
                    {
                        var exsitDate = invoices.Where(i => i.CreatedDate.Value.ToShortDateString().Contains(toDate) && i.Description.Contains("مربوط به مابه التفاوت دستمزد تبدیل می باشد")).ToList();
                        if (exsitDate.Count() == 0)
                        {
                            invoiceCode = (Convert.ToInt32(invoices.FirstOrDefault().InvoiceCode) + 1).ToString();
                            //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDifferenceInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                            cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                            Message = "صورتحساب مابه التفاوت دستمزد تبدیل مربوط به  کارگاه منتخب، با موفقیت صادر گردید!";
                        }
                        else
                        {
                            Message = "صورتحساب مابه‌التفاوت دستمزد برای بازه مورد نظر قبلا صادر شده است!";
                        }
                    }
                    else
                    {
                        Message = "تا کنون هیچ فاکتوری برای این کارگاه ثبت نشده است!";
                    }
                }                   
            };
            //
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title",WorkshopID);
            ViewBag.Message = Message;
            return View();
        }

        //CREATE Invoice FOR Workshops
        [HttpGet]
        public ActionResult CreateWorkshopsInvoice()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult CreateWorkshopsInvoice(int? WorkshopID, String fromDate, String toDate)
        {
            string Message = "خطا";
            string WorkshopTitle = Helper.Helpers.GetWorkshops(WorkshopID).Title;
            String fromdateInString = "";
            int Year = int.Parse(fromDate.Split('/')[0]);
            int Month = int.Parse(fromDate.Split('/')[1]);
            int Day = int.Parse(fromDate.Split('/')[2]);
            DateTime dt1 = new DateTime(Year, Month, Day, pc);
            fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            String todateInString = "";
            int Year2 = int.Parse(toDate.Split('/')[0]);
            int Month2 = int.Parse(toDate.Split('/')[1]);
            int Day2 = int.Parse(toDate.Split('/')[2]);
            DateTime dt2 = new DateTime(Year2, Month2, Day2, pc);
            todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).ToList();
            int? userId = null;
            string invoiceCode = "";
            if (user.Count() > 0)
                userId = user.SingleOrDefault().UserID;
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (WorkshopID != null || WorkshopID > 0)
                {
                    var invoices = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == WorkshopID).OrderByDescending(i => i.InvoiceID).ToList();
                    if (invoices.Count() > 0)
                    {
                        var exsitDate = invoices.Where(i => i.CreatedDate.Value.ToShortDateString().Contains(toDate)).ToList();
                        if (exsitDate.Count() == 0)
                        {
                            try
                            {
                                invoiceCode = (Convert.ToInt32(invoices.FirstOrDefault().InvoiceCode) + 1).ToString();
                            }
                            catch
                            {
                                invoiceCode = "1";
                            }

                            //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                            cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                            Message = "صورتحساب دستمزد تبدیل مربوط به  کارگاه: "+ WorkshopTitle + "با موفقیت صادر گردید!";
                        }
                        else
                        {
                            Message = "صورتحساب برای بازه مورد نظر قبلا صادر شده است!";
                        }
                    }
                    //
                    if (invoices.Count() == 0)
                    {
                        invoiceCode = "1";
                        //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                        SqlConnection cnn = new SqlConnection(connStr);
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[CreateWorkshopsInvoice]";
                        cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                        cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                        cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                        cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                        //add any parameters the stored procedure might require
                        cnn.Open();
                        object o = cmd.ExecuteScalar();
                        cnn.Close();
                        //----------------------
                        Message = "صورتحساب دستمزد تبدیل مربوط به  کارگاه: " + WorkshopTitle + "با موفقیت صادر گردید!";
                    }
                }
            };
            //
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", WorkshopID);
            ViewBag.Message = Message;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View();
        }
        //
        /// <summary>
        /// مشاهده لیست جزئیات فاکتور اقلام ضایعاتی کارگاه ها
        /// </summary>
        /// <returns></returns>
        // GET: Financials InvoicesDamagesDetails
        [HttpPost]
        public ActionResult InvoicesDamagesDetails(string WorkshopId, string Year, string Month)
        {          
            string VehicleType = "";
            string Years = "";
            string Months = "";
            string ChassisNumber = "";
            string Plate = "";
            string Literage = "";
            string AcceptanceDate = "";
            //
            List<DetailsList> detailList = new List<DetailsList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_InvoicesDamagesDetails]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshopId", SqlDbType.VarChar).Value = WorkshopId;
                cmd.Parameters.Add("@Year", SqlDbType.VarChar).Value = Year;
                cmd.Parameters.Add("@Month", SqlDbType.VarChar).Value = Month;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VehicleType = reader["VehicleType"].ToString();
                    //if (VehicleType == "20" || VehicleType == "28")
                    //    VehicleType = "20-28";
                    Years = reader["Year"].ToString();
                    Months = reader["Month"].ToString();
                    ChassisNumber = reader["ChassisNumber"].ToString();
                    Plate = reader["Plate"].ToString();
                    Literage = reader["Literage"].ToString();
                    AcceptanceDate = reader["Acceptance"].ToString();
                    //
                    detailList.Add(new DetailsList
                    {
                        VehicleType = VehicleType,
                        Year = Year,
                        Month = Month,
                        ChassisNumber = ChassisNumber,
                        Plate = Plate,
                        Literage = Literage,
                        AcceptanceDate= AcceptanceDate
                    });
                }

                conn.Close();
            }
            //
            ViewBag.TableOut = detailList.ToList();
            return View();
        }
        public class DetailsList
        {
            public string VehicleType { get; set; }
            public string Literage { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public string ChassisNumber { get; set; }
            public string Plate { get; set; }
            public string AcceptanceDate { get; set; }
        }
        // 
        public ActionResult InvoicesDamagesPage(string InvoiceCode, int? workshopId, string Year, string Month)
        {
            if (workshopId != null)
                ViewBag.workshopTitle = Helper.Helpers.GetWorkshops(workshopId).Title;
            //
            ViewBag.fromDate = Year;
            ViewBag.toDate = Month;
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_Workshops, "ID", "Title");
                var invoice = db.tbl_InvoicesDamages.Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments);
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
            else
            {
                ViewBag.InvoiceCode = InvoiceCode;
                var invoice = db.tbl_InvoicesDamages.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode == InvoiceCode).Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments).OrderBy(c=>c.ServiceCode);
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.CustomerId = invoice.FirstOrDefault().VehicleTypesID;
                ViewBag.Description = invoice.FirstOrDefault().Description;
                ViewBag.Comment = invoice.FirstOrDefault().Comment;
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                string totalSum = Math.Round(invoice.Sum(i => i.TotalAmountTaxandComplications).Value).ToString();
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalSum);
                ViewBag.date = invoice.FirstOrDefault().CreatedDate.HasValue ? invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString() : "";
                ViewBag.CurrencyType = "ریال";
                if (invoice.FirstOrDefault().CurrencyTypeID != null)
                    ViewBag.CurrencyType = db.tbl_CurrencyTypes.Where(c => c.ID == invoice.FirstOrDefault().CurrencyTypeID).SingleOrDefault().Title;
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
        }
        //
        public ActionResult PrintInvoicesDamages(int InvoiceCode, int? workshopId, string Year, string Month)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            if(workshopId!=null)
                ViewBag.workshopTitle = Helper.Helpers.GetWorkshops(workshopId).Title;

            var invoice = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode.ToString())).OrderBy(i => i.CreatedDate);
            
            //
            //var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlConnection cnn = new SqlConnection(connStr);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = cnn;
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.CommandText = "[dbo].[sp_LockedVehicleEdit]";
            //cmd.Parameters.Add(new SqlParameter("@invoiceCode", InvoiceCode));
            //cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
            //cmd.Parameters.Add(new SqlParameter("@fromdate", p3fromdate));
            //cmd.Parameters.Add(new SqlParameter("@todate", todate));
            ////add any parameters the stored procedure might require
            //cnn.Open();
            //object o = cmd.ExecuteScalar();
            //cnn.Close();
           
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("InvoicesDamagesPage", new { InvoiceCode = InvoiceCode, workshopId = workshopId, Year = Year, Month = Month, Type = 1 })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
            };
            //return report;
        }
        //
        //CREATE Difference Invoice FOR Workshops
        [HttpGet]
        public ActionResult CreateWorkshopsDamagesInvoice()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult CreateWorkshopsDamagesInvoice(int? WorkshopID, String Year, String Month)
        {
            string Message = "خطا";
            string textClass = "text-danger";
            string existInvoiceCode = "0";
            //
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).ToList();
            int? userId = null;
            string invoiceCode = "";
            if (user.Count() > 0)
                userId = user.SingleOrDefault().UserID;
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (WorkshopID != null || WorkshopID > 0)
                {
                    var invoices = db.tbl_InvoicesDamages.Where(i => i.WorkshopsID == WorkshopID).OrderByDescending(i => i.InvoiceID).ToList();
                    //if (invoices.Count() == 0)
                    //{
                    if (invoices.Count() == 0)
                        {
                            invoiceCode = "1";
                        }
                        else
                        {
                            existInvoiceCode= invoices.FirstOrDefault().InvoiceCode;
                            invoiceCode = (Convert.ToInt32(invoices.FirstOrDefault().InvoiceCode) + 1).ToString();
                        }
                     var exsitInvoice = invoices.Where(i => i.InvoiceCode.Equals(existInvoiceCode) && i.Year.Equals(Year) && i.Month.Equals(Month)).ToList();

                    //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------
                    if (exsitInvoice.Count() == 0)
                        {
                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@Year", Year));
                            cmd.Parameters.Add(new SqlParameter("@Month", Month));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                            Message = "  فاکتور فروش مخازن ضایعاتی سال : "+" " + Year + "ماه " + Month + " " + "با موفقیت صادر گردید.";
                            textClass = "text-success";
                        }
                        else if (exsitInvoice.Count() > 0)
                        {
                            Message = "فاکتور مشابه‌ای قبلا صادر شده است!";
                        }
                        else
                        {
                            Message = "خطای غیر منتظره‌ای رخ داده است!";
                        }

                    //}                        
                    //}

                }
            };
            //
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", WorkshopID);
            ViewBag.Message = Message;
            ViewBag.textClass = textClass;
            return View();
        }
        //
        [HttpGet]
        public ActionResult CreateWorkshopsDamagesInvoice28(int? WorkshopID, String InvoiceCode, String Year, String Month)
        {
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(Month));
            ViewBag.InvoiceCode = InvoiceCode;

            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title",WorkshopID);
            return View();
        }
        [HttpPost]
        public ActionResult InserttoWorkshopsDamagesInvoice(int? WorkshopID, String InvoiceCode, String Year, String Month)
        {
            string Message = "خطا";
            string textClass = "text-danger";
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(Month));
            //
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).ToList();
            int? userId = null;
            string invoiceCode = "";
            if (user.Count() > 0)
                userId = user.SingleOrDefault().UserID;
            //ViewBag.Workshop = db.tbl_Workshops.Where(w => w.ID==WorkshopID).SingleOrDefault().Title;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlConnection cnn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice_28L]";
                cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                //add any parameters the stored procedure might require
                cnn.Open();
                object o = cmd.ExecuteScalar();
                cnn.Close();
                //----------------------
                Message = "  ردیف مربوط به مخازن 28 لیتری با موفقیت به فاکتور فروش مخازن ضایعاتی سال : " + " " + Year + "ماه " + Month + " " + "افزوده گردید.";
                textClass = "text-success";
            };
            //
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", WorkshopID);
            ViewBag.Message = Message;
            ViewBag.textClass = textClass;
            ViewBag.InvoiceCode = InvoiceCode;
            return View("CreateWorkshopsDamagesInvoice28",new { WorkshopID=WorkshopID, InvoiceCode=InvoiceCode, Year=Year, Month=Month });
        }
        //
        [HttpGet]
        public ActionResult AutoInsertIntoDamagesInvoice()
        {
            ViewBag.Year = pc.GetYear(DateTime.Now);
            ViewBag.Month = pc.GetMonth(DateTime.Now);
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(ViewBag.Month));

            return View();
        }
        [HttpPost]
        public ActionResult AutoInsertIntoDamagesInvoice(int? Year, int? Month)
        {
            if(Year == null)
                Year = 1402;
            //
            string Mon = "";
            int invoiceCode = 1;
            string existInvoiceCode = "";
            int userId = 1;
            string Message = "";
            string textClass = "text-danger";
            string errorText = "";
            string insertedList = "";
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;

            //var Workshops = db.tbl_Workshops.Where(w => w.IRNGVCod != null && ( w.ID != 7 && w.ID != 60)).ToList();
            var Workshops = db.tbl_Workshops.Where(w => w.IRNGVCod != null).ToList();

            foreach (var item in Workshops)
            {                
                
                if (Year == 1402)
                {
                    for (int i = 9; i <= 12; i++)
                    {
                        if(i<10)
                            Mon = "0" + i;
                        else
                            Mon = i.ToString();
                        //
                        //cheked exist record in invoice damages
                        var invoices = db.tbl_InvoicesDamages.Where(n => n.WorkshopsID == item.ID).OrderByDescending(n => n.InvoiceID).ToList();
                        var exsitInvoice = invoices.Where(n => n.Year.Equals(Year.ToString())).ToList();
                        var existSameRec = exsitInvoice.Where(n => n.Month == Mon).ToList();
                        //
                        if (invoices.Count() == 0)
                        {
                            invoiceCode = 1;
                        }
                        if (invoices.Count() > 0)
                        {
                            existInvoiceCode = invoices.Max(n => Convert.ToInt32(n.InvoiceCode)).ToString();
                            invoiceCode = (Convert.ToInt32(existInvoiceCode) + 1);
                        }
                        //
                        if (existSameRec.Count() == 0)
                        {
                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", item.ID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@Year", Year));
                            cmd.Parameters.Add(new SqlParameter("@Month", Mon));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //invoiceCode += 1;
                            //----------------------
                            Message = "  فاکتور فروش مخازن ضایعاتی سال : " + " " + Year + "ماه " + Month + " " + "با موفقیت صادر گردید.";
                            textClass = "text-success";
                        }
                        else if (existSameRec.Count() > 0)
                        {
                            Message = "فاکتور مشابه‌ای قبلا صادر شده است!";
                            textClass = "text-danger";
                        }
                        else
                        {
                            Message = "خطای غیر منتظره‌ای رخ داده است!";
                            textClass = "text-danger";
                        }
                    }
                    //reset invoiceCode counter
                    //invoiceCode = 1;
                }
                else
                {
                    for (int i = 1 ; i <= Month ; i++)
                    {
                        if (i < 10)
                            Mon = "0" + i;
                        else
                            Mon = i.ToString();
                        //
                        //cheked exist record in invoice damages
                        var invoices = db.tbl_InvoicesDamages.Where(n => n.WorkshopsID == item.ID).OrderByDescending(n => n.InvoiceID).ToList();
                        var exsitInvoice = invoices.Where(n => n.Year.Equals(Year.ToString())).ToList();
                        var existSameRec = exsitInvoice.Where(n => n.Month == Mon).ToList();
                        //

                        if (invoices.Count() == 0)
                        {
                            invoiceCode = 1;
                        }
                        if (invoices.Count() > 0)
                        {
                            existInvoiceCode = invoices.Max(n => Convert.ToInt32(n.InvoiceCode)).ToString();
                            invoiceCode = (Convert.ToInt32(existInvoiceCode) + 1);
                        }
                        //
                        //                        
                        if (existSameRec.Count() == 0)
                        {
                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", item.ID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@Year", Year));
                            cmd.Parameters.Add(new SqlParameter("@Month", Mon));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //invoiceCode += 1;
                            //----------------------
                            insertedList += Helper.Helpers.GetWorkshops(item.ID).Title + ", "+ "<br />";
                            Message = "  فاکتور فروش مخازن ضایعاتی سال : " + " " + Year + "ماه " + Month + " " + "با موفقیت صادر گردید.";
                            textClass = "text-success";
                        }
                        else if (existSameRec.Count() > 0)
                        {
                            Message = "فاکتور مشابه‌ای قبلا صادر شده است!";
                            textClass = "text-danger";
                        }
                        else
                        {
                            Message = "خطای غیر منتظره‌ای رخ داده است!";
                            textClass = "text-danger";
                        }
                    }
                    //reset invoiceCode counter
                    //invoiceCode = 1;
                }
            }
            //
            ViewBag.Message = Message;
            ViewBag.textClass = textClass;
            ViewBag.insertedList = insertedList;
            ViewBag.Month = Month;
            ViewBag.Year = Year;
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(Month));

            return View();
        }
        //
        public static string GetInvoiceDamagesCount(int? id, string Year, string Month)
        {
            if (!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesDamages
                .Where(c => c.WorkshopsID == id && c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesDamages
                .Where(c => c.WorkshopsID == id)
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
                
        }
        //
        public static string GetInvoiceDamagesValue(int? id, string Year, string Month)
        {
            if(!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesDamages
               .Where(c => c.WorkshopsID == id && c.Year.Equals(Year) && c.Month.Equals(Month))
               .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesDamages
               .Where(c => c.WorkshopsID == id)
               .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
           
        }
        //
        public static string GetInvoiceDamagesCountTotal(string Year, string Month)
        {
            if(!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesDamages
                .Where(c => c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesDamages
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
            
        }
        //
        public static string GetInvoiceDamagesValueTotal(string Year, string Month)
        {
            if(!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesDamages
                .Where(c => c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesDamages
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
            
        }
        //
        public ActionResult InvoicesFapa_DamagesCylinder(int? workshopId, string workshopTitle)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            //
            ViewBag.workshopTitle = workshopTitle;
            ViewBag.workshopId = workshopId;
            var userName = Helper.Helpers.GetCurrentUser();
            string InvoiceCode = "";
            string CreatedDate = "";
            string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            string TotalAmount2 = "";
            string FromDate = "";
            string CheckStatus = "در حال بررسی";
            string FinancialStatus = "در حال بررسی";
            string ReciveStatus = "در حال بررسی";            //
            
            int TotalCount = 0;
            int SumTotalCount = 0;
            double SumTotalAmount = 0.0;
            double SumTotalAmount2 = 0.0;
            //double SumDoubledInvoice = 0.0;
            string DefectsCount = "0";
            int existFromYear = pc.GetYear(DateTime.Now);
            int existFromMonth = pc.GetMonth(DateTime.Now)-1;
            int existToYear = pc.GetYear(DateTime.Now);
            int existToMonth = pc.GetMonth(DateTime.Now);


            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();

            var list = db.tbl_InvoicesFapa_DamagesCylinder.Where(i => i.WorkshopsID == workshopId).OrderByDescending(i => i.InvoiceID).ToList();
            var Registrationlist = db.tbl_VehicleRegistrations.Where(i => i.RegisterStatus == true && i.WorkshopID == workshopId).OrderBy(i => i.CreateDate).ToList();
            //
            if (list.Count() > 0)
            {
                existFromYear = pc.GetYear(Convert.ToDateTime(list.FirstOrDefault().EmployerEconomicalnumber));
                existFromMonth = pc.GetMonth(Convert.ToDateTime(list.FirstOrDefault().EmployerEconomicalnumber));
                existToYear = pc.GetYear(list.FirstOrDefault().CreatedDate.Value);
                existToMonth = pc.GetMonth(list.FirstOrDefault().CreatedDate.Value);
            }           
            
            //begin for show results in table view
            var connStr2 = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader2;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesFapa_DamagesCylinder]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    conn.Open();
                    reader2 = command.ExecuteReader();

                    while (reader2.Read())
                    {
                        InvoiceCode = reader2["InvoiceCode"].ToString();
                        CreatedDate = Convert.ToDateTime(reader2["CreatedDate"].ToString()).ToShortDateString();
                        FromDate = Convert.ToDateTime(reader2["EmployerEconomicalnumber"].ToString()).ToShortDateString();
                        // EmployerTitle = reader2["EmployerTitle"].ToString();
                        DepartmentTitle = reader2["Description"].ToString();
                        Title = reader2["Title"].ToString();
                        Status = reader2["Status"].ToString();
                        TotalAmount = reader2["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";
                        SumTotalAmount += Convert.ToDouble(TotalAmount);
                        TotalAmount2 = reader2["TotalAmount2"].ToString();
                        if (TotalAmount2 == "")
                            TotalAmount2 = "0";
                        SumTotalAmount2 += Convert.ToDouble(TotalAmount2);
                        DefectsCount = reader2["DefectsCount"].ToString();
                        TotalCount = Convert.ToInt32(reader2["TotalCount"].ToString());
                        if (!DepartmentTitle.Equals("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                            SumTotalCount += TotalCount;
                        CheckStatus = reader2["CheckStatus"].ToString();
                        FinancialStatus = reader2["FinancialStatus"].ToString();
                        ReciveStatus = reader2["ReciveStatus"].ToString();

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            FromDate = FromDate,
                            DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##")),
                            TotalAmount2 = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount2)).ToString("#,##.##")),
                            DefectsCount = DefectsCount,
                            TotalCount = TotalCount,
                            CheckStatus = CheckStatus,
                            FinancialStatus = FinancialStatus,
                            ReciveStatus = ReciveStatus
                        });
                    }
                }
            }
            //end for show results in table view

            ViewBag.TableOut = TableOuts;
            ViewBag.SumTotalCount = SumTotalCount;
            ViewBag.SumTotalAmount = SumTotalAmount.ToString("#,##");
            ViewBag.SumTotalAmount2 = SumTotalAmount2.ToString("#,##");
            return View();
        }
        //
        [HttpGet]
        public ActionResult AutoInsertIntoInvoicesFapa_DamagesCylinder()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }        
        //
        [HttpPost]
        public ActionResult AutoInsertIntoInvoicesFapa_DamagesCylinder(int? WorkshopID, String fromDate, String toDate)
        {
            string Message = "خطا";
            string WorkshopTitle = Helper.Helpers.GetWorkshops(WorkshopID).Title;
            String fromdateInString = "";
            int Year = int.Parse(fromDate.Split('/')[0]);
            int Month = int.Parse(fromDate.Split('/')[1]);
            int Day = int.Parse(fromDate.Split('/')[2]);
            DateTime dt1 = new DateTime(Year, Month, Day, pc);
            fromdateInString = dt1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            String todateInString = "";
            int Year2 = int.Parse(toDate.Split('/')[0]);
            int Month2 = int.Parse(toDate.Split('/')[1]);
            int Day2 = int.Parse(toDate.Split('/')[2]);
            DateTime dt2 = new DateTime(Year2, Month2, Day2, pc);
            todateInString = dt2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            //
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).ToList();
            int? userId = null;
            string invoiceCode = "";
            if (user.Count() > 0)
                userId = user.SingleOrDefault().UserID;
            var workshops = db.tbl_Workshops.Where(w => w.isServices == true).ToList();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (WorkshopID != null || WorkshopID > 0)
                {
                    var invoices = db.tbl_InvoicesFapa_DamagesCylinder.Where(i => i.WorkshopsID == WorkshopID).OrderByDescending(i => i.InvoiceID).ToList();
                    if (invoices.Count() > 0)
                    {
                        var exsitDate = invoices.Where(i => i.CreatedDate.Value.ToShortDateString().Contains(toDate)).ToList();
                        if (exsitDate.Count() == 0)
                        {
                            try
                            {
                                invoiceCode = (Convert.ToInt32(invoices.FirstOrDefault().InvoiceCode) + 1).ToString();
                            }
                            catch
                            {
                                invoiceCode = "1";
                            }

                            //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsInvoices_DamagesCylinder]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                            cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //----------------------
                            Message = "صورتحساب دستمزد تعویض مربوط به  کارگاه: " + WorkshopTitle + " " + " در بازه: " + fromDate + " تا " + " " + toDate + "با موفقیت صادر گردید!";
                        }
                        else
                        {
                            Message = "صورتحساب برای بازه " + fromDate + " تا " + " " + toDate + " قبلا صادر شده است!";
                        }
                    }
                    //
                    if (invoices.Count() == 0)
                    {
                        invoiceCode = "1";
                        //-----------------------------------------------for insert invoice value into table invoicefapa-----------------------

                        SqlConnection cnn = new SqlConnection(connStr);
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[sp_CreateWorkshopsInvoices_DamagesCylinder]";
                        cmd.Parameters.Add(new SqlParameter("@workshopId", WorkshopID));
                        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                        cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                        cmd.Parameters.Add(new SqlParameter("@fromdate", fromdateInString.Replace("/", "-")));
                        cmd.Parameters.Add(new SqlParameter("@todate", todateInString.Replace("/", "-")));
                        //add any parameters the stored procedure might require
                        cnn.Open();
                        object o = cmd.ExecuteScalar();
                        cnn.Close();
                        //----------------------
                        Message = "صورتحساب دستمزد تعویض مربوط به  کارگاه: " + WorkshopTitle + " " + " در بازه: " + fromDate + " تا " + " " + toDate + "با موفقیت صادر گردید!";
                    }
                }
            };
            //
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", WorkshopID);
            ViewBag.Message = Message;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View();
        }
        //
        //[HttpGet]
        public ActionResult InvoiceFapaPage_Damages(string InvoiceCode, int? workshopId, string fromDate, string toDate)
        {
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_Workshops, "ID", "Title");
                var invoice = db.tbl_InvoicesFapa_DamagesCylinder.Include(c => c.Workshops);
                Thread.Sleep(1000);
                return PartialView(invoice.ToList());
            }
            else
            {
                ViewBag.InvoiceCode = InvoiceCode;
                var invoice = db.tbl_InvoicesFapa_DamagesCylinder.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode == InvoiceCode).Include(c => c.Workshops);
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.CustomerId = invoice.FirstOrDefault().VehicleTypesID;
                ViewBag.Description = invoice.FirstOrDefault().Description;
                ViewBag.Comment = invoice.FirstOrDefault().Comment;
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                string totalSum = Math.Round(invoice.Sum(i => i.TotalAmountTaxandComplications).Value).ToString();
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalSum);
                ViewBag.date = invoice.FirstOrDefault().CreatedDate.HasValue ? invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString() : "";
                ViewBag.CurrencyType = "ریال";
                if (invoice.FirstOrDefault().CurrencyTypeID != null)
                    ViewBag.CurrencyType = db.tbl_CurrencyTypes.Where(c => c.ID == invoice.FirstOrDefault().CurrencyTypeID).SingleOrDefault().Title;
                Thread.Sleep(1000);
                return PartialView(invoice.ToList());
            }
        }
        //[HttpPost]
        public ActionResult PrintInvoicesFapa_DamagesCylinder(string InvoiceCode, int? workshopId, string fromDate, string toDate)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            var invoice = db.tbl_InvoicesFapa_DamagesCylinder.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode.ToString())).OrderBy(i => i.CreatedDate);
            string fromdateInString = invoice.FirstOrDefault().EmployerEconomicalnumber;
            DateTime todateInString = invoice.FirstOrDefault().CreatedDate.Value;
            int fYear = pc.GetYear(Convert.ToDateTime(fromdateInString));
            int fMonth = pc.GetMonth(Convert.ToDateTime(fromdateInString));
            int fDay = pc.GetDayOfMonth(Convert.ToDateTime(fromdateInString));

            DateTime Pfromdate = new DateTime(fYear, fMonth, fDay);
            string p2fromdate = Pfromdate.ToString(CultureInfo.InvariantCulture);
            string p2fMonth = "01";
            string p2fDay = "01";
            string p2tMonth = "01";
            string p2tDay = "01";
            if (Pfromdate.Month < 10)
                p2fMonth = "0" + Pfromdate.Month;
            else
                p2fMonth = Pfromdate.Month.ToString();

            if (Pfromdate.Day < 10)
                p2fDay = "0" + Pfromdate.Day;
            else
                p2fDay = (Pfromdate.Day).ToString();

            string p3fromdate = Pfromdate.Year + "-" + p2fMonth + "-" + p2fDay;
            //
            if (todateInString.Month < 10)
                p2tMonth = "0" + todateInString.Month;
            else
                p2tMonth = todateInString.Month.ToString();

            if (todateInString.Day < 10)
                p2tDay = "0" + todateInString.Day;
            else
                p2tDay = (todateInString.Day).ToString();
            string todate = todateInString.Year + "-" + p2tMonth + "-" + p2tDay;
           
            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("InvoiceFapaPage_Damages", new { InvoiceCode = InvoiceCode, workshopId = workshopId, fromDate = fromDate, toDate = toDate })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
            };
            //return report;
        }
        //
        public static string GetInvoiceFapa_DamageValue(int? id)
        {
            var value = dbStatic.tbl_InvoicesFapa_DamagesCylinder
                .Where(c => c.WorkshopsID == id)
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
            else
                return "0";
        }
        //
        public static string GetInvoiceFapa_DamageValueTotal()
        {
            var value = dbStatic.tbl_InvoicesFapa_DamagesCylinder
                .ToList();

            if (value.Count > 0)
                return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
            else
                return "0";
        }
        //
        [HttpGet]
        public ActionResult CheckStatus_DamagesCylinder(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult CheckStatus_DamagesCylinder(string InvoiceCode, int? workshopId, bool Status)
        {
            var items = db.tbl_InvoicesFapa_DamagesCylinder.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach (var item in items)
            {
                item.CheckStatus = Status;
                item.CkeckedDate = DateTime.Now;
                item.CkeckedUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            string workshopTitle = items.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapa_DamagesCylinder", new { workshopId = workshopId, workshopTitle = workshopTitle });
        }
        //
        [HttpGet]
        public ActionResult ReciveStatus_DamagesCylinder(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult ReciveStatus_DamagesCylinder(string InvoiceCode, int? workshopId, bool Status)
        {
            var items = db.tbl_InvoicesFapa_DamagesCylinder.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach (var item in items)
            {
                item.ReciveStatus = Status;
                item.ReciveDate = DateTime.Now;
                item.ReciveUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            string workshopTitle = items.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapa_DamagesCylinder", new { workshopId = workshopId, workshopTitle = workshopTitle });
        }
        //
        [HttpGet]
        public ActionResult FinancialStatus_DamagesCylinder(string InvoiceCode, int? workshopId)
        {
            ViewBag.InvoiceCode = InvoiceCode;
            ViewBag.workshopId = workshopId;
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult FinancialStatus_DamagesCylinder(string InvoiceCode, int? workshopId, bool Status)
        {
            string Type ="2";
            var invoice = db.tbl_InvoicesFapa_DamagesCylinder.Where(u => u.InvoiceCode.Equals(InvoiceCode) && u.WorkshopsID == workshopId).Include(u => u.Workshops).ToList();
            foreach (var item in invoice)
            {
                item.FinancialStatus = Status;
                item.FinancialDate = DateTime.Now;
                item.FinancialUser = User.Identity.Name;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            //
            string fromdateInString = invoice.FirstOrDefault().EmployerEconomicalnumber;
            DateTime todateInString = invoice.FirstOrDefault().CreatedDate.Value;
            int fYear = pc.GetYear(Convert.ToDateTime(fromdateInString));
            int fMonth = pc.GetMonth(Convert.ToDateTime(fromdateInString));
            int fDay = pc.GetDayOfMonth(Convert.ToDateTime(fromdateInString));

            DateTime Pfromdate = new DateTime(fYear, fMonth, fDay);
            string p2fromdate = Pfromdate.ToString(CultureInfo.InvariantCulture);
            string p2fMonth = "01";
            string p2fDay = "01";
            string p2tMonth = "01";
            string p2tDay = "01";
            if (Pfromdate.Month < 10)
                p2fMonth = "0" + Pfromdate.Month;
            else
                p2fMonth = Pfromdate.Month.ToString();

            if (Pfromdate.Day < 10)
                p2fDay = "0" + Pfromdate.Day;
            else
                p2fDay = (Pfromdate.Day).ToString();

            string p3fromdate = Pfromdate.Year + "-" + p2fMonth + "-" + p2fDay;
            //
            if (todateInString.Month < 10)
                p2tMonth = "0" + todateInString.Month;
            else
                p2tMonth = todateInString.Month.ToString();

            if (todateInString.Day < 10)
                p2tDay = "0" + todateInString.Day;
            else
                p2tDay = (todateInString.Day).ToString();
            string todate = todateInString.Year + "-" + p2tMonth + "-" + p2tDay;
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[sp_FinancialAssesment_Vehicle]";
            cmd.Parameters.Add(new SqlParameter("@invoiceCode", InvoiceCode));
            cmd.Parameters.Add(new SqlParameter("@workshopId", workshopId));
            cmd.Parameters.Add(new SqlParameter("@fromdate", p3fromdate));
            cmd.Parameters.Add(new SqlParameter("@todate", todate));
            cmd.Parameters.Add(new SqlParameter("@user", User.Identity.Name));
            cmd.Parameters.Add(new SqlParameter("@type", Type));
            //add any parameters the stored procedure might require
            cnn.Open();
            object o = cmd.ExecuteScalar();
            cnn.Close();
            //
            //
            string workshopTitle = invoice.FirstOrDefault().Workshops.Title;
            return RedirectToAction("InvoicesFapa_DamagesCylinder", new { workshopId = workshopId, workshopTitle = workshopTitle });
        }
        //
        public static string GetInvoiceValveDamagesCountTotal(string Year, string Month)
        {
            if (!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .Where(c => c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }

        }
        //
        public static string GetInvoiceValveDamagesValueTotal(string Year, string Month)
        {
            if (!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .Where(c => c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }

        }
        //
        public ActionResult InvoicesValveDamagesList(int? workshopId, string workshopTitle)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            ViewBag.workshopTitle = workshopTitle;
            ViewBag.workshopId = workshopId;
            var userName = Helper.Helpers.GetCurrentUser();
            string InvoiceCode = "";
            string CreatedDate = "";
            string DepartmentTitle = "";
            string Title = "";
            string Status = "";
            string TotalAmount = "";
            string TotalAmount2 = "";
            string FromDate = "";
            string CheckStatus = "در حال بررسی";
            string FinancialStatus = "در حال بررسی";
            string ReciveStatus = "در حال بررسی";
            //
            int Year = pc.GetYear(DateTime.Now);
            DateTime dt = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            DateTime oneDueDate = Convert.ToDateTime("1399/04/31");
            string todateInString = DateTime.Now.ToShortDateString();
            string DefectsCount = "0";
            //
            int TotalCount = 0;
            int SumTotalCount = 0;
            double SumTotalAmount = 0.0;
            double SumTotalAmount2 = 0.0;

            List<InvoicesTableList> TableOuts = new List<InvoicesTableList>();
            //
            var connStr2 = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_InvoicesValveDamagesList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@permission", SqlDbType.VarChar).Value = workshopId;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceCode = reader["InvoiceCode"].ToString();
                        CreatedDate = reader["Year"].ToString();
                        FromDate = reader["Month"].ToString();
                        DepartmentTitle = reader["Description"].ToString();
                        Title = reader["Title"].ToString();
                        Status = reader["Status"].ToString();
                        TotalAmount = reader["TotalAmount"].ToString();
                        if (TotalAmount == "")
                            TotalAmount = "0";
                        SumTotalAmount += Convert.ToDouble(TotalAmount);
                        TotalAmount2 = reader["TotalAmount2"].ToString();
                        if (TotalAmount2 == "")
                            TotalAmount2 = "0";
                        SumTotalAmount2 += Convert.ToDouble(TotalAmount2);
                        DefectsCount = reader["DefectsCount"].ToString();
                        TotalCount = Convert.ToInt32(reader["TotalCount"].ToString());
                        if (!DepartmentTitle.Equals("مربوط به مابه التفاوت دستمزد تبدیل می باشد."))
                            SumTotalCount += TotalCount;
                        CheckStatus = reader["CheckStatus"].ToString();
                        FinancialStatus = reader["FinancialStatus"].ToString();
                        ReciveStatus = reader["ReciveStatus"].ToString();

                        TableOuts.Add(new InvoicesTableList
                        {
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate,
                            FromDate = FromDate,
                            DepartmentTitle = DepartmentTitle,
                            Title = Title,
                            Status = Status,
                            TotalAmount = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount)).ToString("#,##.##")),
                            TotalAmount2 = string.Format(null, Convert.ToDouble(Convert.ToString(TotalAmount2)).ToString("#,##.##")),
                            DefectsCount = DefectsCount,
                            TotalCount = TotalCount,
                            CheckStatus = CheckStatus,
                            FinancialStatus = FinancialStatus,
                            ReciveStatus = ReciveStatus
                        });
                    }
                }
            }
            //
            ViewBag.TableOut = TableOuts;
            ViewBag.SumTotalCount = SumTotalCount;
            ViewBag.SumTotalAmount = SumTotalAmount.ToString("#,##");
            ViewBag.SumTotalAmount2 = SumTotalAmount2.ToString("#,##");
            return View();
        }
        //
        public static string GetInvoiceValveDamagesCount(int? id, string Year, string Month)
        {
            if (!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .Where(c => c.WorkshopsID == id && c.Year.Equals(Year) && c.Month.Equals(Month))
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesValveDamages
                .Where(c => c.WorkshopsID == id)
                .ToList();

                if (value.Count > 0)
                    return value.Sum(c => Convert.ToInt64(c.Number)).ToString("#,##");
                else
                    return "0";
            }

        }
        //
        public static string GetInvoiceValveDamagesValue(int? id, string Year, string Month)
        {
            if (!String.IsNullOrEmpty(Year) || !String.IsNullOrEmpty(Month))
            {
                var value = dbStatic.tbl_InvoicesValveDamages
               .Where(c => c.WorkshopsID == id && c.Year.Equals(Year) && c.Month.Equals(Month))
               .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }
            else
            {
                var value = dbStatic.tbl_InvoicesValveDamages
               .Where(c => c.WorkshopsID == id)
               .ToList();

                if (value.Count > 0)
                    return value.Sum(c => c.TotalAmountTaxandComplications.HasValue ? c.TotalAmountTaxandComplications.Value : 0).ToString("#,##");
                else
                    return "0";
            }

        }
        //
        /// <summary>
        /// مشاهده لیست جزئیات فاکتور شیر مخزن های ضایعاتی کارگاه ها
        /// </summary>
        /// <returns></returns>
        // GET: Financials InvoicesDamagesDetails
        [HttpPost]
        public ActionResult InvoicesValveDamagesDetails(string WorkshopId, string Year, string Month)
        {
            string VehicleType = "";
            string Years = "";
            string Months = "";
            string ChassisNumber = "";
            string Plate = "";
            string Literage = "";
            string AcceptanceDate = "";
            //
            List<DetailsList> detailList = new List<DetailsList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_InvoicesDamagesDetails]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshopId", SqlDbType.VarChar).Value = WorkshopId;
                cmd.Parameters.Add("@Year", SqlDbType.VarChar).Value = Year;
                cmd.Parameters.Add("@Month", SqlDbType.VarChar).Value = Month;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VehicleType = reader["VehicleType"].ToString();
                    //if (VehicleType == "20" || VehicleType == "28")
                    //    VehicleType = "20-28";
                    Years = reader["Year"].ToString();
                    Months = reader["Month"].ToString();
                    ChassisNumber = reader["ChassisNumber"].ToString();
                    Plate = reader["Plate"].ToString();
                    Literage = reader["Literage"].ToString();
                    AcceptanceDate = reader["Acceptance"].ToString();
                    //
                    detailList.Add(new DetailsList
                    {
                        VehicleType = VehicleType,
                        Year = Year,
                        Month = Month,
                        ChassisNumber = ChassisNumber,
                        Plate = Plate,
                        Literage = Literage,
                        AcceptanceDate = AcceptanceDate
                    });
                }

                conn.Close();
            }
            //
            ViewBag.TableOut = detailList.ToList();
            return View();
        }
        //
        public ActionResult InvoicesValveDamagesPage(string InvoiceCode, int? workshopId, string Year, string Month)
        {
            if (workshopId != null)
                ViewBag.workshopTitle = Helper.Helpers.GetWorkshops(workshopId).Title;
            //
            ViewBag.fromDate = Year;
            ViewBag.toDate = Month;
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;

            if (InvoiceCode == null || InvoiceCode == "")
            {
                ViewBag.EmployerID = new SelectList(db.tbl_Workshops, "ID", "Title");
                var invoice = db.tbl_InvoicesValveDamages.Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments);
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
            else
            {
                ViewBag.InvoiceCode = InvoiceCode;
                var invoice = db.tbl_InvoicesValveDamages.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode == InvoiceCode).Include(c => c.Workshops).Include(c => c.VehicleTypes).Include(c => c.Equipments).OrderBy(c => c.ServiceCode);
                ViewBag.WorkshopId = invoice.FirstOrDefault().WorkshopsID;
                ViewBag.CustomerId = invoice.FirstOrDefault().VehicleTypesID;
                ViewBag.Description = invoice.FirstOrDefault().Description;
                ViewBag.Comment = invoice.FirstOrDefault().Comment;
                ViewBag.SaleCondition = invoice.FirstOrDefault().SaleCondition;
                ViewBag.TotalAmount = invoice.Sum(i => i.TotalAmount);
                ViewBag.DiscountAmount = invoice.Sum(i => i.DiscountAmount);
                ViewBag.TotalAmountafterDiscount = invoice.Sum(i => i.TotalAmountafterDiscount);
                ViewBag.AmountTaxandComplications = invoice.Sum(i => i.AmountTaxandComplications);
                ViewBag.TotalAmountTaxandComplications = invoice.Sum(i => i.TotalAmountTaxandComplications);
                string totalSum = Math.Round(invoice.Sum(i => i.TotalAmountTaxandComplications).Value).ToString();
                Helper.PNumberTString AmounttoLetter = new PNumberTString();
                ViewBag.AmounttoLetter = AmounttoLetter.num2str(totalSum);
                ViewBag.date = invoice.FirstOrDefault().CreatedDate.HasValue ? invoice.FirstOrDefault().CreatedDate.Value.ToShortDateString() : "";
                ViewBag.CurrencyType = "ریال";
                if (invoice.FirstOrDefault().CurrencyTypeID != null)
                    ViewBag.CurrencyType = db.tbl_CurrencyTypes.Where(c => c.ID == invoice.FirstOrDefault().CurrencyTypeID).SingleOrDefault().Title;
                Thread.Sleep(1000);
                return View(invoice.ToList());
            }
        }
        //
        public ActionResult PrintInvoicesValveDamages(int InvoiceCode, int? workshopId, string Year, string Month)
        {
            int userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            Workshop workshop = db.tbl_Workshops.Find(workshopId);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                return RedirectToAction("Page403", "Home");
            }
            //
            if (workshopId == null)
                workshopId = Helper.Helpers.GetWorkshopCurrentUser().ID;
            if (workshopId != null)
                ViewBag.workshopTitle = Helper.Helpers.GetWorkshops(workshopId).Title;

            var invoice = db.tbl_InvoicesFapa.Where(i => i.WorkshopsID == workshopId && i.InvoiceCode.Equals(InvoiceCode.ToString())).OrderBy(i => i.CreatedDate);

            string footer = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("yyyy/MM/dd") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            return new ActionAsPdf("InvoicesValveDamagesPage", new { InvoiceCode = InvoiceCode, workshopId = workshopId, Year = Year, Month = Month, Type = 1 })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = footer
            };
        }
        //
        //
        [HttpGet]
        public ActionResult AutoInsertIntoValveDamagesInvoice()
        {
            ViewBag.Year = pc.GetYear(DateTime.Now);
            ViewBag.Month = pc.GetMonth(DateTime.Now);
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(ViewBag.Month));

            return View();
        }
        [HttpPost]
        public ActionResult AutoInsertIntoValveDamagesInvoice(int? Year, int? Month)
        {
            if (Year == null)
                Year = 1402;
            //
            string Mon = "";
            int invoiceCode = 1;
            string existInvoiceCode = "";
            int userId = 1;
            string Message = "";
            string textClass = "text-danger";
            string errorText = "";
            string insertedList = "";
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;

            //var Workshops = db.tbl_Workshops.Where(w => w.IRNGVCod != null && ( w.ID != 7 && w.ID != 60)).ToList();
            var Workshops = db.tbl_Workshops.Where(w => w.IRNGVCod != null).ToList();

            foreach (var item in Workshops)
            {

                if (Year == 1402)
                {
                    for (int i = 9; i <= 12; i++)
                    {
                        if (i < 10)
                            Mon = "0" + i;
                        else
                            Mon = i.ToString();
                        //
                        //cheked exist record in invoice damages
                        var invoices = db.tbl_InvoicesValveDamages.Where(n => n.WorkshopsID == item.ID).OrderByDescending(n => n.InvoiceID).ToList();
                        var exsitInvoice = invoices.Where(n => n.Year.Equals(Year.ToString())).ToList();
                        var existSameRec = exsitInvoice.Where(n => n.Month == Mon).ToList();
                        //
                        if (invoices.Count() == 0)
                        {
                            invoiceCode = 1;
                        }
                        if (invoices.Count() > 0)
                        {
                            existInvoiceCode = invoices.Max(n => Convert.ToInt32(n.InvoiceCode)).ToString();
                            invoiceCode = (Convert.ToInt32(existInvoiceCode) + 1);
                        }
                        //
                        if (existSameRec.Count() == 0)
                        {
                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", item.ID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@Year", Year));
                            cmd.Parameters.Add(new SqlParameter("@Month", Mon));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //invoiceCode += 1;
                            //----------------------
                            Message = "  فاکتور فروش شیر ضایعاتی مخزن سال : " + " " + Year + "ماه " + Month + " " + "با موفقیت صادر گردید.";
                            textClass = "text-success";
                        }
                        else if (existSameRec.Count() > 0)
                        {
                            Message = "فاکتور مشابه‌ای قبلا صادر شده است!";
                            textClass = "text-danger";
                        }
                        else
                        {
                            Message = "خطای غیر منتظره‌ای رخ داده است!";
                            textClass = "text-danger";
                        }
                    }
                    //reset invoiceCode counter
                    //invoiceCode = 1;
                }
                else
                {
                    for (int i = 1; i <= Month; i++)
                    {
                        if (i < 10)
                            Mon = "0" + i;
                        else
                            Mon = i.ToString();
                        //
                        //cheked exist record in invoice damages
                        var invoices = db.tbl_InvoicesValveDamages.Where(n => n.WorkshopsID == item.ID).OrderByDescending(n => n.InvoiceID).ToList();
                        var exsitInvoice = invoices.Where(n => n.Year.Equals(Year.ToString())).ToList();
                        var existSameRec = exsitInvoice.Where(n => n.Month == Mon).ToList();
                        //

                        if (invoices.Count() == 0)
                        {
                            invoiceCode = 1;
                        }
                        if (invoices.Count() > 0)
                        {
                            existInvoiceCode = invoices.Max(n => Convert.ToInt32(n.InvoiceCode)).ToString();
                            invoiceCode = (Convert.ToInt32(existInvoiceCode) + 1);
                        }
                        //
                        //                        
                        if (existSameRec.Count() == 0)
                        {
                            SqlConnection cnn = new SqlConnection(connStr);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_CreateWorkshopsDamagesInvoice]";
                            cmd.Parameters.Add(new SqlParameter("@workshopId", item.ID));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@invoiceCode", invoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@Year", Year));
                            cmd.Parameters.Add(new SqlParameter("@Month", Mon));
                            //add any parameters the stored procedure might require
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //invoiceCode += 1;
                            //----------------------
                            insertedList += Helper.Helpers.GetWorkshops(item.ID).Title + ", " + "<br />";
                            Message = "  فاکتور فروش شیر ضایعاتی مخزن سال : " + " " + Year + "ماه " + Month + " " + "با موفقیت صادر گردید.";
                            textClass = "text-success";
                        }
                        else if (existSameRec.Count() > 0)
                        {
                            Message = "فاکتور مشابه‌ای قبلا صادر شده است!";
                            textClass = "text-danger";
                        }
                        else
                        {
                            Message = "خطای غیر منتظره‌ای رخ داده است!";
                            textClass = "text-danger";
                        }
                    }
                    //reset invoiceCode counter
                    //invoiceCode = 1;
                }
            }
            //
            ViewBag.Message = Message;
            ViewBag.textClass = textClass;
            ViewBag.insertedList = insertedList;
            ViewBag.Month = Month;
            ViewBag.Year = Year;
            ViewBag.MonthTitle = Helper.Helpers.GetLongMonth(Convert.ToInt32(Month));

            return View();
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
        //
        //[HttpGet]
        public ActionResult LatestFinancialStatus(string fcheckedControl, string fromDate, string toDate)
        {            
            string condition = "0";

            if (fcheckedControl == null)
                fcheckedControl = "false";

            if (fcheckedControl.Equals("true"))
                condition = "1";

            if (String.IsNullOrEmpty(fromDate))
                fromDate = "1399/01/01";
            if (String.IsNullOrEmpty(toDate))
                toDate = DateTime.Now.ToShortDateString();            

            List<LatestFinancialStatusList> tableOuts = new List<LatestFinancialStatusList>();
           
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            // SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_LatestFinancialStatus]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@condition", SqlDbType.NVarChar).Value = condition;
                        command.Parameters.Add("@fromDate", SqlDbType.Date).Value = fromDate;
                        command.Parameters.Add("@toDate", SqlDbType.Date).Value = toDate;

                        conn.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var model = Read(reader).ToList();
                            //
                            if(fcheckedControl.Equals("true"))
                                ViewBag.fchecked = "checked";

                            ViewBag.fromDate = fromDate;
                            ViewBag.toDate = toDate;
                            //
                            return View(model);
                        }                        
                    }
                }
                
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                ViewBag.errorMesage = ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.errorMesage = ex.Message;
            }
            //
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //
            return View();
        }
        //
        public ActionResult LatestFinancialStatusDetails(string fchecked, string fromDate, string toDate)
        {
            string condition = "0";

            if (fchecked.Equals("checked"))
                condition = "1";

            if (String.IsNullOrEmpty(fromDate))
                fromDate = "1399/01/01";
            if (String.IsNullOrEmpty(toDate))
                toDate = DateTime.Now.ToShortDateString();

            List<LatestFinancialStatusList> tableOuts = new List<LatestFinancialStatusList>();

            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            // SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_LatestFinancialStatusDetails]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@condition", SqlDbType.NVarChar).Value = condition;
                        command.Parameters.Add("@fromDate", SqlDbType.Date).Value = fromDate;
                        command.Parameters.Add("@toDate", SqlDbType.Date).Value = toDate;

                        conn.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var model = Read(reader).ToList();
                            //
                            if (fchecked.Equals("checked"))
                            {
                                ViewBag.fchecked = "checked";
                                ViewBag.fcheckedControl = "true";
                            }

                            ViewBag.fromDate = fromDate;
                            ViewBag.toDate = toDate;
                            //
                            return View(model);
                        }
                    }
                }

            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                ViewBag.errorMesage = ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.errorMesage = ex.Message;
            }
            //
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            //
            return View();
        }
        //
        public class LatestFinancialStatusList
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string Owner { get; set; }
            public string TabNumber { get; set; }
            public string TabAmount { get; set; }
            public string TavNumber { get; set; }
            public string TavAmount { get; set; }
            public string fMNumber { get; set; }
            public string fMAmount { get; set; }
            public string fSMNumber { get; set; }
            public string fSMAmount { get; set; }
            public string RBarCost { get; set; }
            public string DTab { get; set; }
            public string DTaav { get; set; }
            public string CreditSales { get; set; }
            public string BarCost { get; set; }
            public string AudCost { get; set; }
            public string HoAn { get; set; }
            public string BimehBack { get; set; }
            public string Credit { get; set; }
            public string Debit { get; set; }
            public string Taraz { get; set; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //
    }
}
