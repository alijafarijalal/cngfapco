using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using cngfapco.Models;
using System.Data.SqlClient;
using System.IO;
using context = System.Web.HttpContext;
using System.Data.Entity;
using System.Globalization;

namespace cngfapco.Controllers
{
    public class OnlinePaymentsController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();

        // GET: OnlinePayments
        public static readonly string PgwSite = ConfigurationManager.AppSettings["PgwSite"];
        public static readonly string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"];
        public static readonly string TerminalId = ConfigurationManager.AppSettings["TerminalId"];
        public static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        public static readonly string UserPassword = ConfigurationManager.AppSettings["UserPassword"];
        void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        public static string RefId = "";
        public string SetDefaultDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

        }
        public string SetDefaultTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
        public ActionResult Index(string name,string nationalcode,string mobilenumber, string email, string amount, string PreInvoiceCode, string InvoiceCode,string PayerCode)
        {
            if (!string .IsNullOrEmpty(name))
            {
                ViewBag.Nationalcode = nationalcode;
                ViewBag.Name = name;
                ViewBag.Mobile = mobilenumber;
                ViewBag.Email = "-";
                ViewBag.Amount = amount;
                ViewBag.PreInvoiceCode = PreInvoiceCode;
                ViewBag.InvoiceCode = InvoiceCode;
                string _PreInvoiceCode = PreInvoiceCode;
                string _InvoiceCode = InvoiceCode;
                string _name = name;
                string _PayerCode = PayerCode;
                string _email = email;
                string _nationalcode = nationalcode;
                string _mobilenumber = mobilenumber;
                int? _amount = int.Parse(amount);
                DateTime _paydate = DateTime.Now;
                string _payerip = Helper.Helpers.GetVisitorIPAddress();
                int? payment_id = 0;
                //
                Random rnd = new Random();               
                int card = rnd.Next(10000);    // creates a number between 0 and 10000
                payment_id = card;
                //                 
                try
                {
                    string result = "";
                    BypassCertificateError();
                    BankMellatServices.PaymentGatewayClient bp = new BankMellatServices.PaymentGatewayClient();
                    result = bp.bpPayRequest(Int64.Parse(TerminalId), UserName, UserPassword, long.Parse(payment_id.Value.ToString()), long.Parse(_amount.ToString()), SetDefaultDate(), SetDefaultTime(), "cng.fapco.ir", CallBackUrl, "0");
                    string[] res = result.Split(',');
                    if (res[0] == "0")
                    {                        
                        try
                        {
                            string cnnString = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                            SqlConnection cnn = new SqlConnection(cnnString);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_InsertPayment]";
                            cmd.Parameters.Add(new SqlParameter("@FullName", _name));
                            cmd.Parameters.Add(new SqlParameter("@PayerCode", _PayerCode));
                            cmd.Parameters.Add(new SqlParameter("@PreInvoiceCode", _PreInvoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@InvoiceCode", InvoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@NationalCode", _nationalcode));
                            cmd.Parameters.Add(new SqlParameter("@MobileNumber", _mobilenumber));
                            cmd.Parameters.Add(new SqlParameter("@EMailAddress", _email));
                            cmd.Parameters.Add(new SqlParameter("@Amount", _amount));
                            cmd.Parameters.Add(new SqlParameter("@PayerIPAddress", _payerip));
                            cmd.Parameters.Add(new SqlParameter("@Status", "پرداخت نشده"));
                            cmd.Parameters.Add(new SqlParameter("@RefID", res[1]));
                            cmd.Parameters.Add(new SqlParameter("@OrderID", long.Parse(payment_id.Value.ToString())));
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                        }
                        catch (Exception ex)
                        {
                            string filePath = @"C:\Error.txt";

                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                writer.WriteLine("-----------------------------------------------------------------------------");
                                writer.WriteLine("Date : " + DateTime.Now.ToString());
                                writer.WriteLine();

                                while (ex != null)
                                {
                                    writer.WriteLine(ex.GetType().FullName);
                                    writer.WriteLine("Message : " + ex.Message);
                                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                                    ex = ex.InnerException;
                                }
                            }
                        }

                        ViewBag.jscode = "<script>postRefId('" + res[1] + "')</script>";
                    }
                    else
                    {
                        ViewBag.message = "خطای " + res[0] + " در ارتباط با درگاه بانکی";
                        ViewBag.textcolor = "#ce2626";
                    }

                }
                catch
                {
                    ViewBag.message = "خطا در ارتباط با درگاه بانکی";
                    ViewBag.textcolor = "#ce2626";
                }
            }
            return View();
        }
        //
        public ActionResult Index2(string name, string nationalcode, string mobilenumber, string email, string amount, string PreInvoiceCode, string InvoiceCode)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ViewBag.Nationalcode = nationalcode;
                ViewBag.Name = name;
                ViewBag.Mobile = mobilenumber;
                ViewBag.Email = "-";
                ViewBag.Amount = amount;
                ViewBag.PreInvoiceCode = PreInvoiceCode;
                ViewBag.InvoiceCode = InvoiceCode;
                string _PreInvoiceCode = PreInvoiceCode;
                string _InvoiceCode = InvoiceCode;
                string _name = name;
                string _email = email;
                string _nationalcode = nationalcode;
                string _mobilenumber = mobilenumber;
                int? _amount = int.Parse(amount);
                DateTime _paydate = DateTime.Now;
                string _payerip = Helper.Helpers.GetVisitorIPAddress();
                int? payment_id = 0;
                //
                Random rnd = new Random();
                int card = rnd.Next(10000);    // creates a number between 0 and 10000
                payment_id = card;
                //                 
                try
                {
                    string result = "";
                    BypassCertificateError();
                    BankMellatServices.PaymentGatewayClient bp = new BankMellatServices.PaymentGatewayClient();
                    result = bp.bpPayRequest(Int64.Parse(TerminalId), UserName, UserPassword, long.Parse(payment_id.Value.ToString()), long.Parse(_amount.ToString()), SetDefaultDate(), SetDefaultTime(), "cng.fapco.ir", CallBackUrl, "0");
                    string[] res = result.Split(',');
                    if (res[0] == "0")
                    {
                        try
                        {
                            string cnnString = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                            SqlConnection cnn = new SqlConnection(cnnString);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_InsertPayment]";
                            cmd.Parameters.Add(new SqlParameter("@FullName", _name));
                            cmd.Parameters.Add(new SqlParameter("@PreInvoiceCode", _PreInvoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@InvoiceCode", _InvoiceCode));
                            cmd.Parameters.Add(new SqlParameter("@FullName", _name));
                            cmd.Parameters.Add(new SqlParameter("@NationalCode", _nationalcode));
                            cmd.Parameters.Add(new SqlParameter("@MobileNumber", _mobilenumber));
                            cmd.Parameters.Add(new SqlParameter("@EMailAddress", _email));
                            cmd.Parameters.Add(new SqlParameter("@Amount", _amount));
                            cmd.Parameters.Add(new SqlParameter("@PayerIPAddress", _payerip));
                            cmd.Parameters.Add(new SqlParameter("@Status", "پرداخت نشده"));
                            cmd.Parameters.Add(new SqlParameter("@RefID", res[1]));
                            cmd.Parameters.Add(new SqlParameter("@OrderID", long.Parse(payment_id.Value.ToString())));
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                        }
                        catch (Exception ex)
                        {
                            string filePath = @"C:\Error.txt";

                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                writer.WriteLine("-----------------------------------------------------------------------------");
                                writer.WriteLine("Date : " + DateTime.Now.ToString());
                                writer.WriteLine();

                                while (ex != null)
                                {
                                    writer.WriteLine(ex.GetType().FullName);
                                    writer.WriteLine("Message : " + ex.Message);
                                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                                    ex = ex.InnerException;
                                }
                            }
                        }

                        ViewBag.jscode = "<script>postRefId('" + res[1] + "')</script>";
                    }
                    else
                    {
                        ViewBag.message = "خطای " + res[0] + " در ارتباط با درگاه بانکی";
                        ViewBag.textcolor = "#ce2626";
                    }

                }
                catch
                {
                    ViewBag.message = "خطا در ارتباط با درگاه بانکی";
                    ViewBag.textcolor = "#ce2626";
                }
            }
            return View();
        }
        //
        public ActionResult CallBackResults(string RefId, string ResCode, string saleOrderId, string SaleReferenceId)
        {            
            try
            {
                if (ResCode == "0")
                {
                    string result = "";
                    BypassCertificateError();
                    BankMellatServices.PaymentGatewayClient bp = new BankMellatServices.PaymentGatewayClient();
                    result = bp.bpVerifyRequest(Int64.Parse(TerminalId), UserName, UserPassword, long.Parse(saleOrderId), long.Parse(saleOrderId), long.Parse(SaleReferenceId));
                    if (result == "0")
                    {
                        try
                        {
                            int? payment_id = 0;
                            var payment = db.tbl_Payments.Where(c => c.RefID == RefId);
                            if (payment != null)
                                payment_id = payment.SingleOrDefault().ID;
                            else
                                payment_id = 1;                            

                            string cnnString = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                            SqlConnection cnn = new SqlConnection(cnnString);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[sp_UpdatePayment]";
                            cmd.Parameters.Add(new SqlParameter("@Status", "پرداخت شده"));
                            cmd.Parameters.Add(new SqlParameter("@RefID", RefId));
                            cmd.Parameters.Add(new SqlParameter("@SaleReferenceId", SaleReferenceId));
                            cmd.Parameters.Add(new SqlParameter("@PaymentId", payment_id));
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                            //
                            string orderId = payment.SingleOrDefault().OrderID;
                            ViewBag.Date = payment.SingleOrDefault().PayDate.ToShortDateString();
                            ViewBag.Time = payment.SingleOrDefault().PayDate.ToShortTimeString();
                            ViewBag.FullName = payment.SingleOrDefault().FullName;
                            ViewBag.InvoiceCode = payment.SingleOrDefault().PreInvoiceCode;
                            ViewBag.Amount = payment.SingleOrDefault().Amount.ToString("#,##");

                            //در صورت موفق بودن نتیجه پرداخت فاکتور نهایی صادر خواهد شد.
                            //if(payment.SingleOrDefault().Status.Equals("پرداخت شده"))
                            //{
                            //    List<FreeSaleInvoice> invoices = db.tbl_FreeSaleInvoices.Where(f => f.InvoiceCode == payment.SingleOrDefault().PreInvoiceCode).ToList();
                            //    using (ContextDB entities = new ContextDB())
                            //    {
                            //        string invoiceparam = "";
                            //        int CurrYear = pc.GetYear(DateTime.Now);
                            //        var departmentid = cngfapco.Helper.Helpers.GetWorkshopCurrentUser().ID;
                            //        //Check for NULL.
                            //        if (invoices == null || invoices.Count == 0)
                            //        {
                            //            //Thread.Sleep(1000);
                            //            return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                            //        }
                            //        else
                            //        {
                            //            var invoicelist = db.tbl_FinallFreeSaleInvoices.ToList();
                            //            var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
                            //            int maxcode = 1;
                            //            string finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";

                            //            if (invoicelist2.Count() > 0)
                            //            {
                            //                var invoicecount = invoicelist2.Take(1).Max(i => i.InvoiceCode).Replace(pc.GetYear(DateTime.Now) + "-" + "109" + "-", "");
                            //                maxcode = Convert.ToInt32(invoicecount) + 1;
                            //                finallmaxcode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + maxcode;
                            //                ViewBag.invoiceCode = (maxcode).ToString();
                            //                ViewBag.finallinvoiceCode = finallmaxcode;
                            //            }
                            //            else
                            //            {
                            //                ViewBag.invoiceCode = "1";
                            //                ViewBag.finallinvoiceCode = pc.GetYear(DateTime.Now) + "-" + "109" + "-" + "1";
                            //            }

                            //            FreeSaleInvoice additem = new FreeSaleInvoice();
                            //            //Loop and insert records.
                            //            foreach (var invoice in invoices)
                            //            {
                            //                RequestFreeSale updateStatus = db.tbl_RequestFreeSales.Find(invoice.InvoiceID);
                            //                updateStatus.Status = true;
                            //                //
                            //                if (invoice.EquipmentsID != null)
                            //                {
                            //                    additem.EquipmentsID = invoice.EquipmentsID; //db.tbl_EquipmentList.Where(e => e.ID.Equals(invoice.EquipmentsID.ToString())).SingleOrDefault().ID;
                            //                }
                            //                else
                            //                {
                            //                    additem.EquipmentsID = null;
                            //                }
                            //                //
                            //                additem.WorkshopsID = db.tbl_Workshops.Where(c => c.ID == invoice.WorkshopsID).SingleOrDefault().ID;
                            //                additem.Comment = invoice.Comment;

                            //                if (invoice.CreatedDate == null)
                            //                    additem.CreatedDate = DateTime.Now;
                            //                else
                            //                    additem.CreatedDate = invoice.CreatedDate;
                            //                additem.AmountTaxandComplications = Convert.ToDouble(invoice.AmountTaxandComplications);
                            //                additem.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                            //                additem.TotalAmount = Convert.ToDouble(invoice.TotalAmount);
                            //                additem.TotalAmountafterDiscount = Convert.ToDouble(invoice.TotalAmountafterDiscount);
                            //                additem.TotalAmountTaxandComplications = Convert.ToDouble(invoice.TotalAmountTaxandComplications);
                            //                if (invoice.ServiceDesc == null)
                            //                    additem.ServiceDesc = "مقداری ثبت نشده!";
                            //                else
                            //                    additem.ServiceDesc = invoice.ServiceDesc;
                            //                if (invoice.UnitAmount < 1)
                            //                    additem.UnitAmount = 1;
                            //                else
                            //                    additem.UnitAmount = Convert.ToDouble(invoice.UnitAmount);
                            //                if (invoice.Number == null)
                            //                    additem.Number = "1";
                            //                else
                            //                    additem.Number = invoice.Number;
                            //                additem.EmployerEconomicalnumber = invoice.EmployerEconomicalnumber;
                            //                additem.Employerregistrationnumber = invoice.Employerregistrationnumber;
                            //                additem.EmployerPostalcode = invoice.EmployerPostalcode;
                            //                additem.EmployerPhone = invoice.EmployerPhone;
                            //                additem.EmployerState = invoice.EmployerState;
                            //                additem.UnitofMeasurement = invoice.UnitofMeasurement;
                            //                additem.EmployerAddress = invoice.EmployerAddress;
                            //                additem.EmployerFax = invoice.EmployerFax;
                            //                additem.Description = invoice.Description;
                            //                additem.SaleCondition = invoice.SaleCondition;
                            //                additem.Tax = 0;
                            //                additem.Complications = 0;
                            //                additem.Status = false;
                            //                additem.ViewStatus = false;
                            //                additem.ServiceCode = invoice.ServiceCode;
                            //                additem.CustomersID = invoice.CustomersID;
                            //                if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            //                    additem.CurrencyTypeID = invoice.CurrencyTypeID;
                            //                else
                            //                    additem.CurrencyTypeID = 6;
                            //                //
                            //                additem.InvoiceCode = finallmaxcode;
                            //                additem.RequestInvoiceCode = invoice.InvoiceCode;
                            //                additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                            //                //
                            //                db.tbl_FreeSaleInvoices.Add(additem);
                            //                db.SaveChanges();
                            //                //
                            //                invoiceparam = invoice.InvoiceCode;
                            //            }
                            //        }
                            //    }
                            //}                            
                        }
                        catch { }

                        ViewBag.message = "پرداخت شما با موفقیت انجام شد" + "<br /> <br />" + "شماره سفارش: " + saleOrderId + "<br /> <br />" + "شماره تراکنش: " + SaleReferenceId + "<br /> <br />" + "شماره ارجاع: " + RefId;
                        ViewBag.textcolor = "#1cb22f";

                        //try
                        //{
                        //    var invoice = db.tbl_FreeSaleInvoices.Where(f => f.InvoiceCode.Equals(InvoiceCode)).ToList();
                        //    foreach(var item in invoice)
                        //    {
                        //        FreeSaleInvoice updateData = db.tbl_FreeSaleInvoices.Find(item.InvoiceID);
                        //        updateData.AcceptedAmount = item.TotalAmountTaxandComplications;
                        //        updateData.AcceptedDate = DateTime.Now;
                        //        updateData.Status = true;
                        //        updateData.ViewDate = DateTime.Now;
                        //        updateData.ViewStatus = true;
                        //        updateData.Viewer = User.Identity.Name;
                        //        db.Entry(updateData).State = EntityState.Modified;
                        //        db.SaveChanges();
                        //    }
                        //}
                        //catch { }
                    }
                    else
                    {
                        ViewBag.message = "پرداخت آنلاین با خطا مواجه شده است.";
                        ViewBag.textcolor = "#ce2626";
                    }
                }
                else
                {
                    ViewBag.message = "پرداخت آنلاین با خطا مواجه شده است.";
                    ViewBag.textcolor = "#ce2626";
                }
            }
            catch(Exception ex)
            {
                string filePath = @"C:\Error.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Message : " + ex.Message);
                        writer.WriteLine("StackTrace : " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }

                ViewBag.message = "خطا در برقراری ارتباط با درگاه بانک ملت";
            }

            return View();
        }
        //
        public ActionResult TransactionLogs()
        {
            var transaction = db.tbl_Payments.OrderByDescending(p => p.PayDate);
            return View(transaction.ToList());
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