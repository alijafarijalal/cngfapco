using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;
using System.Globalization;

namespace cngfapco.Controllers
{
    //[RBAC]
    [Authorize]
    public class InstructionsController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
       
        // GET: Instructions
        public ActionResult Index()
        {
            var instructions = db.tbl_Instructions.Include(i => i.Categories);
            return View(instructions.ToList());
        }

        // GET: Instructions
        public ActionResult UserView()
        {
            var instructions = db.tbl_Instructions.Include(i => i.Categories);
            return View(instructions.ToList());
        }

        // GET: Instructions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.tbl_Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            return View(instruction);
        }

       
        // GET: Instructions/Create
        public ActionResult Create()
        {
            ViewBag.CategoriesID = new SelectList(db.tbl_Categories, "ID", "Cat");
            return View();
        }

        // POST: Instructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,DateofApproval,ValidityDate,Code,Status,CategoriesID,Description")] Instruction instruction, HttpPostedFileBase Attachment)
        {
            instruction.Creator = User.Identity.Name;
            instruction.CreateDate = DateTime.Now;
            //
            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    instruction.Attachment = instruction.Code + "_" + pc.GetYear(DateTime.Now) + "_" + pc.GetMonth(DateTime.Now) + "_" + pc.GetDayOfMonth(DateTime.Now) + "_" + Attachment.FileName;// + Path.GetExtension(CommissionImage.FileName);

                    string ImagePath = Server.MapPath("/UploadedFiles/Instructions/" + instruction.Attachment);

                    Attachment.SaveAs(ImagePath);
                }
                db.tbl_Instructions.Add(instruction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriesID = new SelectList(db.tbl_Categories, "ID", "Cat", instruction.CategoriesID);
            return View(instruction);
        }
        
        // GET: Instructions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.tbl_Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriesID = new SelectList(db.tbl_Categories, "ID", "Cat", instruction.CategoriesID);
            return View(instruction);
        }

        // POST: Instructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,DateofApproval,ValidityDate,Code,Status,Attachment,CategoriesID,Description")] Instruction instruction, HttpPostedFileBase Attachment)
        {
            instruction.Creator = User.Identity.Name;
            instruction.CreateDate = DateTime.Now;
            //
            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    if (instruction.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Instructions/" + instruction.Attachment));
                    }
                    instruction.Attachment = instruction.Code + "_" + pc.GetYear(DateTime.Now) + "_" + pc.GetMonth(DateTime.Now) + "_" + pc.GetDayOfMonth(DateTime.Now) + "_" + Attachment.FileName;// + Path.GetExtension(CommissionImage.FileName);

                    string ImagePath = Server.MapPath("/UploadedFiles/Instructions/" + instruction.Attachment);

                    Attachment.SaveAs(ImagePath);
                }
                db.Entry(instruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriesID = new SelectList(db.tbl_Categories, "ID", "Cat", instruction.CategoriesID);
            return View(instruction);
        }

        [RBAC]
        // GET: Instructions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.tbl_Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            return View(instruction);
        }

        // POST: Instructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instruction instruction = db.tbl_Instructions.Find(id);
            db.tbl_Instructions.Remove(instruction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Instructions/InstructionFormCreate
        public ActionResult InstructionFormList(int? id)
        {
            ViewBag.InstructionID = id;
            ViewBag.InstructionTitle = db.tbl_Instructions.Find(id).Title;
            ViewBag.InstructionAttach= db.tbl_Instructions.Find(id).Attachment;
            var instructions = db.tbl_InstructionForms.Where(i=>i.InstructionID==id).Include(i => i.Instruction);
            return View(instructions.ToList());
        }

        // GET: Instructions/InstructionFormCreate
        public ActionResult InstructionFormCreate(int? id)
        {
            ViewBag.InstructionID = id;
            ViewBag.InstructionTitle = db.tbl_Instructions.Find(id).Title;
            return View();
        }
        // POST: Instructions/InstructionFormCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstructionFormCreate([Bind(Include = "ID,Title,InstructionID")] InstructionForm instruction, HttpPostedFileBase Attachment)
        {
            instruction.Creator = User.Identity.Name;
            instruction.CreateDate = DateTime.Now;
            //
            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    instruction.Attachment = instruction.InstructionID + "_" + pc.GetYear(DateTime.Now) + "_" + pc.GetMonth(DateTime.Now) + "_" + pc.GetDayOfMonth(DateTime.Now) + "_" + Attachment.FileName;// + Path.GetExtension(CommissionImage.FileName);

                    string ImagePath = Server.MapPath("/UploadedFiles/Instructions/InstructionForm/" + instruction.Attachment);

                    Attachment.SaveAs(ImagePath);
                }
                db.tbl_InstructionForms.Add(instruction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instruction);
        }

        // GET: Instructions/InstructionFormCreate
        public ActionResult InstructionFormEdit(int? id,int? InstructionID)
        {
            ViewBag.InstructionID = InstructionID;
            ViewBag.InstructionTitle = db.tbl_Instructions.Find(InstructionID).Title;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionForm instruction = db.tbl_InstructionForms.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            return View(instruction);
        }
        // POST: Instructions/InstructionFormCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstructionFormEdit([Bind(Include = "ID,Title,InstructionID,Attachment")] InstructionForm instruction, HttpPostedFileBase Attachment)
        {
            instruction.Creator = User.Identity.Name;
            instruction.CreateDate = DateTime.Now;
            //
            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    if (instruction.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/Instructions/InstructionForm/" + instruction.Attachment));
                    }
                    instruction.Attachment = instruction.InstructionID + "_" + pc.GetYear(DateTime.Now) + "_" + pc.GetMonth(DateTime.Now) + "_" + pc.GetDayOfMonth(DateTime.Now) + "_" + Attachment.FileName;// + Path.GetExtension(CommissionImage.FileName);

                    string ImagePath = Server.MapPath("/UploadedFiles/Instructions/InstructionForm/" + instruction.Attachment);

                    Attachment.SaveAs(ImagePath);
                }
                db.Entry(instruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InstructionFormList",new {id=instruction.InstructionID });
            }

            return View(instruction);
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
