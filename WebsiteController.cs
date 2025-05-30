using cngfapco.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cngfapco.Controllers
{
    [RBACAttribute.NoCache]
    public class WebsiteController : Controller
    {
        private ContextDB db = new ContextDB();
        DAL objdal = new DAL();

        // GET: Website
        public ActionResult Index()
        {
            return View();
        }

        // GET: Website
        public ActionResult HomePage()
        {
            return View();
        }
        // GET: Website / CNGHandBook
        public ActionResult CNGHandBook()
        {
            var handbooks = db.tbl_CNGHandBooks;
            return View(handbooks.ToList());
        }
        //
        public JsonResult GetCities(int id)
        {
            string countrystring = "select * from [dbo].[tbl_Cities] where [StateID]='" + id + "'";

            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--انتخاب شهر--", Value = "" });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            }

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        // GET: Website / WorkshopsList
        public ActionResult WorkshopsList(string CityID, string State)
        {
            //ViewBag.cityId = new SelectList(db.tbl_Cities, "ID", "Title", Convert.ToInt32(cityId));
            ViewBag.State = new SelectList(db.tbl_States, "ID", "Title");

            var list = db.tbl_Workshops.Where(w=>w.isServices == true && w.closedServices != true).ToList();
            if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(CityID))
                return View(list.Where(w => w.CityID.ToString().Equals(CityID) && w.City.StateID.ToString().Equals(State)));
            if (!string.IsNullOrEmpty(State) || !string.IsNullOrEmpty(CityID))
                return View(list.Where(w => w.City.StateID.ToString().Equals(State) || w.City.ToString().Equals(CityID)));
            else
                return View(list);
        }
        // GET: Website / WorkshopsList
        public ActionResult DailyQuote()
        {
            List<DailyQuoteList> dailyquotelist = new List<DailyQuoteList>();
            dailyquotelist.Add(new DailyQuoteList
            {
                Title = "عاقل ترين مردم خوش خلق ترين آنهاست. امام جعفر صادق (ع) "
            });

            return PartialView(dailyquotelist.Skip(2).Take(1).Select(d=>d.Title));
        }
        public class DailyQuoteList
        {
            public string Title { get; set; }
        }
        // GET: CNGHandBook/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CNGHandBook/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CNGHandBooks cnghandbook, HttpPostedFileBase Image)
        {
            cnghandbook.CreatDate = DateTime.Now;
            cnghandbook.Creator = User.Identity.Name;

            if (Image != null)
            {
                if (cnghandbook.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Website/CNGHandBooks/" + cnghandbook.Image));
                }
                cnghandbook.Image = Image.FileName;
                string ImagePath = Server.MapPath("/UploadedFiles/Website/CNGHandBooks/" + cnghandbook.Image);
                Image.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                db.tbl_CNGHandBooks.Add(cnghandbook);
                db.SaveChanges();
                return RedirectToAction("CNGHandBook");
            }

            return View(cnghandbook);
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