using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    //[RBAC]
    [Authorize]
    public class DivisionPlansController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbstatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        //check exist kit when add new division
        public JsonResult CheckexistKit(int? id,int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_KitDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.VehicleTypeID == id).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //check exist tank when add new division
        public JsonResult CheckexistTank(int? id, int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_TankDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.TypeofTankID == id).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //check exist tank base when add new division
        public JsonResult CheckexistTankBase(int? id, int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_TankBaseDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.TypeofTankBaseID == id).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //check exist valve when add new division
        public JsonResult CheckexistValve(string Type, int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_ValveDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.Type.Equals(Type)).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //check exist tank cover when add new division
        public JsonResult CheckexistTankCover(int? id, int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_TankCoverDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.TypeofTankCoverID == id).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //check exist otherthings when add new division
        public JsonResult CheckexistOtherthings(int? id, int? DivisionPlanID)
        {
            string status = "false";
            var Checkexist = db.tbl_OtherThingsDivisionPlans.Where(k => k.DivisionPlanID == DivisionPlanID && k.DiThingsID == id).ToList();
            if (Checkexist != null && Checkexist.Count != 0)
                status = "true";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        // GET: DivisionPlans
        public ActionResult Index()
        {
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }
        
        #region(بخش اصلی و پایه طرح تقسیم)       

        /// <summary>
        /// مشاهده لیست بخش مشترک طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DivisionPlan_Old()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            //DivisionPlan divisionplanlist = new Models.DivisionPlan();
            List<DivisionPlanTable> divisionplanlist = new List<DivisionPlanTable>();
            //var result = string.Join(", ", IDs);
            var divisionplan = db.tbl_DivisionPlans.Include(d=>d.Workshop).OrderByDescending(d => d.CreateDate).ToList();
            var userrole = Helper.Helpers.GetCurrentUserRole();

            if(userrole.Contains("مرکز خدمات (کارگاه)"))
            {
                foreach (var item in divisionplan.Where(d => d.Send == true))
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                    User _user = db.tbl_Users.Find(user.UserID);

                    if (workshop.Users.Contains(_user))
                    {
                        divisionplanlist.Add(new DivisionPlanTable
                        {
                            ID = item.ID.ToString(),
                            Code = item.Code,
                            Creator = GetUserName(item.Creator.ToString()),
                            WorkshopTitle = item.Workshop.Title,
                            CreateDate = item.CreateDate,
                            Confirmation = item.Confirmation,
                            ConfirmationDate = item.ConfirmationDate,
                            ConfirmationUser = item.ConfirmationUser.ToString(), //db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Lastname,
                            Send = item.Send,
                            SendDate = item.SendDate,
                            Sender = item.Sender.ToString(),//db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Lastname,
                            Description = item.Description,
                            FinalCheck = item.FinalCheck

                        });
                    }
                }
                ViewBag.divisionplanlist = divisionplanlist;
            }
            else
            {
                foreach (var item in divisionplan)
                {
                    Workshop workshop = db.tbl_Workshops.Find(item.WorkshopID);
                    User _user = db.tbl_Users.Find(user.UserID);
                    if (workshop.Users.Contains(_user))
                    {
                        divisionplanlist.Add(new DivisionPlanTable
                        {
                            ID = item.ID.ToString(),
                            Code = item.Code,
                            Creator = GetUserName(item.Creator.ToString()),
                            WorkshopTitle = item.Workshop.Title,
                            CreateDate = item.CreateDate,
                            Confirmation = item.Confirmation,
                            ConfirmationDate = item.ConfirmationDate,
                            ConfirmationUser = item.ConfirmationUser.ToString(), //db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Lastname,
                            Send = item.Send,
                            SendDate = item.SendDate,
                            Sender = item.Sender.ToString(),//db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Lastname,
                            Description = item.Description,
                            FinalCheck = item.FinalCheck

                        });
                    }
                }
                ViewBag.divisionplanlist = divisionplanlist;
            }
            //return PartialView(divisionplanlist);
            return PartialView();
        }
        //
        public ActionResult DivisionPlan()
        {
            var divisionPlan = db.tbl_DivisionPlans.Include(i => i.Workshop).ToList();
            var workshop = db.tbl_Workshops.ToList();
            List<DivisionPlanTable> TableOuts = new List<DivisionPlanTable>();
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
            string Code = "";
            string WorkshopTitle = "";
            DateTime? CreateDate = null;
            string Creator = "";
            bool? Confirmation = false;
            string ConfirmationUser = "";
            bool? Send = false;
            DateTime? SendDate =null;
            string Sender = "";
            string Description = "";
            DateTime? ConfirmationDate = null;
            bool? FinalCheck = false;
            DateTime? FinalCheckDate = null;
            string ExistBOM = "Preview";

            //
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_DivisionPlan]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Workshops", SqlDbType.VarChar).Value = workshops.TrimEnd(',');

                    conn.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ID = reader["ID"].ToString();
                        Code= reader["Code"].ToString();
                        WorkshopTitle = reader["WorkshopTitle"].ToString();
                        CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                        Creator = reader["Creator"].ToString();
                        Confirmation =Convert.ToBoolean(reader["Confirmation"].ToString());
                        ConfirmationUser = reader["ConfirmationUser"].ToString();
                        Send = Convert.ToBoolean(reader["Send"].ToString());
                        if (!string.IsNullOrEmpty(reader["SendDate"].ToString()))
                            SendDate = Convert.ToDateTime(reader["SendDate"].ToString());
                        else
                            SendDate = null;                        
                        Sender = reader["Sender"].ToString();
                        Description = reader["Description"].ToString();
                        if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                            ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"].ToString());
                        else
                            ConfirmationDate = null;                        
                        FinalCheck = Convert.ToBoolean(reader["FinalCheck"].ToString());
                        if (!string.IsNullOrEmpty(reader["FinalCheckDate"].ToString()))
                            FinalCheckDate = Convert.ToDateTime(reader["FinalCheckDate"].ToString());
                        else
                            FinalCheckDate = null;
                        ExistBOM = reader["ExistBOM"].ToString();

                        TableOuts.Add(new DivisionPlanTable
                        {
                            Code=Code,
                            Confirmation=Confirmation,
                            ConfirmationDate=ConfirmationDate,
                            ConfirmationUser=ConfirmationUser,
                            CreateDate=CreateDate.GetValueOrDefault(),
                            Creator=Creator,
                            Description=Description,
                            ExistBOM=ExistBOM,
                            FinalCheck=FinalCheck,
                            ID=ID,
                            Send=Send,
                            SendDate=SendDate,
                            Sender=Sender,
                            WorkshopTitle=WorkshopTitle                            
                            
                        });
                    }                   
                    //
                    conn.Close();
                }
            }//end using
            //

            ViewBag.TableOut = TableOuts;

            return PartialView(divisionPlan);
        }
        //
        public static string GetExistBOM(int? id)
        {
            var existBOM = dbstatic.tbl_DivisionPlanBOMs.Where(d=>d.DivisionPlanID==id).ToList();
            if (existBOM.Count()>0)
                return "PreviewBOM";
            else
                return "Preview";
        }
        //
        public static string GetUserName(string id)
        {
            var username = dbstatic.tbl_Users.Find(Convert.ToInt32(id));
            return username.Firstname + " " + username.Lastname;
        }

        //WarehouseKeeper
        public ActionResult WarehouseKeeper()
        {
            List<DivisionPlanTable> divisionplanlist = new List<DivisionPlanTable>();
            var divisionplan = db.tbl_DivisionPlans.Where(d=>(d.Confirmation == true) && (d.Send == false || d.Send == null)).Include(d => d.Workshop).OrderByDescending(d => d.CreateDate).ToList();
            //          
            foreach (var item in divisionplan)
            {
                divisionplanlist.Add(new DivisionPlanTable
                {
                    ID = item.ID.ToString(),
                    Code = item.Code,
                    Creator =GetUserName(item.Creator.ToString()) ,//db.tbl_Users.Where(u => u.UserID == item.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Creator).SingleOrDefault().Lastname,
                    WorkshopTitle = item.Workshop.Title,
                    CreateDate = item.CreateDate,
                    Confirmation = item.Confirmation,
                    ConfirmationDate = item.ConfirmationDate,
                    ConfirmationUser = item.ConfirmationUser.ToString(), //db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Lastname,
                    Send = item.Send,
                    SendDate = item.SendDate,
                    Sender = item.Sender.ToString(),//db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Lastname,
                    Description = item.Description,
                    FinalCheck=item.FinalCheck

                });
            }
            ViewBag.divisionplanlist = divisionplanlist;
            //return PartialView(divisionplanlist);
            return PartialView();
        }

        //ChangedManager
        public ActionResult ChangedManager()
        {
            List<DivisionPlanTable> divisionplanlist = new List<DivisionPlanTable>();
            var divisionplan = db.tbl_DivisionPlans.Where(d => (d.FinalCheck == true) && (d.Confirmation == false || d.Confirmation == null)).Include(d => d.Workshop).OrderByDescending(d => d.CreateDate).ToList();
            //          
            foreach (var item in divisionplan)
            {
                divisionplanlist.Add(new DivisionPlanTable
                {
                    ID = item.ID.ToString(),
                    Code = item.Code,
                    Creator = GetUserName(item.Creator.ToString()),//db.tbl_Users.Where(u => u.UserID == item.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Creator).SingleOrDefault().Lastname,
                    WorkshopTitle = item.Workshop.Title,
                    CreateDate = item.CreateDate,
                    Confirmation = item.Confirmation,
                    ConfirmationDate = item.ConfirmationDate,
                    ConfirmationUser = item.ConfirmationUser.ToString(), //db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.ConfirmationUser).SingleOrDefault().Lastname,
                    Send = item.Send,
                    SendDate = item.SendDate,
                    Sender = item.Sender.ToString(),//db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == item.Sender).SingleOrDefault().Lastname,
                    Description = item.Description,
                    FinalCheck=item.FinalCheck

                });
            }
            ViewBag.divisionplanlist = divisionplanlist;
            //return PartialView(divisionplanlist);
            return PartialView();
        }
        //
        public class DivisionPlanTable
        {
            public string ID { get; set; }
            public string Code { get; set; }
            public string WorkshopTitle { get; set; }
            public string Creator { get; set; }
            public DateTime CreateDate { get; set; }
            public bool? Confirmation { get; set; }
            public DateTime? ConfirmationDate { get; set; }
            public string ConfirmationUser { get; set; }
            public bool? Send { get; set; }
            public DateTime? SendDate { get; set; }
            public string Sender { get; set; }
            public string Description { get; set; }
            public bool? FinalCheck { get; set; }
            public string ExistBOM { get; set; }
        }
        //
        public ActionResult AddDivisionPlanType()
        {
            return PartialView();
        }
        /// <summary>
        /// ایجاد ردیف مشترک اطلاعات در طرح تقسیم
        /// </summary>
        /// <param name="divisionplan"></param>
        /// <returns></returns>
        public ActionResult AddDivisionPlan()
        {
            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            var tbl_Users = db.tbl_Users.ToList();
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            List<WorkshopsList> tableOuts = new List<WorkshopsList>();

            foreach (var item in tbl_Workshops.Where(w=>w.isServices==true).ToList())
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(userId.UserID);

                if (workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new WorkshopsList
                    {
                        id = item.ID,
                        title=item.Title
                    });
                }
            }
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            return View();
        }
        public class WorkshopsList
        {
            public int id { get; set; }
            public string title { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDivisionPlan([Bind(Include = "ID,Code,WorkshopID,Description")] DivisionPlan divisionplan)
        {
            if(divisionplan.WorkshopID != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                    var FapCode = db.tbl_Workshops.Where(w => w.ID == divisionplan.WorkshopID).SingleOrDefault().FapCode;
                    int Count = db.tbl_DivisionPlans.Where(v => v.WorkshopID == divisionplan.WorkshopID).Count() + 1;
                    string MaxofRow = "1";
                    var hasRow = db.tbl_DivisionPlans.ToList();

                    //if (hasRow.Count >= 1)
                    //    MaxofRow = db.tbl_DivisionPlans.Max(d => d.ID
                    if (Count < 10)
                        MaxofRow = "00" + Count;
                    if (Count >= 10 && Count < 100)
                        MaxofRow = "0" + Count;
                    if (Count >= 100)
                        MaxofRow = Count.ToString();

                    divisionplan.Creator = user.UserID;

                    //if(divisionplan.CreateDate==null)
                    divisionplan.CreateDate = DateTime.Now;

                    //divisionplan.Code = FapCode + "" + Count + "" + MaxofRow;
                    divisionplan.Code = FapCode + "-" + MaxofRow;

                    db.tbl_DivisionPlans.Add(divisionplan);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = divisionplan.ID });
                }

            }

            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return View("Index",divisionplan);
        }

        public ActionResult CreateDivisionPlanBOM([Bind(Include = "ID,Code,WorkshopID,Description")] DivisionPlan divisionplan)
        {
            if (divisionplan.WorkshopID != null)
            {
                if (ModelState.IsValid)
                {
                    var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                    var FapCode = db.tbl_Workshops.Where(w => w.ID == divisionplan.WorkshopID).SingleOrDefault().FapCode;
                    int Count = db.tbl_DivisionPlans.Where(v => v.WorkshopID == divisionplan.WorkshopID).Count() + 1;
                    string MaxofRow = "1";
                    var hasRow = db.tbl_DivisionPlans.ToList();

                    //if (hasRow.Count >= 1)
                    //    MaxofRow = db.tbl_DivisionPlans.Max(d => d.ID
                    if (Count < 10)
                        MaxofRow = "00" + Count;
                    if (Count >= 10 && Count < 100)
                        MaxofRow = "0" + Count;
                    if (Count >= 100)
                        MaxofRow = Count.ToString();

                    divisionplan.Creator = user.UserID;

                    //if(divisionplan.CreateDate==null)
                    divisionplan.CreateDate = DateTime.Now;

                    //divisionplan.Code = FapCode + "" + Count + "" + MaxofRow;
                    divisionplan.Code = FapCode + "-" + MaxofRow;

                    db.tbl_DivisionPlans.Add(divisionplan);
                    db.SaveChanges();
                    return RedirectToAction("DivisionPlanwithBOMs", new { id = divisionplan.ID, code= divisionplan.Code });
                }
            }

            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return View("Index", divisionplan);
        }

        /// <summary>
        /// ویرایش بخش مشترک طرح تقسیم
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Confirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return PartialView(divisionplan);
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation(DivisionPlan divisionplanlist)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(divisionplanlist.ID);
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            //
            //if (divisionplan.Confirmation == true)
            //if(type=="1")
            //{
                divisionplan.ConfirmationDate = DateTime.Now;
                divisionplan.ConfirmationUser = user.UserID;
                divisionplan.Confirmation = true;
                divisionplan.Description = divisionplanlist.Description;
            //}
            //
            //if (divisionplan.Send == true)
            //if (type == "2")
            //{
            //    divisionplan.SendDate = DateTime.Now;
            //    divisionplan.Sender = user.UserID;
            //}
            //
            if (ModelState.IsValid)
            {
                db.Entry(divisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PreviewBOM",new { id = divisionplan.ID });
            }
            //ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return View(divisionplan);
        }
        //
        public ActionResult Sendsubmit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.ConfirmationStatus = false;
            if (divisionplan.ConfirmationDate!=null)
            {
                ViewBag.ConfirmationStatus = true;
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return PartialView(divisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sendsubmit(DivisionPlan divisionplanlist, string type)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(divisionplanlist.ID);
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            //
            
            divisionplan.SendDate = DateTime.Now;
            divisionplan.Sender = user.UserID;
            divisionplan.Send = true;
            divisionplan.Description = divisionplanlist.Description;
            //
            if (ModelState.IsValid)
            {
                db.Entry(divisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PreviewBOM", new { id = divisionplan.ID });
            }
            //ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", divisionplan.WorkshopID);
            return View(divisionplan);
        }

        // GET: DivisionPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            ViewBag.DivisionPlanID = divisionplan.ID;
            ViewBag.Code = divisionplan.Code;
            ViewBag.Creator = db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Lastname;
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return View(divisionplan);
        }

        // GET: DivisionPlans/Preview/5
        public ActionResult Preview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var workshopId = db.tbl_Users.Where(u => u.UserID == userId).SingleOrDefault().WorkshopID;
            var existInList = db.tbl_DivisionPlans.Where(v => v.ID == id);

            List<Workshop> list = new List<Workshop>();

            foreach (var item in existInList)
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

            if (list.Count() == 0)
                return RedirectToAction("Page403", "Home");
            //
           
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            ViewBag.DivisionPlanID = divisionplan.ID;
            ViewBag.Code = divisionplan.Code;
            ViewBag.CreateDate = divisionplan.CreateDate;
            ViewBag.Creator = db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Lastname;
            if(divisionplan.ConfirmationUser!=null)
                ViewBag.ConfirmationUser = db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Lastname;
            if (divisionplan.Sender != null)
                ViewBag.Sender = db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Lastname;
            ViewBag.Workshop = divisionplan.Workshop.Title;
            ViewBag.kit = db.tbl_KitDivisionPlans.Include(K=>K.VehicleType).Include(K => K.Genarations).Include(K => K.RegistrationType).Where(k=>k.DivisionPlanID==id).ToList();
            ViewBag.Tank = db.tbl_TankDivisionPlans.Include(T => T.TypeofTank).Include(T => T.TankConstractor).Include(K => K.Genarations).Include(K => K.RegistrationType).Where(T => T.DivisionPlanID == id).ToList();
            ViewBag.TankBase = db.tbl_TankBaseDivisionPlans.Include(B => B.TypeofTankBase).Include(K => K.Genarations).Include(K => K.RegistrationType).Where(B => B.DivisionPlanID == id).ToList();
            ViewBag.Valve = db.tbl_ValveDivisionPlans.Include(V => V.ValveConstractor).Include(K => K.Genarations).Include(K => K.RegistrationType).Where(V => V.DivisionPlanID == id).ToList();
            ViewBag.Cover = db.tbl_TankCoverDivisionPlans.Include(V => V.TypeofTankCover).Include(K => K.Genarations).Include(K => K.RegistrationType).Where(V => V.DivisionPlanID == id).ToList();
            ViewBag.Otherthings = db.tbl_OtherThingsDivisionPlans.Include(K => K.Genarations).Include(K => K.RegistrationType).Where(V => V.DivisionPlanID == id).ToList();

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return View(divisionplan);
        }

        // GET: DivisionPlans/PreviewBOM/5
        public ActionResult PreviewBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var workshopId = db.tbl_Users.Where(u => u.UserID == userId).SingleOrDefault().WorkshopID;
            var existInList = db.tbl_DivisionPlans.Where(v => v.ID == id);

            List<Workshop> list = new List<Workshop>();

            foreach (var item in existInList)
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

            if (list.Count() == 0)
                return RedirectToAction("Page403", "Home");
            //
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            ViewBag.DivisionPlanID = divisionplan.ID;
            var Remittances = db.tbl_Remittances.Include(r => r.DivisionPlan).Where(r => r.DivisionPlanID == divisionplan.ID);
            if (Remittances != null && Remittances.Count() > 0)
            {
                var remittanceId = db.tbl_RemittanceDetails.Include(r => r.Remittances).Where(r => r.RemittancesID == Remittances.FirstOrDefault().ID);
                if (remittanceId != null && remittanceId.Count() > 0)
                    ViewBag.remittanceId = remittanceId.FirstOrDefault().ID;
            }
            ViewBag.Code = divisionplan.Code;
            ViewBag.CreateDate = divisionplan.CreateDate;
            ViewBag.Creator = db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Lastname;
            if (divisionplan.ConfirmationUser != null)
                ViewBag.ConfirmationUser = db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Lastname;
            if (divisionplan.Sender != null)
                ViewBag.Sender = db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Lastname;
            ViewBag.Workshop = divisionplan.Workshop.Title;
            ViewBag.kit = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id && b.BOM.EquipmentList.Pid==1).Include(b=>b.BOM).Include(b=>b.DivisionPlan).ToList();
            ViewBag.Tank = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id && b.BOM.EquipmentList.Pid == 2).Include(b => b.BOM).Include(b => b.DivisionPlan).ToList();
            ViewBag.count = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id).Count();

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return View(divisionplan);
        }
        // GET: DivisionPlans/Remittance/5
        public ActionResult Remittance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            ViewBag.DivisionPlanID = divisionplan.ID;
            ViewBag.Code = divisionplan.Code;
            ViewBag.CreateDate = divisionplan.CreateDate;
            ViewBag.Creator = db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Lastname;
            if (divisionplan.ConfirmationUser != null)
                ViewBag.ConfirmationUser = db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Lastname;
            if (divisionplan.Sender != null)
                ViewBag.Sender = db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Lastname;
            ViewBag.Workshop = divisionplan.Workshop.Title;
            ViewBag.kit = db.tbl_KitDivisionPlans.Include(K => K.VehicleType).Where(k => k.DivisionPlanID == id).ToList();
            ViewBag.Tank = db.tbl_TankDivisionPlans.Include(T => T.TypeofTank).Include(T => T.TankConstractor).Where(T => T.DivisionPlanID == id).ToList();
            ViewBag.TankBase = db.tbl_TankBaseDivisionPlans.Include(B => B.TypeofTankBase).Where(B => B.DivisionPlanID == id).ToList();
            ViewBag.Valve = db.tbl_ValveDivisionPlans.Include(V => V.ValveConstractor).Where(V => V.DivisionPlanID == id).ToList();
            ViewBag.Cover = db.tbl_TankCoverDivisionPlans.Include(V => V.TypeofTankCover).Where(V => V.DivisionPlanID == id).ToList();
            ViewBag.Otherthings = db.tbl_OtherThingsDivisionPlans.Where(V => V.DivisionPlanID == id).ToList();

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }
        // GET: DivisionPlans/EditPreviewBOM/5
        public ActionResult EditPreviewBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            ViewBag.DivisionPlanID = divisionplan.ID;
            var Remittances = db.tbl_Remittances.Include(r => r.DivisionPlan).Where(r => r.DivisionPlanID == divisionplan.ID);
            if (Remittances != null && Remittances.Count() > 0)
            {
                var remittanceId = db.tbl_RemittanceDetails.Include(r => r.Remittances).Where(r => r.RemittancesID == Remittances.FirstOrDefault().ID);
                if (remittanceId != null && remittanceId.Count() > 0)
                    ViewBag.remittanceId = remittanceId.FirstOrDefault().ID;
            }
            ViewBag.Code = divisionplan.Code;
            ViewBag.CreateDate = divisionplan.CreateDate;
            ViewBag.Creator = db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Creator).SingleOrDefault().Lastname;
            if (divisionplan.ConfirmationUser != null)
                ViewBag.ConfirmationUser = db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.ConfirmationUser).SingleOrDefault().Lastname;
            if (divisionplan.Sender != null)
                ViewBag.Sender = db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Firstname + " " + db.tbl_Users.Where(u => u.UserID == divisionplan.Sender).SingleOrDefault().Lastname;
            ViewBag.Workshop = divisionplan.Workshop.Title;
            ViewBag.kit = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id && b.BOM.EquipmentList.Pid == 1).Include(b => b.BOM).Include(b => b.DivisionPlan).ToList();
            ViewBag.Tank = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id && b.BOM.EquipmentList.Pid == 2).Include(b => b.BOM).Include(b => b.DivisionPlan).ToList();

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return View(divisionplan);
        }

        //GET: DivisionPlans/AddDivisionPlanBOM/5
        public ActionResult AddDivisionPlanBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            int? vehicleTypeID = null;
            var vehicleType = db.tbl_DivisionPlanBOMs.Where(b => b.DivisionPlanID == id).FirstOrDefault();

            if (vehicleType != null)
                vehicleTypeID = vehicleType.BOM.VehicleTypeID;

            ViewBag.DivisionPlanID = new SelectList(db.tbl_DivisionPlans, "ID", "Code",id);
            ViewBag.BOMID = new SelectList(db.tbl_BOMs.Where(b=>b.VehicleTypeID==vehicleTypeID), "ID", "EquipmentList.Title");

            return PartialView();
        }

        [HttpPost]
        public ActionResult AddDivisionPlanBOM(DivisionPlanBOM divisionplan,int? DivisionPlanID)
        {
            divisionplan.CreateDate = DateTime.Now;
            divisionplan.Creator = User.Identity.Name;

            db.tbl_DivisionPlanBOMs.Add(divisionplan);
            db.SaveChanges();

            //
            return RedirectToAction("EditPreviewBOM", new { id = divisionplan.DivisionPlanID });
        }

        //GET: DivisionPlans/EditDivisionPlanBOM/5
        public ActionResult EditDivisionPlanBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlanBOM divisionplan = db.tbl_DivisionPlanBOMs.Find(id);
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }

        [HttpPost]
        public ActionResult EditDivisionPlanBOM(DivisionPlanBOM divisionplan)
        {            
            //DivisionPlanBOM divisionplan = db.tbl_DivisionPlanBOMs.Find(Convert.ToInt32(id));
            //string msg = "تعداد: " + divisionplan.NumberofSend + "به: " + NumberofSend + " تغییر یافت .";
            //           
            divisionplan.Creator = User.Identity.Name;
            divisionplan.CreateDate = DateTime.Now;
            db.Entry(divisionplan).State = EntityState.Modified;
            db.SaveChanges();
            
            //
            return RedirectToAction("EditPreviewBOM", new { id=divisionplan.DivisionPlanID});
        }

        //GET: DivisionPlans/DeleteDivisionPlanBOM/5
        public ActionResult DeleteDivisionPlanBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlanBOM divisionplan = db.tbl_DivisionPlanBOMs.Find(id);
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }

        [HttpPost, ActionName("DeleteDivisionPlanBOM")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDivisionPlanBOMConfirmed(int id)
        {
            DivisionPlanBOM divisionplan = db.tbl_DivisionPlanBOMs.Find(id);
            //     
            int? DivisionPlanID = divisionplan.DivisionPlanID;
            db.tbl_DivisionPlanBOMs.Remove(divisionplan);
            db.SaveChanges();           
            //
            return RedirectToAction("EditPreviewBOM", new { id = DivisionPlanID });
        }
        #endregion

        #region(شیر مخزن)
        /// <summary>
        /// مشاهده لیست طرح تقسیم بخش شیر مخزن
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ValveDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_ValveDivisionPlans.Where(v => v.DivisionPlanID == id);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// افزودن اطلاعات شیر مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoValveDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve");

            return PartialView();
        }

        /// <summary>
        /// افزودن اطلاعات شیر مخزن در طرح تقسیم
        /// </summary>
        /// <param name="valvedivisionplan"></param>
        /// <param name="DivisionPlanID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoValveDivisionPlans([Bind(Include = "ID,Type,ValveConstractorID,Number,Description")] ValveDivisionPlan valvedivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_ValveDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.Type == valvedivisionplan.Type).ToList();

            if (ModelState.IsValid)
            {
                if (existItem.Count==0)
                {
                    valvedivisionplan.NumberofSend = valvedivisionplan.Number;
                    valvedivisionplan.DivisionPlanID = DivisionPlanID;
                    db.tbl_ValveDivisionPlans.Add(valvedivisionplan);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = DivisionPlanID });
            }
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", valvedivisionplan.ValveConstractorID);
            return View(valvedivisionplan);
        }

        /// <summary>
        /// ویرایش اطلاعات شیر مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateValveDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValveDivisionPlan valvedivisionplan = db.tbl_ValveDivisionPlans.Find(id);
            if (valvedivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = valvedivisionplan.DivisionPlan.Code;
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", valvedivisionplan.ValveConstractorID);

            return PartialView(valvedivisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateValveDivisionPlans([Bind(Include = "ID,Type,ValveConstractorID,Number,NumberofSend,Description,DivisionPlanID")] ValveDivisionPlan valvedivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(valvedivisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = valvedivisionplan.DivisionPlanID });
            }
            //
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", valvedivisionplan.ValveConstractorID);
            return PartialView(valvedivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات شیر مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteValveDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValveDivisionPlan valvedivisionplan = db.tbl_ValveDivisionPlans.Find(id);
            if (valvedivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = valvedivisionplan.DivisionPlan.Code;
            ViewBag.ValveConstractorID = new SelectList(db.tbl_ValveConstractors, "ID", "Valve", valvedivisionplan.ValveConstractorID);

            return PartialView(valvedivisionplan);
        }

        [HttpPost, ActionName("DeleteValveDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteValveDivisionPlansConfirmed(int id)
        {
            ValveDivisionPlan valvedivisionplan = db.tbl_ValveDivisionPlans.Find(id);
            int? DivisionPlanID = valvedivisionplan.DivisionPlanID;
            db.tbl_ValveDivisionPlans.Remove(valvedivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }

        #endregion

        #region(کیت)
        /// <summary>
        /// افزودن اطلاعات کیت در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoKitDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
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

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoKitDivisionPlans([Bind(Include = "ID,VehicleTypeID,Number,Description")] KitDivisionPlan kitdivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_KitDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.VehicleTypeID == kitdivisionplan.VehicleTypeID).ToList();
            var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == kitdivisionplan.VehicleTypeID && b.Presentable==true ).Include(b=>b.EquipmentList).ToList();
            DivisionPlanBOM addtoBOM = new DivisionPlanBOM();

            if (ModelState.IsValid)
            {
                if (existItem.Count==0)
                {
                    kitdivisionplan.DivisionPlanID = DivisionPlanID;
                    kitdivisionplan.NumberofSend = kitdivisionplan.Number;
                    db.tbl_KitDivisionPlans.Add(kitdivisionplan);
                    db.SaveChanges();
                    //--------------------------------------for add value in bom division plan--------------------------------------------
                    //foreach(var item in bomList)
                    //{
                    //    addtoBOM.DivisionPlanID = DivisionPlanID;
                    //    addtoBOM.Description = kitdivisionplan.Description;
                    //    addtoBOM.BOMID = item.ID;
                    //    addtoBOM.CreateDate = DateTime.Now;
                    //    addtoBOM.Creator = User.Identity.Name;
                    //    addtoBOM.Number= (item.Ratio.HasValue? item.Ratio.Value:1) * kitdivisionplan.Number;
                    //    addtoBOM.NumberofSend= (item.Ratio.HasValue ? item.Ratio.Value : 1) * kitdivisionplan.Number;
                    //    db.tbl_DivisionPlanBOMs.Add(addtoBOM);
                    //    db.SaveChanges();
                    //}
                }

                return RedirectToAction("Details", new { id = DivisionPlanID });
            }
            
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", kitdivisionplan.VehicleTypeID);
            return View(kitdivisionplan);
        }

        /// <summary>
        /// مشاهده لیست طرح تقسیم بخش کیت
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult KitDivisionPlans(string code, int? id,string roleName)
        {
            ViewBag.roleName = roleName;
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_KitDivisionPlans.Where(v => v.DivisionPlanID == id);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// ویرایش اطلاعات کیت در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateKitDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KitDivisionPlan kitdivisionplan = db.tbl_KitDivisionPlans.Find(id);
            if (kitdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = kitdivisionplan.DivisionPlan.Code;
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text", kitdivisionplan.VehicleTypeID);
            return PartialView(kitdivisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateKitDivisionPlans([Bind(Include = "ID,VehicleTypeID,Number,NumberofSend,Description,DivisionPlanID")] KitDivisionPlan kitdivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(kitdivisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = kitdivisionplan.DivisionPlanID });
            }
            //
            ViewBag.VehicleTypeID = new SelectList(db.tbl_VehicleTypes, "ID", "Type", kitdivisionplan.VehicleTypeID);
            return PartialView(kitdivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات کیت در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteKitDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KitDivisionPlan kitdivisionplan = db.tbl_KitDivisionPlans.Find(id);
            if (kitdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = kitdivisionplan.DivisionPlan.Code;
            return PartialView(kitdivisionplan);
        }

        [HttpPost, ActionName("DeleteKitDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteKitDivisionPlansConfirmed(int id)
        {
            KitDivisionPlan kitdivisionplan = db.tbl_KitDivisionPlans.Find(id);
            int? DivisionPlanID = kitdivisionplan.DivisionPlanID;
            db.tbl_KitDivisionPlans.Remove(kitdivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }

        #endregion

        #region(مخزن)

        /// <summary>
        /// مشاهده لیست طرح تقسیم بخش مخزن
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TankDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_TankDivisionPlans.Where(v => v.DivisionPlanID == id);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// افزودن اطلاعات مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoTankDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            ViewBag.TypeofTankID = new SelectList(db.tbl_TypeofTanks, "ID", "Type");
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor");

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoTankDivisionPlans([Bind(Include = "ID,TypeofTankID,TankConstractorID,Number,Description")] TankDivisionPlan tankdivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_TankDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.TypeofTankID == tankdivisionplan.TypeofTankID).ToList();
            //var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == tankdivisionplan.TypeofTank.VehicleTypeId && b.Presentable == true && b.EquipmentList.Pid == 2).Include(b => b.EquipmentList).ToList();
            //DivisionPlanBOM addtoBOM = new DivisionPlanBOM();

            if (ModelState.IsValid)
            {
                if (existItem.Count == 0)
                {
                    tankdivisionplan.NumberofSend = tankdivisionplan.Number;
                    tankdivisionplan.DivisionPlanID = DivisionPlanID;
                    db.tbl_TankDivisionPlans.Add(tankdivisionplan);
                    db.SaveChanges();
                    //--------------------------------------for add value in bom division plan--------------------------------------------
                    //foreach (var item in bomList)
                    //{
                    //    addtoBOM.DivisionPlanID = DivisionPlanID;
                    //    addtoBOM.BOMID = item.ID;
                    //    addtoBOM.Description = tankdivisionplan.Description;
                    //    addtoBOM.CreateDate = DateTime.Now;
                    //    addtoBOM.Creator = User.Identity.Name;
                    //    addtoBOM.Number = (item.Ratio.HasValue ? item.Ratio.Value : 1) * tankdivisionplan.Number;
                    //    addtoBOM.NumberofSend = (item.Ratio.HasValue ? item.Ratio.Value : 1) * tankdivisionplan.Number;
                    //    db.tbl_DivisionPlanBOMs.Add(addtoBOM);
                    //    db.SaveChanges();
                    //}
                }
                return RedirectToAction("Details", new { id = DivisionPlanID });
            }
          
            ViewBag.TypeofTankID = new SelectList(db.tbl_TypeofTanks, "ID", "Type", tankdivisionplan.TypeofTankID);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", tankdivisionplan.TankConstractorID);

            return View(tankdivisionplan);
        }

        /// <summary>
        /// ویرایش اطلاعات مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateTankDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankDivisionPlan tankdivisionplan = db.tbl_TankDivisionPlans.Find(id);
            if (tankdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankdivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = tankdivisionplan.DivisionPlanID;
            ViewBag.TypeofTankID = new SelectList(db.tbl_TypeofTanks, "ID", "Type", tankdivisionplan.TypeofTankID);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", tankdivisionplan.TankConstractorID);

            return PartialView(tankdivisionplan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTankDivisionPlans([Bind(Include = "ID,TypeofTankID,TankConstractorID,Number,NumberofSend,Description,DivisionPlanID")] TankDivisionPlan tankdivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tankdivisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tankdivisionplan.DivisionPlanID });
            }
            ViewBag.TypeofTankID = new SelectList(db.tbl_TypeofTanks, "ID", "Type", tankdivisionplan.TypeofTankID);
            ViewBag.TankConstractorID = new SelectList(db.tbl_TankConstractors, "ID", "Constractor", tankdivisionplan.TankConstractorID);

            return View(tankdivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteTankDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankDivisionPlan tankdivisionplan = db.tbl_TankDivisionPlans.Find(id);
            if (tankdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankdivisionplan.DivisionPlan.Code;
            return PartialView(tankdivisionplan);
        }

        [HttpPost, ActionName("DeleteTankDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTankDivisionPlansConfirmed(int id)
        {
            TankDivisionPlan tankdivisionplan = db.tbl_TankDivisionPlans.Find(id);
            int? DivisionPlanID = tankdivisionplan.DivisionPlanID;
            db.tbl_TankDivisionPlans.Remove(tankdivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }

        #endregion

        #region(پایه مخزن)

        /// <summary>
        /// مشاهده لیست طرح تقسیم بخش پایه مخزن
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TankBaseDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_TankBaseDivisionPlans.Where(v => v.DivisionPlanID == id);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// افزودن اطلاعات پایه مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoTankBaseDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            ViewBag.TypeofTankBaseID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type");

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoTankBaseDivisionPlans([Bind(Include = "ID,TypeofTankBaseID,Number,Description")] TankBaseDivisionPlan tankbasedivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_TankBaseDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.TypeofTankBaseID == tankbasedivisionplan.TypeofTankBaseID).ToList();

            if (ModelState.IsValid)
            {
                if (existItem.Count == 0 )
                {
                    tankbasedivisionplan.NumberofSend = tankbasedivisionplan.Number;
                    tankbasedivisionplan.DivisionPlanID = DivisionPlanID;
                    db.tbl_TankBaseDivisionPlans.Add(tankbasedivisionplan);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = DivisionPlanID });
            }

            ViewBag.TypeofTankBaseID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type", tankbasedivisionplan.TypeofTankBaseID);

            return View(tankbasedivisionplan);
        }

        /// <summary>
        /// ویرایش اطلاعات پایه مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateTankBaseDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankBaseDivisionPlan tankbasedivisionplan = db.tbl_TankBaseDivisionPlans.Find(id);
            if (tankbasedivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankbasedivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = tankbasedivisionplan.DivisionPlanID;
            ViewBag.TypeofTankBaseID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type", tankbasedivisionplan.TypeofTankBaseID);

            return PartialView(tankbasedivisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTankBaseDivisionPlans([Bind(Include = "ID,TypeofTankBaseID,Number,NumberofSend,Description,DivisionPlanID")] TankBaseDivisionPlan tankbasedivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tankbasedivisionplan).State=EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tankbasedivisionplan.DivisionPlanID });
            }
            ViewBag.TypeofTankBaseID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type", tankbasedivisionplan.TypeofTankBaseID);

            return View(tankbasedivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات پایه مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteTankBaseDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankBaseDivisionPlan tankbasedivisionplan = db.tbl_TankBaseDivisionPlans.Find(id);
            if (tankbasedivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankbasedivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = tankbasedivisionplan.DivisionPlanID;
            ViewBag.TypeofTankBaseID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type", tankbasedivisionplan.TypeofTankBaseID);

            return PartialView(tankbasedivisionplan);
        }

        [HttpPost, ActionName("DeleteTankBaseDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTankBaseDivisionPlansConfirmed(int id)
        {
            TankBaseDivisionPlan tankbasedivisionplan = db.tbl_TankBaseDivisionPlans.Find(id);
            int? DivisionPlanID = tankbasedivisionplan.DivisionPlanID;
            db.tbl_TankBaseDivisionPlans.Remove(tankbasedivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }

        #endregion

        #region(سایر اقلام)

        /// <summary>
        /// مشاهده لیست طرح تقسیم سایر اقلام
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OtherThingsDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_OtherThingsDivisionPlans.Where(v => v.DivisionPlanID == id).Include(v=>v.DiThings);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// افزودن اطلاعات سایر اقلام در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoOtherThingsDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            ViewBag.DiThingsID = new SelectList(db.tbl_Otherthings, "ID", "Title");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoOtherThingsDivisionPlans([Bind(Include = "ID,DiThingsID,Number,Description")] OtherThingsDivisionPlan otherthingsdivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_OtherThingsDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.DiThingsID == otherthingsdivisionplan.DiThingsID).ToList();

            if (ModelState.IsValid)
            {
                if (existItem.Count == 0)
                {
                    otherthingsdivisionplan.NumberofSend = otherthingsdivisionplan.Number;
                    otherthingsdivisionplan.DivisionPlanID = DivisionPlanID;
                    db.tbl_OtherThingsDivisionPlans.Add(otherthingsdivisionplan);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = DivisionPlanID });
            }
            ViewBag.DiThingsID = new SelectList(db.tbl_Otherthings, "ID", "Title",otherthingsdivisionplan.DiThingsID);
            return View(otherthingsdivisionplan);
        }

        /// <summary>
        /// ویرایش اطلاعات سایر اقلام در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateOtherThingsDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherThingsDivisionPlan otherthingsdivisionplan = db.tbl_OtherThingsDivisionPlans.Find(id);
            if (otherthingsdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = otherthingsdivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = otherthingsdivisionplan.DivisionPlanID;
            ViewBag.DiThingsID = new SelectList(db.tbl_Otherthings, "ID", "Title", otherthingsdivisionplan.DiThingsID);
            return PartialView(otherthingsdivisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOtherThingsDivisionPlans([Bind(Include = "ID,Title,Number,NumberofSend,Description,DivisionPlanID")] OtherThingsDivisionPlan otherthingsdivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(otherthingsdivisionplan).State=EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = otherthingsdivisionplan.DivisionPlanID });
            }
            ViewBag.DiThingsID = new SelectList(db.tbl_Otherthings, "ID", "Title", otherthingsdivisionplan.DiThingsID);
            return View(otherthingsdivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات سایر اقلام در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteOtherThingsDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherThingsDivisionPlan otherthingsdivisionplan = db.tbl_OtherThingsDivisionPlans.Find(id);
            if (otherthingsdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = otherthingsdivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = otherthingsdivisionplan.DivisionPlanID;
            return PartialView(otherthingsdivisionplan);
        }

        [HttpPost, ActionName("DeleteOtherThingsDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOtherThingsDivisionPlansConfirmed(int id)
        {
            OtherThingsDivisionPlan otherthingsdivisionplan = db.tbl_OtherThingsDivisionPlans.Find(id);
            int? DivisionPlanID = otherthingsdivisionplan.DivisionPlanID;
            db.tbl_OtherThingsDivisionPlans.Remove(otherthingsdivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }


        #endregion

        #region(کاور مخزن)

        /// <summary>
        /// مشاهده لیست طرح تقسیم بخش کاور مخزن
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TankCoverDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            var list = db.tbl_TankCoverDivisionPlans.Where(v => v.DivisionPlanID == id);
            return PartialView(list.ToList());
        }

        /// <summary>
        /// افزودن اطلاعات کاور مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult AddtoTankCoverDivisionPlans(string code, int? id)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            ViewBag.TypeofTankCoverID = new SelectList(db.tbl_TypeofTankCovers, "ID", "Type");

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddtoTankCoverDivisionPlans([Bind(Include = "ID,TypeofTankCoverID,Number,Description")] TankCoverDivisionPlan tankcoverdivisionplan, int? DivisionPlanID)
        {
            var existItem = db.tbl_TankCoverDivisionPlans.Where(v => v.DivisionPlanID == DivisionPlanID && v.TypeofTankCoverID == tankcoverdivisionplan.TypeofTankCoverID).ToList();

            if (ModelState.IsValid)
            {
                if (existItem.Count == 0)
                {
                    tankcoverdivisionplan.NumberofSend = tankcoverdivisionplan.Number;
                    tankcoverdivisionplan.DivisionPlanID = DivisionPlanID;
                    db.tbl_TankCoverDivisionPlans.Add(tankcoverdivisionplan);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = DivisionPlanID });
            }

            ViewBag.TypeofTankCoverID = new SelectList(db.tbl_TypeofTankBases, "ID", "Type", tankcoverdivisionplan.TypeofTankCoverID);

            return View(tankcoverdivisionplan);
        }

        /// <summary>
        /// ویرایش اطلاعات کاور مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateTankCoverDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankCoverDivisionPlan tankcoverdivisionplan = db.tbl_TankCoverDivisionPlans.Find(id);
            if (tankcoverdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankcoverdivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = tankcoverdivisionplan.DivisionPlanID;
            ViewBag.TypeofTankCoverID = new SelectList(db.tbl_TypeofTankCovers, "ID", "Type", tankcoverdivisionplan.TypeofTankCoverID);

            return PartialView(tankcoverdivisionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTankCoverDivisionPlans([Bind(Include = "ID,TypeofTankCoverID,Number,NumberofSend,Description,DivisionPlanID")] TankCoverDivisionPlan tankcoverdivisionplan)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tankcoverdivisionplan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = tankcoverdivisionplan.DivisionPlanID });
            }
            ViewBag.TypeofTankCoverID = new SelectList(db.tbl_TypeofTankCovers, "ID", "Type", tankcoverdivisionplan.TypeofTankCoverID);

            return View(tankcoverdivisionplan);
        }

        /// <summary>
        /// حذف اطلاعات کاور مخزن در طرح تقسیم
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteTankCoverDivisionPlans(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TankCoverDivisionPlan tankcoverdivisionplan = db.tbl_TankCoverDivisionPlans.Find(id);
            if (tankcoverdivisionplan == null)
            {
                return HttpNotFound();
            }
            //
            ViewBag.Code = tankcoverdivisionplan.DivisionPlan.Code;
            ViewBag.DivisionPlanID = tankcoverdivisionplan.DivisionPlanID;
            ViewBag.TypeofTankCoverID = new SelectList(db.tbl_TypeofTankCovers, "ID", "Type", tankcoverdivisionplan.TypeofTankCoverID);

            return PartialView(tankcoverdivisionplan);
        }

        [HttpPost, ActionName("DeleteTankCoverDivisionPlans")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTankCoverDivisionPlansConfirmed(int id)
        {
            TankCoverDivisionPlan tankcoverdivisionplan = db.tbl_TankCoverDivisionPlans.Find(id);
            int? DivisionPlanID = tankcoverdivisionplan.DivisionPlanID;
            db.tbl_TankCoverDivisionPlans.Remove(tankcoverdivisionplan);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = DivisionPlanID });
        }
        #endregion
        //
        public ActionResult OldDivisionPlanRem(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post,int? RegistrationTypeID,int? GenarationID)
        {
            if (RegistrationTypeID == null)
                RegistrationTypeID = 0;
            if (GenarationID == null)
                GenarationID = 0;
            if (WorkshopID != null)
                ViewBag.WorkshopsID = WorkshopID;
            //DateTime fdate = DateTime.Now;
            //int passYear = pc.GetYear(DateTime.Now)-1;
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            //string[] permission;
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
                        Title = item.Title + " - " + item.City.Title
                    });
                }
            }
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            ViewBag.RegistrationTypeID = new SelectList(db.tbl_RegistrationTypes, "ID", "Type");
            ViewBag.GenarationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
            //string filterItems="";
            //
            if (WorkshopID == null)
                if (cngfapco.Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
                    WorkshopID = db.tbl_Users.Find(cngfapco.Helper.Helpers.GetCurrentUserId()).WorkshopID;
                else
                    WorkshopID = 0;
            //else
            //{
            //    foreach(var item in WorkshopID)
            //    {
            //        filterItems += item.Value + ",";
            //    }
            //}
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string CylinderReturnofParts = "0";
            string KitReturnofParts = "0";
            string ValveReturnofParts = "0";
            string CylinderBaseReturnofParts = "0";
            string CylinderCoverReturnofParts = "0";

            List<KitRem> KitTableOuts = new List<KitRem>();
            List<TankRem> TankTableOuts = new List<TankRem>();
            List<TankBaseRem> TankBaseTableOuts = new List<TankBaseRem>();
            List<ValveRem> ValveTableOuts = new List<ValveRem>();
            List<TankCoverRem> TankCoverTableOuts = new List<TankCoverRem>();
            List<OtherthingsRem> OtherthingsTableOuts = new List<OtherthingsRem>();

            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlan]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshop", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                        command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@RegistrationType", SqlDbType.VarChar).Value = RegistrationTypeID;
                        command.Parameters.Add("@Genaration", SqlDbType.VarChar).Value = GenarationID;

                        conn.Open();
                        reader = command.ExecuteReader();
                        int SumofkSend = 0;
                        int SumofkUsed = 0;
                        int SumofkReturnofParts = 0;
                        //kit rem
                        while (reader.Read())
                        {                           
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofkSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofkUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            KitReturnofParts = reader["KitReturnofParts"].ToString();
                            SumofkReturnofParts+= Convert.ToInt32(KitReturnofParts);

                            KitTableOuts.Add(new KitRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofkSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofkUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                KitReturnofParts= KitReturnofParts
                            });
                        }
                        ViewBag.SumofkSend = SumofkSend;
                        ViewBag.SumofkUsed = SumofkUsed;
                        ViewBag.SumofkReturnofParts = SumofkReturnofParts;
                        ViewBag.SumofkRem = SumofkSend - SumofkUsed- SumofkReturnofParts;

                        //tank rem
                        reader.NextResult();
                        int SumoftSend = 0;
                        int SumoftUsed = 0;
                        int SumofReturnofParts = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumoftSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumoftUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            CylinderReturnofParts = reader["CylinderReturnofParts"].ToString();
                            SumofReturnofParts+= Convert.ToInt32(CylinderReturnofParts);

                            TankTableOuts.Add(new TankRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend= SumoftSend.ToString(),
                                Used = Used,
                                SumofUsed= SumoftUsed.ToString(),
                                Rem = Rem,
                                SumofRem=(SumoftSend-SumoftUsed).ToString(),
                                CylinderReturnofParts= CylinderReturnofParts

                            });
                        }
                        ViewBag.SumoftSend = SumoftSend;
                        ViewBag.SumoftUsed = SumoftUsed;
                        ViewBag.SumofReturnofParts = SumofReturnofParts;
                        ViewBag.SumoftRem = SumoftSend - SumoftUsed- SumofReturnofParts;

                        //valve rem
                        reader.NextResult();
                        int SumofvSend = 0;
                        int SumofvUsed = 0;
                        int SumofvReturnofParts = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofvSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofvUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            ValveReturnofParts = reader["ValveReturnofParts"].ToString();
                            SumofvReturnofParts += Convert.ToInt32(ValveReturnofParts);

                            ValveTableOuts.Add(new ValveRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofvSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofvUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofvSend - SumofvUsed).ToString(),
                                ValveReturnofParts = ValveReturnofParts
                            });
                        }
                        ViewBag.SumofvSend = SumofvSend;
                        ViewBag.SumofvUsed = SumofvUsed;
                        ViewBag.SumofvReturnofParts = SumofvReturnofParts;
                        ViewBag.SumofvRem = SumofvSend - SumofvUsed - SumofvReturnofParts;

                        //tank base rem
                        reader.NextResult();
                        int SumofbSend = 0;
                        int SumofbUsed = 0;
                        int SumofbReturnofParts = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofbSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofbUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            CylinderBaseReturnofParts = reader["CylinderBaseReturnofParts"].ToString();
                            SumofbReturnofParts+= Convert.ToInt32(CylinderBaseReturnofParts);

                            TankBaseTableOuts.Add(new TankBaseRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofbSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofbUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofbSend - SumofbUsed).ToString(),
                                CylinderBaseReturnofParts= CylinderBaseReturnofParts
                            });
                        }
                        ViewBag.SumofbSend = SumofbSend;
                        ViewBag.SumofbUsed = SumofbUsed;
                        ViewBag.SumofbReturnofParts = SumofbReturnofParts;
                        ViewBag.SumofbRem = SumofbSend - SumofbUsed- SumofbReturnofParts;
                        
                        //Tank Cover rem
                        reader.NextResult();
                        int SumofcSend = 0;
                        int SumofcUsed = 0;
                        int SumofcReturnofParts = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofcSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofcUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            CylinderCoverReturnofParts = reader["CylinderCoverReturnofParts"].ToString();
                            SumofcReturnofParts+= Convert.ToInt32(CylinderCoverReturnofParts);

                            TankCoverTableOuts.Add(new TankCoverRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofcSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofcUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofcSend - SumofcUsed).ToString(),
                                CylinderCoverReturnofParts= CylinderCoverReturnofParts
                            });
                        }
                        ViewBag.SumofcSend = SumofcSend;
                        ViewBag.SumofcUsed = SumofcUsed;
                        ViewBag.CylinderCoverReturnofParts = SumofcReturnofParts;
                        ViewBag.SumofcRem = SumofcSend - SumofcUsed- SumofcReturnofParts;

                        //Otherthings Rem
                        reader.NextResult();
                        int SumofoSend = 0;
                        int SumofoUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofcSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofcUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            OtherthingsTableOuts.Add(new OtherthingsRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofoSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofoUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofoSend - SumofoUsed).ToString()
                            });
                        }
                        ViewBag.SumofoSend = SumofoSend;
                        ViewBag.SumofoUsed = SumofoUsed;
                        ViewBag.SumofoRem = SumofoSend - SumofoUsed;

                    }
                    conn.Close();
                }//end using
                ViewBag.KitTableOut = KitTableOuts;
                ViewBag.TankTableOut = TankTableOuts;
                ViewBag.TankBaseTableOut = TankBaseTableOuts;
                ViewBag.ValveTableOut = ValveTableOuts; 
                ViewBag.TankCoverTableOut = TankCoverTableOuts;
                ViewBag.OtherthingsTableOut = OtherthingsTableOuts;
            }
            catch
            {
                //ViewBag.TableOut = null;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult DivisionPlanRem(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post)
        {
            if (WorkshopID != null)
                ViewBag.WorkshopsID = WorkshopID;
            //DateTime fdate = DateTime.Now;
            //int passYear = pc.GetYear(DateTime.Now)-1;
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            //string[] permission;
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
                        Title = item.Title.Replace("مرکز خدمات CNG ", "")
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
            //else
            //{
            //    foreach(var item in WorkshopID)
            //    {
            //        filterItems += item.Value + ",";
            //    }
            //}
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";

            List<KitRem> KitTableOuts = new List<KitRem>();
            List<TankRem> TankTableOuts = new List<TankRem>();
            List<TankBaseRem> TankBaseTableOuts = new List<TankBaseRem>();
            List<ValveRem> ValveTableOuts = new List<ValveRem>();
            List<TankCoverRem> TankCoverTableOuts = new List<TankCoverRem>();
            List<OtherthingsRem> OtherthingsTableOuts = new List<OtherthingsRem>();

            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlan]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshop", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                        command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                        //command.Parameters.Add("@permission", SqlDbType.DateTime).Value = permission;

                        conn.Open();
                        reader = command.ExecuteReader();
                        int SumofkSend = 0;
                        int SumofkUsed = 0;
                        //kit rem
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofkSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofkUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();


                            KitTableOuts.Add(new KitRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofkSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofkUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofkSend - SumofkUsed).ToString()
                            });
                        }
                        ViewBag.SumofkSend = SumofkSend;
                        ViewBag.SumofkUsed = SumofkUsed;
                        ViewBag.SumofkRem = SumofkSend - SumofkUsed;
                        //tank rem
                        reader.NextResult();

                        int SumoftSend = 0;
                        int SumoftUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumoftSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumoftUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            TankTableOuts.Add(new TankRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumoftSend.ToString(),
                                Used = Used,
                                SumofUsed = SumoftUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumoftSend - SumoftUsed).ToString()

                            });
                        }
                        ViewBag.SumoftSend = SumoftSend;
                        ViewBag.SumoftUsed = SumoftUsed;
                        ViewBag.SumoftRem = SumoftSend - SumoftUsed;
                        //tank base rem
                        reader.NextResult();
                        int SumofbSend = 0;
                        int SumofbUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofbSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofbUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            TankBaseTableOuts.Add(new TankBaseRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofbSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofbUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofbSend - SumofbUsed).ToString()
                            });
                        }
                        ViewBag.SumofbSend = SumofbSend;
                        ViewBag.SumofbUsed = SumofbUsed;
                        ViewBag.SumofbRem = SumofbSend - SumofbUsed;
                        //valve rem
                        reader.NextResult();
                        int SumofvSend = 0;
                        int SumofvUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofvSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofvUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            ValveTableOuts.Add(new ValveRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofvSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofvUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofvSend - SumofvUsed).ToString()
                            });
                        }
                        ViewBag.SumofvSend = SumofvSend;
                        ViewBag.SumofvUsed = SumofvUsed;
                        ViewBag.SumofvRem = SumofvSend - SumofvUsed;
                        //Tank Cover rem
                        reader.NextResult();
                        int SumofcSend = 0;
                        int SumofcUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofcSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofcUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            TankCoverTableOuts.Add(new TankCoverRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofcSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofcUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofcSend - SumofcUsed).ToString()
                            });
                        }
                        ViewBag.SumofcSend = SumofcSend;
                        ViewBag.SumofcUsed = SumofcUsed;
                        ViewBag.SumofcRem = SumofcSend - SumofcUsed;
                        //Otherthings Rem
                        reader.NextResult();
                        int SumofoSend = 0;
                        int SumofoUsed = 0;
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofcSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofcUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();

                            OtherthingsTableOuts.Add(new OtherthingsRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofoSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofoUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofoSend - SumofoUsed).ToString()
                            });
                        }
                        ViewBag.SumofoSend = SumofoSend;
                        ViewBag.SumofoUsed = SumofoUsed;
                        ViewBag.SumofoRem = SumofoSend - SumofoUsed;

                    }
                    conn.Close();
                }//end using
                ViewBag.KitTableOut = KitTableOuts;
                ViewBag.TankTableOut = TankTableOuts;
                ViewBag.TankBaseTableOut = TankBaseTableOuts;
                ViewBag.ValveTableOut = ValveTableOuts;
                ViewBag.TankCoverTableOut = TankCoverTableOuts;
                ViewBag.OtherthingsTableOut = OtherthingsTableOuts;
            }
            catch
            {
                ViewBag.TableOut = null;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult DivisionPlanRemKitDetails(string VehicleType, int? WorkshopID, DateTime fromDate, DateTime toDate)
        {
            if(VehicleType.Contains("سمند/ پژو"))
            {
                var results = db.tbl_VehicleRegistrations
               .Include(v => v.VehicleType)
               .Include(v => v.Workshop)
               .Where(v => (v.VehicleType.Type.Contains("پژو") || v.VehicleType.Type.Contains("سمند")) && !v.VehicleType.Type.Contains("RD"));
                if (WorkshopID != null)
                    return View(results.Where(w => w.RegisterStatus==true && w.WorkshopID == WorkshopID).ToList());
                else
                    return View(results.Where(w => w.RegisterStatus == true).ToList());
            }
            else
            {
                var results = db.tbl_VehicleRegistrations
               .Include(v => v.VehicleType)
               .Include(v => v.Workshop)
               .Where(v => v.VehicleType.Type.Contains(VehicleType));
                if (WorkshopID != null)
                    return View(results.Where(w => w.RegisterStatus == true && w.WorkshopID == WorkshopID).ToList());
                else
                    return View(results.Where(w => w.RegisterStatus == true).ToList());
            }
        }
        //
        public ActionResult DivisionPlanRemCylinderDetails(string cylinderType, int? WorkshopID, DateTime fromDate, DateTime toDate)
        {
            string replace = cylinderType.Replace("لیتری", "");
            var cylinderId = db.tbl_TypeofTanks.Where(t => t.Type==replace).ToList();
            List<VehicleRegistration> vehicleList = new List<VehicleRegistration>();
            foreach(var item in cylinderId)
            {
                var vehicleregistration = db.tbl_VehicleRegistrations.Where(v => v.RegisterStatus==true && v.VehicleTypeID == item.VehicleTypeId).ToList();
                foreach(var vehicle in vehicleregistration)
                {
                    vehicleList.Add(new VehicleRegistration
                    {
                        Address= vehicle.Address,
                        VehicleTypeID= vehicle.VehicleTypeID,
                        AlphaPlate= vehicle.AlphaPlate,
                        ChassisNumber= vehicle.ChassisNumber,
                        Checked=vehicle.Checked,
                        CheckedDate=vehicle.CheckedDate,
                        Checker=vehicle.Checker,
                        ConstructionYear=vehicle.ConstructionYear,
                        CreateDate=vehicle.CreateDate,
                        Creator=vehicle.Creator,
                        CreatorIPAddress=vehicle.CreatorIPAddress,
                        Description=vehicle.Description,
                        EditDate=vehicle.EditDate,
                        Editor=vehicle.Editor,
                        EditorIPAddress=vehicle.EditorIPAddress,
                        EngineNumber=vehicle.EngineNumber,
                        FuelCard=vehicle.FuelCard,
                        HealthCertificate=vehicle.HealthCertificate,
                        ID=vehicle.ID,
                        InstallationStatus=vehicle.InstallationStatus,
                        InvoiceCode=vehicle.InvoiceCode,
                        IranNumberPlate=vehicle.IranNumberPlate,
                        LeftNumberPlate=vehicle.LeftNumberPlate,
                        License=vehicle.License,
                        LicenseImage=vehicle.LicenseImage,
                        MobileNumber=vehicle.MobileNumber,
                        NationalCard=vehicle.NationalCard,
                        NationalCode=vehicle.NationalCode,
                        OwnerFamily=vehicle.OwnerFamily,
                        OwnerName=vehicle.OwnerName,
                        PhoneNumber=vehicle.PhoneNumber,
                        RefuelingLable=vehicle.RefuelingLable,
                        RightNumberPlate=vehicle.RightNumberPlate,
                        SerialKey=vehicle.SerialKey,
                        SerialKit=vehicle.SerialKit,
                        SerialRefuelingValve=vehicle.SerialRefuelingValve,
                        SerialSparkPreview=vehicle.SerialSparkPreview,
                        Status=vehicle.Status,
                        System=vehicle.System,
                        TrackingCode=vehicle.TrackingCode,
                        TypeofUse=vehicle.TypeofUse,
                        TypeofUseID=vehicle.TypeofUseID,
                        VehicleCard=vehicle.VehicleCard,
                        VehicleType=vehicle.VehicleType,
                        VIN=vehicle.VIN,
                        Workshop=vehicle.Workshop,
                        WorkshopID=vehicle.WorkshopID
                    });
                }
                
            }
            //
            return View(vehicleList.ToList());
        }
        //
        public class KitRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
            public string KitReturnofParts { get; set; }
        }
        //
        public class TankRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
            public string CylinderReturnofParts { get; set; }
        }
        //
        public class TankBaseRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
            public string CylinderBaseReturnofParts { get; set; }
        }
        //
        public class ValveRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
            public string ValveReturnofParts { get; set; }
        }
        //
        public class TankCoverRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
            public string CylinderCoverReturnofParts { get; set; }
        }
        //
        public class OtherthingsRem
        {
            public string Type { get; set; }
            public string Send { get; set; }
            public string Used { get; set; }
            public string Rem { get; set; }
            public string SumofSend { get; set; }
            public string SumofUsed { get; set; }
            public string SumofRem { get; set; }
            public string Title { get; set; }
            public string TextColor { get; set; }
        }
        //
        public ActionResult AllowEditConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }

        [HttpPost, ActionName("AllowEditConfirmation")]
        [ValidateAntiForgeryToken]
        public ActionResult AllowEditConfirmationSubmit(int? id)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            divisionplan.Confirmation = false;
            divisionplan.ConfirmationDate = null;
            divisionplan.ConfirmationUser = null;
            divisionplan.Send = false;
            divisionplan.SendDate = null;
            divisionplan.Sender = null;
            divisionplan.FinalCheck = false;

            db.Entry(divisionplan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult AllowEditSend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }

        [HttpPost, ActionName("AllowEditSend")]
        [ValidateAntiForgeryToken]
        public ActionResult AllowEditSendSubmit(int? id)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            divisionplan.Send = false;
            divisionplan.SendDate = null;
            divisionplan.Sender = null;

            db.Entry(divisionplan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult FinalCheck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);

            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return PartialView(divisionplan);
        }

        [HttpPost, ActionName("FinalCheck")]
        [ValidateAntiForgeryToken]
        public ActionResult FinalCheckSubmit(int? id)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            divisionplan.FinalCheck = true;
            divisionplan.FinalCheckDate = DateTime.Now;

            db.Entry(divisionplan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult DivisionPlanwithBOMs(double? number, string code, int? id,int? VehicleTypeID, string TypeofValve,int? GenerationID)
        {
            ViewBag.Code = code;
            ViewBag.DivisionPlanID = id;
            if (id != null)
                ViewBag.Workshop = db.tbl_DivisionPlans.Find(id).Workshop.Title;
            else
                ViewBag.Workshop = "";
            string countrystring = "select * from [dbo].[tbl_VehicleTypes]";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            //
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text");
            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
            ViewBag.number = number;
            ViewBag.VehicleType = VehicleTypeID;
            ViewBag.TypeofValve = TypeofValve;
            ViewBag.Generation = GenerationID;
            //
            return View();
        }
        public ActionResult ShowBOMDivisionResults(int? id,string GenerationId)
        {
            //if (GenerationId.Equals("5"))
            //    Session[GenerationId] = 1;
            string Generation = "";
            string Code = "";
            string VehicleType = "";
            string WorkshopTitle = "";
            string CreateDate = "";
            string Creator = "";
            string Number = "";
            string VehicleTypeID = "";
            string DivisionPlanID = "";
            int count = 0;

            List<DivisionList> tableOuts = new List<DivisionList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_ShowBOMDivisionResults]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                        command.Parameters.Add("@GenerationId", SqlDbType.VarChar).Value = Session[GenerationId];

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            count += 1;
                            Code = reader["Code"].ToString();
                            VehicleType = reader["VehicleType"].ToString();
                            VehicleTypeID = reader["VehicleTypeID"].ToString();
                            DivisionPlanID = reader["DivisionPlanID"].ToString();
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            CreateDate =Convert.ToDateTime(reader["CreateDate"].ToString()).ToShortDateString();
                            Creator = reader["Creator"].ToString();
                            Number = reader["Number"].ToString();
                            Generation = reader["Generation"].ToString();

                            tableOuts.Add(new DivisionList
                            {
                                Number = Number,
                                WorkshopTitle = WorkshopTitle,
                                Code = Code,
                                CreateDate = CreateDate,
                                Creator = Creator,
                                VehicleType = VehicleType,
                                VehicleTypeID = VehicleTypeID,
                                DivisionPlanID = DivisionPlanID,
                                Generation = Generation
                            });
                        }
                    }
                }//end using
                ViewBag.tableOut = tableOuts;
                ViewBag.count = count;
                ViewBag.DivisionPlanID = id;
            }
            catch
            {
                ViewBag.tableOut = null;
            }
            return PartialView();
        }
        //
        public ActionResult ShowBOMDivisionResults_TKhF(int? id, string GenerationId)
        {
            string Code = "";
            string VehicleType = "";
            string WorkshopTitle = "";
            string CreateDate = "";
            string Creator = "";
            string Number = "";
            string VehicleTypeID = "";
            string DivisionPlanID = "";
            int count = 0;

            List<DivisionList> tableOuts = new List<DivisionList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_ShowBOMDivisionResults_TKhF]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                        command.Parameters.Add("@GenerationId", SqlDbType.VarChar).Value = GenerationId;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            count += 1;
                            Code = reader["Code"].ToString();
                            VehicleType = reader["VehicleType"].ToString();
                            VehicleTypeID = reader["VehicleTypeID"].ToString();
                            DivisionPlanID = reader["DivisionPlanID"].ToString();
                            WorkshopTitle = reader["WorkshopTitle"].ToString();
                            CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString()).ToShortDateString();
                            Creator = reader["Creator"].ToString();
                            Number = reader["Number"].ToString();

                            tableOuts.Add(new DivisionList
                            {
                                Number = Number,
                                WorkshopTitle = WorkshopTitle,
                                Code = Code,
                                CreateDate = CreateDate,
                                Creator = Creator,
                                VehicleType = VehicleType,
                                VehicleTypeID = VehicleTypeID,
                                DivisionPlanID = DivisionPlanID
                            });
                        }
                    }
                }//end using
                ViewBag.tableOut = tableOuts;
                ViewBag.count = count;
                ViewBag.DivisionPlanID = id;
            }
            catch
            {
                ViewBag.tableOut = null;
            }
            return PartialView();
        }
        //
        public class DivisionList
        {
            public string Title { get; set; }
            public string Code { get; set; }
            public string VehicleType { get; set; }
            public string VehicleTypeID { get; set; }
            public string DivisionPlanID { get; set; }
            public string WorkshopTitle { get; set; }
            public string CreateDate { get; set; }
            public string Creator { get; set; }
            public string Number { get; set; }
            public string Generation { get; set; }
        }
        //
        public class CheckedReminBOMList
        {
            public string Title { get; set; }
            public string FinancialCode { get; set; }
            public string Type { get; set; }
            public double? Ratio { get; set; }
            public string Unit { get; set; }
            public double? Rem { get; set; }
            public double? CurrRem { get; set; }
            public string ParentTitle { get; set; }
        }
        //
        public ActionResult CheckedReminBOMs(double? number,int? VehicleTypeID, int? GenerationID, string columnhidden)
        {
            ViewBag.columnhidden = columnhidden;
            ViewBag.number = number;
            if (VehicleTypeID == null)
                VehicleTypeID = 0;
            ViewBag.VehicleType = VehicleTypeID;
            //
            string Title = ""; ;
            string FinancialCode = "";
            //string Type = "";
            string ParentTitle = "";
            double? Ratio = 0.0;
            string Unit = "";
            double? Rem = 0.0;
            double? CurrRem = 0.0;
            
            List<CheckedReminBOMList> tableOuts = new List<CheckedReminBOMList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_DivisionPlanwithBOMs]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@VehicleTypeID", SqlDbType.VarChar).Value = VehicleTypeID;
                        command.Parameters.Add("@GenerationID", SqlDbType.VarChar).Value = GenerationID;

                        conn.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            CurrRem =Convert.ToDouble(reader["CurrRem"].ToString());
                            Rem = Convert.ToDouble(reader["Rem"].ToString());
                            FinancialCode = reader["FinancialCode"].ToString();
                            Ratio = Convert.ToDouble(reader["Ratio"].ToString());
                            Title = reader["Title"].ToString();
                            //Type = reader["Type"].ToString();
                            Unit = reader["Unit"].ToString();
                            ParentTitle= reader["ParentTitle"].ToString();

                            tableOuts.Add(new CheckedReminBOMList
                            {
                                CurrRem=CurrRem,
                                Rem=Rem,
                                FinancialCode=FinancialCode,
                                Ratio=Ratio,
                                Title=Title,
                                //Type=Type,
                                Unit=Unit,
                                ParentTitle= ParentTitle
                            });
                        }
                    }
                }//end using
                ViewBag.tableOut = tableOuts;
            }
            catch
            {
                ViewBag.tableOut = null;
            }
            //if(VehicleTypeID!=null)
            //{
            //    var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == VehicleTypeID).Include(b => b.EquipmentList).Include(b => b.VehicleType);
            //    return PartialView(bomList.ToList());
            //}
            //else
            //{
            //    var bomList = db.tbl_BOMs.Include(b => b.EquipmentList).Include(b => b.VehicleType);
            //    return PartialView(bomList.ToList());
            //}
            return PartialView();
        }
        //
        public ActionResult AddDivisionPlanBOMs()
        {
            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            var tbl_Users = db.tbl_Users.ToList();
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            List<WorkshopsList> tableOuts = new List<WorkshopsList>();

            foreach (var item in tbl_Workshops.Where(w => w.isServices == true).ToList())
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(userId.UserID);

                if (workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new WorkshopsList
                    {
                        id = item.ID,
                        title = item.Title
                    });
                }
            }
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddDivisionPlanBOMs(int? number, int? VehicleTypeID, int? DivisionPlanID, int? GenerationID, string TypeofValve)
        {
            if(number == null || VehicleTypeID == null || GenerationID == null)
            {
                return Json(new { result = "مقادیر اطلاعات اولیه خالی و یا ناصحیح می‌باشد، لطفا پس از انتخاب مقادیر مجدد تلاش نمایید....!" }, JsonRequestBehavior.AllowGet);
            }
            //
            int RegistrationTypeID = 1;

            if (GenerationID == 5)
                RegistrationTypeID = 2;
            //
            var divisinonplan = db.tbl_DivisionPlans.Find(DivisionPlanID);
            var checkedExist = db.tbl_DivisionPlanBOMs.Include(d => d.BOM).Where(d => d.DivisionPlanID == DivisionPlanID && d.BOM.VehicleTypeID == VehicleTypeID && d.BOM.GenerationID == GenerationID).ToList();
            bool isExist = false;
            if (checkedExist.Count() > 0)
                isExist = true;

            ViewBag.number = number;
            ViewBag.VehicleType = VehicleTypeID;
            ViewBag.TypeofValve = TypeofValve;

            VehicleType vehicletitle = db.tbl_VehicleTypes.Find(VehicleTypeID);
            var bomList = db.tbl_BOMs.Where(b => b.VehicleTypeID == VehicleTypeID && b.GenerationID==GenerationID && b.Presentable == true).Include(b => b.EquipmentList).ToList();            
            DivisionPlanBOM addtoBOM = new DivisionPlanBOM();
            bool valveRow = false;
            string Desc = "";
            string Desc2 = "";
            //
            KitDivisionPlan kit = new KitDivisionPlan();
            TankDivisionPlan cylinder = new TankDivisionPlan();
            TankBaseDivisionPlan cylinderbase = new TankBaseDivisionPlan();
            TankCoverDivisionPlan cylindercover = new TankCoverDivisionPlan();
            ValveDivisionPlan valve = new ValveDivisionPlan();
            //
            //
            if (isExist == true)
            {
                return Json(new { result = " طرح تقسیم مربوط به خودروی : " + vehicletitle.Type + " " + vehicletitle.Description + " به تعداد : " + number + " قلم کالا تکراری می باشد و امکان ثبت آن وجود ندارد!" }, JsonRequestBehavior.AllowGet);
            }
            //if (DivisionPlanID != null)
            if (bomList.Count() != 0)
            {
                // برای طرح تقسیم طرح تبدیل
                if (isExist == false && GenerationID != 5)
                {
                    foreach (var item in bomList)
                    {
                        addtoBOM.DivisionPlanID = DivisionPlanID;
                        if (item.EquipmentListID == 22 || item.EquipmentListID == 23)
                        {
                            valveRow = true;
                            if (!item.EquipmentList.Title.Replace("ي", "ی").Contains(TypeofValve))
                                Desc = "شير مخزن دستي " + TypeofValve;
                            Desc2 = item.EquipmentList.Title;
                        }
                        addtoBOM.BOMID = item.ID;
                        addtoBOM.CreateDate = DateTime.Now;
                        addtoBOM.Creator = User.Identity.Name;
                        addtoBOM.Description = Desc;
                        addtoBOM.Number = (item.Ratio.HasValue ? item.Ratio.Value : 1) * number.Value;
                        addtoBOM.NumberofSend = (item.Ratio.HasValue ? item.Ratio.Value : 1) * number;
                        addtoBOM.GenarationID = item.GenerationID;
                        addtoBOM.RegistrationTypeID = RegistrationTypeID;
                        db.tbl_DivisionPlanBOMs.Add(addtoBOM);
                        db.SaveChanges();
                        Desc = "";

                        //
                        try
                        {
                            if (valveRow == true)
                            {
                                if (TypeofValve.Replace("ي", "ی").Equals("استوانه ای"))
                                {
                                    var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals("1920")).SingleOrDefault();
                                    warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                    db.Entry(warehouseRem).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                if (TypeofValve.Replace("ي", "ی").Equals("مخروطی"))
                                {
                                    var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals("1911")).SingleOrDefault();
                                    warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                    db.Entry(warehouseRem).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals(item.EquipmentList.FinancialCode)).SingleOrDefault();
                                warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                db.Entry(warehouseRem).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //
                            valveRow = false;
                        }
                        catch
                        {

                        }

                    }
                    //-------------------------------add to kit division plans old division system--------------------------------------
                    kit.DivisionPlanID = DivisionPlanID;
                    kit.Number = number.Value;
                    kit.NumberofSend = number.Value;
                    kit.VehicleTypeID = VehicleTypeID;
                    kit.Description = Desc;
                    kit.GenarationID = GenerationID;
                    kit.RegistrationTypeID = RegistrationTypeID;
                    db.tbl_KitDivisionPlans.Add(kit);
                    db.SaveChanges();
                    //-------------------------------add to cylinder division plans old division system--------------------------------------
                    var typofTank = db.tbl_TypeofTanks.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                    cylinder.DivisionPlanID = DivisionPlanID;
                    cylinder.Number = number.Value;
                    cylinder.NumberofSend = number.Value;
                    cylinder.TypeofTankID = typofTank.ID;
                    if (Desc2.Replace("ي", "ی").Contains("مخروطی"))
                        cylinder.Description = "رزوه مخروطی";
                    else
                        cylinder.Description = "رزوه استوانه ای";
                    cylinder.TankConstractorID = null;
                    cylinder.GenarationID = GenerationID;
                    cylinder.RegistrationTypeID = RegistrationTypeID;
                    db.tbl_TankDivisionPlans.Add(cylinder);
                    db.SaveChanges();
                    //-------------------------------add to cylinder base division plans old division system--------------------------------------
                    var typoftankBase = db.tbl_TypeofTankBases.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                    cylinderbase.DivisionPlanID = DivisionPlanID;
                    cylinderbase.Number = number.Value;
                    cylinderbase.NumberofSend = number.Value;
                    cylinderbase.TypeofTankBaseID = typoftankBase.ID;
                    cylinderbase.Description = Desc;
                    cylinderbase.GenarationID = GenerationID;
                    cylinderbase.RegistrationTypeID = RegistrationTypeID;
                    db.tbl_TankBaseDivisionPlans.Add(cylinderbase);
                    db.SaveChanges();
                    //-------------------------------add to cylinder cover division plans old division system--------------------------------------
                    var typoftankCover = db.tbl_TypeofTankCovers.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                    cylindercover.DivisionPlanID = DivisionPlanID;
                    cylindercover.Number = number.Value;
                    cylindercover.NumberofSend = number.Value;
                    cylindercover.TypeofTankCoverID = typoftankCover.ID;
                    cylindercover.Description = Desc;
                    cylindercover.GenarationID = GenerationID;
                    cylindercover.RegistrationTypeID = RegistrationTypeID;
                    db.tbl_TankCoverDivisionPlans.Add(cylindercover);
                    db.SaveChanges();
                    //-------------------------------add to valve division plans old division system--------------------------------------
                    valve.DivisionPlanID = DivisionPlanID;
                    valve.Number = number.Value;
                    valve.NumberofSend = number.Value;
                    valve.Type = "دستی";
                    if (Desc2.Replace("ي", "ی").Contains("مخروطی"))
                        valve.Description = "مخروطی";
                    else
                        valve.Description = "استوانه ای";
                    valve.ValveConstractorID = 1;
                    valve.GenarationID = GenerationID;
                    valve.RegistrationTypeID = RegistrationTypeID;
                    db.tbl_ValveDivisionPlans.Add(valve);
                    db.SaveChanges();
                    //

                    return Json(new { result = " طرح تقسیم مربوط به خودروی : " + vehicletitle.Type + " " + vehicletitle.Description + " به تعداد : " + number + " قلم کالا با موفقیت صادر شد." }, JsonRequestBehavior.AllowGet);
                }
                // برای طرح تقسیم طرح تعویض قطعات
                if (isExist == false && GenerationID == 5)
                {
                    foreach (var item in bomList)
                    {
                        addtoBOM.DivisionPlanID = DivisionPlanID;
                        if (item.EquipmentListID == 22 || item.EquipmentListID == 23)
                        {
                            valveRow = true;
                            if (!item.EquipmentList.Title.Replace("ي", "ی").Contains(TypeofValve))
                                Desc = "شير مخزن دستي " + TypeofValve;
                            Desc2 = item.EquipmentList.Title;
                        }
                        addtoBOM.BOMID = item.ID;
                        addtoBOM.CreateDate = DateTime.Now;
                        addtoBOM.Creator = User.Identity.Name;
                        addtoBOM.Description = Desc;
                        addtoBOM.Number = (item.Ratio.HasValue ? item.Ratio.Value : 1) * number.Value;
                        addtoBOM.NumberofSend = (item.Ratio.HasValue ? item.Ratio.Value : 1) * number;
                        addtoBOM.GenarationID = item.GenerationID;
                        addtoBOM.RegistrationTypeID = RegistrationTypeID;
                        db.tbl_DivisionPlanBOMs.Add(addtoBOM);
                        db.SaveChanges();
                        Desc = "";

                        //
                        try
                        {
                            if (valveRow == true)
                            {
                                if (TypeofValve.Replace("ي", "ی").Equals("استوانه ای"))
                                {
                                    var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals("1920")).SingleOrDefault();
                                    warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                    db.Entry(warehouseRem).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                if (TypeofValve.Replace("ي", "ی").Equals("مخروطی"))
                                {
                                    var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals("1911")).SingleOrDefault();
                                    warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                    db.Entry(warehouseRem).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                var warehouseRem = db.tbl_Warehouses.Where(w => w.FinancialCode.Equals(item.EquipmentList.FinancialCode)).SingleOrDefault();
                                warehouseRem.CurrentRem = warehouseRem.CurrentRem - addtoBOM.Number;
                                db.Entry(warehouseRem).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //
                            valveRow = false;
                        }
                        catch
                        {

                        }

                    }

                    int checkCountKit = 0;
                    int checkCountcylinder = 0;
                    foreach (var item in bomList)
                    {
                        //-------------------------------add to kit division plans old division system--------------------------------------

                        if (item.EquipmentList.Pid == 1)
                        {
                            if (checkCountKit == 0)
                            {
                                kit.DivisionPlanID = DivisionPlanID;
                                kit.Number = number.Value;
                                kit.NumberofSend = number.Value;
                                kit.VehicleTypeID = VehicleTypeID;
                                kit.Description = Desc;
                                kit.GenarationID = GenerationID;
                                kit.RegistrationTypeID = RegistrationTypeID;
                                db.tbl_KitDivisionPlans.Add(kit);
                                db.SaveChanges();
                                checkCountKit += 1;
                            }                            
                        }
                        //-------------------------------add to cylinder division plans old division system--------------------------------------

                        if (item.EquipmentList.Pid == 2)
                        {
                            if(checkCountcylinder == 0)
                            {
                                var typofTank = db.tbl_TypeofTanks.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                                cylinder.DivisionPlanID = DivisionPlanID;
                                cylinder.Number = number.Value;
                                cylinder.NumberofSend = number.Value;
                                cylinder.TypeofTankID = typofTank.ID;
                                if (Desc2.Replace("ي", "ی").Contains("مخروطی"))
                                    cylinder.Description = "رزوه مخروطی";
                                else
                                    cylinder.Description = "رزوه استوانه ای";
                                cylinder.TankConstractorID = null;
                                cylinder.GenarationID = GenerationID;
                                cylinder.RegistrationTypeID = RegistrationTypeID;
                                db.tbl_TankDivisionPlans.Add(cylinder);
                                db.SaveChanges();
                                checkCountcylinder += 1;
                            }                            
                        }
                        //-------------------------------add to valve division plans old division system--------------------------------------
                        //if (item.EquipmentList.Address.Contains("شیر مخزن"))
                        if (item.EquipmentList.Title.Contains("شير مخزن"))
                        {
                            valve.DivisionPlanID = DivisionPlanID;
                            valve.Number = number.Value;
                            valve.NumberofSend = number.Value;
                            valve.Type = "دستی";
                            if (Desc2.Replace("ي", "ی").Contains("مخروطی"))
                                valve.Description = "مخروطی";
                            else
                                valve.Description = "استوانه ای";
                            valve.ValveConstractorID = 1;
                            valve.GenarationID = GenerationID;
                            valve.RegistrationTypeID = RegistrationTypeID;
                            db.tbl_ValveDivisionPlans.Add(valve);
                            db.SaveChanges();
                        }
                        //-------------------------------add to cylinder Base division plans old division system--------------------------------------
                        //if (item.EquipmentList.Address.Contains("پایه مخزن"))
                        if (item.EquipmentList.Title.Contains("پايه مخزن"))
                        {
                            var typoftankBase = db.tbl_TypeofTankBases.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                            cylinderbase.DivisionPlanID = DivisionPlanID;
                            cylinderbase.Number = number.Value;
                            cylinderbase.NumberofSend = number.Value;
                            cylinderbase.TypeofTankBaseID = typoftankBase.ID;
                            cylinderbase.Description = Desc;
                            cylinderbase.GenarationID = GenerationID;
                            cylinderbase.RegistrationTypeID = RegistrationTypeID;
                            db.tbl_TankBaseDivisionPlans.Add(cylinderbase);
                            db.SaveChanges();
                        }

                        //-------------------------------add to cylinder Cover division plans old division system--------------------------------------
                        //if (item.EquipmentList.Address.Contains("کاور مخزن"))
                        if (item.EquipmentList.Title.Contains("کاور"))
                        {
                            var typoftankCover = db.tbl_TypeofTankCovers.Where(t => t.VehicleTypeId == VehicleTypeID).SingleOrDefault();
                            cylindercover.DivisionPlanID = DivisionPlanID;
                            cylindercover.Number = number.Value;
                            cylindercover.NumberofSend = number.Value;
                            cylindercover.TypeofTankCoverID = typoftankCover.ID;
                            cylindercover.Description = Desc;
                            cylindercover.GenarationID = GenerationID;
                            cylindercover.RegistrationTypeID = RegistrationTypeID;
                            db.tbl_TankCoverDivisionPlans.Add(cylindercover);
                            db.SaveChanges();
                        }
                        
                    }

                    return Json(new { result = " طرح تقسیم مربوط به خودروی : " + vehicletitle.Type + " " + vehicletitle.Description + " به تعداد : " + number + " قلم کالا مربوط به طرح تعویض با موفقیت صادر شد." }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { result = "با توجه به اطلاعات ورودی، داده‌ای جهت صدور طرح تقسیم یافت نشد!" }, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpGet]
        public ActionResult DeleteDivisionPlan(int? VehicleTypeID, int? DivisionPlanID,int? GenerationID, string number, string VehicleType, string Code, string WorkshopTitle,string CreateDate, string Creator)
        {
            ViewBag.VehicleTypeID = VehicleTypeID;
            ViewBag.DivisionPlanID = DivisionPlanID;
            ViewBag.GenerationID = GenerationID;
            ViewBag.number = number;
            ViewBag.VehicleType = VehicleType;
            ViewBag.Code = Code;
            ViewBag.WorkshopTitle = WorkshopTitle;
            ViewBag.CreateDate = CreateDate;
            ViewBag.Creator = Creator;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteDivisionPlan(int? number, int? VehicleTypeID, int? DivisionPlanID, int? GenerationID, string code)
        {
            var divisinonplan = db.tbl_DivisionPlans.Find(DivisionPlanID);
            var checkedExist = db.tbl_DivisionPlanBOMs.Include(d => d.BOM).Where(d => d.DivisionPlanID == DivisionPlanID && d.BOM.VehicleTypeID == VehicleTypeID).ToList();
            VehicleType vehicletitle = db.tbl_VehicleTypes.Find(VehicleTypeID);
            var kit = db.tbl_KitDivisionPlans.Where(d => d.DivisionPlanID == DivisionPlanID && d.VehicleTypeID == VehicleTypeID).ToList();
            var cylinder = db.tbl_TankDivisionPlans.Include(t=>t.TypeofTank).Where(d => d.DivisionPlanID == DivisionPlanID && d.TypeofTank.VehicleTypeId == VehicleTypeID).ToList();
            var cylinderbase = db.tbl_TankBaseDivisionPlans.Include(t => t.TypeofTankBase).Where(d => d.DivisionPlanID == DivisionPlanID && d.TypeofTankBase.VehicleTypeId == VehicleTypeID).ToList();
            var cylindercover = db.tbl_TankCoverDivisionPlans.Include(t => t.TypeofTankCover).Where(d => d.DivisionPlanID == DivisionPlanID && d.TypeofTankCover.VehicleTypeId == VehicleTypeID).ToList();
            if (kit.Count > 0)
                number = kit.FirstOrDefault().NumberofSend;
            var valve = db.tbl_ValveDivisionPlans.Where(d => d.DivisionPlanID == DivisionPlanID && d.Number == number).Take(1).ToList();

            foreach (var item in checkedExist)
            {
                DivisionPlanBOM divisionplan = db.tbl_DivisionPlanBOMs.Find(item.ID);
                db.tbl_DivisionPlanBOMs.Remove(divisionplan);
                db.SaveChanges();
            }

            //for delete items in kit
            foreach (var item in kit)
            {
                KitDivisionPlan divisionplan = db.tbl_KitDivisionPlans.Find(item.ID);
                db.tbl_KitDivisionPlans.Remove(divisionplan);
                db.SaveChanges();
            }

            //for delete items in cylinder
            foreach (var item in cylinder)
            {
                TankDivisionPlan divisionplan = db.tbl_TankDivisionPlans.Find(item.ID);
                db.tbl_TankDivisionPlans.Remove(divisionplan);
                db.SaveChanges();
            }

            //for delete items in valve
            foreach (var item in valve)
            {
                ValveDivisionPlan divisionplan = db.tbl_ValveDivisionPlans.Find(item.ID);
                db.tbl_ValveDivisionPlans.Remove(divisionplan);
                db.SaveChanges();
            }

            //for delete items in cylinderbase
            foreach (var item in cylinderbase)
            {
                TankBaseDivisionPlan divisionplan = db.tbl_TankBaseDivisionPlans.Find(item.ID);
                db.tbl_TankBaseDivisionPlans.Remove(divisionplan);
                db.SaveChanges();
            }

            //for delete items in cylindercover
            foreach (var item in cylindercover)
            {
                TankCoverDivisionPlan divisionplan = db.tbl_TankCoverDivisionPlans.Find(item.ID);
                db.tbl_TankCoverDivisionPlans.Remove(divisionplan);
                db.SaveChanges();
            }
            //
            return RedirectToAction("DivisionPlanwithBOMs",new { id= DivisionPlanID, code= code});
            //return Json(new { result = " طرح تقسیم مربوط به خودروی : " + vehicletitle.Type + " " + vehicletitle.Description + " به تعداد : " + number + " قلم کالا با موفقیت حذف شد." }, JsonRequestBehavior.AllowGet);
        }
        //
        public static string Rem(string code)
        {
            var count = dbstatic.tbl_Warehouses.Where(w => w.FinancialCode.Equals(code)).SingleOrDefault();
            if (count != null)
                return count.CurrentRem.Value.ToString();
            else
                return "0";
        }
        //
        public FileResult DownloadExcel()
        {
            string path = "/UploadedFiles/DownloadExcel.xlsx";
            return File(path, "application/vnd.ms-excel", "DownloadExcel.xlsx");
        }
        [HttpPost]
        public JsonResult UpdateWarehouse(HttpPostedFileBase FileUpload)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            string query = "TRUNCATE TABLE tbl_Warehouses";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            List<string> data = new List<string>();

            #region"بخش انبار"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود و بروزرسانی موجودی انبار از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var List = from a in excelFile.Worksheet<Warehouse>(sheetName) select a;
                    int dataCount = 0;
                    foreach (var a in List)
                    {
                        try
                        {
                            dataCount += 1;
                            Warehouse TU = new Warehouse();
                            TU.CreateDate = DateTime.Now;
                            TU.Creator = User.Identity.Name;
                            TU.CurrentRem = a.CurrentRem;
                            TU.Date = DateTime.Now;
                            TU.FinancialCode = a.FinancialCode;
                            if (!string.IsNullOrEmpty(a.Name))
                                TU.Name = a.Name;
                            else
                                TU.Name = "انبار200 هزار CNG";
                            TU.Rem = a.Rem;
                            TU.Title = a.Title;
                            TU.Units = a.Units;

                            db.tbl_Warehouses.Add(TU);
                            db.SaveChanges();
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    //if ((System.IO.File.Exists(pathToExcelFile)))
                    //{
                    //    System.IO.File.Delete(pathToExcelFile);
                    //}
                    string Message = "تعداد " + " " + dataCount + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                    ViewBag.Message = Message;
                    RedirectToAction("UploadExcel");
                    return Json(Message, JsonRequestBehavior.AllowGet);
                    //return View();
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        public ActionResult Warehouses()
        {
            var warehouse = db.tbl_Warehouses;
            return View(warehouse.ToList());
        }
        //
        public ActionResult RemDivisionPlanDetails(string vehicleType, DateTime fromDate, DateTime toDate, bool? Post, string functionUsed, string functionSend)
        {
            //string typeIds = "0,";

            List<VehicleType> vehicleTypes = null;

            if (vehicleType.Contains("سمند/ پژو"))
            {
                vehicleTypes = db.tbl_VehicleTypes.Where(v => v.ID == 1 || v.ID == 6).ToList();
            }
            else
            {
                vehicleTypes = db.tbl_VehicleTypes.Where(v => v.Type.Replace("انژکتوری", "").Equals(vehicleType)).ToList();
            }                
            
            //else
            //{
            //    foreach(var item in vehicleTypes)
            //    {
            //        typeIds += item.ID + ",";
            //    }
                
            //}
         
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01");// DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();          
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string TextColor = "";
            int SumofkSend = 0;
            int SumofkUsed = 0;

            List<KitRem> KitTableOuts = new List<KitRem>();           
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                foreach(var item in vehicleTypes)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlanDetails]", conn))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add("@vehicleType", SqlDbType.VarChar).Value = item.ID; //typeIds.TrimEnd(',');
                            command.Parameters.Add("@functionUsed", SqlDbType.VarChar).Value = functionUsed;
                            command.Parameters.Add("@functionSend", SqlDbType.VarChar).Value = functionSend;
                            command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                            command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                            conn.Open();
                            reader = command.ExecuteReader();
                            
                            //kit rem
                            while (reader.Read())
                            {
                                Type = reader["Type"].ToString();
                                Send = reader["Send"].ToString();
                                SumofkSend += Convert.ToInt32(Send);
                                Used = reader["Used"].ToString();
                                SumofkUsed += Convert.ToInt32(Used);
                                Rem = reader["Rem"].ToString();
                                TextColor = reader["TextColor"].ToString();

                                KitTableOuts.Add(new KitRem
                                {
                                    Title=item.Type + " " + item.Description,
                                    Type = Type,
                                    Send = Send,
                                    SumofSend = SumofkSend.ToString(),
                                    Used = Used,
                                    SumofUsed = SumofkUsed.ToString(),
                                    Rem = Rem,
                                    SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                    TextColor= TextColor
                                });
                            }
                            ViewBag.SumofkSend = SumofkSend;
                            ViewBag.SumofkUsed = SumofkUsed;
                            ViewBag.SumofkRem = SumofkSend - SumofkUsed;

                        }
                        conn.Close();
                    }//end using

                }
               
                ViewBag.KitTableOut = KitTableOuts;
            }
            catch
            {
                KitTableOuts.Add(new KitRem
                {
                    Type = "",
                    Send = "0",
                    SumofSend = "0",
                    Used = "0",
                    SumofUsed = "0",
                    Rem = "0",
                    SumofRem = "0",
                    TextColor=""
                });
                ViewBag.KitTableOut = KitTableOuts;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult CylinderRemDivisionPlanDetails(string cylinderType, DateTime fromDate, DateTime toDate, bool? Post)
        {
            //string typeIds = "0,";

            List<TypeofTank> vehicleTypes = null;

            vehicleTypes = db.tbl_TypeofTanks.Where(v => v.Type.Equals(cylinderType.Replace(" لیتری","").Replace(" ",""))).Include(v=>v.VehicleType).ToList();

            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01");// DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            string Title = "";
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string TextColor = "Green";
            int SumofkSend = 0;
            int SumofkUsed = 0;

            List<TankRem> CylinderTableOuts = new List<TankRem>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                //foreach (var item in vehicleTypes)
                //{ }
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[CylinderRemDivisionPlanDetails]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = cylinderType.Replace(" لیتری", "").Replace(" ", "");
                        //command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                        //command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                        conn.Open();
                        reader = command.ExecuteReader();

                        //kit rem
                        while (reader.Read())
                        {
                            Title = reader["Title"].ToString();
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofkSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofkUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            TextColor = reader["TextColor"].ToString();


                            CylinderTableOuts.Add(new TankRem
                            {
                                Title = Title,
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofkSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofkUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                TextColor = TextColor
                            });
                        }
                        ViewBag.SumofkSend = SumofkSend;
                        ViewBag.SumofkUsed = SumofkUsed;
                        ViewBag.SumofkRem = SumofkSend - SumofkUsed;

                    }
                    conn.Close();
                }//end using
                ViewBag.CylinderTableOuts = CylinderTableOuts.OrderBy(k=>k.Type);
            }
            catch
            {
                CylinderTableOuts.Add(new TankRem
                {
                    Title = "",
                    Type = "",
                    Send = "0",
                    SumofSend = "0",
                    Used = "0",
                    SumofUsed = "0",
                    Rem = "0",
                    SumofRem = "0",
                    TextColor=""
                });

                ViewBag.CylinderTableOuts = CylinderTableOuts;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult CylinderBaseRemDivisionPlanDetails(string vehicleType, DateTime fromDate, DateTime toDate, bool? Post, string functionUsed, string functionSend)
        {
            //string typeIds = "0,";

            List<TypeofTankBase> vehicleTypes = null;

            if (vehicleType.Contains("سمند/ پژو"))
            {
                vehicleTypes = db.tbl_TypeofTankBases.Where(v => v.VehicleTypeId == 1 || v.VehicleTypeId == 6).ToList();
            }
            else
            {
                vehicleTypes = db.tbl_TypeofTankBases.Where(v => v.Type.Replace("انژکتوری", "").Equals(vehicleType)).ToList();
            }

            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01");// DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string TextColor = "";
            int SumofkSend = 0;
            int SumofkUsed = 0;

            List<KitRem> KitTableOuts = new List<KitRem>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                foreach (var item in vehicleTypes)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlanDetails]", conn))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add("@vehicleType", SqlDbType.VarChar).Value = item.VehicleTypeId; //typeIds.TrimEnd(',');
                            command.Parameters.Add("@functionUsed", SqlDbType.VarChar).Value = functionUsed;
                            command.Parameters.Add("@functionSend", SqlDbType.VarChar).Value = functionSend;
                            command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                            command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                            conn.Open();
                            reader = command.ExecuteReader();

                            //kit rem
                            while (reader.Read())
                            {
                                Type = reader["Type"].ToString();
                                Send = reader["Send"].ToString();
                                SumofkSend += Convert.ToInt32(Send);
                                Used = reader["Used"].ToString();
                                SumofkUsed += Convert.ToInt32(Used);
                                Rem = reader["Rem"].ToString();
                                TextColor = reader["TextColor"].ToString();

                                KitTableOuts.Add(new KitRem
                                {
                                    Title = item.Type + " " + item.Description,
                                    Type = Type,
                                    Send = Send,
                                    SumofSend = SumofkSend.ToString(),
                                    Used = Used,
                                    SumofUsed = SumofkUsed.ToString(),
                                    Rem = Rem,
                                    SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                    TextColor = TextColor
                                });
                            }
                            ViewBag.SumofkSend = SumofkSend;
                            ViewBag.SumofkUsed = SumofkUsed;
                            ViewBag.SumofkRem = SumofkSend - SumofkUsed;

                        }
                        conn.Close();
                    }//end using

                }

                ViewBag.KitTableOut = KitTableOuts;
            }
            catch
            {
                KitTableOuts.Add(new KitRem
                {
                    Type = "",
                    Send = "0",
                    SumofSend = "0",
                    Used = "0",
                    SumofUsed = "0",
                    Rem = "0",
                    SumofRem = "0",
                    TextColor = ""
                });
                ViewBag.KitTableOut = KitTableOuts;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult CylinderCoverRemDivisionPlanDetails(string vehicleType, DateTime fromDate, DateTime toDate, bool? Post, string functionUsed, string functionSend)
        {
            //string typeIds = "0,";

            List<TypeofTankCover> vehicleTypes = null;

            if (vehicleType.Contains("سمند/ پژو"))
            {
                vehicleTypes = db.tbl_TypeofTankCovers.Where(v => v.VehicleTypeId == 1 || v.VehicleTypeId == 6).ToList();
            }
            else
            {
                vehicleTypes = db.tbl_TypeofTankCovers.Where(v => v.Type.Replace("انژکتوری", "").Equals(vehicleType)).ToList();
            }

            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);

            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w => w.Users);
            List<Workshop> tableOuts = new List<Workshop>();
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string TextColor = "";
            int SumofkSend = 0;
            int SumofkUsed = 0;

            List<KitRem> KitTableOuts = new List<KitRem>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                foreach (var item in vehicleTypes)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlanDetails]", conn))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add("@vehicleType", SqlDbType.VarChar).Value = item.VehicleTypeId; //typeIds.TrimEnd(',');
                            command.Parameters.Add("@functionUsed", SqlDbType.VarChar).Value = functionUsed;
                            command.Parameters.Add("@functionSend", SqlDbType.VarChar).Value = functionSend;
                            command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                            command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                            conn.Open();
                            reader = command.ExecuteReader();

                            //kit rem
                            while (reader.Read())
                            {
                                Type = reader["Type"].ToString();
                                Send = reader["Send"].ToString();
                                SumofkSend += Convert.ToInt32(Send);
                                Used = reader["Used"].ToString();
                                SumofkUsed += Convert.ToInt32(Used);
                                Rem = reader["Rem"].ToString();
                                TextColor = reader["TextColor"].ToString();

                                KitTableOuts.Add(new KitRem
                                {
                                    Title = item.Type + " " + item.Description,
                                    Type = Type,
                                    Send = Send,
                                    SumofSend = SumofkSend.ToString(),
                                    Used = Used,
                                    SumofUsed = SumofkUsed.ToString(),
                                    Rem = Rem,
                                    SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                    TextColor = TextColor
                                });
                            }
                            ViewBag.SumofkSend = SumofkSend;
                            ViewBag.SumofkUsed = SumofkUsed;
                            ViewBag.SumofkRem = SumofkSend - SumofkUsed;

                        }
                        conn.Close();
                    }//end using

                }

                ViewBag.KitTableOut = KitTableOuts;
            }
            catch
            {
                KitTableOuts.Add(new KitRem
                {
                    Type = "",
                    Send = "0",
                    SumofSend = "0",
                    Used = "0",
                    SumofUsed = "0",
                    Rem = "0",
                    SumofRem = "0",
                    TextColor = ""
                });
                ViewBag.KitTableOut = KitTableOuts;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return View();
        }
        //
        public ActionResult RemittanceWithBOM(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<RemittanceList> remittanceList = new List<RemittanceList>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_remittancewithBOM]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

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
                        Title = reader["Title"].ToString(),
                        Genaration = reader["Genaration"].ToString(),
                        RegistrationType = reader["RegistrationType"].ToString()
                    });
                }
                ViewBag.RemittanceList = remittanceList;
                conn.Close();
            }
            return PartialView();
        }
        //
        public class RemittanceList
        {            
            public string Title { get; set; }
            public string Unit { get; set; }
            public string NumberofSend { get; set; }
            public string Description { get; set; }
            public string FinancialCode { get; set; }
            public string Genaration { get; set; }
            public string RegistrationType { get; set; }
        }
        //GET: DivisionPlans/UpdateSaleWarehouse/5
        [HttpPost]
        public JsonResult UpdateSaleWarehouse(HttpPostedFileBase FileUpload)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            string query = "TRUNCATE TABLE tbl_Warehouses";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            List<string> data = new List<string>();

            #region"بخش انبار"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود و بروزرسانی موجودی انبار از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var List = from a in excelFile.Worksheet<SaleWarehouse>(sheetName) select a;
                    int dataCount = 0;
                    foreach (var a in List)
                    {
                        try
                        {
                            dataCount += 1;
                            SaleWarehouse TU = new SaleWarehouse();
                            TU.CreateDate = DateTime.Now;
                            TU.Creator = User.Identity.Name;
                            TU.CurrentRem = a.CurrentRem;
                            TU.Date = DateTime.Now;
                            TU.FinancialCode = a.FinancialCode;
                            TU.Name = a.Name;
                            TU.Rem = a.Rem;
                            TU.Title = a.Title;
                            TU.Units = a.Units;

                            db.tbl_SaleWarehouses.Add(TU);
                            db.SaveChanges();
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    //if ((System.IO.File.Exists(pathToExcelFile)))
                    //{
                    //    System.IO.File.Delete(pathToExcelFile);
                    //}
                    string Message = "تعداد " + " " + dataCount + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                    ViewBag.Message = Message;
                    RedirectToAction("UploadExcel");
                    return Json(Message, JsonRequestBehavior.AllowGet);
                    //return View();
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        public ActionResult SaleWarehouses()
        {
            var warehouse = db.tbl_SaleWarehouses;
            return View(warehouse.ToList());
        }
        //
        //GET: DivisionPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            if (divisionplan == null)
            {
                return HttpNotFound();
            }
            return View(divisionplan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DivisionPlan divisionplan = db.tbl_DivisionPlans.Find(id);
            //     
            db.tbl_DivisionPlans.Remove(divisionplan);
            db.SaveChanges();
            //
            return RedirectToAction("Index");
        }
        //
        public ActionResult WorkshopDivisionPlanRem(int? WorkshopID, DateTime fromDate, DateTime toDate, bool? Post, int? RegistrationTypeID, int? GenarationID)
        {
            if (RegistrationTypeID == null)
                RegistrationTypeID = 0;
            if (GenarationID == null)
                GenarationID = 0;
            if (WorkshopID != null)
                ViewBag.WorkshopsID = WorkshopID;
            if (Post != true)
                fromDate = Convert.ToDateTime("1399/01/01"); //DateTime.Now.AddYears(-2);
            string Type = "";
            string Send = "0";
            string Used = "0";
            string Rem = "0";
            string CylinderReturnofParts = "0";
            string KitReturnofParts = "0";
            string ValveReturnofParts = "0";
            string CylinderBaseReturnofParts = "0";
            string CylinderCoverReturnofParts = "0";

            List<KitRem> KitTableOuts = new List<KitRem>();
            List<TankRem> TankTableOuts = new List<TankRem>();
            List<TankBaseRem> TankBaseTableOuts = new List<TankBaseRem>();
            List<ValveRem> ValveTableOuts = new List<ValveRem>();
            List<TankCoverRem> TankCoverTableOuts = new List<TankCoverRem>();
            List<OtherthingsRem> OtherthingsTableOuts = new List<OtherthingsRem>();

            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[RemDivisionPlan]", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@workshop", SqlDbType.VarChar).Value = WorkshopID;
                        command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                        command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                        command.Parameters.Add("@RegistrationType", SqlDbType.VarChar).Value = RegistrationTypeID;
                        command.Parameters.Add("@Genaration", SqlDbType.VarChar).Value = GenarationID;

                        conn.Open();
                        reader = command.ExecuteReader();
                        int SumofkSend = 0;
                        int SumofkUsed = 0;
                        int SumofkReturnofParts = 0;
                        //kit rem
                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofkSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofkUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            KitReturnofParts = reader["KitReturnofParts"].ToString();
                            SumofkReturnofParts += Convert.ToInt32(KitReturnofParts);

                            KitTableOuts.Add(new KitRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofkSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofkUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofkSend - SumofkUsed).ToString(),
                                KitReturnofParts = KitReturnofParts
                            });
                        }
                        ViewBag.SumofkSend = SumofkSend;
                        ViewBag.SumofkUsed = SumofkUsed;
                        ViewBag.SumofkRem = SumofkSend - SumofkUsed;
                        ViewBag.SumofkReturnofParts = SumofkReturnofParts;
                        ViewBag.SumofkRem = SumofkSend - SumofkUsed - SumofkReturnofParts;
                        //tank rem
                        reader.NextResult();

                        int SumoftSend = 0;
                        int SumoftUsed = 0;
                        int SumofReturnofParts = 0;

                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumoftSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumoftUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            CylinderReturnofParts = reader["CylinderReturnofParts"].ToString();
                            SumofReturnofParts += Convert.ToInt32(CylinderReturnofParts);

                            TankTableOuts.Add(new TankRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumoftSend.ToString(),
                                Used = Used,
                                SumofUsed = SumoftUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumoftSend - SumoftUsed).ToString(),
                                CylinderReturnofParts = CylinderReturnofParts

                            });
                        }
                        ViewBag.SumoftSend = SumoftSend;
                        ViewBag.SumoftUsed = SumoftUsed;
                        ViewBag.SumoftRem = SumoftSend - SumoftUsed;
                        ViewBag.SumofReturnofParts = SumofReturnofParts;
                        ViewBag.SumoftRem = SumoftSend - SumoftUsed - SumofReturnofParts;                      
                        //valve rem
                        reader.NextResult();
                        int SumofvSend = 0;
                        int SumofvUsed = 0;
                        int SumofvReturnofParts = 0;

                        while (reader.Read())
                        {
                            Type = reader["Type"].ToString();
                            Send = reader["Send"].ToString();
                            SumofvSend += Convert.ToInt32(Send);
                            Used = reader["Used"].ToString();
                            SumofvUsed += Convert.ToInt32(Used);
                            Rem = reader["Rem"].ToString();
                            ValveReturnofParts = reader["ValveReturnofParts"].ToString();
                            SumofvReturnofParts += Convert.ToInt32(ValveReturnofParts);

                            ValveTableOuts.Add(new ValveRem
                            {
                                Type = Type,
                                Send = Send,
                                SumofSend = SumofvSend.ToString(),
                                Used = Used,
                                SumofUsed = SumofvUsed.ToString(),
                                Rem = Rem,
                                SumofRem = (SumofvSend - SumofvUsed).ToString(),
                                ValveReturnofParts = ValveReturnofParts
                            });
                        }
                        ViewBag.SumofvSend = SumofvSend;
                        ViewBag.SumofvUsed = SumofvUsed;
                        ViewBag.SumofvRem = SumofvSend - SumofvUsed;
                        ViewBag.SumofvReturnofParts = SumofvReturnofParts;
                        ViewBag.SumofvRem = SumofvSend - SumofvUsed - SumofvReturnofParts;
                       
                    }
                    conn.Close();
                }//end using
                ViewBag.KitTableOut = KitTableOuts;
                ViewBag.TankTableOut = TankTableOuts;
                ViewBag.ValveTableOut = ValveTableOuts;
            }
            catch
            {
                //ViewBag.TableOut = null;
            }
            ViewBag.fromDate = fromDate.ToShortDateString();
            if (toDate == null)
                toDate = DateTime.Now;
            ViewBag.toDate = toDate.ToShortDateString();
            return PartialView();
        }
        //
        public ActionResult ShowVehicleBOMs(int? VehicleTypeID,int? GenerationID)
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
            //
            ViewBag.VehicleTypeID = new SelectList(list, "Value", "Text");
            ViewBag.VehicleType = VehicleTypeID;
            ViewBag.GenerationID = new SelectList(db.tbl_GenerationofRegulators, "ID", "Title");
            ViewBag.Generation = GenerationID;
            //
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