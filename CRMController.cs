using cngfapco.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace cngfapco.Controllers
{
    public class CRMController : Controller
    {
        private ContextDB db = new ContextDB();
        private static ContextDB dbStatic = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        // GET: CRM / ViewComments
        public ActionResult ViewCommentPages()
        {
            return View();
        }
        // GET: CRM
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                id = "3019";
            ViewBag.Id = id;
            var indexItems = db.tbl_CRMIndexes;
            return View(indexItems.ToList());
        }

        // GET: CRM / ViewComments
        public ActionResult ViewComments()
        {
            string OwnersId = "";
            string FullName = "";
            DateTime? CreateDate = null;
            DateTime? RegistrationDate = null;
            string VehicleType = "";
            string Workshop = "";

            //
            List<ViewCommentList> tableOuts = new List<ViewCommentList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            //
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_CRMViewComments]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add("@", SqlDbType.VarChar).Value = ;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        OwnersId = reader["OwnersId"].ToString();
                        FullName = reader["FullName"].ToString();
                        CreateDate =Convert.ToDateTime(reader["CreateDate"].ToString());
                        VehicleType = reader["VehicleType"].ToString();
                        Workshop = reader["Workshop"].ToString();
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"].ToString());

                        tableOuts.Add(new ViewCommentList
                        {
                            CreateDate=CreateDate.GetValueOrDefault().ToShortDateString(),
                            FullName=FullName,
                            OwnersId=OwnersId,
                            RegistrationDate = RegistrationDate.GetValueOrDefault().ToShortDateString(),
                            VehicleType=VehicleType,
                            Workshop=Workshop
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
        public class ViewCommentList
        {
            public string FullName { get; set; }
            public string CreateDate { get; set; }
            public string OwnersId { get; set; }
            public string VehicleType { get; set; }
            public string RegistrationDate { get; set; }
            public string Workshop { get; set; }
        }
        //
        public ActionResult AnswerQuestionResults()
        {
            string ip = "";
            string Mobile = "";
            string FullName = "";
            string Type = "";
            string Workshop = "";
            DateTime? CreateDate = null;
            DateTime? RegisterDate = null;
            //
            List<AnswerList> tableOuts = new List<AnswerList>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlDataReader reader;
            //
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_AnswerQuestionResults]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add("@", SqlDbType.VarChar).Value = ;
                    conn.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ip = reader["ip"].ToString();
                        FullName = reader["FullName"].ToString();
                        Mobile = reader["Mobile"].ToString();
                        Workshop = reader["Workshop"].ToString();
                        Type = reader["Type"].ToString();
                        CreateDate = Convert.ToDateTime(reader["CreateDate"].ToString());
                        if (!string.IsNullOrEmpty(reader["RegisterDate"].ToString()))
                            RegisterDate = Convert.ToDateTime(reader["RegisterDate"].ToString());
                        else
                            RegisterDate = null;

                        tableOuts.Add(new AnswerList
                        {
                            CreateDate = CreateDate.GetValueOrDefault(),
                            FullName = FullName,
                            ip = ip,
                            Mobile=Mobile,
                            Type=Type,
                            Workshop=Workshop,
                            RegisterDate=RegisterDate.HasValue?RegisterDate.GetValueOrDefault().ToShortDateString():null
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
        public class AnswerList
        {
            public DateTime? CreateDate { get; set; }
            public string FullName { get; set; }
            public string ip { get; set; }
            public string Mobile { get; set; }
            public string Type { get; set; }
            public string Workshop { get; set; }
            public string RegisterDate { get; set; }
        }
        // GET: CRM/AnswerQuestionDetails/5
        public ActionResult AnswerQuestionDetails(string ip, string Details)
        {
            ViewBag.Details = Details;

            if (ip == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var crm = db.tbl_AnswerQuestions.Where(c => c.ip == ip);
            
            if (crm == null)
            {
                return HttpNotFound();
            }
            return View(crm.ToList());
        }
        //
        public JsonResult Vote(int[] model)
        {
            // do something:
            // model.Stars

            return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
        }
        public class YourModel
        {
            public int[] Stars { get; set; }
        }        

        // GET: Add to CRM 
        [HttpPost]
        public JsonResult AddResults(int[] value,int[] value2,int? OwnersId, string Description1, string Description2, string Description3)
        {
            CRM addCRM = new CRM();
            var existItem = db.tbl_CRMs.Where(c => c.OwnersId == OwnersId).Count();
            if(existItem!=0)
            {
                return Json(new { result = "ضمن تشکر، شما قبلا در نظرسنجی شرکت نموده اید!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (value != null)
                {
                    for (int i = 0; i < value.Count(); i++)
                    {
                        addCRM.CreateDate = DateTime.Now;
                        addCRM.Description1 = Description1;
                        addCRM.Description2 = Description2;
                        addCRM.IndexId = value2[i];
                        addCRM.OwnersId = OwnersId;
                        addCRM.Point = value[i];
                        addCRM.Suggestion = Description3;
                        db.tbl_CRMs.Add(addCRM);
                        db.SaveChanges();
                        // do something here
                    }

                    return Json(new { result = "نظرات شما با موفقیت ثبت شد!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "هیچ موردی انتخاب نشده است!" }, JsonRequestBehavior.AllowGet);
                }
            }
            
        }

        // GET: CRM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var crm = db.tbl_CRMs.Where(c=>c.OwnersId==id);

            if (crm == null)
            {
                return HttpNotFound();
            }
            return View(crm.ToList());
        }

        // GET: CRM/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CRM/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CRM/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CRM/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CRM/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CRM/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
        public ActionResult CRMOneView()
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            //SqlDataReader reader;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[sp_CRMOneView]", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // command.Parameters.Add("@", SqlDbType.VarChar).Value = ;                       

                    conn.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var model = Read(reader).ToList();
                        return View(model);
                    }
                    //
                    //conn.Close();
                }
            }//end using
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
