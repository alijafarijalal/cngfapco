using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    [Authorize]
    //
    public class FreeSaleRemittancesController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();

        //Get workshop title
        public JsonResult GetWorkshopTitle(string id)
        {
            if(id!=null)
            {
                var workshopTitle = db.tbl_FinallFreeSaleInvoices.Where(d => d.InvoiceCode == id).Include(d => d.Workshops).FirstOrDefault().Workshops.Title;
                return Json(workshopTitle, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var workshopTitle = "یافت نشد!";
                return Json(workshopTitle, JsonRequestBehavior.AllowGet);
            }
            
        }
        //
        public static string GetDetailsCheck(int? id)
        {
            RemittanceDetails remittancedetails = dbStatic.tbl_RemittanceDetails
                .Include(i => i.Remittances)
                .Where(c => c.RemittancesID == id)
                .SingleOrDefault();

            if (remittancedetails != null)
                return remittancedetails.BillofLading;
            else
                return "+";
        }
        // GET: Remittances
        public ActionResult Index()
        {
            List<cngfapco.Models.Role> rolName = cngfapco.Helper.Helpers.GetCurrentUserRoles();
            List<FreeSaleRemittances> remittancelist = new List<FreeSaleRemittances>();
            
            foreach (var role in rolName)
            {
                var tbl_Remittances = db.tbl_FreeSaleRemittances.OrderByDescending(r=>r.ID).ToList();
                
                foreach (var item in tbl_Remittances)
                {
                    remittancelist.Add(new FreeSaleRemittances
                    {
                        CreateDate = item.CreateDate,
                        Creator = item.Creator,
                        Description = item.Description,
                        InvoiceCode = item.InvoiceCode,
                        ID = item.ID,
                        Number = item.Number,
                        Status = item.Status,
                        StatusDate = item.StatusDate
                    });
                };
            }
           
            return View(remittancelist.ToList());
        }

        // GET: Remittances
        public ActionResult DetailsIndex(int? id, string url)
        {
            ViewBag.url = url;
            ViewBag.remittanceId = id;
            List<FreeSaleRemittanceDetails> remittancedetailslist = new List<FreeSaleRemittanceDetails>();

            var tbl_RemittanceDetails = db.tbl_FreeSaleRemittanceDetails.Where(r=>r.RemittancesID==id).Include(r => r.Remittances).ToList();
            foreach (var item in tbl_RemittanceDetails)
            {
                remittancedetailslist.Add(new FreeSaleRemittanceDetails
                {
                    CreateDate = item.CreateDate,
                    Creator = item.Creator,
                    Description = item.Description,
                    BillofLading = item.BillofLading,
                    Date = item.Date,
                    Plate = item.Plate,
                    Remittances = item.Remittances,
                    RemittancesID = item.RemittancesID,
                    Transferee = item.Transferee,
                    Vehicle = item.Vehicle,
                    Attachment = item.Attachment,
                    CarrierName = item.CarrierName,
                    CarryFare = item.CarryFare,
                    ID = item.ID
                });
            };


            return View(remittancedetailslist);
        }
        // GET: Remittances/View with Users Role in Home/Index
        public ActionResult LockscreenView()
        {
            List<cngfapco.Models.Role> rolName = cngfapco.Helper.Helpers.GetCurrentUserRoles();
            List<Remittances> remittancelist = new List<Remittances>();

            foreach (var role in rolName)
            {
                if (role.RoleName.Contains("مدیر تبدیل ناوگان") || role.RoleName.Contains("انبار") || role.RoleName.Equals("admin"))
                {
                    var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.Send==true).Include(r => r.DivisionPlan).Include(r => r.User);
                    foreach (var item in tbl_Remittances)
                    {
                        remittancelist.Add(new Remittances
                        {
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            Description = item.Description,
                            DivisionPlan = item.DivisionPlan,
                            DivisionPlanID = item.DivisionPlanID,
                            ID = item.ID,
                            Number = item.Number,
                            Status = item.Status,
                            StatusDate = item.StatusDate,
                            User = item.User
                        });
                    };

                    //return View(tbl_Remittances.ToList());
                }
                else
                {
                    var workshopId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().WorkshopID;
                    var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.WorkshopID == workshopId).Include(r => r.DivisionPlan).Include(r => r.User);

                    foreach (var item in tbl_Remittances)
                    {
                        if ((DateTime.Now - item.CreateDate).Days > 0)
                        {
                            remittancelist.Add(new Remittances
                            {
                                CreateDate = item.CreateDate,
                                Creator = item.Creator,
                                Description = item.Description,
                                DivisionPlanID = item.DivisionPlanID,
                                DivisionPlan = item.DivisionPlan,
                                ID = item.ID,
                                Number = item.Number,
                                Status = item.Status,
                                StatusDate = item.StatusDate,
                                User = item.User
                            });
                        }
                    }

                }
            }

            return View(remittancelist.ToList());
        }

        // GET: Remittances/View with Users Role in Home/Lockscreen 
        public ActionResult UsersView()
        {
            List<cngfapco.Models.Role> rolName = cngfapco.Helper.Helpers.GetCurrentUserRoles();
            List<Remittances> remittancelist = new List<Remittances>();

            foreach (var role in rolName)
            {
                var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null && r.DivisionPlan.Send != false )).Include(r => r.DivisionPlan).Include(r => r.User);

                foreach (var item in tbl_Remittances)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.DivisionPlan.WorkshopID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        remittancelist.Add(new Remittances
                        {
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            Description = item.Description,
                            DivisionPlan = item.DivisionPlan,
                            DivisionPlanID = item.DivisionPlanID,
                            ID = item.ID,
                            Number = item.Number,
                            Status = item.Status,
                            StatusDate = item.StatusDate,
                            User = item.User
                        });
                    }

                };
            }
            return View(remittancelist.ToList());
        }

        // GET: Remittances/View with Users Role in Home/Index
        public ActionResult IncompleteItems()
        {
            List<cngfapco.Models.Role> rolName = cngfapco.Helper.Helpers.GetCurrentUserRoles();
            List<Remittances> remittancelist = new List<Remittances>();

            foreach (var role in rolName)
            {
                var tbl_Remittances = db.tbl_Remittances.Where(r=>r.Incomplete==true).Include(r => r.DivisionPlan).Include(r => r.User).ToList();
                foreach (var item in tbl_Remittances)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.DivisionPlan.WorkshopID); //cngfapco.Helper.Helpers.GetWorkshops(item.DivisionPlan.WorkshopID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        //if (!string.IsNullOrEmpty(item.Description))
                        //{
                            remittancelist.Add(new Remittances
                            {
                                CreateDate = item.CreateDate,
                                Creator = item.Creator,
                                Description = item.Description,
                                DivisionPlan = item.DivisionPlan,
                                DivisionPlanID = item.DivisionPlanID,
                                ID = item.ID,
                                Number = item.Number,
                                Status = item.Status,
                                StatusDate = item.StatusDate,
                                User = item.User,
                                Incomplete=item.Incomplete,
                                IncompleteDesc=item.IncompleteDesc
                            });
                        //}
                    }

                };
            }
            return View(remittancelist.ToList());
        }
        // GET: Remittances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remittances remittances = db.tbl_Remittances.Find(id);
            if (remittances == null)
            {
                return HttpNotFound();
            }
            return View(remittances);
        }

        // GET: Remittances/Create
        
        public ActionResult Create(string InvoiceCode)
        {
            var invoiceItems = db.tbl_FinallFreeSaleInvoices.Where(f=>f.InvoiceCode== InvoiceCode).ToList();
            var remittance = db.tbl_FreeSaleRemittances.OrderByDescending(f=>f.ID).ToList();
            string remittanceCode = "001";
            if (remittance.Count > 0)
                if((Convert.ToInt32(remittance.FirstOrDefault().Number) + 1)<10)
                    remittanceCode ="00" + (Convert.ToInt32(remittance.FirstOrDefault().Number) + 1).ToString();
                else if ((Convert.ToInt32(remittance.FirstOrDefault().Number) + 1) < 100)
                    remittanceCode = "0" + (Convert.ToInt32(remittance.FirstOrDefault().Number) + 1).ToString();
                else if ((Convert.ToInt32(remittance.FirstOrDefault().Number) + 1) >= 100)
                    remittanceCode = (Convert.ToInt32(remittance.FirstOrDefault().Number) + 1).ToString();
            ViewBag.remittanceCode = remittanceCode;

            List<FinallFreeSaleInvoice> dropdownList = new List<FinallFreeSaleInvoice>();
            foreach(var item in invoiceItems.Take(1))
            {
                var remittanceItems = db.tbl_FreeSaleRemittances.Where(r => r.InvoiceCode == item.InvoiceCode).FirstOrDefault();
                if (remittanceItems==null)
                {
                    dropdownList.Add(new FinallFreeSaleInvoice
                    {
                        Description= item.Workshops.Title + " " + item.InvoiceCode,
                        InvoiceCode= item.InvoiceCode
                    });
                }
            }
            ViewBag.InvoiceCode = new SelectList(dropdownList, "InvoiceCode", "Description",InvoiceCode);
            return View();
        }

        // POST: Remittances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Number,InvoiceCode,CreateDate,Creator,Status,StatusDate,Description")] FreeSaleRemittances remittances,
            DateTime? Date,string Vehicle,string PlateLeft, string PlateMiddle, string PlateRight, string PlateIR,string BillofLading,string Transferee,string Description2, string CarryFare, string CarrierName)
        {
            remittances.CreateDate = DateTime.Now;
            remittances.Creator = User.Identity.Name;
            //remittances.Number = Number.Trim();
            FreeSaleRemittanceDetails remittancedetails = new FreeSaleRemittanceDetails();

            if (ModelState.IsValid)
            {
                db.tbl_FreeSaleRemittances.Add(remittances);
                db.SaveChanges();
                //ثبت اطلاعات بارنامه ارسال
                if(Date!=null && (Vehicle!=null || Vehicle!=""))
                {
                    remittancedetails.BillofLading = BillofLading;
                    remittancedetails.CreateDate = DateTime.Now;
                    remittancedetails.Creator = User.Identity.Name;
                    remittancedetails.Date = Date;
                    remittancedetails.Description = Description2;
                    //remittancedetails.Plate = PlateLeft + " " + PlateMiddle + " " + PlateRight + " | ایران " + PlateIR;
                    remittancedetails.Plate = PlateRight + " " + PlateMiddle + " " + PlateLeft + " | ایران " + PlateIR;
                    remittancedetails.RemittancesID = remittances.ID;
                    remittancedetails.Transferee = Transferee;
                    remittancedetails.Vehicle = Vehicle;
                    remittancedetails.CarrierName = CarrierName;
                    remittancedetails.CarryFare = double.Parse(CarryFare.Replace(",", "").Replace(".", "").Replace("/", ""), System.Globalization.CultureInfo.InvariantCulture); ;
                    db.tbl_FreeSaleRemittanceDetails.Add(remittancedetails);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceCode = new SelectList(db.tbl_FinallFreeSaleInvoices, "InvoiceCode", "InvoiceCode", remittances.InvoiceCode);
            return View(remittances);
        }

        public ActionResult AddRemittanceDetails(int? id)
        {
            ViewBag.workshopTitle = "";
            ViewBag.remittanceId = id;
            return PartialView();
        }

        // POST: Remittances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRemittanceDetails(int? remittanceId, DateTime? Date, string Vehicle, string PlateLeft, string PlateMiddle, string PlateRight, string PlateIR, string BillofLading, string Transferee, string Description2, string CarryFare,string CarrierName)
        {           
            FreeSaleRemittanceDetails remittancedetails = new FreeSaleRemittanceDetails();
            remittancedetails.BillofLading = BillofLading;
            remittancedetails.CreateDate = DateTime.Now;
            remittancedetails.Creator = User.Identity.Name;
            remittancedetails.Date = Date;
            remittancedetails.Description = Description2;
            remittancedetails.Plate = PlateRight + " " + PlateMiddle + " " + PlateLeft + " | ایران " + PlateIR;
            remittancedetails.RemittancesID = remittanceId;
            remittancedetails.Transferee = Transferee;
            remittancedetails.Vehicle = Vehicle;
            remittancedetails.CarrierName = CarrierName;
            remittancedetails.CarryFare = double.Parse(CarryFare.Replace(",", "").Replace(".", "").Replace("/", ""), System.Globalization.CultureInfo.InvariantCulture);
            db.tbl_FreeSaleRemittanceDetails.Add(remittancedetails);
            db.SaveChanges();

            return RedirectToAction("DetailsIndex",new {id= remittanceId });
        }

        
        // GET: Remittances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FreeSaleRemittances remittances = db.tbl_FreeSaleRemittances.Find(id);
            if (remittances == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceCode = new SelectList(db.tbl_FreeSaleRemittances, "InvoiceCode", "Description", remittances.InvoiceCode);
            return View(remittances);
        }

        // POST: Remittances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Number,DivisionPlanID,CreateDate,Creator,Status,StatusDate,Description")] FreeSaleRemittances remittances)
        {
            remittances.CreateDate = DateTime.Now;
            remittances.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(remittances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceCode = new SelectList(db.tbl_FreeSaleRemittances, "InvoiceCode", "Description", remittances.InvoiceCode);
            return View(remittances);
        }

        // GET: Remittances/Edit/5
        public ActionResult Confirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remittances remittances = db.tbl_Remittances.Find(id);
            if (remittances == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivisionPlanID = new SelectList(db.tbl_DivisionPlans, "ID", "Code", remittances.DivisionPlanID);
            return PartialView(remittances);
        }

        // POST: Remittances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation(Remittances remittances,bool? Incomplete, string IncompleteDesc)
        {           
            remittances.Status = true;
            remittances.StatusDate = DateTime.Now;
            remittances.Incomplete = Incomplete;
            remittances.IncompleteDesc = IncompleteDesc;

            db.Entry(remittances).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Home");

        }

        
        // GET: Remittances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remittances remittances = db.tbl_Remittances.Find(id);
            if (remittances == null)
            {
                return HttpNotFound();
            }
            return View(remittances);
        }

        // POST: Remittances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Remittances remittances = db.tbl_Remittances.Find(id);
            db.tbl_Remittances.Remove(remittances);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        // GET: FreeSaleFinancials/Remittance/5
        public ActionResult Remittance(string invoiceCode)
        {
            string BillofLading = "";
            string Description = "";
            DateTime? CreateDate = null;
            string CarrierName = "";
            double CarryFare = 0.0;
            string Plate = "";
            string Transferee = "";
            string Vehicle = "";
            int RemittancesID = 0;
            string Attachment = "";
            DateTime? Date = null;
            int Count = 0;

            List<RemittanceList> remittanceList = new List<RemittanceList>();
            List<FreeSaleRemittanceDetails> remittanceDetailsList = new List<FreeSaleRemittanceDetails>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_FreeSaleRemittance]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@invoiceCode", SqlDbType.VarChar).Value = invoiceCode;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader["BillofLading"].ToString()))
                        BillofLading = reader["BillofLading"].ToString();
                    else
                        BillofLading = "----------";
                    if (!string.IsNullOrEmpty(reader["Description"].ToString()))
                        Description = reader["Description"].ToString();
                    else
                        Description = "----------";
                    if (!string.IsNullOrEmpty(reader["CreateDate"].ToString()))
                        CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                    else
                        CreateDate = null;
                    if (!string.IsNullOrEmpty(reader["CarrierName"].ToString()))
                        CarrierName = reader["CarrierName"].ToString();
                    else
                        CarrierName = "----------";
                    if (!string.IsNullOrEmpty(reader["CarryFare"].ToString()))
                        CarryFare = Convert.ToDouble(reader["CarryFare"].ToString());
                    else
                        CarryFare = 0.0;
                    if (!string.IsNullOrEmpty(reader["Plate"].ToString()))
                        Plate = reader["Plate"].ToString();
                    else
                        Plate = "----------";
                    if (!string.IsNullOrEmpty(reader["Transferee"].ToString()))
                        Transferee = reader["Transferee"].ToString();
                    else
                        Transferee = "----------";
                    if (!string.IsNullOrEmpty(reader["Vehicle"].ToString()))
                        Vehicle = reader["Vehicle"].ToString();
                    else
                        Vehicle = "----------";
                    if (!string.IsNullOrEmpty(reader["RemittancesID"].ToString()))
                        RemittancesID = Convert.ToInt32(reader["RemittancesID"].ToString());
                    else
                        RemittancesID = 0;
                    if (!string.IsNullOrEmpty(reader["Number"].ToString()))
                        Attachment = reader["Number"].ToString();
                    else
                        Attachment = "----------";
                    if (!string.IsNullOrEmpty(reader["Date"].ToString()))
                        Date = Convert.ToDateTime(reader["Date"].ToString());
                    else
                        Date = null;
                    Count += 1;

                    remittanceDetailsList.Add(new FreeSaleRemittanceDetails
                    {
                       BillofLading=BillofLading,
                       Attachment=Attachment,
                       CarrierName=CarrierName,
                       CarryFare=CarryFare,
                       CreateDate=CreateDate.GetValueOrDefault(),
                       Date=Date,
                       Description=Description,
                       Plate=Plate,
                       RemittancesID=RemittancesID,
                       Transferee=Transferee,
                       Vehicle=Vehicle
                    });
                }
                reader.NextResult();
                while (reader.Read())
                {
                    remittanceList.Add(new RemittanceList
                    {
                        NumberofSend = reader["Number"].ToString(),
                        Description = reader["Description"].ToString(),
                        Unit = reader["UnitofMeasurement"].ToString(),
                        Title = reader["ServiceDesc"].ToString(),
                        FinancialCode = reader["ServiceCode"].ToString(),
                        WorkshopTitle = reader["Title"].ToString()
                    });
                }
                //
                conn.Close();
            }
            //
            if (remittanceList.FirstOrDefault() != null)
            {
                ViewBag.RemittanceList = remittanceList;
                ViewBag.WorkshopTitle = remittanceList.FirstOrDefault().WorkshopTitle;
            }
                
            if (remittanceDetailsList.FirstOrDefault() != null)
            {
                ViewBag.RemittanceDetailsList = remittanceDetailsList;
                ViewBag.RemittancesNumber = remittanceDetailsList.FirstOrDefault().Attachment;
                ViewBag.Date = remittanceDetailsList.FirstOrDefault().Date.HasValue? remittanceDetailsList.FirstOrDefault().Date.Value.ToShortDateString():null;
                ViewBag.CreateDate = remittanceDetailsList.FirstOrDefault().CreateDate.ToShortDateString();
            }
            //
            ViewBag.Count = Count;
            return View();
        }
        //
        public class RemittanceList
        {
            public string Title { get; set; }
            public string Unit { get; set; }
            public string NumberofSend { get; set; }
            public string Description { get; set; }
            public string FinancialCode { get; set; }
            public string WorkshopTitle { get; set; }
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
