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
    [Authorize]
    [RBAC]
    public class SideBarItemsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: SideBarItems
        public ActionResult Index()
        {
            var tbl_SideBarItems = db.tbl_SideBarItems.Include(s => s.Parent).OrderBy(s=>s.orderBy);
            return View(tbl_SideBarItems.ToList());
        }

        // GET: SideBarItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBarItem sideBarItem = db.tbl_SideBarItems.Find(id);
            if (sideBarItem == null)
            {
                return HttpNotFound();
            }
            return View(sideBarItem);
        }

        // GET: SideBarItems/Create
        public ActionResult Create()
        {
            ViewBag.parentId = new SelectList(db.tbl_SideBarItems, "ID", "nameOption");
            return View();
        }

        // POST: SideBarItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nameOption,controller,action,imageClass,status,isParent,parentId,orderBy")] SideBarItem sideBarItem)
        {
            if (ModelState.IsValid)
            {
                db.tbl_SideBarItems.Add(sideBarItem);
                db.SaveChanges();
                //
                var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                SideBarItem sidebaritem = db.tbl_SideBarItems.Find(sideBarItem.ID);
                User user = db.tbl_Users.Find(userId);
                if (!user.SideBarItems.Contains(sidebaritem))
                {
                    user.SideBarItems.Add(sidebaritem);
                    db.SaveChanges();
                }
                //
                return RedirectToAction("Index");
            }
            
            ViewBag.parentId = new SelectList(db.tbl_SideBarItems, "ID", "nameOption", sideBarItem.parentId);
            return View(sideBarItem);
        }

        // GET: SideBarItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBarItem sideBarItem = db.tbl_SideBarItems.Find(id);
            if (sideBarItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.parentId = new SelectList(db.tbl_SideBarItems, "ID", "nameOption", sideBarItem.parentId);
            return View(sideBarItem);
        }

        // POST: SideBarItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nameOption,controller,action,imageClass,status,isParent,parentId,orderBy")] SideBarItem sideBarItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sideBarItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parentId = new SelectList(db.tbl_SideBarItems, "ID", "nameOption", sideBarItem.parentId);
            return View(sideBarItem);
        }

        // GET: SideBarItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBarItem sideBarItem = db.tbl_SideBarItems.Find(id);
            if (sideBarItem == null)
            {
                return HttpNotFound();
            }
            return View(sideBarItem);
        }

        // POST: SideBarItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SideBarItem sideBarItem = db.tbl_SideBarItems.Find(id);
            db.tbl_SideBarItems.Remove(sideBarItem);
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
