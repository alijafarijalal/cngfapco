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
    public class SlidersController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Sliders
        public ActionResult Index()
        {
            var sliderList = db.tbl_Sliders.OrderByDescending(s => s.CreatDate);
            return View(sliderList.ToList());
        }

        /// <summary>
        /// نمایش و مدیریت اطلاعات بخش اسلایدر صفحه اصلی وب سایت
        /// </summary>
        /// <returns></returns>
        public ActionResult Carousel()
        {
            return PartialView(db.tbl_Sliders.Where(s=> s.Presentable==true && s.Section.Equals("slider")).OrderBy(s=>s.Order).ToList());
        }

        /// <summary>
        /// نمایش و مدیریت اطلاعات بخش اطلاعیه صفحه اصلی وب سایت
        /// </summary>
        /// <returns></returns>
        public ActionResult Announcement()
        {
            return PartialView(db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("announce")).OrderBy(s => s.Order).ToList());
        }

        /// <summary>
        /// نمایش اطلاعیه های مربوط به کارگاه ها در صفحه اصلی سامانه
        /// </summary>
        /// <returns></returns>
        public ActionResult MainAnnouncement()
        {
            return PartialView(db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("mainannounce")).OrderBy(s => s.Order).ToList());
        }

        /// <summary>
        /// نمایش آرشیواطلاعیه های مربوط به کارگاه ها 
        /// </summary>
        /// <returns></returns>
        public ActionResult ArchiveAnnouncement()
        {
            return View(db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("mainannounce")).OrderBy(s => s.Order).ToList());
        }

        /// <summary>
        /// نمایش و مدیریت اطلاعات بخش اطلاعیه صفحه اصلی وب سایت
        /// </summary>
        /// <returns></returns>
        public ActionResult UsefulLink()
        {
            return PartialView(db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("usefullink")).OrderBy(s => s.Order).ToList());
        }

        /// <summary>
        /// نمایش و مدیریت اطلاعات بخش اخبار صفحه اصلی وب سایت
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            return PartialView(db.tbl_Sliders.Where(s => s.Presentable == true && s.Section.Equals("news")).OrderBy(s => s.Order).ToList());
        }

        /// <summary>
        /// نمایش و مدیریت اطلاعات بخش فوتر (تماس با ما) صفحه اصلی وب سایت
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUs()
        {
            return PartialView(db.tbl_ContactUs.Where(s => s.Presentable == true).OrderBy(s => s.Order).ToList());
        }
        // GET: Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.tbl_Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Cat,Order,Presentable,DueDate,Url,Section,Refrence")] Slider slider,HttpPostedFileBase Image)
        {
            slider.CreatDate = DateTime.Now;
            slider.Creator = User.Identity.Name;

            if (Image != null)
            {
                if (slider.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Website/" + slider.Image));
                }
                slider.Image = Image.FileName;
                string ImagePath = Server.MapPath("/UploadedFiles/Website/" + slider.Image);
                Image.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                db.tbl_Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.tbl_Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Image,Title,Description,Cat,Order,Presentable,DueDate,Url,Section,Refrence,CreatDate")] Slider slider,HttpPostedFileBase Image)
        {
            slider.Creator = User.Identity.Name;

            if (Image != null)
            {
                if (slider.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/Website/" + slider.Image));
                }
                slider.Image = Image.FileName;
                string ImagePath = Server.MapPath("/UploadedFiles/Website/" + slider.Image);
                Image.SaveAs(ImagePath);
            }
            //
            if (ModelState.IsValid)
            {
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.tbl_Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.tbl_Sliders.Find(id);
            db.tbl_Sliders.Remove(slider);
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
