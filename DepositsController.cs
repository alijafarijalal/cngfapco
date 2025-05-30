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
using System.ComponentModel.DataAnnotations;

namespace cngfapco.Controllers
{
    public class DepositsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Deposits
        public async Task<ActionResult> Index()
        {
            var tbl_Deposits = db.tbl_Deposits.Include(d => d.Workshops);
            return View(await tbl_Deposits.ToListAsync());
        }

        // GET: Deposits/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = await db.tbl_Deposits.FindAsync(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }

        // GET: Deposits/Create
        public ActionResult Create()
        {
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Date,WorkshopsID,NationalCode,FullName,TrackingCode,Value,Bank,Branch,DifinitiveCode,InflectionCode,DepositorID,Serial,Depositor,Description")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Deposits.Add(deposit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", deposit.WorkshopsID);
            return View(deposit);
        }

        // GET: Deposits/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = await db.tbl_Deposits.FindAsync(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", deposit.WorkshopsID);
            return View(deposit);
        }

        // POST: Deposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Date,WorkshopsID,NationalCode,FullName,TrackingCode,Value,Bank,Branch,DifinitiveCode,InflectionCode,DepositorID,Serial,Depositor,Description")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deposit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.WorkshopsID = new SelectList(db.tbl_Workshops, "ID", "Title", deposit.WorkshopsID);
            return View(deposit);
        }

        // GET: Deposits/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deposit deposit = await db.tbl_Deposits.FindAsync(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }

        // POST: Deposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Deposit deposit = await db.tbl_Deposits.FindAsync(id);
            db.tbl_Deposits.Remove(deposit);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Deposits/DynamicForm
        public ActionResult DynamicForm()
        {
            var model = new DynamicFormViewModel();
            model.Rows.Add(new DynamicRow()); // یک ردیف خالی برای شروع
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitForm(DynamicFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ذخیره در دیتابیس
                foreach (var row in model.Rows)
                {
                    // کد ذخیره هر ردیف در دیتابیس
                    // dbContext.YourTable.Add(row);
                    // dbContext.SaveChanges();
                }

                return RedirectToAction("Success");
            }

            return View("DynamicForm", model);
        }

        public ActionResult Success()
        {
            return View();
        }
        //
        public ActionResult ListView(int? countRows,string post)
        {
            string tableShow = "display:none";

            ViewBag.WorkshopsList = new SelectList(db.tbl_Workshops, "ID", "Title");

            var deposits = db.tbl_Deposits.Where(d=>d.ID==0).Include(d => d.Workshops).ToList();

            if (countRows == null)
            {
                countRows = 10;
            }

            if (!deposits.Any() && post!=null)
            {
                deposits = Enumerable.Range(1, countRows.Value).Select(i => new Deposit()).ToList();
                tableShow = "display:block";
            }

            ViewBag.tableShow = tableShow;

            return View(deposits);
        }
        //
        [HttpPost]
        public ActionResult SaveAll(List<Deposit> deposits) 
        {
            ViewBag.WorkshopsList = new SelectList(db.tbl_Workshops.Include(w=>w.City), "ID", "Title");

            if (ModelState.IsValid)
            {
                foreach (var deposit in deposits.Where(d => d.WorkshopsID > 0))
                {
                    var workshop = db.tbl_Workshops.Find(deposit.WorkshopsID);

                    if (workshop != null)
                    {
                        deposit.FullName = $"{workshop.Title} ({workshop.City.Title})";
                        deposit.Depositor = $"{workshop.OwnerName} {workshop.OwnerFamily}";
                        deposit.DepositorID = workshop.ID.ToString();
                        deposit.NationalCode = workshop.Economicalnumber;
                    }

                    deposit.Creator = User.Identity.Name;
                    deposit.CreateDate = DateTime.Now;
                    //deposit.Serial = db.tbl_Deposits.Max(d => (int?)d.Serial) + 1 ?? 1; // تولید شماره سریال
                }
                //db.tbl_Deposits.RemoveRange(db.tbl_Deposits);
                //db.tbl_Deposits.AddRange(deposits.Where(d => d.WorkshopsID > 0));
                //db.SaveChanges();
                // ذخیره داده‌ها
                db.tbl_Deposits.RemoveRange(db.tbl_Deposits);
                var savedDeposits = deposits.Where(d => d.WorkshopsID > 0).ToList();
                db.tbl_Deposits.AddRange(savedDeposits);
                db.SaveChanges();
                // ذخیره در TempData برای نمایش در صفحه بعد
                TempData["SavedDeposits"] = savedDeposits;
                TempData["SaveMessage"] = "اطلاعات با موفقیت ذخیره شدند";

                return RedirectToAction("SaveResults");
                //return RedirectToAction("ListView");
            }
            return View("ListView", deposits);
        }
        //
        public ActionResult SaveResults()
        {
            if (TempData["SavedDeposits"] == null)
            {
                return RedirectToAction("ListView");
            }

            var deposits = (List<Deposit>)TempData["SavedDeposits"];
            ViewBag.SaveMessage = TempData["SaveMessage"];
            return View(deposits);
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
