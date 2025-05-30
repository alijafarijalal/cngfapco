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
using System.Data.SqlClient;
using System.Configuration;

namespace cngfapco.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ContextDB db = new ContextDB();
        PersianCalendar pc = new PersianCalendar();
        DAL objdal = new DAL();

        //Notification
        public ActionResult Notification()
        {
            var users= db.tbl_Users.Where(u => u.Username == User.Identity.Name).Include(u=>u.Workshop);
            int? userId = 0;
            if (users.Count() > 0)
                userId = users.SingleOrDefault().UserID;
            string workshops = "0";
            List<Message> _messages = db.tbl_Messages.Where(m => m.ReadStatus != true && m.ReciverID == userId).ToList();
            //
            try
            {
                var isServices = db.tbl_Users.Find(userId).Workshop.isServices;
                //
                if (isServices == true)
                    workshops = Helper.Helpers.GetCurrentUserWorkshopId(userId).ToString();
                //
                ViewBag.userRole = false;
                ViewBag.isWorkshop = false;
                string userRole = Helper.Helpers.GetCurrentUserRole();
                if (userRole.Equals("مدیر فروش"))
                    ViewBag.userRole = true;
                else if (userRole.Equals("مرکز خدمات (کارگاه)"))
                    ViewBag.isWorkshop = true;
            }
            catch
            {
                ViewBag.userRole = false;
                ViewBag.isWorkshop = false;
            }
            
            //
            string Workshop = "";
            string InvoiceCode = "";
            DateTime? CreatedDate = null;
            string Number = "";
            //
            int requestCount = 0;
            int preInvoiceCount = 0;
            SqlDataReader reader;
            List<FreeSaleRequestAlarms> request = new List<FreeSaleRequestAlarms>();
            List<FreeSaleRequestAlarms> preInvoice = new List<FreeSaleRequestAlarms>();
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_FreeSaleRequestAlarms]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = workshops;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader["CreatedDate"].ToString()))
                        CreatedDate =Convert.ToDateTime(reader["CreatedDate"].ToString());
                    Workshop = reader["Workshop"].ToString();
                    InvoiceCode = reader["InvoiceCode"].ToString();
                    Number = reader["Number"].ToString();
                    request.Add(new FreeSaleRequestAlarms
                    {
                        CreatedDate=CreatedDate.GetValueOrDefault().ToShortDateString(),
                        InvoiceCode=InvoiceCode,
                        Number=Number,
                        Workshop=Workshop
                    });
                    requestCount += 1;
                }
                ViewBag.request = request.Take(5);
                ViewBag.requestCount = requestCount;
                //
                reader.NextResult();
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader["CreatedDate"].ToString()))
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    Workshop = reader["Workshop"].ToString();
                    InvoiceCode = reader["RequestInvoiceCode"].ToString();
                    Number = reader["Number"].ToString();
                    preInvoice.Add(new FreeSaleRequestAlarms
                    {
                        CreatedDate = CreatedDate.GetValueOrDefault().ToShortDateString(),
                        InvoiceCode = InvoiceCode,
                        Number = Number,
                        Workshop = Workshop
                    });
                    preInvoiceCount += 1;
                }
                ViewBag.preInvoice = preInvoice.Take(5);
                ViewBag.preInvoiceCount = preInvoiceCount;
                //
                conn.Close();
            }
            //

            return PartialView(_messages);
        }
        //
        public class FreeSaleRequestAlarms
        {
            public string CreatedDate { get; set; }
            public string Workshop { get; set; }
            public string InvoiceCode { get; set; }
            public string Number { get; set; }
        }
        // GET: Messages
        public ActionResult Inbox()
        {
            int? userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            User _user = db.tbl_Users.Find(userId);
            List<Message> _messages = db.tbl_Messages.Where(m => m.ReciverID == userId).ToList();
            ViewBag.unreadCount = _messages.Where(m => m.ReadStatus == null).Count();
            ViewBag.Image = _user.Image;
            ViewBag.EMail = _user.EMail;
            //List<Message> list = new List<Message>();

            //foreach (Message _message in _messages)
            //{
            //    Workshop _workshop = db.tbl_Workshops.Find(_message.WorkshopID);

            //    if (_user.Workshops.Contains(_workshop))
            //    {
            //        list.Add(new Message
            //        {
            //            ID = _message.ID,
            //            SenderDate = _message.SenderDate,
            //            Sender = _message.Sender,
            //            ReadStatus = _message.ReadStatus,
            //            Subject = _message.Subject,
            //            Attachment = _message.Attachment,
            //            Description = _message.Description,
            //            Priority = _message.Priority,
            //            LetterNumber=_message.LetterNumber,
            //            ReadDate=_message.ReadDate,
            //            SenderID=_message.SenderID,
            //            Workshop=_message.Workshop,
            //            WorkshopID=_message.WorkshopID
            //        });
            //    }
            //}
            return PartialView(_messages.OrderByDescending(m => m.ID));
        }

        // GET: Messages
        public ActionResult PriorityInbox()
        {
            int? userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            User _user = db.tbl_Users.Find(userId);
            List<Message> _messages = db.tbl_Messages.Where(m => m.ReciverID == userId && m.Priority.Contains("فوری") && (m.ReadStatus == null || m.ReadStatus == false)).ToList();
            ViewBag.unreadCount = _messages.Where(m => m.ReadStatus == null).Count();
            ViewBag.Image = _user.Image;
            ViewBag.EMail = _user.EMail;
            return PartialView(_messages.OrderByDescending(m=>m.SenderDate));
        }
        // GET: Messages
        public ActionResult Sendbox()
        {
            int? userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var tbl_Messages = db.tbl_Messages.Include(m => m.Sender).Where(m => m.SenderID == userId);
            ViewBag.unreadCount = tbl_Messages.Where(m => m.ReadStatus == null).Count();
            User _user = db.tbl_Users.Find(userId);
            ViewBag.Image = _user.Image;
            ViewBag.EMail = _user.EMail;
            return View(tbl_Messages.OrderByDescending(m => m.ID).ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.tbl_Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            //
            if(message.ReadStatus==null)
            {
                message.ReadStatus = true;
                message.ReadDate = DateTime.Now;
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.ReplyItems = db.tbl_Messages.Where(m=>m.ID == message.MessageID && message.Type==1).Include(m=>m.Messages).Include(m => m.Sender).OrderByDescending(m=>m.SenderDate).ToList();
            //
            return View(message);
        }
        // GET: Messages/Details/5
        public ActionResult SendboxDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.tbl_Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            //
            //message.ReadStatus = true;
            //message.ReadDate = DateTime.Now;
            //db.Entry(message).State = EntityState.Modified;
            //db.SaveChanges();
            //
            ViewBag.ReplyItems = db.tbl_MessageReplies.Where(m => m.MessageID == message.MessageID).Include(m => m.Message).Include(m => m.Sender).OrderByDescending(m => m.SenderDate).Take(1).ToList();
            return View(message);
        }


        // GET: Messages/Create
        public ActionResult Create()
        {
            string countrystring = "SELECT wo.[ID],wo.[Title],st.Title,ci.Title FROM [CNGFAPCO].[dbo].[tbl_Workshops] wo inner join[CNGFAPCO].[dbo].[tbl_Cities] ci on wo.CityID = ci.ID inner join[CNGFAPCO].[dbo].[tbl_States] st on ci.StateID=st.ID where wo.ID<>1";
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> userList = new List<SelectListItem>();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });
            var Users = db.tbl_Users.Include(u => u.Workshops).ToList();

            foreach (var user in Users.Where(u=>u.WorkshopID==1 || u.WorkshopID==8))
            {
                //Workshop workshop = db.tbl_Workshops.Find(1);
                //User _user = db.tbl_Users.Find(user.UserID);

                //if (_user.Workshops.Contains(workshop))
                //{
                    userList.Add(new SelectListItem { Text = user.Firstname + " " + user.Lastname + "-" + user.Workshop.Title.Replace("مرکز خدمات CNG ", ""), Value = user.UserID.ToString() });
                //}
            }

            ViewBag.UserID = new SelectList(userList, "Value", "Text");
            //ViewBag.WorkshopID2 = db.tbl_Workshops.Where(w=>w.FapCode.Equals("FAP001")).SingleOrDefault().ID;
            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                var roles = db.tbl_Roles.ToList();
                foreach (var item in roles)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    if (!role.Users.Contains(user))
                    {
                        list.Add(new SelectListItem
                        {
                            Value = item.Role_Id.ToString(),
                            Text = item.RoleName
                        });
                    }
                }
                //
                ViewBag.RoleId = new SelectList(list, "Value", "Text");

            }
            
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    Workshop workshop = db.tbl_Workshops.Find(row.ItemArray[0]);
                    User _user = db.tbl_Users.Find(userId);

                    if (workshop.Users.Contains(_user))
                    {
                        list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]) + " - " + Convert.ToString(row.ItemArray[3]) + " - " + Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
                    }
                }

                ViewBag.WorkshopID = new SelectList(list, "Value", "Text");
            }

            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,Description,SenderID,SenderDate,ReadStatus,ReadDate,Priority,LetterNumber")] Message message,HttpPostedFileBase Attachment,bool? Allworkshop,bool? isAdmin,int?[] WorkshopID,int?[] RoleId, string Subject2,int?[] UserID)
        {
            if (message.Subject == "0")
                message.Subject = Subject2;

            if (string.IsNullOrEmpty(Subject2) && message.Subject == "0")
                message.Subject = "بدون موضوع";

            var userId= db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var _workshops = db.tbl_Workshops.Where(w=>w.isServices==true).ToList();
            ViewBag.isAdmin = isAdmin;
            message.SenderID = userId;
            message.SenderDate = DateTime.Now;
            message.Type = 0;

            try
            {
                if (message.WorkshopID == null)
                    message.WorkshopID = db.tbl_Workshops.Where(w => w.Title.Contains("دفتر مرکزی") || w.FapCode.Contains("FAP001")).SingleOrDefault().ID;
            }
            catch
            {
                message.WorkshopID = 1;
            }
            

            if (ModelState.IsValid)
            {
                if(Attachment!=null)
                {
                    if(message.Attachment!=null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + message.Attachment));
                    }
                    message.Attachment = message.SenderID + "_" + pc.GetYear(message.SenderDate) + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/MessagesAttach/" + message.Attachment);
                    Attachment.SaveAs(ImagePath);
                }
                //  
                //if(RoleId!=null || WorkshopID!=null || UserID!=null)
                //{
                    if (RoleId != null)
                    {
                        var _users = db.tbl_Users.ToList();

                        foreach (var item in RoleId)
                        {
                            foreach (var users in _users)
                            {
                                Role role = db.tbl_Roles.Find(item.Value);
                                User user = db.tbl_Users.Find(users.UserID);
                                if (user.Roles.Contains(role))
                                {
                                    message.WorkshopID = users.WorkshopID;
                                    message.ReciverID = users.UserID;
                                    db.tbl_Messages.Add(message);
                                    db.SaveChanges();
                                }
                            }
                        }

                        //return RedirectToAction("Sendbox");
                    }
                    if (UserID != null)
                    {
                        foreach (var users in UserID)
                        {
                            message.WorkshopID = Helper.Helpers.GetCurrentUserWorkshopId(users);
                            message.ReciverID = users;
                            db.tbl_Messages.Add(message);
                            db.SaveChanges();
                        }
                        //return RedirectToAction("Sendbox");
                    }
                    else
                    {
                        if (Allworkshop == true)
                        {
                            foreach (var item in _workshops)
                            {
                                Workshop workshop = db.tbl_Workshops.Find(item.ID);
                                User _user = db.tbl_Users.Find(userId);

                                if (workshop.Users.Contains(_user))
                                {
                                    var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == item.ID).ToList();
                                    foreach (var items in _users)
                                    {
                                        if (items.UserID != userId)
                                        {
                                            message.WorkshopID = items.WorkshopID;
                                            message.ReciverID = items.UserID;
                                            db.tbl_Messages.Add(message);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            //Workshop _workshop = db.tbl_Workshops.Find(message.WorkshopID);
                            if (WorkshopID != null)
                            {
                                foreach (var _workshop in WorkshopID)
                                {
                                    var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == _workshop.Value).ToList();
                                    foreach (var item in _users)
                                    {
                                        if (item.UserID != userId)
                                        {
                                            message.WorkshopID = item.WorkshopID;
                                            message.ReciverID = item.UserID;
                                            db.tbl_Messages.Add(message);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                            }
                        }

                        //return RedirectToAction("Sendbox");
                    }

                    return RedirectToAction("Sendbox");
               // }
               
            }
            //
            List<SelectListItem> userList = new List<SelectListItem>();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });
            var Users = db.tbl_Users.Include(u => u.Workshops).ToList();

            foreach (var user in Users.Where(u => u.WorkshopID == 1 || u.WorkshopID == 8))
            {                
                userList.Add(new SelectListItem { Text = user.Firstname + " " + user.Lastname + "-" + user.Workshop.Title.Replace("مرکز خدمات CNG ", ""), Value = user.UserID.ToString() });
            }

            ViewBag.UserID = new SelectList(userList, "Value", "Text");
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            ViewBag.Description = message.Description;
            ViewBag.errorMesage = "لطفا فرد/ کارگاه دریافت کننده پیام را انتخاب کنید!";
            //
            return View(message);
        }

        /// <summary>
        /// برای ارسال پاسخ به پیام های دریافتی
        /// </summary>
        /// <returns></returns>
        // GET: Messages/Create
        public ActionResult Reply(int? id)
        {
            //string countrystring = "SELECT wo.[ID],wo.[Title],st.Title,ci.Title FROM [CNGFAPCO].[dbo].[tbl_Workshops] wo inner join[CNGFAPCO].[dbo].[tbl_Cities] ci on wo.CityID = ci.ID inner join[CNGFAPCO].[dbo].[tbl_States] st on ci.StateID=st.ID where wo.ID<>1";
            //var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            //DataTable dt = new DataTable();
            //dt = objdal.MyMethod(countrystring);
            //List<SelectListItem> userList = new List<SelectListItem>();
            //List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Text = "", Value = "0" });
            //var Users = db.tbl_Users.Include(u => u.Workshops).ToList();

            //foreach (var user in Users.Where(u => u.WorkshopID == 1 || u.WorkshopID == 8))
            //{
            //    Workshop workshop = db.tbl_Workshops.Find(1);
            //    User _user = db.tbl_Users.Find(user.UserID);

            //    if (_user.Workshops.Contains(workshop))
            //    {
            //        userList.Add(new SelectListItem { Text = user.Firstname + " " + user.Lastname + "-" + user.Workshop.Title.Replace("مرکز خدمات CNG ", ""), Value = user.UserID.ToString() });
            //    }
            //}

            //ViewBag.UserID = new SelectList(userList, "Value", "Text");
            ////ViewBag.WorkshopID2 = db.tbl_Workshops.Where(w=>w.FapCode.Equals("FAP001")).SingleOrDefault().ID;
            //if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            //{
            //    var roles = db.tbl_Roles.ToList();
            //    foreach (var item in roles)
            //    {
            //        Role role = db.tbl_Roles.Find(item.Role_Id);
            //        User user = db.tbl_Users.Find(userId);

            //        if (!role.Users.Contains(user))
            //        {
            //            list.Add(new SelectListItem
            //            {
            //                Value = item.Role_Id.ToString(),
            //                Text = item.RoleName
            //            });
            //        }
            //    }
            //    //
            //    ViewBag.RoleId = new SelectList(list, "Value", "Text");

            //}

            //else
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        Workshop workshop = db.tbl_Workshops.Find(row.ItemArray[0]);
            //        User _user = db.tbl_Users.Find(userId);

            //        if (workshop.Users.Contains(_user))
            //        {
            //            list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]) + " - " + Convert.ToString(row.ItemArray[3]) + " - " + Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
            //        }
            //    }

            //    ViewBag.WorkshopID = new SelectList(list, "Value", "Text");
            //}

            var message = db.tbl_Messages.Find(id);
            ViewBag.Id = id;
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "ID,Subject,Description,SenderID,SenderDate,ReadStatus,ReadDate,Priority,LetterNumber")] Message message, HttpPostedFileBase Attachment, bool? Allworkshop, bool? isAdmin, int?[] WorkshopID, int?[] RoleId, int?[] UserID,int? MessageId)
        {
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var _workshops = db.tbl_Workshops.ToList();
            ViewBag.isAdmin = isAdmin;
            var oldMessage = db.tbl_Messages.Find(MessageId);
            MessageReply reply = new MessageReply();
            message.SenderID = userId;
            message.SenderDate = DateTime.Now;
            message.Type = 1;
            message.MessageID = MessageId;
            message.ReciverID = oldMessage.SenderID;
            message.WorkshopID = db.tbl_Users.Where(u=>u.UserID == oldMessage.SenderID).FirstOrDefault().WorkshopID;

            reply.SenderID = userId;
            reply.SenderDate = DateTime.Now;
            reply.MessageID = MessageId;
            reply.Description = oldMessage.Description;
            reply.Subject = oldMessage.Subject;
            reply.ReciverID = oldMessage.SenderID;

            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    if (oldMessage.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + oldMessage.Attachment));
                    }
                    message.Attachment = oldMessage.SenderID + "_" + pc.GetYear(oldMessage.SenderDate) + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/MessagesAttach/" + message.Attachment);
                    Attachment.SaveAs(ImagePath);
                }
                //
                if (Attachment != null)
                {
                    if (reply.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + reply.Attachment));
                    }
                    reply.Attachment = reply.SenderID + "_" + pc.GetYear(reply.SenderDate) + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/MessagesAttach/" + reply.Attachment);
                    Attachment.SaveAs(ImagePath);
                }

                db.tbl_Messages.Add(message);
                db.SaveChanges();

                //reply.ReciverID = message.ReciverID;
                db.tbl_MessageReplies.Add(reply);
                db.SaveChanges();
                return RedirectToAction("Sendbox");
                //  
                //if (RoleId != null)
                //{
                //    var _users = db.tbl_Users.ToList();

                //    foreach (var item in RoleId)
                //    {
                //        foreach (var users in _users)
                //        {
                //            Role role = db.tbl_Roles.Find(item.Value);
                //            User user = db.tbl_Users.Find(users.UserID);
                //            if (user.Roles.Contains(role))
                //            {
                //                message.ReciverID = users.UserID;
                //                db.tbl_Messages.Add(message);
                //                db.SaveChanges();

                //                reply.ReciverID = users.UserID;
                //                db.tbl_MessageReplies.Add(reply);
                //                db.SaveChanges();
                //            }
                //        }
                //    }

                //    //return RedirectToAction("Sendbox");
                //}
                //if (UserID != null)
                //{
                //    foreach (var users in UserID)
                //    {
                //        message.ReciverID = users;
                //        message.WorkshopID = Helper.Helpers.GetCurrentUserWorkshopId(users);
                //        db.tbl_Messages.Add(message);
                //        db.SaveChanges();

                //        reply.ReciverID = users;
                //        db.tbl_MessageReplies.Add(reply);
                //        db.SaveChanges();
                //    }
                //    //return RedirectToAction("Sendbox");
                //}
                //else
                //{
                //    if (Allworkshop == true)
                //    {
                //        foreach (var item in _workshops)
                //        {
                //            Workshop workshop = db.tbl_Workshops.Find(item.ID);
                //            User _user = db.tbl_Users.Find(userId);

                //            if (workshop.Users.Contains(_user))
                //            {
                //                var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == item.ID).ToList();
                //                foreach (var items in _users)
                //                {
                //                    if (items.UserID != userId)
                //                    {
                //                        message.ReciverID = items.UserID;
                //                        message.WorkshopID = items.WorkshopID;
                //                        db.tbl_Messages.Add(message);
                //                        db.SaveChanges();

                //                        reply.ReciverID = items.UserID;
                //                        db.tbl_MessageReplies.Add(reply);
                //                        db.SaveChanges();
                //                    }
                //                }
                //            }
                //        }

                //    }
                //    else
                //    {
                //        //Workshop _workshop = db.tbl_Workshops.Find(message.WorkshopID);
                //        foreach (var _workshop in WorkshopID)
                //        {
                //            var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == _workshop.Value).ToList();
                //            foreach (var item in _users)
                //            {
                //                if (item.UserID != userId)
                //                {
                //                    message.ReciverID = item.UserID;
                //                    db.tbl_Messages.Add(message);
                //                    db.SaveChanges();

                //                    reply.ReciverID = item.UserID;
                //                    db.tbl_MessageReplies.Add(reply);
                //                    db.SaveChanges();
                //                }
                //            }

                //        }
                //    }

                //    //return RedirectToAction("Sendbox");
                //}

            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            //ViewBag.ReciverID = new SelectList(db.tbl_Users, "UserID", "Username", message.ReciverID);
            return View(message);
        }

        /// <summary>
        /// برای ارسال پاسخ به پیام های دریافتی
        /// </summary>
        /// <returns></returns>
        // GET: Messages/Create
        public ActionResult Forward(int? id)
        {
            string countrystring = "SELECT wo.[ID],wo.[Title],st.Title,ci.Title FROM [CNGFAPCO].[dbo].[tbl_Workshops] wo inner join[CNGFAPCO].[dbo].[tbl_Cities] ci on wo.CityID = ci.ID inner join[CNGFAPCO].[dbo].[tbl_States] st on ci.StateID=st.ID where wo.ID<>1";
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            DataTable dt = new DataTable();
            dt = objdal.MyMethod(countrystring);
            List<SelectListItem> userList = new List<SelectListItem>();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "0" });
            var Users = db.tbl_Users.Include(u => u.Workshops).ToList();

            foreach (var user in Users.Where(u => u.WorkshopID == 1 || u.WorkshopID == 8))
            {
                //Workshop workshop = db.tbl_Workshops.Find(1);
                //User _user = db.tbl_Users.Find(user.UserID);

                //if (_user.Workshops.Contains(workshop))
                //{
                    userList.Add(new SelectListItem { Text = user.Firstname + " " + user.Lastname + "-" + user.Workshop.Title.Replace("مرکز خدمات CNG ", ""), Value = user.UserID.ToString() });
                //}
            }

            ViewBag.UserID = new SelectList(userList, "Value", "Text");
            //ViewBag.WorkshopID2 = db.tbl_Workshops.Where(w=>w.FapCode.Equals("FAP001")).SingleOrDefault().ID;
            if (Helper.Helpers.GetCurrentUserRole().Contains("مرکز خدمات (کارگاه)"))
            {
                var roles = db.tbl_Roles.ToList();
                foreach (var item in roles)
                {
                    Role role = db.tbl_Roles.Find(item.Role_Id);
                    User user = db.tbl_Users.Find(userId);

                    if (!role.Users.Contains(user))
                    {
                        list.Add(new SelectListItem
                        {
                            Value = item.Role_Id.ToString(),
                            Text = item.RoleName
                        });
                    }
                }
                //
                ViewBag.RoleId = new SelectList(list, "Value", "Text");

            }

            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    Workshop workshop = db.tbl_Workshops.Find(row.ItemArray[0]);
                    User _user = db.tbl_Users.Find(userId);

                    if (workshop.Users.Contains(_user))
                    {
                        list.Add(new SelectListItem { Text = Convert.ToString(row.ItemArray[2]) + " - " + Convert.ToString(row.ItemArray[3]) + " - " + Convert.ToString(row.ItemArray[1]), Value = Convert.ToString(row.ItemArray[0]) });
                    }
                }

                ViewBag.WorkshopID = new SelectList(list, "Value", "Text");
            }

            var message = db.tbl_Messages.Find(id);
            ViewBag.Id = id;
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forward([Bind(Include = "ID,Subject,Description,SenderID,SenderDate,ReadStatus,ReadDate,Priority,LetterNumber")] Message message, HttpPostedFileBase Attachment, bool? Allworkshop, bool? isAdmin, int?[] WorkshopID, int?[] RoleId, int?[] UserID, int? MessageId)
        {
            var userId = db.tbl_Users.Where(u => u.Username == User.Identity.Name).SingleOrDefault().UserID;
            var _workshops = db.tbl_Workshops.ToList();
            ViewBag.isAdmin = isAdmin;
            MessageForward reply = new MessageForward();
            message.SenderID = userId;
            message.SenderDate = DateTime.Now;
            message.Type = 2;
            message.MessageID = MessageId;

            reply.SenderID = userId;
            reply.SenderDate = DateTime.Now;
            reply.MessageID = MessageId;
            reply.ReciverID = userId;

            if (ModelState.IsValid)
            {
                if (Attachment != null)
                {
                    if (message.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + message.Attachment));
                    }
                    message.Attachment = message.SenderID + "_" + pc.GetYear(message.SenderDate) + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/MessagesAttach/" + message.Attachment);
                    Attachment.SaveAs(ImagePath);
                }
                //
                if (Attachment != null)
                {
                    if (reply.Attachment != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/UploadedFiles/MessagesAttach/" + reply.Attachment));
                    }
                    reply.Attachment = reply.SenderID + "_" + pc.GetYear(reply.SenderDate) + "_" + Attachment.FileName;
                    string ImagePath = Server.MapPath("/UploadedFiles/MessagesAttach/" + reply.Attachment);
                    Attachment.SaveAs(ImagePath);
                }
                //  
                if (RoleId != null)
                {
                    var _users = db.tbl_Users.ToList();

                    foreach (var item in RoleId)
                    {
                        foreach (var users in _users)
                        {
                            Role role = db.tbl_Roles.Find(item.Value);
                            User user = db.tbl_Users.Find(users.UserID);
                            if (user.Roles.Contains(role))
                            {
                                message.ReciverID = users.UserID;
                                db.tbl_Messages.Add(message);
                                db.SaveChanges();

                                reply.ReciverID = users.UserID;
                                db.tbl_MessageForwards.Add(reply);
                                db.SaveChanges();
                            }
                        }
                    }

                    //return RedirectToAction("Sendbox");
                }
                if (UserID != null)
                {
                    foreach (var users in UserID)
                    {
                        message.ReciverID = users;
                        message.WorkshopID = Helper.Helpers.GetCurrentUserWorkshopId(users);
                        db.tbl_Messages.Add(message);
                        db.SaveChanges();

                        reply.ReciverID = users;
                        db.tbl_MessageForwards.Add(reply);
                        db.SaveChanges();
                    }
                    //return RedirectToAction("Sendbox");
                }
                else
                {
                    if (Allworkshop == true)
                    {
                        foreach (var item in _workshops)
                        {
                            Workshop workshop = db.tbl_Workshops.Find(item.ID);
                            User _user = db.tbl_Users.Find(userId);

                            if (workshop.Users.Contains(_user))
                            {
                                var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == item.ID).ToList();
                                foreach (var items in _users)
                                {
                                    if (items.UserID != userId)
                                    {
                                        message.ReciverID = items.UserID;
                                        message.WorkshopID = items.WorkshopID;
                                        db.tbl_Messages.Add(message);
                                        db.SaveChanges();

                                        reply.ReciverID = items.UserID;
                                        db.tbl_MessageForwards.Add(reply);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        //Workshop _workshop = db.tbl_Workshops.Find(message.WorkshopID);
                        foreach (var _workshop in WorkshopID)
                        {
                            var _users = db.tbl_Users.Include(u => u.Workshop).Where(u => u.WorkshopID == _workshop.Value).ToList();
                            foreach (var item in _users)
                            {
                                if (item.UserID != userId)
                                {
                                    message.ReciverID = item.UserID;
                                    db.tbl_Messages.Add(message);
                                    db.SaveChanges();

                                    reply.ReciverID = item.UserID;
                                    db.tbl_MessageForwards.Add(reply);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    //return RedirectToAction("Sendbox");
                }

                return RedirectToAction("Sendbox");

            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title");
            //ViewBag.ReciverID = new SelectList(db.tbl_Users, "UserID", "Username", message.ReciverID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.tbl_Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", message.WorkshopID);
            //ViewBag.ReciverID = new SelectList(db.tbl_Users, "UserID", "Username", message.ReciverID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,Description,SenderID,SenderDate,WorkshopID,ReadStatus,ReadDate,Attachment,Priority,LetterNumber")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Inbox");
            }
            ViewBag.WorkshopID = new SelectList(db.tbl_Workshops, "ID", "Title", message.WorkshopID);
            //ViewBag.ReciverID = new SelectList(db.tbl_Users, "UserID", "Username", message.ReciverID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.tbl_Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.tbl_Messages.Find(id);
            db.tbl_Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Inbox");
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
