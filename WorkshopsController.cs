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
using System.Globalization;

namespace cngfapco.Controllers
{
    //[RBAC]
    [Authorize]
    public class WorkshopsController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        // GET: Workshops
        public ActionResult Index()
        {
            var tbl_Workshops = db.tbl_Workshops.Include(w => w.City).Include(w=>w.Users).OrderByDescending(w=>w.ID);
            var tbl_Users = db.tbl_Users.ToList();
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            List<WorkshopsTable> tableOuts = new List<WorkshopsTable>();

            foreach (var item in tbl_Workshops.ToList())
            {
                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                User _user = db.tbl_Users.Find(userId.UserID);

                //var operators=null;
                string auditor = "یافت نشد";
                try
                {
                    //Operator workuser = db.tbl_Operators.Where(w => w.WorkshopID == item.ID && w.Status==true).SingleOrDefault();
                    //operators = workuser.Name + " " + workuser.Family + " " + workuser.MobileNumber;
                  AuditCompany auditcomponies = db.tbl_AuditComponies.Find(item.CompaniesID);
                    if (auditcomponies != null)
                        auditor = auditcomponies.Title;
                }
                catch { }              

                if (workshop.Users.Contains(_user))
                {
                    tableOuts.Add(new WorkshopsTable
                    {
                        ID = item.ID.ToString(),
                        FapCode = item.FapCode,
                        Title=item.Title,
                        FullName = item.OwnerName +" "+ item.OwnerFamily,
                        PhoneNumber=item.PhoneNumber,
                        MobileNumber=item.MobileNumber,
                        FaxNumber=item.FaxNumber,
                        AuditCompony= auditor,
                        Auditor = item.Auditor,
                        Email =item.Email,
                        Address=item.Address,
                        BusinessLicense=item.BusinessLicense,
                        Location= item.City.State.Title + " " + item.City.Title,
                        Economicalnumber = item.Economicalnumber,
                        Registrationnumber=item.Registrationnumber

                    });
                }
            }
            ViewBag.tableOuts = tableOuts;
            return View();
        }
        public class WorkshopsTable
        {
            public string ID { get; set; }
            public string FapCode { get; set; }
            public string Title { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string FaxNumber { get; set; }
            public string AuditCompony { get; set; }
            public string Auditor { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string BusinessLicense { get; set; }
            public string Location { get; set; }
            public string Economicalnumber { get; set; }
            public string Registrationnumber { get; set; }

        }
        // GET: Workshops/Details/5
        public ActionResult Details(int? id)
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
        //Get Cities
        public JsonResult GetCities(int id)
        {
            string countrystring = "select * from [dbo].[tbl_Cities] where [StateID]='" + id + "'";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Text = "--انتخاب شهر--", Value = "0" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            }
            
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        //Get Cities
        public JsonResult GetCities2(int? id)
        {
            string countrystring = "select * from [dbo].[tbl_Cities] where [ID]='" + id + "'";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        // GET: Workshops/Create
        public ActionResult Create()
        {
            var users = db.tbl_Users.Where(u => (u.WorkshopID == 1 || u.WorkshopID == 8 || u.WorkshopID == 9) && u.UserID!=33).ToList();
            List<User> addlist = new List<User>();
            foreach (var item in users)
            {
                addlist.Add(new Models.User
                {
                    UserID = item.UserID,
                    Firstname = item.Firstname + " " + item.Lastname
                });
            }

            ViewBag.Users = new SelectList(addlist, "UserID", "Firstname");
            ViewBag.State = new SelectList(db.tbl_States, "ID", "Title");
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title");
            //ViewBag.CityID = new SelectList(db.tbl_Cities, "ID", "Title");
            //ViewBag.CityID = db.tbl_Cities.ToList();
            return View();
        }

        // POST: Workshops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,CityID,OwnerName,OwnerFamily,PhoneNumber,MobileNumber,FaxNumber,Email,Address,Creator,CreateDate,isServices,CompaniesID,Auditor,Registrationnumber,Economicalnumber")] Workshop workshop,HttpPostedFileBase BusinessLicense, int?[] Users)
        {
            if (workshop.isServices == null)
                workshop.isServices = false;

            int fapCode = db.tbl_Workshops.Count();
            if (fapCode < 10)
                workshop.FapCode = "FAP00" + (fapCode+1);
            if (fapCode>10 && fapCode<100)
                workshop.FapCode = "FAP0" + (fapCode + 1);
            if (fapCode > 100)
                workshop.FapCode = "FAP" + (fapCode + 1);
            //
            if (BusinessLicense != null)
            {
                if (workshop.BusinessLicense != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/BusinessLicense/" + workshop.BusinessLicense));
                }

                workshop.BusinessLicense = BusinessLicense.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/BusinessLicense/" + workshop.BusinessLicense);

                BusinessLicense.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                workshop.CreateDate = DateTime.Now;
                workshop.Creator = User.Identity.Name;

                db.tbl_Workshops.Add(workshop);
                db.SaveChanges();
                //asigned users permission to new workshop
                if (Users.Count() == 0)
                {
                    var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                    Workshop workshops = db.tbl_Workshops.Find(workshop.ID);
                    User user = db.tbl_Users.Find(userId);
                    if (!user.Workshops.Contains(workshops))
                    {
                        user.Workshops.Add(workshops);
                        db.SaveChanges();
                    }
                }
                else
                {
                    for(int i = 0; i < Users.Count(); i++)
                    {
                        Workshop workshops = db.tbl_Workshops.Find(workshop.ID);
                        User user = db.tbl_Users.Find(Users[i]);
                        if (!user.Workshops.Contains(workshops))
                        {
                            user.Workshops.Add(workshops);
                            db.SaveChanges();
                        }
                    }                   
                }
                //

                return RedirectToAction("Index");
            }
            
            ViewBag.State = new SelectList(db.tbl_States, "ID", "Title");
            ViewBag.CityID = new SelectList(db.tbl_Cities, "ID", "Title", workshop.CityID);
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", workshop.CompaniesID);
            return View(workshop);
        }
       
        //[RBAC]

        // GET: Workshops/Edit/5
        public ActionResult Edit(int? id)
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
            //if (workshop.isServices == true)
            //    ViewBag.isServices = true;
            ViewBag.State= new SelectList(db.tbl_States, "ID", "Title", workshop.City.StateID);
            ViewBag.CityID = new SelectList(db.tbl_Cities, "ID", "Title", workshop.CityID);
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", workshop.CompaniesID);
            return View(workshop);
        }

        // POST: Workshops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,CityID,OwnerName,OwnerFamily,PhoneNumber,MobileNumber,FaxNumber,Email,Address,Creator,CreateDate,BusinessLicense,FapCode,isServices,CompaniesID,Auditor,Registrationnumber,Economicalnumber,closedDate,IRNGVCod")] Workshop workshop,bool? isServices,bool? closedServices, HttpPostedFileBase BusinessLicense)
        {
            //if (workshop.isServices == null)
            //    workshop.isServices = false;

            if (BusinessLicense != null)
            {
                if (workshop.BusinessLicense != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/BusinessLicense/" + workshop.BusinessLicense));
                }

                workshop.BusinessLicense = BusinessLicense.FileName;// + Path.GetExtension(CommissionImage.FileName);

                string ImagePath = Server.MapPath("/UploadedFiles/BusinessLicense/" + workshop.BusinessLicense);

                BusinessLicense.SaveAs(ImagePath);
            }
            //
            workshop.CreateDate = DateTime.Now;
            workshop.Creator = User.Identity.Name;
            workshop.isServices = isServices;
            workshop.closedServices = closedServices;
            //
            //Workshop _workshop = new Workshop();
            //_workshop.Address = workshop.Address;
            //_workshop.BusinessLicense = workshop.BusinessLicense;
            //_workshop.CityID = workshop.CityID;
            //_workshop.CreateDate = workshop.CreateDate;
            //_workshop.Creator = workshop.Creator;
            //_workshop.Email = workshop.Email;
            //_workshop.FapCode = workshop.FapCode;
            //_workshop.FaxNumber = workshop.FaxNumber;
            //_workshop.MobileNumber = workshop.MobileNumber;
            //_workshop.OwnerFamily = workshop.OwnerFamily;
            //_workshop.OwnerName = workshop.OwnerName;
            //_workshop.PhoneNumber = workshop.PhoneNumber;
            //_workshop.Title = workshop.Title;
            //_workshop.Users = workshop.Users;

            if (ModelState.IsValid)
            {                 
                db.Entry(workshop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            int? stateId = db.tbl_Cities.Find(workshop.CityID).StateID;
            ViewBag.State = new SelectList(db.tbl_States, "ID", "Title", stateId);
            ViewBag.CityID = new SelectList(db.tbl_Cities, "ID", "Title", workshop.CityID);
            ViewBag.CompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", workshop.CompaniesID);
            return View(workshop);
        }
        //[RBAC]
        // GET: Workshops/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Workshops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Workshop workshop = await db.tbl_Workshops.FindAsync(id);
            db.tbl_Workshops.Remove(workshop);
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
