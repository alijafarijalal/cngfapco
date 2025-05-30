using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.IO;
using cngfapco.Models;
using System.Web.Security;
using System.Data;
using System.Data.Entity.Validation;

namespace cngfapco.Controllers
{    
    public class UsersController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        private string IP = cngfapco.Helper.Helpers.GetVisitorIPAddress();
        DAL objdal = new DAL();

        //User Profile        
        public ActionResult UserProfile()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
            return PartialView(user);
        }
        [HttpGet]
        public JsonResult CheckEmail(string Username, string Password)
        {
            string retval = "";
            SqlConnection con = new SqlConnection("Data Source=JAFARI_ALI-PC;Database=RBAC;uid=sa;pwd=jafari_@LI");
            con.Open();
            SqlCommand cmd = new SqlCommand("select Username from USERS where Username=@UserName and Password=@Password", con);
            cmd.Parameters.AddWithValue("@UserName", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                retval = "true";
            }
            else
            {
                retval = "false";
            }
            return Json(retval, JsonRequestBehavior.AllowGet);            //return retval;
        }
        [HttpPost]
        // GET: /Permision/
        public ActionResult AllowPermission(List<string> IDs)
        {
            //var statement = db.Statement
            //                 .Include(s => s.Contract)
            //                 .ToList();
            foreach (string ID in IDs)
            {
                //foreach (var item in statement.Where(s => s.Contract.ID.ToString().Equals(ID)))
                //{
                //    item.FinallChek = true;
                //    db.Statement.Attach(item);
                //    db.Entry<Statement>(item).State = EntityState.Modified;
                //}
                db.SaveChanges();
            }

            return Json("مجوز ویرایش اطلاعات داده شد");
        }

        // GET: Users        
        public ActionResult LoginPage(string aspxerrorpath,string ReturnUrl)
        {
            //So that the user can be referred back to where they were when they click logon %2f
            if (string.IsNullOrEmpty(aspxerrorpath) && Request.UrlReferrer != null)
                aspxerrorpath = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(aspxerrorpath) && !string.IsNullOrEmpty(aspxerrorpath))
            {
                ViewBag.ReturnURL = aspxerrorpath;
            }
            //
            if (string.IsNullOrEmpty(ReturnUrl) && Request.UrlReferrer != null)
                ReturnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(ReturnUrl) && !string.IsNullOrEmpty(ReturnUrl))
            {
                ViewBag.ReturnURL = ReturnUrl.Replace("%2f","/");
            }
            return View();
        }
        // GET: Users
        [Authorize]
        [RBAC]
        public ActionResult Index()
        {
            return View(db.tbl_Users.OrderByDescending(u=>u.UserID).ToList());
        }
        
        public ViewResult UserDetails(int id)
        {
            User user = db.tbl_Users.Find(id);
            SetViewBagData(id);
            return View(user);
        }
        //
        [HttpGet]
        public ActionResult AddUserRole(int? userId)
        {
            ViewBag.userID = userId;
            ViewBag.Role_Id = new SelectList(db.tbl_Roles, "Role_Id", "RoleName");
            return PartialView();
            
        }

        [HttpPost]
        public ActionResult AddUserRole(int? Role_Id, int? userId)
        {
            Role role = db.tbl_Roles.Find(Role_Id);
            User user = db.tbl_Users.Find(userId);

            if (!role.Users.Contains(user))
            {
                role.Users.Add(user);
                db.SaveChanges();
            }
            SetViewBagData(userId.Value);
            //return PartialView("_ListUserRoleTable", db.tbl_Users.Find(userId));
            return RedirectToAction("Index");
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permissionId)
        {
            Role role = db.tbl_Roles.Find(id);
            Permission _permission = db.tbl_Permissions.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                db.SaveChanges();
            }
            return PartialView("_ListPermissions", role);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllPermissions2RoleReturnPartialView(int id)
        {
            Role _role = db.tbl_Roles.Where(p => p.Role_Id == id).FirstOrDefault();
            List<Permission> _permissions = db.tbl_Permissions.ToList();
            foreach (Permission _permission in _permissions)
            {
                if (!_role.Permissions.Contains(_permission))
                {
                    _role.Permissions.Add(_permission);

                }
            }
            db.SaveChanges();
            return PartialView("_ListPermissions", _role);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserRoleReturnPartialView(int id, int userId)
        {
            Role role = db.tbl_Roles.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (role.Users.Contains(user))
            {
                role.Users.Remove(user);
                db.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListUserRoleTable", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserWorkshopReturnPartialView(int id, int userId)
        {
            Workshop workshop = db.tbl_Workshops.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (workshop.Users.Contains(user))
            {
                workshop.Users.Remove(user);
                db.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListUserWorkshopTable", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserFromRoleReturnPartialView(int id, int userId)
        {
            Role role = db.tbl_Roles.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (role.Users.Contains(user))
            {
                role.Users.Remove(user);
                db.SaveChanges();
            }
            return PartialView("_ListUsersTable4Role", role);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUser2RoleReturnPartialView(int id, int userId)
        {
            Role role = db.tbl_Roles.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (!role.Users.Contains(user))
            {
                role.Users.Add(user);
                db.SaveChanges();
            }
            return PartialView("_ListUsersTable4Role", role);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeletePermissionFromRoleReturnPartialView(int id, int permissionId)
        {
            Role _role = db.tbl_Roles.Find(id);
            Permission _permission = db.tbl_Permissions.Find(permissionId);

            if (_role.Permissions.Contains(_permission))
            {
                _role.Permissions.Remove(_permission);
                db.SaveChanges();
            }
            return PartialView("_ListPermissions", _role);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermissionReturnPartialView(int id, int permissionId)
        {
            Role role = db.tbl_Roles.Find(id);
            Permission permission = db.tbl_Permissions.Find(permissionId);

            if (role.Permissions.Contains(permission))
            {
                role.Permissions.Remove(permission);
                db.SaveChanges();
            }
            return PartialView("_ListRolesTable4Permission", permission);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddRole2PermissionReturnPartialView(int permissionId, int roleId)
        {
            Role role = db.tbl_Roles.Find(roleId);
            Permission _permission = db.tbl_Permissions.Find(permissionId);

            if (!role.Permissions.Contains(_permission))
            {
                role.Permissions.Add(_permission);
                db.SaveChanges();
            }
            return PartialView("_ListRolesTable4Permission", _permission);
        }
        //
        [HttpGet]
        public ActionResult DeleteUserRole(int? userId)
        {
            ViewBag.userID = userId;
            //ViewBag.Role_Id = Role_Id;
            return PartialView();

        }

        [HttpPost, ActionName("DeleteUserRole")]
        public ActionResult DeleteConfirmedUserRole(int? Role_Id, int? userId)
        {
            Role role = db.tbl_Roles.Find(Role_Id);
            User user = db.tbl_Users.Find(userId);

            if (!role.Users.Contains(user))
            {
                role.Users.Remove(user);
                db.SaveChanges();
            }
            SetViewBagData(userId.Value);
            //return PartialView("_ListUserRoleTable", db.tbl_Users.Find(userId));
            return RedirectToAction("UserEdit");
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserRoleReturnPartialView(int id, int userId)
        {
            Role role = db.tbl_Roles.Find(id);
            User user = db.tbl_Users.Find(userId);           

            if (!role.Users.Contains(user))
            {
                role.Users.Add(user);
                db.SaveChanges();
            }
            //
            var sidebaritems = db.tbl_SideBarItems.ToList();

            //دادن منوهای مشابه به کاربر با نقش مرکز خدمات(کارگاه
            if (id == 2)
            {
                foreach (var item in sidebaritems)
                {
                    SideBarItem sidebarselect = db.tbl_SideBarItems.Find(item.ID);
                    User sameasuser = db.tbl_Users.Find(8);

                    if (sidebarselect.Users.Contains(sameasuser))
                    {
                        sidebarselect.Users.Add(user);
                        db.SaveChanges();
                    }

                }
            }
            //دادن منوهای مشابه به کاربر با نقش کارشناس ناظر
            if (id == 3)
            {
                foreach (var item in sidebaritems)
                {
                    SideBarItem sidebarselect = db.tbl_SideBarItems.Find(item.ID);
                    User sameasuser = db.tbl_Users.Find(10);

                    if (sidebarselect.Users.Contains(sameasuser))
                    {
                        sidebarselect.Users.Add(user);
                        db.SaveChanges();
                    }

                }
            }
            //دادن منوهای مشابه به کاربر با نقش انبار
            if (id == 4)
            {
                foreach (var item in sidebaritems)
                {
                    SideBarItem sidebarselect = db.tbl_SideBarItems.Find(item.ID);
                    User sameasuser = db.tbl_Users.Find(9);

                    if (sidebarselect.Users.Contains(sameasuser))
                    {
                        sidebarselect.Users.Add(user);
                        db.SaveChanges();
                    }

                }
            }
            //دادن منوهای مشابه به کاربر با نقش مدیر تبدیل ناوگان
            if (id == 5)
            {
                foreach (var item in sidebaritems)
                {
                    SideBarItem sidebarselect = db.tbl_SideBarItems.Find(item.ID);
                    User sameasuser = db.tbl_Users.Find(6);

                    if (sidebarselect.Users.Contains(sameasuser))
                    {
                        sidebarselect.Users.Add(user);
                        db.SaveChanges();
                    }

                }
            }
            //
            SetViewBagData(userId);
            return PartialView("_ListUserRoleTable", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserWorkshopReturnPartialView(int id, int userId)
        {
            Workshop workshop = db.tbl_Workshops.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (!user.Workshops.Contains(workshop))
            {
                user.Workshops.Add(workshop);
                db.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListUserWorkshopTable", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserDataPermissionReturnPartialView(int id, int userId)
        {
            //ROLE role = database.ROLES.Find(id);
            string result = null;
            User user = db.tbl_Users.Find(userId);
            var AddtoUserPermissionColumn = db.tbl_Users.Where(u => u.UserID == userId).SingleOrDefault();
            if (AddtoUserPermissionColumn.DataPermission == null || AddtoUserPermissionColumn.DataPermission == "")
            {
                result = id.ToString();
            }
            else
            {
                if (!AddtoUserPermissionColumn.DataPermission.Contains(id.ToString()))
                    result = AddtoUserPermissionColumn.DataPermission + ", " + id;
            }
            //var result = string.Join(", ", id);
            if (result != null)
            {
                AddtoUserPermissionColumn.DataPermission = result;
                db.tbl_Users.Attach(AddtoUserPermissionColumn);
                db.Entry<User>(AddtoUserPermissionColumn).State = System.Data.Entity.EntityState.Modified;                
                db.SaveChanges();
            }
            //user.DataPermission = id.ToString();
            //database.USERS.Add(user);
            //database.SaveChanges();
            SetViewBagData(userId);
            return PartialView("_ListUserDataPermissionTable", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        public ActionResult AddUserDataPermissionList(int id)
        {
            User user = db.tbl_Users.Find(id);
            SetViewBagData(id);
            return View(user);
        }
        //
        [HttpPost]
        public ActionResult AddUserDataPermissionList(User user)
        {
            User _user = db.tbl_Users.Where(p => p.UserID == user.UserID).FirstOrDefault();
            if (_user != null)
            {
                try
                {
                    db.Entry(_user).CurrentValues.SetValues(user);
                    db.Entry(_user).Entity.LastModified = System.DateTime.Now;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("UserDetails", new RouteValueDictionary(new { id = user.UserID }));
        }
        //
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult InserttoUserDataPermission(Workshop category, List<string> IDs, int? User_id)
        {
            if (User_id == null)
            {
                User_id = (int?)Session["User_id"];
            }
            var users = db.tbl_Users.Select(u => new
            {
                User_id = u.UserID,
                fullNmae = u.Lastname + " " + u.Firstname
            })
            .ToList();

            ViewBag.User = new SelectList(users, "User_id", "fullNmae");
            if (IDs == null)
            {
                ModelState.AddModelError(string.Empty, "هیچ ردیفی برای اعطای مجوز انتخاب نشده است!");
                return View("UserDataPermissionList");
            }
            var result = string.Join(", ", IDs);
            var AddtoUserPermissionColumn = db.tbl_Users.Where(u => u.UserID == User_id).SingleOrDefault();
            if (result != null)
            {
                AddtoUserPermissionColumn.DataPermission = result;
                db.tbl_Users.Attach(AddtoUserPermissionColumn);
                db.Entry<User>(AddtoUserPermissionColumn).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json("عملیات ثبت با موفقیت انجام شد.", JsonRequestBehavior.AllowGet);
        }
        //
        private void SetViewBagData(int _userId)
        {
            ViewBag.UserId = _userId;
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            ViewBag.RoleId = new SelectList(db.tbl_Roles.OrderBy(p => p.RoleName), "Role_Id", "RoleName");
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title");
            ViewBag.SideBarItemsId = new SelectList(db.tbl_SideBarItems, "ID", "nameOption");
            ViewBag.Datapermission = db.tbl_Users.Where(u => u.UserID == _userId).SingleOrDefault().DataPermission;

        }

        public List<SelectListItem> List_boolNullYesNo()
        {
            var _retVal = new List<SelectListItem>();
            try
            {
                _retVal.Add(new SelectListItem { Text = "--انتخاب از لیست--", Value = null });
                _retVal.Add(new SelectListItem { Text = "بله", Value = bool.TrueString });
                _retVal.Add(new SelectListItem { Text = "خیر", Value = bool.FalseString });
            }
            catch { }
            return _retVal;
        }
        //
        //[RBAC]
        public ActionResult UserCreate()
        {           
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            ViewBag.AuditCompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult UserCreate(User user, HttpPostedFileBase Image)
        {
            if (user.Username == "" || user.Username == null)
            {
                ModelState.AddModelError(string.Empty, "نام کاربری نمی تواند خالی باشد!");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    List<string> results = db.Database.SqlQuery<String>(string.Format("SELECT Username FROM Users WHERE Username = '{0}'", user.Username)).ToList();
                    bool _userExistsInTable = (results.Count > 0);

                    User _user = null;

                    if (_userExistsInTable)
                    {
                        _user = db.tbl_Users.Where(p => p.Username == user.Username).FirstOrDefault();
                        if (_user != null)
                        {
                            if (_user.Inactive == false)
                            {
                                ModelState.AddModelError(string.Empty, "این نام کاربری قبلا ثبت شده است!");
                            }
                            else
                            {
                                if (Image != null)
                                {
                                    if (_user.Image != null)
                                    {
                                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/" + _user.Image));
                                    }

                                    _user.Image = _user.Firstname + "_" + _user.Lastname + "_" + Image.FileName;

                                    string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/" + _user.Image);

                                    Image.SaveAs(ImagePath);
                                }
                                //db.Entry(_user).Entity.Inactive = false;
                                db.Entry(_user).Entity.LastModified = System.DateTime.Now;
                                db.Entry(_user).State = EntityState.Modified;
                                db.SaveChanges();
                                
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else
                    {
                        _user = new User();
                        _user.Username = user.Username;
                        _user.Password = user.Password;
                        _user.Lastname = user.Lastname;
                        _user.Firstname = user.Firstname;
                        _user.MobileNumber = user.MobileNumber;
                        _user.WorkshopID = user.WorkshopID;
                        _user.DataPermission = user.WorkshopID.ToString();
                        _user.EMail = user.EMail;
                        _user.AuditCompaniesID = user.AuditCompaniesID;
                        _user.isAuditManager = user.isAuditManager;
                        _user.Inactive = user.Inactive;

                        if (ModelState.IsValid)
                        {
                            if (Image != null)
                            {
                                if (user.Image != null)
                                {
                                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/" + user.Image));
                                }

                                user.Image = user.Firstname + "_" + user.Lastname + "_" + Image.FileName;

                                string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/" + user.Image);

                                Image.SaveAs(ImagePath);
                            }

                            _user.Image = user.Image;
                            _user.LastModified = System.DateTime.Now;

                            db.tbl_Users.Add(_user);
                            db.SaveChanges();

                            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", _user.WorkshopID);
                            ViewBag.AuditCompaniesID = new SelectList(db.tbl_AuditComponies, "ID", "Title", _user.AuditCompaniesID);
                            return RedirectToAction("Index");
                        }
                    }

                }
            }
            catch (Exception ex)
            {                
                return View(user);
            }
            
            return View(user);
        }

        [RBAC]
        //UserEdit
        [HttpGet]
        public ActionResult UserEdit(int id, string errorMessage)
        {
            User user = db.tbl_Users.Find(id);
            SetViewBagData(id);
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", user.WorkshopID);
            ViewBag.message = errorMessage;
            return View(user);
        }

        //UserEdit
        [HttpPost]
        public ActionResult UserEdit(User user, HttpPostedFileBase Image)
        {
            string errorMessage = "";
            User _user = db.tbl_Users.Find(user.UserID);
            if (_user != null)
            {               
                //if (Image != null)
                //{
                if (Image != null)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Image.InputStream);
                    int height = img.Width;
                    int width = img.Width;

                    if (width>35 || height>35)
                    {
                        errorMessage = " حداکثر اندازه عکس 35*35 می تواند باشد!";
                    }
                    if (user.Image != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/" + user.Image));
                    }
                    user.Image = user.Firstname + "_" + user.Lastname + "_" + Image.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/" + user.Image);
                    Image.SaveAs(ImagePath);

                }

                {                    
                    db.Entry(_user).CurrentValues.SetValues(user);
                    db.Entry(_user).Entity.LastModified = System.DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //}
                //user.Image = Helper.Helpers.GetImageFromFile(Image, 25, 25);

            }
            ViewBag.message = errorMessage;
            return RedirectToAction("UserEdit", new RouteValueDictionary(new { id = user.UserID,errorMessage= errorMessage }));
            //return View();
        }
        //
        [RBAC]
        [HttpPost]
        public ActionResult UserDetails(User user)
        {
            if (user.Username == null)
            {
                ModelState.AddModelError(string.Empty, "کاربر مورد نظر یافت نشد!");
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).Entity.Inactive = user.Inactive;
                db.Entry(user).Entity.LastModified = System.DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(user);
        }
        //
        [HttpGet]
        public ActionResult DeleteUserRole(int id, int userId)
        {
            Role role = db.tbl_Roles.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (role.Users.Contains(user))
            {
                role.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Users", new { id = userId });
        }
        //
        #region Roles
        //[RBAC]
        //
        [Authorize]
        [RBAC]
        public ActionResult RoleIndex()
        {
            return View(db.tbl_Roles.OrderBy(r => r.RoleDescription).ToList());
        }
        [RBAC]
        public ViewResult RoleDetails(int id)
        {
            User user = db.tbl_Users.Where(r => r.Username == User.Identity.Name).FirstOrDefault();
            Role role = db.tbl_Roles.Where(r => r.Role_Id == id)
                   .Include(a => a.Permissions)
                   .Include(a => a.Users)
                   .FirstOrDefault();

            // USERS combo
            ViewBag.UserId = new SelectList(db.tbl_Users.Where(r => r.Inactive == false || r.Inactive == null), "Id", "Username");
            ViewBag.RoleId = id;

            // Rights combo
            ViewBag.PermissionId = new SelectList(db.tbl_Permissions.OrderBy(a => a.PermissionDescription), "Permission_Id", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(role);
        }
        //
        //[RBAC]
        public ActionResult RoleCreate()
        {
            User user = db.tbl_Users.Where(r => r.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View();
        }

        [HttpPost]
        public ActionResult RoleCreate(Role _role)
        {
            if (_role.RoleDescription == null)
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }

            User user = db.tbl_Users.Where(r => r.Username == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.tbl_Roles.Add(_role);
                db.SaveChanges();
                return RedirectToAction("RoleIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }

        //
        [RBAC]
        public ActionResult RoleEdit(int id)
        {
            User user = db.tbl_Users.Where(r => r.Username == User.Identity.Name).FirstOrDefault();

            Role _role = db.tbl_Roles.Where(r => r.Role_Id == id)
                    .Include(a => a.Permissions)
                    .Include(a => a.Users)
                    .FirstOrDefault();

            // USERS combo
            ViewBag.UserId = new SelectList(db.tbl_Users, "UserID", "Username");
            ViewBag.RoleId = id;

            // Rights combo
            ViewBag.PermissionId = new SelectList(db.tbl_Permissions.OrderBy(a => a.Permission_Id), "Permission_Id", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(_role);
        }

        [HttpPost]
        public ActionResult RoleEdit(Role _role)
        {
            if (string.IsNullOrEmpty(_role.RoleDescription))
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }

            //EntityState state = database.Entry(_role).State;
            User user = db.tbl_Users.Where(r => r.Username == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(_role).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("RoleDetails", new RouteValueDictionary(new { id = _role.Role_Id }));
                return RedirectToAction("RoleIndex");
            }
            // USERS combo
            ViewBag.UserId = new SelectList(db.tbl_Users.Where(r => r.Inactive == false || r.Inactive == null), "User_Id", "Username");

            // Rights combo
            ViewBag.PermissionId = new SelectList(db.tbl_Permissions.OrderBy(a => a.Permission_Id), "Permission_Id", "PermissionDescription");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }

        //
        //[RBAC]
        public ActionResult RoleDelete(int id)
        {
            Role _role = db.tbl_Roles.Find(id);
            if (_role != null)
            {
                _role.Users.Clear();
                _role.Permissions.Clear();

                db.Entry(_role).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("RoleIndex");
        }
        #endregion
        //
        #region PERMISSIONS
        //
        
        [Authorize]
        [RBAC]
        [Display(Name = "لیست کلی دسترسی ها")]
        public ViewResult PermissionIndex()
        {
            List<Permission> _permissions = db.tbl_Permissions
                               .OrderBy(wn => wn.PermissionDescription)
                               .Include(a => a.Roles)
                               .ToList();
            return View(_permissions);
        }

        public ViewResult PermissionDetails(int id)
        {
            Permission _permission = db.tbl_Permissions.Find(id);
            return View(_permission);
        }
        //
        [RBAC]
        public ActionResult PermissionCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PermissionCreate(Permission _permission)
        {
            if (_permission.PermissionDescription == null)
            {
                ModelState.AddModelError("Permission Description", "Permission Description must be entered");
            }

            if (ModelState.IsValid)
            {
                db.tbl_Permissions.Add(_permission);
                db.SaveChanges();
                return RedirectToAction("PermissionIndex");
            }
            return View(_permission);
        }
        //
        [RBAC]
        public ActionResult PermissionEdit(int id)
        {
            Permission _permission = db.tbl_Permissions.Find(id);
            ViewBag.RoleId = new SelectList(db.tbl_Roles.OrderBy(p => p.RoleDescription), "Role_Id", "RoleName");
            return View(_permission);
        }

        [HttpPost]
        public ActionResult PermissionEdit(Permission _permission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(_permission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PermissionDetails", new RouteValueDictionary(new { id = _permission.Permission_Id }));
            }
            return View(_permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PermissionDelete(int id)
        {
            Permission permission = db.tbl_Permissions.Find(id);
            if (permission.Roles.Count > 0)
                permission.Roles.Clear();

            db.Entry(permission).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("PermissionIndex");
        }
        public ActionResult PermissionsImport()
        {
            var _controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypes())
                            .Where(t => t != null
                                && t.IsPublic
                                && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                                && !t.IsAbstract
                                && typeof(IController).IsAssignableFrom(t));

            var _controllerMethods = _controllerTypes.ToDictionary(controllerType => controllerType,
                    controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));

            foreach (var _controller in _controllerMethods)
            {
                string _controllerName = _controller.Key.Name;
                // string _controllerPersianDesc = _controller.Key.FullNa;

                foreach (var _controllerAction in _controller.Value)
                {
                    string _controllerActionName = _controllerAction.Name;

                    if (_controllerName.EndsWith("Controller"))
                    {
                        _controllerName = _controllerName.Substring(0, _controllerName.LastIndexOf("Controller"));
                    }

                    //HelperNamespace.GetDisplayName(Model, m => m.Prop)
                    string _permissionDescription = string.Format("{0}-{1}", _controllerName, _controllerActionName);
                    Permission _permission = db.tbl_Permissions.Where(p => p.PermissionDescription == _permissionDescription).FirstOrDefault();
                    if (_permission == null)
                    {
                        if (ModelState.IsValid)
                        {
                            try
                            {
                                Permission _perm = new Permission();
                                _perm.PermissionDescription = _permissionDescription;
                                _perm.PersianDescription = "تعریف نشده";
                                db.tbl_Permissions.Add(_perm);
                                db.SaveChanges();
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                    }
                                }
                                throw;
                            }                            
                        }
                    }
                }
            }
            return RedirectToAction("PermissionIndex");
        }
        #endregion
        [HttpPost]
        public ActionResult Login(string UserName, string Password,int? count, string ReturnURL)
        {
            var isUserAlreadyExists = db.tbl_Users.Any(x => x.Username == UserName);
            User _userExist = db.tbl_Users.Where(u => u.Username == UserName).SingleOrDefault();

            if (isUserAlreadyExists)
            {
                FormsAuthentication.SetAuthCookie(UserName, true);

                var _user = db.tbl_Users.Where(u => u.Username == UserName && u.Password == Password).SingleOrDefault();
                if (_user != null)
                {
                    if (HttpRuntime.Cache["LoggedInUsers"] != null) //if the list exists, add this user to it
                    {
                        //get the list of logged in users from the cache
                        List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                        //add this user to the list
                        loggedInUsers.Add(_user.Username);
                        //add the list back into the cache
                        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                    }
                    else //the list does not exist so create it
                    {
                        //create a new list
                        List<string> loggedInUsers = new List<string>();
                        //add this user to the list
                        loggedInUsers.Add(_user.Username);
                        //add the list into the cache
                        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                    }

                }

                //when user is true but inactive status
                if (_user != null && _user.Inactive == false)
                {
                    ViewBag.Message = "نام کاربری شما غیر فعال شده است!";
                    ViewBag.Message2 = "به دلیل تکرار بیش از 5 بار اشتباه در ورود، لطفا با مدیر سیستم تماس بگیرید.";
                }

                //when user status is true
                if (_user!=null && _user.Inactive==true)
                {
                    Session["userId"] = _user.UserID;
                    Session["permission"] = _user.DataPermission;

                    UserEntranceInfo userloged = new UserEntranceInfo();
                    userloged.EntranceDate = DateTime.Now;
                    userloged.IPAddress = IP;
                    userloged.UserID = _user.UserID;
                    userloged.Status = true;
                    db.tbl_UserEntranceInfos.Add(userloged);
                    db.SaveChanges();
                    double datediff=0;
                    datediff = (DateTime.Now - _user.LastModified).HasValue?(DateTime.Now - _user.LastModified).Value.TotalDays:0;

                    if(datediff >= 90)
                    {
                        System.Threading.Thread.Sleep(1000);
                        return RedirectToAction("ChangePassword", "Users");
                    }                  
                    
                    else
                    {
                        //returnURL needs to be decoded
                        string decodedUrl = "";
                        if (!string.IsNullOrEmpty(ReturnURL))
                            decodedUrl = Server.UrlDecode(ReturnURL);
                        //Login logic...
                        if (Url.IsLocalUrl(decodedUrl))
                        {
                            return Redirect(decodedUrl);
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                            ////if (_user.AuditCompaniesID != null)
                            ////   return RedirectToAction("AuditComponies", "Audits");
                            ////else
                            ///Audits/AuditComponies
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                    
                }

                //when user password not currect
                if (_user==null)
                {
                    int i = 1;

                    if (count != null)
                    {
                        if(count<=5)
                        {
                            count++;

                            UserEntranceInfo userloged = new UserEntranceInfo();
                            userloged.EntranceDate = DateTime.Now;
                            userloged.IPAddress = IP;
                            userloged.UserID = _userExist.UserID;
                            userloged.Status = false;
                            db.tbl_UserEntranceInfos.Add(userloged);
                            db.SaveChanges();

                            ViewBag.userlogedStatusCount = count;
                            ViewBag.Message = "کلمه عبور صحیح نمی باشد!";
                            ViewBag.focus = "password";
                        }
                        //
                        if(count==5)
                        {
                            _userExist.Inactive = false;
                            db.Entry(_userExist).State = EntityState.Modified;
                            db.SaveChangesAsync();

                            ViewBag.Message = "نام کاربری شما غیر فعال شده است!";
                            ViewBag.Message2 = "به دلیل تکرار بیش از 5 بار اشتباه در ورود، لطفا با مدیر سیستم تماس بگیرید.";
                        }
                    }

                    else
                    {
                        i++;
                        ViewBag.userlogedStatusCount = i;
                        ViewBag.Message = "کلمه عبور صحیح نمی باشد!";
                        ViewBag.focus = "password";
                    }
                    
                }
            }

            if(!isUserAlreadyExists)
            {
                ViewBag.Message = "نام کاربری صحیح نمی باشد!";
                ViewBag.focus = "username";
            }

            System.Threading.Thread.Sleep(1000);
            return View("LoginPage");

        }
        //
        public ActionResult LogOff(string aspxerrorpath, string ReturnUrl)
        {
            //So that the user can be referred back to where they were when they click logon %2f
            if (string.IsNullOrEmpty(aspxerrorpath) && Request.UrlReferrer != null)
                aspxerrorpath = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(aspxerrorpath) && !string.IsNullOrEmpty(aspxerrorpath))
            {
                ViewBag.ReturnURL = aspxerrorpath;
            }
            //
            if (string.IsNullOrEmpty(ReturnUrl) && Request.UrlReferrer != null)
                ReturnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(ReturnUrl) && !string.IsNullOrEmpty(ReturnUrl))
            {
                ViewBag.ReturnURL = ReturnUrl.Replace("%2f", "/");
            }
            string username = User.Identity.Name; //get the users username who is logged in
            if (HttpRuntime.Cache["LoggedInUsers"] != null)//check if the list has been created
            {
                //the list is not null so we retrieve it from the cache
                List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                if (loggedInUsers.Contains(username))//if the user is in the list
                {
                    //then remove them
                    loggedInUsers.Remove(username);
                }
                // else do nothing
            }
            //else do nothing
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginPage",new { ReturnUrl= ReturnUrl });
        }
        public ActionResult MyProfile()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).Include(u=>u.Workshop);
            ViewBag.Image = user.SingleOrDefault().Image;
            return View(user.ToList());
        }
        //
        public ActionResult UserImageUpdate()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).Include(u => u.Workshop).SingleOrDefault();
            return PartialView(user);
        }
        //
        [HttpPost]
        public ActionResult UserImageUpdate(HttpPostedFileBase Image)
        {
            //string errorMessage = "";
            User _user = db.tbl_Users.Where(p => p.Username==User.Identity.Name).FirstOrDefault();

            if (Image != null)
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(Image.InputStream);
                int height = img.Width;
                int width = img.Width;

                if (width > 35 || height > 35)
                {
                    ViewBag.errorMessage = " حداکثر اندازه عکس 35*35 می تواند باشد!";
                }
                if (_user.Image != null)
                {
                    System.IO.File.Delete(Server.MapPath("/UploadedFiles/UsersImage/" + _user.Image));
                }
                _user.Image = _user.Firstname + "_" + _user.Lastname + "_" + Image.FileName;
                string ImagePath = Server.MapPath("/UploadedFiles/UsersImage/" + _user.Image);
                Image.SaveAs(ImagePath);

            }

            _user.Image = _user.Image;
            _user.LastModified = DateTime.Now;
            db.Entry(_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyProfile");
        }
        //
        //
        public ActionResult UserPasswordUpdate()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).Include(u => u.Workshop).SingleOrDefault();
            return PartialView(user);
        }

        //
        public ActionResult ChangePassword()
        {
            var user = db.tbl_Users.Where(u => u.Username == User.Identity.Name).Include(u => u.Workshop).SingleOrDefault();
            return View();
        }
        //
        [HttpPost]
        public ActionResult UserPasswordUpdate(string Password)
        {
            User _user = db.tbl_Users.Where(p => p.Username == User.Identity.Name).FirstOrDefault();

            _user.Password = Password;
            _user.LastModified = DateTime.Now;
            db.Entry(_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyProfile");
        }
        //
        [HttpPost]
        public ActionResult ChangedPassword(string NewPassword)
        {
            User _user = db.tbl_Users.Where(p => p.Username == User.Identity.Name).FirstOrDefault();

            _user.Password = NewPassword;
            _user.LastModified = DateTime.Now;
            db.Entry(_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        //
        public ActionResult UserInactiveUpdate(int? userID)
        {
            var user = db.tbl_Users.Where(u => u.UserID == userID).Include(u => u.Workshop).SingleOrDefault();
            return PartialView(user);
        }
        //
        [HttpPost]
        public ActionResult UserInactiveUpdate(bool? Inactive,int? userID)
        {
            User _user = db.tbl_Users.Where(u => u.UserID == userID).SingleOrDefault();

            _user.Inactive = Inactive;
            _user.LastModified = DateTime.Now;
            db.Entry(_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        //[ChildActionOnly]
        public ActionResult SideBarItems(int? parentId,int? itemId)
        {
            if(User.Identity.Name=="" || string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("LogOff");
            }
            else
            {
                //
                if (parentId != null)
                {
                    Session["parentId"] = parentId;
                    Session["itemId"] = itemId;
                }

                if (parentId == null)
                {
                    parentId = (int?)Session["parentId"];
                    itemId = (int?)Session["itemId"];
                }

                ViewBag.parentId = parentId;
                ViewBag.itemId = itemId;

                //var userID = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;

                //string countrystring = "SELECT [SideBarItems_Id],[User_Id],nameOption,controller,action,imageClass,isParent, parentId FROM[CNGFAPCO].[dbo].[LNK_USER_SideBarItem] INNER JOIN tbl_SideBarItems ON[dbo].[LNK_USER_SideBarItem].SideBarItems_Id=tbl_SideBarItems.ID WHERE User_Id='"+ userID + "' and status = 1 order by orderBy";

                //DataTable dt = new DataTable();
                //dt = objdal.MyMethod(countrystring);
                //List<sidebaritemlist> list = new List<sidebaritemlist>();
                //List<SideBarItem> list2 = new List<SideBarItem>();
                //var modelitems = db.tbl_SideBarItems.ToList();

                //foreach (DataRow row in dt.Rows)
                //{
                //    list.Add(new sidebaritemlist
                //    {
                //        ID = Convert.ToString(row.ItemArray[0]),
                //        nameOption = Convert.ToString(row.ItemArray[2]),
                //        controller = Convert.ToString(row.ItemArray[3]),
                //        action = Convert.ToString(row.ItemArray[4]),
                //        imageClass = Convert.ToString(row.ItemArray[5]),
                //        isParent = Convert.ToBoolean(row.ItemArray[6]),
                //        parentId = Convert.ToString(row.ItemArray[7])
                //    });
                //    modelitems.Where(m => m.ID == Convert.ToInt32(row.ItemArray[0]));
                //    //
                //    //list.Add(new SideBarItem
                //    //{
                //    //    ID= Convert.ToInt32(row.ItemArray[0]),
                //    //    nameOption=Convert.ToString(row.ItemArray[2]),
                //    //    controller= Convert.ToString(row.ItemArray[3]),
                //    //    action = Convert.ToString(row.ItemArray[4]),
                //    //    imageClass = Convert.ToString(row.ItemArray[5]),
                //    //    isParent = Convert.ToBoolean(row.ItemArray[6]),
                //    //    parentId = Convert.ToInt32(row.ItemArray[7])
                //    //});
                //}
                //ViewBag.tableOuts = list.ToList();

                //return PartialView(modelitems);
                var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                User _user = db.tbl_Users.Where(p => p.UserID == userId).FirstOrDefault();
                List<SideBarItem> _sidebarItems = db.tbl_SideBarItems.Where(s => s.status == true && String.IsNullOrEmpty(s.Category)).OrderBy(s => s.orderBy).ToList();
                List<SideBarItem> itemShow = new List<SideBarItem>();
                foreach (SideBarItem _sidebarItem in _sidebarItems)
                {
                    if (_user.SideBarItems.Contains(_sidebarItem))
                    {
                        itemShow.Add(new SideBarItem
                        {
                            action = _sidebarItem.action,
                            Childs = _sidebarItem.Childs,
                            controller = _sidebarItem.controller,
                            ID = _sidebarItem.ID,
                            imageClass = _sidebarItem.imageClass,
                            isParent = _sidebarItem.isParent,
                            nameOption = _sidebarItem.nameOption,
                            orderBy = _sidebarItem.orderBy,
                            Parent = _sidebarItem.Parent,
                            parentId = _sidebarItem.parentId,
                            status = _sidebarItem.status,
                            Users = _sidebarItem.Users,
                            Category=_sidebarItem.Category
                        });
                    }
                }
                return PartialView(itemShow);
            }
           
        }
        //
        public ActionResult SubSideBarItems(int? parentId, int? itemId)
        {
            ViewBag.parentOptionName = "یافت نشد!";
            //
            if (User.Identity.Name == "" || string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("LogOff");
            }
            else
            {
                //
                if (parentId != null)
                {
                    ViewBag.parentOptionName = db.tbl_SideBarItems.Find(parentId).nameOption;
                    Session["parentId"] = parentId;
                    Session["itemId"] = itemId;                   
                }

                if (parentId == null)
                {
                    parentId = (int?)Session["parentId"];
                    itemId = (int?)Session["itemId"];
                    if (parentId == null)
                    {
                        parentId = 35;
                        ViewBag.parentOptionName = db.tbl_SideBarItems.Find(parentId).nameOption;
                    }
                    
                }

                ViewBag.parentId = parentId;
                ViewBag.itemId = itemId;
                var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
                User _user = db.tbl_Users.Where(p => p.UserID == userId).FirstOrDefault();
                List<SideBarItem> _sidebarItems = db.tbl_SideBarItems.Where(s => s.status == true && s.parentId == parentId && !String.IsNullOrEmpty(s.Category)).OrderBy(s => s.orderBy).ToList();
                List<SideBarItem> itemShow = new List<SideBarItem>();
                foreach (SideBarItem _sidebarItem in _sidebarItems)
                {
                    if (_user.SideBarItems.Contains(_sidebarItem))
                    {
                        itemShow.Add(new SideBarItem
                        {
                            action = _sidebarItem.action,
                            Childs = _sidebarItem.Childs,
                            controller = _sidebarItem.controller,
                            ID = _sidebarItem.ID,
                            imageClass = _sidebarItem.imageClass,
                            isParent = _sidebarItem.isParent,
                            nameOption = _sidebarItem.nameOption,
                            orderBy = _sidebarItem.orderBy,
                            Parent = _sidebarItem.Parent,
                            parentId = _sidebarItem.parentId,
                            status = _sidebarItem.status,
                            Users = _sidebarItem.Users,
                            Category= _sidebarItem.Category
                        });
                    }
                }
                return View(itemShow);
            }

        }
        //
        public class sidebaritemlist
        {
            public string ID { get; set; }
            public string nameOption { get; set; }
            public string controller { get; set; }
            public string action { get; set; }
            public string imageClass { get; set; }
            public bool isParent { get; set; }
            public string parentId { get; set; }
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllSideBarItems2User(int? userId)
        {
            User _user = db.tbl_Users.Where(p => p.UserID == userId).FirstOrDefault();
            List<SideBarItem> _sidebaritems = db.tbl_SideBarItems.ToList();
            foreach (SideBarItem _sidebaritem in _sidebaritems)
            {
                if (!_user.SideBarItems.Contains(_sidebaritem))
                {
                    _user.SideBarItems.Add(_sidebaritem);

                }
            }
            db.SaveChanges();
            return PartialView("_ListSideBarItems", _user);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserSideBarItemsReturnPartialView(int id, int userId)
        {
            SideBarItem sidebaritem = db.tbl_SideBarItems.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (!user.SideBarItems.Contains(sidebaritem))
            {
                user.SideBarItems.Add(sidebaritem);
                db.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListSideBarItems", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserSideBarItemsReturnPartialView(int id, int userId)
        {
            SideBarItem _sidebaritem = db.tbl_SideBarItems.Find(id);
            User user = db.tbl_Users.Find(userId);

            if (_sidebaritem.Users.Contains(user))
            {
                _sidebaritem.Users.Remove(user);
                db.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListSideBarItems", db.tbl_Users.Find(userId));
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteAllSideBarItemsfromUser(int? userId)
        {
            User _user = db.tbl_Users.Where(p => p.UserID == userId).FirstOrDefault();
            List<SideBarItem> _sidebaritems = db.tbl_SideBarItems.ToList();
            foreach (SideBarItem _sidebaritem in _sidebaritems)
            {
                if (_sidebaritem.Users.Contains(_user))
                {
                    _sidebaritem.Users.Remove(_user);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
            return PartialView("_ListSideBarItems", _user);
        }
        //
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteAllPermissionfromUser(int id)
        {
            Role _role = db.tbl_Roles.Where(p => p.Role_Id == id).FirstOrDefault();
            List<Permission> _permissions = db.tbl_Permissions.ToList();
            foreach (Permission _permission in _permissions)
            {
                if (_role.Permissions.Contains(_permission))
                {
                    _role.Permissions.Remove(_permission);

                }
            }
            db.SaveChanges();
            return PartialView("_ListPermissions", _role);
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
