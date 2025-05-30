using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class CustomersController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        // GET: Customers
        public ActionResult Index()
        {
            var tbl_Customers = db.tbl_Customers.Include(c => c.Cities);
            return View(tbl_Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.tbl_Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CitiesId = new SelectList(db.tbl_Cities, "ID", "Title");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LasstName,NationalCode,BirthDate,Economicalnumber,PostalCode,CitiesId,Address,Mobile,Phone,Fax,CreditLimit,Status")] Customer customer)
        {
            customer.CreateDate = DateTime.Now;
            customer.Creator = User.Identity.Name;
            if (string.IsNullOrEmpty(customer.CreditLimit.ToString()))
                customer.CreditLimit = 0;

            if (ModelState.IsValid)
            {
                db.tbl_Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CitiesId = new SelectList(db.tbl_Cities, "ID", "Title", customer.CitiesId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.tbl_Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CitiesId = new SelectList(db.tbl_Cities, "ID", "Title", customer.CitiesId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LasstName,NationalCode,BirthDate,Economicalnumber,PostalCode,CitiesId,Address,Mobile,Phone,Fax,CreditLimit,Status")] Customer customer)
        {
            customer.CreateDate = DateTime.Now;
            customer.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CitiesId = new SelectList(db.tbl_Cities, "ID", "Title", customer.CitiesId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.tbl_Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.tbl_Customers.Find(id);
            db.tbl_Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult Customers(int? customerId)
        {
            var customer = db.tbl_Customers.Find(customerId);
            return PartialView(customer);
        }

        /// <summary>
        /// ثبت درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="InvoiceCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestFreeSale(int? customerId)
        {
            int CurrYear = pc.GetYear(DateTime.Now);
            ViewBag.ServicesID = new SelectList(db.tbl_EquipmentList.Where(e=>e.Value>0 || e.Value2>0).OrderBy(e => e.Title), "ID", "Title");
            var requestList = db.tbl_CustomerRequests.Where(r=>r.InvoiceID==0);
            var invoicelist = db.tbl_CustomerRequests.OrderByDescending(i => i.InvoiceID).ToList();
            var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));
            int MaxofRow = 1;

            if (invoicelist.Count() > 0)
            {
                MaxofRow = Convert.ToInt32(invoicelist2.Max(i => Convert.ToInt32(i.InvoiceCode))) + 1;
                ViewBag.invoiceCode = MaxofRow.ToString();
            }
            else
            {
                ViewBag.invoiceCode = MaxofRow.ToString();
            }

            ViewBag.customerId = customerId;
            //
            return View(requestList);
        }

        /// <summary>
        /// ثبت اطلاعات درخواست خرید اقلام فروش آزاد توسط کارگاه ها
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequestFreeSale(List<CustomerRequest> invoices, string CustomersID)
        {
            var invoiceCodeList = db.tbl_CustomerRequests.Distinct();
            int MaxofRow = 1;
            //if (invoiceCodeList.Count() > 0)
            //    MaxofRow = (invoiceCodeList.Count() + 1).ToString();

            using (ContextDB entities = new ContextDB())
            {
                string invoiceparam = "";
                int CurrYear = pc.GetYear(DateTime.Now);
                //Check for NULL.
                if (invoices == null || invoices.Count == 0)
                {
                    Thread.Sleep(1000);
                    return Json(new { success = false, responseText = "متاسفانه ثبت اطلاعات با مشکل مواجه شده است، لطفا از درستی اطلاعات اطمینان حاصل کنید" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var invoicelist = db.tbl_CustomerRequests.OrderByDescending(i => i.InvoiceID).ToList();
                    var invoicelist2 = invoicelist.Where(i => pc.GetYear(i.CreatedDate.Value) == CurrYear).OrderByDescending(i => Convert.ToInt32(i.InvoiceCode));

                    if (invoicelist.Count() > 0)
                    {
                        MaxofRow = Convert.ToInt32(invoicelist2.Max(i => Convert.ToInt32(i.InvoiceCode))) + 1;
                        ViewBag.invoiceCode = MaxofRow;
                    }
                    else
                    {
                        ViewBag.invoiceCode = MaxofRow;
                    }

                    CustomerRequest additem = new CustomerRequest();
                    //Loop and insert records.
                    foreach (var invoice in invoices)
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
                        additem.CustomersID = invoice.CustomersID;
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
                        if (invoice.Number == null)
                            additem.Number = "1";
                        else
                            additem.Number = invoice.Number;
                        additem.Economicalnumber = invoice.Economicalnumber;
                        additem.Registrationnumber = invoice.Registrationnumber;
                        additem.Postalcode = invoice.Postalcode;
                        additem.Phone = invoice.Phone;
                        additem.State = invoice.State;
                        additem.UnitofMeasurement = invoice.UnitofMeasurement;
                        additem.Address = invoice.Address;
                        additem.Fax = invoice.Fax;
                        additem.Description = invoice.Description;
                        additem.SaleCondition = invoice.SaleCondition;
                        //additem.Comment = invoice.Comment;
                        additem.Status = false;
                        additem.ViewStatus = false;
                        additem.ServiceCode = invoice.ServiceCode;
                        additem.CustomersID = Convert.ToInt32(invoice.Comment);
                        if (!string.IsNullOrEmpty(invoice.CurrencyTypeID.ToString()))
                            additem.CurrencyTypeID = invoice.CurrencyTypeID;
                        else
                            additem.CurrencyTypeID = 6;
                        //
                        additem.InvoiceCode = invoice.InvoiceCode;// maxcode.ToString();
                        additem.CreatorUser = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                        //
                        db.tbl_CustomerRequests.Add(additem);
                        db.SaveChanges();
                        //
                        invoiceparam = invoice.InvoiceCode;
                    }
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "درخواست خرید اقلام شما با موفقیت ثبت و ارسال شد!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //
        /// <summary>
        /// مشاهده لیست درخواست خرید کالا توسط افراد شخصی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestFreeSaleList()
        {
            string Customers = "";
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

            List<CustomerRequestList> TableOuts = new List<CustomerRequestList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_CustomerRequestFreeSaleList]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Customers = reader["Customers"].ToString();
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

                        TableOuts.Add(new CustomerRequestList
                        {
                            Customers = Customers,
                            InvoiceCode = InvoiceCode,
                            CreatedDate = CreatedDate.Value.ToShortDateString(),
                            Number = Number.ToString(),
                            UnitofMeasurement = UnitofMeasurement,
                            TotalAmount = TotalAmount,
                            DiscountAmount = DiscountAmount,
                            Status = Status,
                            StatusColor = ViewStatusColor,
                            Description = preInvoiceCode,
                            SaleCondition = SaleCondition
                        });
                    }

                    //
                    conn.Close();
                }
            }//end using
            ViewBag.TableOuts = TableOuts;
            return View();
        }
        //
        public class CustomerRequestList
        {
            public string Customers { get; set; }
            public string InvoiceCode { get; set; }
            public string CreatedDate { get; set; }
            public string Number { get; set; }
            public string UnitofMeasurement { get; set; }
            public double TotalAmount { get; set; }
            public double DiscountAmount { get; set; }
            public bool Status { get; set; }
            public string StatusColor { get; set; }
            public string Description { get; set; }
            public string SaleCondition { get; set; }
        }
        //
        /// <summary>
        /// لیست درخواست خرید اقلام توسط کارگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CallBackRequestFreeSale(int? CustomersID)
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
                    command.Parameters.Add("@CustomersID", SqlDbType.VarChar).Value = CustomersID;

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

            return View(TableOuts.ToList());
        }
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
