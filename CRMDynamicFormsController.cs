using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;

namespace cngfapco.Controllers
{
    public class CRMDynamicFormsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: CRMDynamicForms
        public ActionResult Index()
        {
            return View(db.tbl_CRMDynamicForms.ToList());
        }
        // GET: CRMDynamicForms/Show Question
        public ActionResult Questions(int? id)
        {
            ViewBag.status = "";
            if (id != null)
            {
                var checkExist = db.tbl_AnswerQuestions.Where(a => a.RegistrationsId == id).ToList();
                if (checkExist.Count() > 0)
                {
                    ViewBag.message = "ضمن تشکر از حسن توجه شما، نظر شما قبلا ثبت شده است.";
                    ViewBag.status = "disabled";
                }
            }
            //
            try
            {
                VehicleRegistration Registration = db.tbl_VehicleRegistrations.Find(id);
                ViewBag.owner = Registration.OwnerName + " " + Registration.OwnerFamily;
                ViewBag.Mobile = Registration.MobileNumber;
                ViewBag.RegistrationsId = Registration.ID;
            }
            catch { }
            
            return View(db.tbl_CRMDynamicForms.ToList());
        }
        // GET: Add to CRM 
        [HttpPost]
        public JsonResult AddResults(int[] Id, bool[] value, string FullName, string Mobile, string[] Description, int? RegistrationsId)
        {
            ViewBag.status = "";
            string desc = "";
            AnswerQuestion addCRM = new AnswerQuestion();
            if (Id != null)
            {
                for (int i = 0; i < value.Count(); i++)
                {
                    var question = db.tbl_CRMDynamicForms.Find(Id[i]);

                    addCRM.RegistrationsId = RegistrationsId;
                    addCRM.CreateDate = DateTime.Now;
                    addCRM.ip = Helper.Helpers.GetIPHelper();
                    try
                    {
                        if (!string.IsNullOrEmpty(Description[i]))
                        {
                            desc= Description[i];
                            //addCRM.Description = Description[i];
                        }                            
                        else
                            addCRM.Description = "";
                    }
                    catch
                    {
                        if(question.isShow==true && question.Description==true)
                            addCRM.Description = desc;
                        else
                            addCRM.Description = "";
                    }
                    addCRM.QuestionId = Id[i];
                    addCRM.FullName = FullName;
                    addCRM.OkNotOk = value[i];
                    addCRM.Mobile = Mobile;
                    db.tbl_AnswerQuestions.Add(addCRM);
                    db.SaveChanges();
                    // do something here
                }
                ViewBag.status = "disabled";
                return Json(new { result = "نظرات شما با موفقیت ثبت شد!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "هیچ موردی انتخاب نشده است!" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CRMDynamicForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMDynamicForm cRMDynamicForm = db.tbl_CRMDynamicForms.Find(id);
            if (cRMDynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(cRMDynamicForm);
        }

        // GET: CRMDynamicForms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CRMDynamicForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,OkNotOk,Description,isShow,Creator,CreateDate")] CRMDynamicForm cRMDynamicForm)
        {
            cRMDynamicForm.CreateDate = DateTime.Now;
            cRMDynamicForm.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.tbl_CRMDynamicForms.Add(cRMDynamicForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cRMDynamicForm);
        }

        // GET: CRMDynamicForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMDynamicForm cRMDynamicForm = db.tbl_CRMDynamicForms.Find(id);
            if (cRMDynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(cRMDynamicForm);
        }

        // POST: CRMDynamicForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,OkNotOk,Description,isShow,Creator,CreateDate")] CRMDynamicForm cRMDynamicForm)
        {
            cRMDynamicForm.CreateDate = DateTime.Now;
            cRMDynamicForm.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(cRMDynamicForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cRMDynamicForm);
        }

        // GET: CRMDynamicForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMDynamicForm cRMDynamicForm = db.tbl_CRMDynamicForms.Find(id);
            if (cRMDynamicForm == null)
            {
                return HttpNotFound();
            }
            return View(cRMDynamicForm);
        }

        // POST: CRMDynamicForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CRMDynamicForm cRMDynamicForm = db.tbl_CRMDynamicForms.Find(id);
            db.tbl_CRMDynamicForms.Remove(cRMDynamicForm);
            db.SaveChanges();
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
