using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;
using System.Threading;
using System.Globalization;

namespace cngfapco.Controllers
{
    public class ReturnofPartsController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        /// <summary>
        /// ثبت و صدور صورتحساب فروش کالا/ خدمات
        /// </summary>
        /// <param name="invoices"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegisterAction(List<ReturnofParts> invoices)
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
                    
                    //
                    Thread.Sleep(1000);
                    return Json(new { success = true, responseText = "اطلاعات ارسالی شما با موفقیت ثبت شد" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // GET: ReturnofParts
        public async Task<ActionResult> Index()
        {
            var tbl_ReturnofParts = db.tbl_ReturnofParts.Include(r => r.VehicleTypes).Include(r => r.Workshop).Include(r => r.Equipments);
            return View(await tbl_ReturnofParts.ToListAsync());
        }

        // GET: ReturnofParts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnofParts returnofParts = await db.tbl_ReturnofParts.FindAsync(id);
            if (returnofParts == null)
            {
                return HttpNotFound();
            }
            return View(returnofParts);
        }

        // GET: ReturnofParts/Create
        //var equipments = db.tbl_EquipmentList.Include(E=>E.Parent).Where(e=>e.Pid!=null).ToList();
        //List<EquipmentList> equipmentlist = new List<EquipmentList>();
        //foreach (var item in equipments)
        //{
        //    equipmentlist.Add(new EquipmentList
        //    {
        //        ID = item.ID,
        //        Title = item.Title
        //    });
        //}
        public ActionResult Create()
        {
            var returnList = db.tbl_ReturnofParts.Include(r=>r.Equipments).Include(r => r.VehicleTypes).Include(r => r.Workshop);
            var vehicle = db.tbl_VehicleTypes.ToList();
            List<VehicleType> selectList = new List<VehicleType>();
            foreach(var item in vehicle)
            {
                selectList.Add(new VehicleType
                {
                    ID=item.ID,
                    Type=item.Type + " - " + item.Description
                });
            }
            ViewBag.VehicleTypeID = new SelectList(selectList, "ID", "Type");
            //            
            ViewBag.EquipmentID = new SelectList(db.tbl_EquipmentList.Where(e => e.Pid != null), "ID", "Title");
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            ViewBag.Transferee = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View(returnList.Where(r => r.ID == 0).ToList());
        }
        //
        public JsonResult GetFreeSaleEquipment(int id, int? type)
        {
            var listofServices = db.tbl_EquipmentList.Where(l => l.ID == id).SingleOrDefault();
            List<EquipmentList> services = new List<EquipmentList>();
            services.Add(new EquipmentList
            {
                ID = listofServices.ID,
                Title = listofServices.Title,
                FinancialCode = listofServices.FinancialCode,
                Value = listofServices.Value
            });

            return Json(services, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult GetEquipmentType(int? id)
        {
            var parent = "";
            var equipmentType = db.tbl_EquipmentList.Where(t => t.ID == id).SingleOrDefault();
            if (equipmentType.Pid == 1)
                parent = "کیت"; //db.tbl_EquipmentList.Where(t => t.ID == equipmentType.Pid).SingleOrDefault().Title;
            else
                parent = equipmentType.Address;
            //
            return Json(new { success = true, responseText = "نوع قطعه اصلی", parent = parent }, JsonRequestBehavior.AllowGet);
        }
        //
        //Get GetBOMs
        public JsonResult GetBOMs(int id, string txtTypeofPiece)
        {
            string countrystring = "";
            //cheked and get data related with Type of Piece
            if (txtTypeofPiece.Equals("کیت"))
                countrystring = "select t1.EquipmentListID,t2.FinancialCode,t2.Title As EqTitle,t3.Title As GTitle,t1.GenerationID from tbl_BOMs t1 inner join tbl_EquipmentList t2 on t1.EquipmentListID = t2.ID inner join tbl_GenerationofRegulators t3 on t1.GenerationID=t3.ID where t1.VehicleTypeID='" + id + "' and t2.Pid=1";
            if (!txtTypeofPiece.Equals("کیت"))
                countrystring = "select t1.EquipmentListID,t2.FinancialCode,t2.Title As EqTitle,t3.Title As GTitle,t1.GenerationID from tbl_BOMs t1 inner join tbl_EquipmentList t2 on t1.EquipmentListID = t2.ID inner join tbl_GenerationofRegulators t3 on t1.GenerationID=t3.ID where t1.VehicleTypeID='" + id + "' and t2.Address like N'" + "%" + txtTypeofPiece + "%" + "'";
            if (txtTypeofPiece.Equals("سایر"))
                countrystring = "select t1.EquipmentListID,t2.FinancialCode,t2.Title As EqTitle,t3.Title As GTitle,t1.GenerationID from tbl_BOMs t1 inner join tbl_EquipmentList t2 on t1.EquipmentListID = t2.ID inner join tbl_GenerationofRegulators t3 on t1.GenerationID=t3.ID where t1.VehicleTypeID='" + id + "'";
            //
            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = "--بدون انتخاب--",
                Value = "0"
            });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]) + " - " + Convert.ToString(row.ItemArray[2]) + " - " + Convert.ToString(row.ItemArray[3]) + "-" + Convert.ToString(row.ItemArray[4]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //

        // POST: ReturnofParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Code,WorkshopID,VehicleTypeID,TypeofPiece,Transferee,Date,Action,NumberofSend,Description,EquipmentID")] ReturnofParts returnofParts)
        {
            returnofParts.CreateDate = DateTime.Now;
            returnofParts.Creator = db.tbl_Users.Where(u => u.Username.Equals(User.Identity.Name)).SingleOrDefault().UserID;
            if (ModelState.IsValid)
            {
                db.tbl_ReturnofParts.Add(returnofParts);
                await db.SaveChangesAsync();
                //
                if (returnofParts.Action.Equals("انتقال"))
                {
                    DivisionPlan divisionplan = new DivisionPlan();
                    var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                    var FapCode = db.tbl_Workshops.Where(w => w.ID == returnofParts.Transferee).SingleOrDefault().FapCode;
                    int Count = db.tbl_DivisionPlans.Where(v => v.WorkshopID == returnofParts.Transferee).Count() + 1;
                    string MaxofRow = "1";
                    var hasRow = db.tbl_DivisionPlans.ToList();

                    if (Count < 10)
                        MaxofRow = "00" + Count;
                    if (Count >= 10 && Count < 100)
                        MaxofRow = "0" + Count;
                    if (Count >= 100)
                        MaxofRow = Count.ToString();

                    divisionplan.Creator = user.UserID;
                    divisionplan.CreateDate = DateTime.Now;
                    divisionplan.Code = FapCode + "-" + MaxofRow;
                    divisionplan.WorkshopID = returnofParts.Transferee;
                    divisionplan.Description= "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                    divisionplan.Confirmation = true;
                    divisionplan.ConfirmationDate = DateTime.Now;
                    divisionplan.ConfirmationUser = user.UserID;
                    divisionplan.Send = true;
                    divisionplan.SendDate = DateTime.Now;
                    divisionplan.Sender = user.UserID;
                    divisionplan.FinalCheck = true;
                    divisionplan.FinalCheckDate = DateTime.Now;
                    db.tbl_DivisionPlans.Add(divisionplan);
                    db.SaveChanges();
                    //
                    returnofParts.Code = divisionplan.Code;
                    db.Entry(returnofParts).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //
                    KitDivisionPlan kit = new KitDivisionPlan();
                    TankDivisionPlan cylinder = new TankDivisionPlan();
                    TankBaseDivisionPlan cylinderbase = new TankBaseDivisionPlan();
                    TankCoverDivisionPlan cylindercover = new TankCoverDivisionPlan();
                    ValveDivisionPlan valve = new ValveDivisionPlan();
                    //-------------------------------add to kit division plans old division system--------------------------------------
                    if (returnofParts.TypeofPiece.Equals("کیت"))
                    {
                        kit.DivisionPlanID = divisionplan.ID;
                        kit.Number = returnofParts.NumberofSend;
                        kit.NumberofSend = returnofParts.NumberofSend;
                        kit.VehicleTypeID = returnofParts.VehicleTypeID;
                        kit.Description = "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به" + " " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                        db.tbl_KitDivisionPlans.Add(kit);
                        db.SaveChanges();
                    }                    
                    //-------------------------------add to cylinder division plans old division system--------------------------------------
                    if (returnofParts.TypeofPiece.Equals("مخزن"))
                    {
                        var typofTank = db.tbl_TypeofTanks.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylinder.DivisionPlanID = divisionplan.ID;
                        cylinder.Number = returnofParts.NumberofSend;
                        cylinder.NumberofSend = returnofParts.NumberofSend;
                        cylinder.TypeofTankID = typofTank.ID;
                        cylinder.Description = "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به" + " " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                        cylinder.TankConstractorID = null;
                        db.tbl_TankDivisionPlans.Add(cylinder);
                        db.SaveChanges();
                    }                    
                    //-------------------------------add to cylinder base division plans old division system--------------------------------------
                    if (returnofParts.TypeofPiece.Equals("پایه مخزن"))
                    {
                        var typoftankBase = db.tbl_TypeofTankBases.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylinderbase.DivisionPlanID = divisionplan.ID;
                        cylinderbase.Number = returnofParts.NumberofSend;
                        cylinderbase.NumberofSend = returnofParts.NumberofSend;
                        cylinderbase.TypeofTankBaseID = typoftankBase.ID;
                        cylinderbase.Description = "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به" + " " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                        db.tbl_TankBaseDivisionPlans.Add(cylinderbase);
                        db.SaveChanges();
                    }                    
                    //-------------------------------add to cylinder cover division plans old division system--------------------------------------
                    if (returnofParts.TypeofPiece.Equals("کاور مخزن"))
                    {
                        var typoftankCover = db.tbl_TypeofTankCovers.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylindercover.DivisionPlanID = divisionplan.ID;
                        cylindercover.Number = returnofParts.NumberofSend;
                        cylindercover.NumberofSend = returnofParts.NumberofSend;
                        cylindercover.TypeofTankCoverID = typoftankCover.ID;
                        cylindercover.Description = "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به" + " " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                        db.tbl_TankCoverDivisionPlans.Add(cylindercover);
                        db.SaveChanges();
                    }                    
                    //-------------------------------add to valve division plans old division system--------------------------------------
                    if (returnofParts.TypeofPiece.Equals("شیر مخزن"))
                    {
                        valve.DivisionPlanID = divisionplan.ID;
                        valve.Number = returnofParts.NumberofSend;
                        valve.NumberofSend = returnofParts.NumberofSend;
                        valve.Type = "دستی";
                        valve.Description = "انتقال مستقیم از کارگاه " + Helper.Helpers.GetWorkshops(returnofParts.WorkshopID).Title + "به" + " " + Helper.Helpers.GetWorkshops(returnofParts.Transferee).Title;
                        valve.ValveConstractorID = 1;
                        db.tbl_ValveDivisionPlans.Add(valve);
                        db.SaveChanges();
                    }                    
                    //
                }
                //
                return RedirectToAction("Index");
            }
            //
            var vehicle = db.tbl_VehicleTypes.ToList();
            List<VehicleType> selectList = new List<VehicleType>();
            foreach (var item in vehicle)
            {
                selectList.Add(new VehicleType
                {
                    ID = item.ID,
                    Type = item.Type + " - " + item.Description
                });
            }
            ViewBag.EquipmentID = new SelectList(db.tbl_EquipmentList.Where(e => e.Pid != null), "ID", "Title", returnofParts.EquipmentID);
            ViewBag.VehicleTypeID = new SelectList(selectList, "ID", "Type", returnofParts.VehicleTypeID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.WorkshopID);
            ViewBag.Transferee = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.Transferee);
            return View(returnofParts);
        }
        //
        /// <summary>
        /// ثبت اطلاعات برگشت/ انتقال اقلام و تجهیزات
        /// </summary>
        /// <param name="Return of Parts"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddReturnofParts(List<ReturnofParts> items, string Description, HttpPostedFileBase Attachment)
        {
            int countter = 0;
            var parent = "";
            int GenarationID = 1;
            int RegistrationID = 1;
            DivisionPlanBOM addtoBOM = new DivisionPlanBOM();
            DivisionPlan divisionplan = new DivisionPlan();
            ReturnofParts returnofParts = new ReturnofParts();
            foreach (var item in items)
            {
                //
                var boms= db.tbl_BOMs.Where(b => b.VehicleTypeID == item.VehicleTypeID && b.GenerationID.ToString().Equals(item.Code) && b.EquipmentListID == item.EquipmentID && b.Presentable == true).Include(b => b.EquipmentList);//
                int? bomId = null;
                if (boms != null)
                {
                    bomId = boms.SingleOrDefault().ID;
                    if (boms.SingleOrDefault().GenerationID != 5)
                        GenarationID = boms.SingleOrDefault().GenerationID.Value;
                    //GenarationID = boms.SingleOrDefault().GenerationID.HasValue? boms.SingleOrDefault().GenerationID.Value:GenarationID;
                    else
                    {
                        GenarationID = 5;
                        RegistrationID = 2;
                    }

                }
                //for give equipment Type for insert value in to Division plans section
                var equipmentType = db.tbl_EquipmentList.Where(t => t.ID == item.EquipmentID).SingleOrDefault();
                if (equipmentType.Pid == 1)
                    parent = "کیت";
                else
                    parent = equipmentType.Address;
                //
                countter += 1;               
                //
                //if (item.Action.Equals("انتقال"))
                //{
                    var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                    var FapCode = db.tbl_Workshops.Where(w => w.ID == item.Transferee).SingleOrDefault().FapCode;
                    var hasRow = db.tbl_DivisionPlans.Where(d => d.WorkshopID == item.Transferee);
                    int Count = 0;
                    string MaxofRow = "1";
                    //
                    string fullText = item.Action + "  " + "قطعه " + equipmentType.Title + "(" + equipmentType.Address + ")" + " " + "از کارگاه " + Helper.Helpers.GetWorkshops(item.WorkshopID).Title + " " + "به " + Helper.Helpers.GetWorkshops(item.Transferee).Title;
                    //
                    if (countter == 1)
                    {                        
                        if (hasRow != null)
                            Count = hasRow.ToList().Count() + 1;
                        //
                        if (Count < 10)
                            MaxofRow = "00" + Count;
                        if (Count >= 10 && Count < 100)
                            MaxofRow = "0" + Count;
                        if (Count >= 100)
                            MaxofRow = Count.ToString();
                        //
                        divisionplan.Creator = user.UserID;
                        divisionplan.CreateDate = DateTime.Now;
                        divisionplan.Code = FapCode + "-" + MaxofRow;
                        divisionplan.WorkshopID = item.Transferee;
                        divisionplan.Description = Description;
                        divisionplan.Confirmation = false;
                        divisionplan.ConfirmationDate = null;
                        divisionplan.ConfirmationUser = null;
                        divisionplan.Send = false;
                        divisionplan.SendDate = null;
                        divisionplan.Sender = null;
                        divisionplan.FinalCheck = true;
                        divisionplan.FinalCheckDate = DateTime.Now;
                        //
                        if (Attachment != null)
                        {
                            if (divisionplan.Attachment != null)
                            {
                                System.IO.File.Delete(Server.MapPath("/UploadedFiles/DivisionPlans/ReturnofParts/" + divisionplan.Attachment));
                            }

                            divisionplan.Attachment = Attachment.FileName;

                            string ImagePath = Server.MapPath("/UploadedFiles/DivisionPlans/ReturnofParts/" + divisionplan.Attachment);
                            Attachment.SaveAs(ImagePath);
                        }
                        //
                        db.tbl_DivisionPlans.Add(divisionplan);
                        db.SaveChanges();
                    }

                    //insert items in to ReturnofParts
                    returnofParts.Action = item.Action;
                    returnofParts.Code = divisionplan.Code;
                    returnofParts.CreateDate = DateTime.Now;
                    returnofParts.Creator = divisionplan.Creator;
                    returnofParts.Date = DateTime.Now;
                    returnofParts.Description = item.Description;
                    returnofParts.EquipmentID = item.EquipmentID;
                    returnofParts.NumberofSend = item.NumberofSend;
                    returnofParts.Transferee = item.Transferee;
                    returnofParts.VehicleTypeID = item.VehicleTypeID;
                    returnofParts.WorkshopID = item.WorkshopID;
                    returnofParts.TypeofPiece = parent;
                    db.tbl_ReturnofParts.Add(returnofParts);
                    db.SaveChanges();

                    //insert items in to Division Plan BOM
                    addtoBOM.DivisionPlanID = divisionplan.ID;                    
                    addtoBOM.BOMID = bomId;
                    addtoBOM.CreateDate = DateTime.Now;
                    addtoBOM.Creator = User.Identity.Name;
                    addtoBOM.Description = item.Description;
                    addtoBOM.Number = item.NumberofSend;
                    addtoBOM.NumberofSend = item.NumberofSend;
                    addtoBOM.GenarationID = GenarationID;
                    addtoBOM.RegistrationTypeID = RegistrationID;
                    db.tbl_DivisionPlanBOMs.Add(addtoBOM);
                    db.SaveChanges();
                    //
                    //insert value in to division plans sections
                    KitDivisionPlan kit = new KitDivisionPlan();
                    TankDivisionPlan cylinder = new TankDivisionPlan();
                    TankBaseDivisionPlan cylinderbase = new TankBaseDivisionPlan();
                    TankCoverDivisionPlan cylindercover = new TankCoverDivisionPlan();
                    ValveDivisionPlan valve = new ValveDivisionPlan();
                    OtherThingsDivisionPlan otherthing = new OtherThingsDivisionPlan();
                    //-------------------------------add to kit division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("کیت"))
                    {
                        kit.DivisionPlanID = divisionplan.ID;
                        kit.Number = returnofParts.NumberofSend;
                        kit.NumberofSend = returnofParts.NumberofSend;
                        kit.VehicleTypeID = returnofParts.VehicleTypeID;
                        kit.Description = fullText;
                        kit.GenarationID = GenarationID;
                        kit.RegistrationTypeID = RegistrationID;
                        db.tbl_KitDivisionPlans.Add(kit);
                        db.SaveChanges();
                    }
                    //-------------------------------add to cylinder division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("مخزن"))
                    {
                        var typofTank = db.tbl_TypeofTanks.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylinder.DivisionPlanID = divisionplan.ID;
                        cylinder.Number = returnofParts.NumberofSend;
                        cylinder.NumberofSend = returnofParts.NumberofSend;
                        cylinder.TypeofTankID = typofTank.ID;
                        cylinder.Description = fullText;
                        cylinder.TankConstractorID = null;
                        cylinder.GenarationID = GenarationID;
                        cylinder.RegistrationTypeID = RegistrationID;
                        db.tbl_TankDivisionPlans.Add(cylinder);
                        db.SaveChanges();
                    }
                    //-------------------------------add to cylinder base division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("پایه مخزن"))
                    {
                        var typoftankBase = db.tbl_TypeofTankBases.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylinderbase.DivisionPlanID = divisionplan.ID;
                        cylinderbase.Number = returnofParts.NumberofSend;
                        cylinderbase.NumberofSend = returnofParts.NumberofSend;
                        cylinderbase.TypeofTankBaseID = typoftankBase.ID;
                        cylinderbase.Description = fullText;
                        cylinderbase.GenarationID = GenarationID;
                        cylinderbase.RegistrationTypeID = RegistrationID;
                        db.tbl_TankBaseDivisionPlans.Add(cylinderbase);
                        db.SaveChanges();
                    }
                    //-------------------------------add to cylinder cover division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("کاور مخزن"))
                    {
                        var typoftankCover = db.tbl_TypeofTankCovers.Where(t => t.VehicleTypeId == returnofParts.VehicleTypeID).SingleOrDefault();
                        cylindercover.DivisionPlanID = divisionplan.ID;
                        cylindercover.Number = returnofParts.NumberofSend;
                        cylindercover.NumberofSend = returnofParts.NumberofSend;
                        cylindercover.TypeofTankCoverID = typoftankCover.ID;
                        cylindercover.Description = fullText;
                        cylindercover.GenarationID = GenarationID;
                        cylindercover.RegistrationTypeID = RegistrationID;
                        db.tbl_TankCoverDivisionPlans.Add(cylindercover);
                        db.SaveChanges();
                    }
                    //-------------------------------add to valve division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("شیر مخزن"))
                    {
                        valve.DivisionPlanID = divisionplan.ID;
                        valve.Number = returnofParts.NumberofSend;
                        valve.NumberofSend = returnofParts.NumberofSend;
                        valve.Type = "دستی";
                        valve.Description = fullText;
                        valve.ValveConstractorID = 1;
                        valve.GenarationID = GenarationID;
                        valve.RegistrationTypeID = RegistrationID;
                        db.tbl_ValveDivisionPlans.Add(valve);
                        db.SaveChanges();
                    }
                    //-------------------------------add to Other Things division plans old division system--------------------------------------
                    if (item.TypeofPiece.Equals("سایر"))
                    {
                        otherthing.DivisionPlanID = divisionplan.ID;
                        otherthing.Number = returnofParts.NumberofSend;
                        otherthing.NumberofSend = returnofParts.NumberofSend;
                        otherthing.Description = fullText;
                        otherthing.DiThingsID = null;
                        otherthing.GenarationID = GenarationID;
                        otherthing.RegistrationTypeID = RegistrationID;
                        db.tbl_OtherThingsDivisionPlans.Add(otherthing);
                        db.SaveChanges();
                    }
                    //
                //}
            }
            //
            Thread.Sleep(1000);
            return Json(new { success = true, responseText = "اطلاعات با موفقیت ثبت شد!" }, JsonRequestBehavior.AllowGet);
        }
        // GET: ReturnofParts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnofParts returnofParts = await db.tbl_ReturnofParts.FindAsync(id);
            if (returnofParts == null)
            {
                return HttpNotFound();
            }
            //
            var vehicle = db.tbl_VehicleTypes.ToList();
            List<VehicleType> selectList = new List<VehicleType>();
            foreach (var item in vehicle)
            {
                selectList.Add(new VehicleType
                {
                    ID = item.ID,
                    Type = item.Type + " - " + item.Description
                });
            }
            ViewBag.EquipmentID = new SelectList(db.tbl_EquipmentList.Where(e => e.Pid != null), "ID", "Title", returnofParts.EquipmentID);
            ViewBag.VehicleTypeID = new SelectList(selectList, "ID", "Type", returnofParts.VehicleTypeID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.WorkshopID);
            ViewBag.Transferee = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.Transferee);
            return View(returnofParts);
        }

        // POST: ReturnofParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Code,WorkshopID,VehicleTypeID,TypeofPiece,Transferee,Date,Action,NumberofSend,Description,EquipmentID")] ReturnofParts returnofParts)
        {
            returnofParts.CreateDate = DateTime.Now;
            returnofParts.Creator = db.tbl_Users.Where(u => u.Username.Equals(User.Identity.Name)).SingleOrDefault().UserID;
            if (ModelState.IsValid)
            {
                db.Entry(returnofParts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //
            var vehicle = db.tbl_VehicleTypes.ToList();
            List<VehicleType> selectList = new List<VehicleType>();
            foreach (var item in vehicle)
            {
                selectList.Add(new VehicleType
                {
                    ID = item.ID,
                    Type = item.Type + " - " + item.Description
                });
            }
            ViewBag.EquipmentID = new SelectList(db.tbl_EquipmentList.Where(e => e.Pid != null), "ID", "Title", returnofParts.EquipmentID);
            ViewBag.VehicleTypeID = new SelectList(selectList, "ID", "Type", returnofParts.VehicleTypeID);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.WorkshopID);
            ViewBag.Transferee = new SelectList(db.tbl_Workshops, "ID", "Title", returnofParts.Transferee);
            return View(returnofParts);
        }

        // GET: ReturnofParts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnofParts returnofParts = await db.tbl_ReturnofParts.FindAsync(id);
            if (returnofParts == null)
            {
                return HttpNotFound();
            }
            return View(returnofParts);
        }

        // POST: ReturnofParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReturnofParts returnofParts = await db.tbl_ReturnofParts.FindAsync(id);
            db.tbl_ReturnofParts.Remove(returnofParts);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
