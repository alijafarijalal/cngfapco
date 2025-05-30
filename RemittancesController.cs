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
    //[RBAC]
    public class RemittancesController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();

        //Get workshop title
        public JsonResult GetWorkshopTitle(int? id)
        {
            if (id != null)
            {
                var workshopTitle = db.tbl_DivisionPlans.Where(d => d.ID == id).Include(d => d.Workshop).SingleOrDefault().Workshop.Title;
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
        public ActionResult Index_Old()
        {
            List<cngfapco.Models.Role> rolName = cngfapco.Helper.Helpers.GetCurrentUserRoles();
            List<Remittances> remittancelist = new List<Remittances>();

            foreach (var role in rolName)
            {
                var tbl_Remittances = db.tbl_Remittances.Include(r => r.DivisionPlan).Include(r => r.User).OrderByDescending(r => r.ID).ToList();

                foreach (var item in tbl_Remittances)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.DivisionPlan.WorkshopID);
                    User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                    if (workshop.Users.Contains(_user))
                    {
                        string url = "Remittance";
                        //var divisionplan = db.tbl_DivisionPlans.Find(item.DivisionPlan.ID);
                        if (item.DivisionPlanID >= 2373)
                            url = "RemittanceWithBOMFixed";
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
                            IncompleteDesc = url
                        });
                    }

                };
            }

            return View(remittancelist.ToList());
        }
        //
        public ActionResult Index()
        {
            var workshop = db.tbl_Workshops.ToList();
            List<RemittanceList> TableOuts = new List<RemittanceList>();
            string workshops = "";
            foreach (var item in workshop)
            {
                //Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (item.Users.Contains(_user))
                {
                    workshops += item.ID + ",";
                }

            };
            //
            string ID = "";
            string Number = "";
            string DivisionPlanID = "";
            string CreateDate = "";
            string Creator = "";
            bool Status = false;
            string StatusDate = "";
            string Description = "";
            bool Incomplete = false;
            string IncompleteDesc = "";
            string Code = "";
            string Workshop = "";
            string Url = "";
            string Count = "";
            string Cost = "";
            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_Remittances]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Workshops", SqlDbType.VarChar).Value = workshops.TrimEnd(',');

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ID = reader["ID"].ToString();
                        Code = reader["Code"].ToString();
                        Workshop = reader["Workshop"].ToString();
                        CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString()).ToShortDateString();
                        if (!string.IsNullOrEmpty(reader["StatusDate"].ToString()))
                            StatusDate = Convert.ToDateTime(reader["StatusDate"].ToString()).ToShortDateString();
                        else
                            StatusDate = "";
                        Creator = reader["Creator"].ToString();
                        Description = reader["Description"].ToString();
                        IncompleteDesc = reader["IncompleteDesc"].ToString();
                        Number = reader["Number"].ToString();
                        if (!string.IsNullOrEmpty(reader["Status"].ToString()))
                            Status = Convert.ToBoolean(reader["Status"].ToString());
                        else
                            Status = false;
                        if (!string.IsNullOrEmpty(reader["Incomplete"].ToString()))
                            Incomplete = Convert.ToBoolean(reader["Incomplete"].ToString());
                        else
                            Incomplete = false;
                        DivisionPlanID = reader["DivisionPlanID"].ToString();
                        Url = reader["Url"].ToString();
                        Count = reader["Count"].ToString();
                        Cost = reader["Cost"].ToString();

                        TableOuts.Add(new RemittanceList
                        {
                            Code = Code,
                            Cost = Cost,
                            Count = Count,
                            Url = Url,
                            StatusDate = StatusDate,
                            IncompleteDesc = IncompleteDesc,
                            Incomplete = Incomplete,
                            Status = Status,
                            CreateDate = CreateDate,
                            Creator = Creator,
                            Description = Description,
                            DivisionPlanID = DivisionPlanID,
                            ID = ID,
                            Number = Number,
                            Workshop = Workshop
                        });
                    }
                    //
                    conn.Close();
                }
            }//end using
            //

            ViewBag.TableOut = TableOuts;

            return PartialView();
        }
        //
        public class RemittanceList
        {
            public string ID { get; set; }
            public string Number { get; set; }
            public string DivisionPlanID { get; set; }
            public string CreateDate { get; set; }
            public string Creator { get; set; }
            public bool Status { get; set; }
            public string StatusDate { get; set; }
            public string Description { get; set; }
            public bool Incomplete { get; set; }
            public string IncompleteDesc { get; set; }
            public string Code { get; set; }
            public string Workshop { get; set; }
            public string Url { get; set; }
            public string Count { get; set; }
            public string Cost { get; set; }
        }
        // GET: Remittances
        public ActionResult DetailsIndex(int? id, string url, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbl_RemittanceDetails = db.tbl_RemittanceDetails.Where(r => r.RemittancesID == id).Include(r => r.Remittances).ToList();
            if (tbl_RemittanceDetails == null)
            {
                return HttpNotFound();
            }

            if (id != null)
                Session["id"] = id;
            if (!String.IsNullOrEmpty(url))
                Session["url"] = url;
            //
            if (id == null)
                id = (int?)Session["id"];
            if (String.IsNullOrEmpty(url))
                url = (string)Session["url"];
            //
            List<RemittanceDetails> remittancedetailslist = new List<RemittanceDetails>();

            foreach (var item in tbl_RemittanceDetails)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.Remittances.DivisionPlan.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    remittancedetailslist.Add(new RemittanceDetails
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
                        ID = item.ID,
                        CheckStatus = item.CheckStatus,
                        CkeckedUser = item.CkeckedUser,
                        CkeckedDate = item.CkeckedDate,
                        FinancialUser = item.FinancialUser,
                        FinancialDate = item.FinancialDate,
                        FinancialCheckStatus = item.FinancialCheckStatus,
                        ManagerCheckStatus = item.ManagerCheckStatus,
                        ManagerDate = item.ManagerDate,
                        ManagerUser = item.ManagerUser
                    });
                }

            };
            //
            if (tbl_RemittanceDetails.Count() > 0)
            {
                ViewBag.remittanceId = tbl_RemittanceDetails.FirstOrDefault().RemittancesID;
                ViewBag.DivisionPlanID = tbl_RemittanceDetails.FirstOrDefault().Remittances.DivisionPlanID;
            }
            else
            {
                ViewBag.remittanceId = id;
                ViewBag.DivisionPlanID = db.tbl_Remittances.Find(id).DivisionPlanID;
            }
            //if (string.IsNullOrEmpty(url))
            //{
            //    url = "RemittanceWithBOMFixed";
            //}
            //else
            //{
            //    url = "Remittance";
            //}

            ViewBag.url = url;
            ViewBag.message = message;
            ViewBag.locked = false;

            if (tbl_RemittanceDetails.Count() > 0)
                ViewBag.locked = true;
            //
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
                    var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null) && r.DivisionPlan.Send == true).Include(r => r.DivisionPlan).Include(r => r.User);
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
                var tbl_Remittances = db.tbl_Remittances.Where(r => (r.Status == false || r.Status == null && r.DivisionPlan.Send != false)).Include(r => r.DivisionPlan).Include(r => r.User);

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
                var tbl_Remittances = db.tbl_Remittances.Where(r => r.Incomplete == true).Include(r => r.DivisionPlan).Include(r => r.User).ToList();
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
                            Incomplete = item.Incomplete,
                            IncompleteDesc = item.IncompleteDesc
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
        [RBAC]
        public ActionResult Create()
        {
            var divisionItems = db.tbl_DivisionPlans.ToList();

            List<DivisionPlan> dropdownList = new List<DivisionPlan>();
            foreach (var item in divisionItems)
            {
                var remittanceItems = db.tbl_Remittances.Where(r => r.DivisionPlanID == item.ID).FirstOrDefault();
                if (remittanceItems == null)
                {
                    dropdownList.Add(new DivisionPlan
                    {
                        ID = item.ID,
                        Code = item.Code
                    });
                }
            }
            ViewBag.DivisionPlanID = new SelectList(dropdownList, "ID", "Code");
            return View();
        }

        // POST: Remittances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Number,DivisionPlanID,CreateDate,Creator,Status,StatusDate,Description")] Remittances remittances,
            DateTime? Date, string Vehicle, string PlateLeft, string PlateMiddle, string PlateRight, string PlateIR, string BillofLading, string Transferee, string Description2, double? CarryFare, string CarrierName)
        {
            remittances.CreateDate = DateTime.Now;
            remittances.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            RemittanceDetails remittancedetails = new RemittanceDetails();

            if (ModelState.IsValid)
            {
                db.tbl_Remittances.Add(remittances);
                db.SaveChanges();
                //ثبت اطلاعات بارنامه ارسال
                if (Date != null && (Vehicle != null || Vehicle != ""))
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
                    remittancedetails.CarryFare = CarryFare;
                    db.tbl_RemittanceDetails.Add(remittancedetails);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.DivisionPlanID = new SelectList(db.tbl_DivisionPlans, "ID", "Code", remittances.DivisionPlanID);
            return View(remittances);
        }

        public ActionResult AddRemittanceDetails(int? id, string url)
        {
            ViewBag.url = url;
            ViewBag.workshopTitle = db.tbl_Remittances.Where(r => r.ID == id).SingleOrDefault().DivisionPlan.Workshop.Title;
            //ViewBag.workshopTitle = db.tbl_DivisionPlans.Where(d => d.ID == id).Include(d => d.Workshop).SingleOrDefault().Workshop.Title;
            ViewBag.remittanceId = id;
            return PartialView();
        }

        // POST: Remittances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRemittanceDetails(int? remittanceId, DateTime? Date, string Vehicle, string PlateLeft, string PlateMiddle, string PlateRight, string PlateIR, string BillofLading, string Transferee, string Description2, string CarryFare, string CarrierName, HttpPostedFileBase Attachment, string url)
        {
            var existRemittance = db.tbl_RemittanceDetails.Where(r => r.RemittancesID == remittanceId).ToList();
            string message = "";
            //
            if (existRemittance.Count() == 0)
            {
                RemittanceDetails remittancedetails = new RemittanceDetails();
                remittancedetails.BillofLading = BillofLading;
                remittancedetails.CreateDate = DateTime.Now;
                remittancedetails.Creator = User.Identity.Name;
                remittancedetails.Date = Date;
                remittancedetails.Description = Description2;
                //remittancedetails.Plate = PlateLeft + " " + PlateMiddle + " " + PlateRight + " | ایران " + PlateIR;
                remittancedetails.Plate = PlateRight + " " + PlateMiddle + " " + PlateLeft + " | ایران " + PlateIR;
                remittancedetails.RemittancesID = remittanceId;
                remittancedetails.Transferee = Transferee;
                remittancedetails.Vehicle = Vehicle;
                remittancedetails.CarrierName = CarrierName;
                remittancedetails.CarryFare = double.Parse(CarryFare.Replace(",", "").Replace(".", "").Replace("/", ""), System.Globalization.CultureInfo.InvariantCulture);
                if (Attachment != null)
                {
                    if (remittancedetails.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Remittance/" + remittancedetails.Attachment));
                    }
                    remittancedetails.Attachment = remittancedetails.Creator + "_" + remittancedetails.RemittancesID + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/Remittance/" + remittancedetails.Attachment);
                    Attachment.SaveAs(ImagePath);
                }

                db.tbl_RemittanceDetails.Add(remittancedetails);
                db.SaveChanges();
                //
                if (remittanceId == null)
                    remittanceId = remittancedetails.RemittancesID;
            }
            else
            {
                message = "شما مجاز به ثبت تنها یک ردیف بارنامه می باشید!";
            }
            return RedirectToAction("DetailsIndex", new { id = remittanceId, message = message, url = url });
        }

        [RBAC]
        // GET: Remittances/Edit/5
        public ActionResult Edit(int? id)
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
            return View(remittances);
        }

        // POST: Remittances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Number,DivisionPlanID,CreateDate,Creator,Status,StatusDate,Description")] Remittances remittances)
        {
            remittances.CreateDate = DateTime.Now;
            remittances.Creator = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

            if (ModelState.IsValid)
            {
                db.Entry(remittances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisionPlanID = new SelectList(db.tbl_DivisionPlans, "ID", "Code", remittances.DivisionPlanID);
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
        public ActionResult Confirmation(Remittances remittances, bool? Incomplete, string IncompleteDesc)
        {
            remittances.Status = true;
            remittances.StatusDate = DateTime.Now;
            remittances.Incomplete = Incomplete;
            remittances.IncompleteDesc = IncompleteDesc;

            db.Entry(remittances).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        [RBAC]
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
        public ActionResult DetailsByWorkshop(int? WorkshopID, string url, string message)
        {
            if (WorkshopID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbl_RemittanceDetails = db.tbl_RemittanceDetails.Where(r => r.Remittances.DivisionPlan.WorkshopID == WorkshopID).Include(r => r.Remittances).OrderByDescending(r => r.CreateDate).ToList();
            if (tbl_RemittanceDetails == null)
            {
                return HttpNotFound();
            }

            List<RemittanceDetails> remittancedetailslist = new List<RemittanceDetails>();

            foreach (var item in tbl_RemittanceDetails)
            {
                Workshop workshop = db.tbl_Workshops.Find(item.Remittances.DivisionPlan.WorkshopID);
                User _user = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId());

                if (workshop.Users.Contains(_user))
                {
                    remittancedetailslist.Add(new RemittanceDetails
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
                        ID = item.ID,
                        CheckStatus=item.CheckStatus,
                        FinancialUser=item.FinancialUser,
                        FinancialDate=item.FinancialDate,
                        CkeckedUser=item.CkeckedUser,
                        CkeckedDate=item.CkeckedDate,
                        ManagerUser=item.ManagerUser,
                        ManagerDate=item.ManagerDate,
                        ManagerCheckStatus=item.ManagerCheckStatus,
                        FinancialCheckStatus=item.FinancialCheckStatus
                    });
                }
            };
            //
            if (tbl_RemittanceDetails.Count() > 0)
            {
                //ViewBag.remittanceId = tbl_RemittanceDetails.FirstOrDefault().RemittancesID;
                //ViewBag.DivisionPlanID = tbl_RemittanceDetails.FirstOrDefault().Remittances.DivisionPlanID;
                ViewBag.SumCarryFare = remittancedetailslist.Sum(s => s.CarryFare).Value.ToString("#,##");
            }
            else
            {
                //ViewBag.remittanceId = id;
                //ViewBag.DivisionPlanID = db.tbl_Remittances.Find(id).DivisionPlanID;
                ViewBag.SumCarryFare = "0";
            }
            //if (string.IsNullOrEmpty(url))
            //{
            //    url = "RemittanceWithBOMFixed";
            //}
            //else
            //{
            //    url = "Remittance";
            //}

            ViewBag.url = url;
            ViewBag.message = message;
            ViewBag.locked = false;

            if (tbl_RemittanceDetails.Count() > 0)
                ViewBag.locked = true;
            //
            return View(remittancedetailslist);
        }
        //

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
                        City = item.City
                    });
                }

            };
            return View(list.ToList());
        }
        //
        //
        [HttpGet]
        public ActionResult CheckStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.tbl_RemittanceDetails.Where(u => u.ID == id).Include(u => u.Remittances).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return PartialView(item);
        }
        //
        [HttpPost]
        public ActionResult CheckStatus(int? id, bool Status)
        {
            RemittanceDetails item = db.tbl_RemittanceDetails.Where(u => u.ID == id).SingleOrDefault();
            item.CheckStatus = Status;
            item.CkeckedDate = DateTime.Now;
            item.CkeckedUser = User.Identity.Name;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            //
            if (id == null)
                id = item.RemittancesID;
            //
            return RedirectToAction("DetailsIndex", new { id = item.RemittancesID });
        }
        //
        [HttpGet]
        public ActionResult CheckFinancialStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.tbl_RemittanceDetails.Where(u => u.ID == id).Include(u => u.Remittances).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return PartialView(item);
        }
        //
        [HttpPost]
        public ActionResult CheckFinancialStatus(int? id, bool Status)
        {
            RemittanceDetails item = db.tbl_RemittanceDetails.Where(u => u.ID == id).SingleOrDefault();
            item.FinancialDate = DateTime.Now;
            item.FinancialUser = User.Identity.Name;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            //
            if (id == null)
                id = item.RemittancesID;
            //
            return RedirectToAction("DetailsIndex", new { id = item.RemittancesID });
        }
        //
        [HttpGet]
        public ActionResult CheckManagerStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.tbl_RemittanceDetails.Where(u => u.ID == id).Include(u => u.Remittances).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return PartialView(item);
        }
        //
        [HttpPost]
        public ActionResult CheckManagerStatus(int? id, bool Status)
        {
            RemittanceDetails item = db.tbl_RemittanceDetails.Where(u => u.ID == id).SingleOrDefault();
            item.ManagerDate = DateTime.Now;
            item.ManagerUser = User.Identity.Name;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            //
            if (id == null)
                id = item.RemittancesID;
            //
            return RedirectToAction("DetailsIndex", new { id = item.RemittancesID });
        }
        //
        [HttpPost]
        public ActionResult UpdateCheckStatus(int id, bool isCompleted)
        {
            // مثلاً وضعیت را در دیتابیس آپدیت کنید
            var task = db.tbl_RemittanceDetails.Find(id);
            if (task != null)
            {
                task.CheckStatus = isCompleted;
                task.CkeckedDate = DateTime.Now;
                task.CkeckedUser = User.Identity.Name;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //
        [HttpPost]
        public ActionResult UpdateFinancialStatus(int id, bool isCompleted)
        {
            // مثلاً وضعیت را در دیتابیس آپدیت کنید
            var task = db.tbl_RemittanceDetails.Find(id);
            if (task != null)
            {
                task.FinancialCheckStatus = isCompleted;
                task.FinancialDate = DateTime.Now;
                task.FinancialUser = User.Identity.Name;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //
        [HttpPost]
        public ActionResult UpdateManagerStatus(int id, bool isCompleted)
        {
            // مثلاً وضعیت را در دیتابیس آپدیت کنید
            var task = db.tbl_RemittanceDetails.Find(id);
            if (task != null)
            {
                task.ManagerCheckStatus = isCompleted;
                task.ManagerDate = DateTime.Now;
                task.ManagerUser = User.Identity.Name;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
