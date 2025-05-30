using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cngfapco.Models;
using LinqToExcel;

namespace cngfapco.Controllers
{
    public class ContradictionsController : Controller
    {
        private ContextDB db = new ContextDB();

        // GET: Contradictions
        public ActionResult Index()
        {
            var tbl_Contradictions = db.tbl_Contradictions.Include(c => c.Workshops);
            return View(tbl_Contradictions.OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Contradictions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contradiction contradiction = db.tbl_Contradictions.Find(id);
            if (contradiction == null)
            {
                return HttpNotFound();
            }
            return View(contradiction);
        }

        // GET: Contradictions/Create
        public ActionResult Create(int?[] WorkshopID,DateTime Date,bool? Post)
        {
            var maxDate = db.tbl_Contradictions.ToList();
            if (Post != true)
                if (maxDate.Count() > 0)
                    Date = maxDate.Max(c => c.Date).Date;

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Contradictions.Include(c => c.Workshops).GroupBy(c => c.WorkshopId).ToList();
            string permission = "";

            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {               
                foreach (var item in workshops)
                {
                    permission += item.Key + ",";

                    tableOuts.Add(new Workshop
                    {
                        ID = Convert.ToInt32(item.Key),
                        Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.Key)).Title
                    });

                };

                ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");

            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    var Record = db.tbl_Contradictions.Where(c => c.WorkshopId == item && c.Date.Equals(Date)).FirstOrDefault();
                    if (Record != null)
                    {
                        ViewBag.Record = "داده های مربوط به این مرکز در تاریخ مربوطه قبلا وارد شده است!";
                        ViewBag.existRecord = true;
                    }
                        
                    permission += item + ",";
                    tableOuts.Add(new Workshop
                    {
                        ID = Convert.ToInt32(item),
                        Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item)).Title
                    });
                };
                ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");

            }
            if (string.IsNullOrEmpty(permission))
                permission = "1,";
            //برای محاسبه تعداد تبدیل فن آووران بخش جدول مغایرت عملکرد
            string VehicleType1 = "0";
            string VehicleType2 = "0";
            string VehicleType3 = "0";
            string VehicleType4 = "0";
            string VehicleType5 = "0";
            //string VehicleTypeOther = "0";
            string Sum = "0";
            //برای محاسبه تعداد اقلام ارسالی بخش جدول مغایرت عملکرد
            string DivisionType1 = "0";
            string DivisionType2 = "0";
            string DivisionType3 = "0";
            string DivisionType4 = "0";
            string DivisionType5 = "0";
            //string DivisionTypeOther = "0";
            //string DivisionSum = "0";
            //
            List<tableOuts> mainTable = new List<tableOuts>();
            List<divisionPlan> divisionTable = new List<divisionPlan>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_VehicleRegistrationinContradictions]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["VehicleTypeID"].ToString().Equals("4"))
                        VehicleType1 = reader["Count"].ToString();
                    if (reader["VehicleTypeID"].ToString().Equals("1"))
                        VehicleType2 = reader["Count"].ToString();
                    if (reader["VehicleTypeID"].ToString().Equals("3"))
                        VehicleType3 = reader["Count"].ToString();
                    if (reader["VehicleTypeID"].ToString().Equals("5"))
                        VehicleType4 = reader["Count"].ToString();
                    if (reader["VehicleTypeID"].ToString().Equals("9"))
                        VehicleType5 = reader["Count"].ToString();

                    Sum =(Convert.ToDouble(VehicleType1)+ Convert.ToDouble(VehicleType2)+ Convert.ToDouble(VehicleType3)+ Convert.ToDouble(VehicleType4)+ Convert.ToDouble(VehicleType5)).ToString();

                    //
                    mainTable.Add(new tableOuts
                    {
                        VehicleType1 = VehicleType1,
                        VehicleType2 = VehicleType2,
                        VehicleType3 = VehicleType3,
                        VehicleType4 = VehicleType4,
                        VehicleType5 = VehicleType5,
                        Sum = Sum
                    });
                }
                try
                {
                    reader.NextResult();
                    while (reader.Read())
                    {
                        if (reader["Type"].ToString().Equals("62 لیتری"))
                        {
                            DivisionType1 = reader["Send"].ToString();
                        }
                        if (reader["Type"].ToString().Equals("75 لیتری"))
                        {
                            DivisionType2 = reader["Send"].ToString();
                        }
                        if (reader["Type"].ToString().Equals("100 لیتری"))
                        {
                            DivisionType3 = reader["Send"].ToString();
                        }
                        if (reader["Type"].ToString().Equals("113 لیتری"))
                        {
                            DivisionType4 = reader["Send"].ToString();
                        }
                        //else
                        //DivisionType5 = reader["Send"].ToString();
                        DivisionType5 = "0";
                        //
                        divisionTable.Add(new divisionPlan
                        {
                            DivisionType1 = DivisionType1,
                            DivisionType2 = DivisionType2,
                            DivisionType3 = DivisionType3,
                            DivisionType4 = DivisionType4,
                            DivisionType5 = DivisionType5
                        });
                    }
                }
                catch { }

                conn.Close();
            }

            ViewBag.VehicleType1 = VehicleType1;
            ViewBag.VehicleType2 = VehicleType2;
            ViewBag.VehicleType3 = VehicleType3;
            ViewBag.VehicleType4 = VehicleType4;
            ViewBag.VehicleType5 = VehicleType5;
            //
            ViewBag.DivisionType1 = DivisionType1;
            ViewBag.DivisionType2 = DivisionType2;
            ViewBag.DivisionType3 = DivisionType3;
            ViewBag.DivisionType4 = DivisionType4;
            ViewBag.DivisionType5 = DivisionType5;
            //
            ViewBag.RemDivisionType1 = Convert.ToDouble(DivisionType1) - Convert.ToDouble(VehicleType1);
            ViewBag.RemDivisionType2 = Convert.ToDouble(DivisionType2) - Convert.ToDouble(VehicleType2);
            ViewBag.RemDivisionType3 = Convert.ToDouble(DivisionType3) - Convert.ToDouble(VehicleType3);
            ViewBag.RemDivisionType4 = Convert.ToDouble(DivisionType4) - Convert.ToDouble(VehicleType4);
            ViewBag.RemDivisionType5 = Convert.ToDouble(DivisionType5) - Convert.ToDouble(VehicleType5);
            //
            ViewBag.Date = Date.ToShortDateString();           
            ViewBag.mainTable = mainTable;
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title");
            return View();
        }

        // POST: Contradictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contradiction contradiction, DateTime Date, int WorkshopId, string[] Description,int?[] VehicleType1, int?[] VehicleType2, int?[] VehicleType3, int?[] VehicleType4, int?[] VehicleType5, int?[] VehicleTypeOther)
        {
            contradiction.CreateDate = DateTime.Now;
            contradiction.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                for(int i=0;i<Description.Count();i++)
                {
                    contradiction.Date = Date;
                    contradiction.WorkshopId = WorkshopId;
                    contradiction.Description = Description[i];
                    if (!string.IsNullOrEmpty(VehicleType1[i].ToString()))
                        contradiction.VehicleType1 = VehicleType1[i];
                    else
                        contradiction.VehicleType1 = 0;

                    if (!string.IsNullOrEmpty(VehicleType2[i].ToString()))
                        contradiction.VehicleType2 = VehicleType2[i];
                    else
                        contradiction.VehicleType2 = 0;

                    if (!string.IsNullOrEmpty(VehicleType3[i].ToString()))
                        contradiction.VehicleType3 = VehicleType3[i];
                    else
                        contradiction.VehicleType3 = 0;

                    if (!string.IsNullOrEmpty(VehicleType4[i].ToString()))
                        contradiction.VehicleType4 = VehicleType4[i];
                    else
                        contradiction.VehicleType4 = 0;

                    if (!string.IsNullOrEmpty(VehicleType5[i].ToString()))
                        contradiction.VehicleType5 = VehicleType5[i];
                    else
                        contradiction.VehicleType5 = 0;

                    if (!string.IsNullOrEmpty(VehicleTypeOther[i].ToString()))
                        contradiction.VehicleTypeOther = VehicleTypeOther[i];
                    else
                        contradiction.VehicleTypeOther = 0;


                    db.tbl_Contradictions.Add(contradiction);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradiction.WorkshopId);
            return View(contradiction);
        }

        // GET: Contradictions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contradiction contradiction = db.tbl_Contradictions.Find(id);
            if (contradiction == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradiction.WorkshopId);
            return View(contradiction);
        }

        // POST: Contradictions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WorkshopId,Description,VehicleType1,VehicleType2,VehicleType3,VehicleType4,VehicleType5,Date")] Contradiction contradiction)
        {
            contradiction.CreateDate = DateTime.Now;
            contradiction.Creator = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(contradiction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkshopId = new SelectList(db.tbl_Workshops, "ID", "Title", contradiction.WorkshopId);
            return View(contradiction);
        }

        // GET: Contradictions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contradiction contradiction = db.tbl_Contradictions.Find(id);
            if (contradiction == null)
            {
                return HttpNotFound();
            }
            return View(contradiction);
        }

        // POST: Contradictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contradiction contradiction = db.tbl_Contradictions.Find(id);
            db.tbl_Contradictions.Remove(contradiction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        public ActionResult Contradictions(int?[] WorkshopID, DateTime Date, bool? Post)
        {
            var maxDate = db.tbl_Contradictions.ToList();
            if (Post != true)
                if(maxDate.Count()>0)
                {
                    Date = maxDate.Max(c => c.Date).Date;
                    ViewBag.DateTime = Date.ToShortTimeString();
                }                    

            List<Workshop> tableOuts = new List<Workshop>();
            var workshops = db.tbl_Contradictions.Include(c=>c.Workshops).GroupBy(c => c.WorkshopId).ToList();
            string permission = "";
            foreach (var item in workshops)
            {
                tableOuts.Add(new Workshop
                {
                    ID = Convert.ToInt32(item.Key),
                    Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.Key)).Title
                });

            };
            ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");

            if (WorkshopID == null || string.IsNullOrEmpty(WorkshopID[0].ToString()))
            {
                foreach (var item in workshops)
                {
                    permission += item.Key + ",";

                    //tableOuts.Add(new Workshop
                    //{
                    //    ID =Convert.ToInt32(item.Key),
                    //    Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.Key)).Title
                    //});

                };

                //ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title");

            }
            else
            {
                foreach (var item in WorkshopID)
                {
                    permission += item + ",";
                    //tableOuts.Add(new Workshop
                    //{
                    //    ID = Convert.ToInt32(item),
                    //    Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item)).Title
                    //    //ID = Convert.ToInt32(item),
                    //    //Title = Helper.Helpers.GetWorkshops(Convert.ToInt32(item)).Title
                    //});
                };
                //ViewBag.WorkshopID = new SelectList(tableOuts, "ID", "Title", WorkshopID[0]);

            }
            
            if (string.IsNullOrEmpty(permission))
                permission = "1,";
            //برای بخش جدول مغایرت عملکرد
            string Description = "";
            string VehicleType1 = "0";
            string VehicleType2 = "0";
            string VehicleType3 = "0";
            string VehicleType4 = "0";
            string VehicleType5 = "0";
            string VehicleTypeOther = "0";
            string Sum = "0";
            //برای بخش جمع جدول مغایرت عملکرد
            string DiffType1 = "0";
            string DiffType2 = "0";
            string DiffType3 = "0";
            string DiffType4 = "0";
            string DiffType5 = "0";
            string DiffTypeOther = "0";
            string DiffSum = "0";
            string Color1 = "text-success";
            string Color2 = "text-success";
            string Color3 = "text-success";
            string Color4 = "text-success";
            string Color5 = "text-success";
            string Color6 = "text-success";
            string Color7 = "text-success";
            //
            List<tableOuts> mainTable = new List<tableOuts>();
            List<tableDiffOuts> diffTable = new List<tableDiffOuts>();            
            List<ChartData> chart = new List<ChartData>();
            //
            SqlDataReader reader;
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("[dbo].[sp_Contradictions]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workshops", SqlDbType.VarChar).Value = permission.TrimEnd(',');
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Description = reader["Description"].ToString();
                    VehicleType1 = reader["VehicleType1"].ToString();
                    VehicleType2 = reader["VehicleType2"].ToString();
                    VehicleType3 = reader["VehicleType3"].ToString();
                    VehicleType4 = reader["VehicleType4"].ToString();
                    VehicleType5 = reader["VehicleType5"].ToString();
                    VehicleType5 = reader["VehicleType5"].ToString();
                    VehicleTypeOther = reader["VehicleTypeOther"].ToString();
                    Sum = reader["Sum"].ToString();
                    
                    //
                    mainTable.Add(new tableOuts
                    {
                       Description=Description,
                       VehicleType1=VehicleType1,
                       VehicleType2=VehicleType2,
                       VehicleType3=VehicleType3,
                       VehicleType4=VehicleType4,
                       VehicleType5 = VehicleType5,
                       VehicleTypeOther= VehicleTypeOther,
                       Sum =Sum
                    });
                }
                reader.NextResult();
                while (reader.Read())
                {
                    Description = reader["Description"].ToString();                   
                    Sum = reader["Sum"].ToString();
                    if (string.IsNullOrWhiteSpace(Sum))
                        Sum = "0";
                    //
                    //mainTable.Add(new tableOuts
                    //{
                    //    Description = Description,                        
                    //    Sum = Sum
                    //});
                    //double[] Value = new double[] { YearPlan, CurrYearPlan, Statement, FinancialStatement };
                    //string[] Cat = new string[] { "برنامه سال", "برنامه تا کنون", "عملکرد برنامه ریزی", "عملکرد مالی" };
                    //for (int i = 0; i < 4; i++)
                    //{
                        ChartData data = new ChartData();
                        data.Value1 =Convert.ToDouble(Sum);
                        data.Category = Description;
                        chart.Add(data);
                    //}
                }
                reader.NextResult();
                while (reader.Read())
                {
                    DiffType1 = reader["DiffType1"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffType1) < 0)
                        {
                            DiffType1 = (-Convert.ToDouble(DiffType1)).ToString();
                            Color1 = "text-danger";
                        }
                    }
                                       
                    DiffType2 = reader["DiffType2"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffType2) < 0)
                        {
                            DiffType2 = (-Convert.ToDouble(DiffType2)).ToString();
                            Color2 = "text-danger";
                        }
                    } 
                    
                    DiffType3 = reader["DiffType3"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffType3) < 0)
                        {
                            DiffType3 = (-Convert.ToDouble(DiffType3)).ToString();
                            Color3 = "text-danger";
                        }
                    }    
                    
                    DiffType4 = reader["DiffType4"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffType4) < 0)
                        {
                            DiffType4 = (-Convert.ToDouble(DiffType4)).ToString();
                            Color4 = "text-danger";
                        }
                    }   
                    
                    DiffType5 = reader["DiffType5"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffType5) < 0)
                        {
                            DiffType5 = (-Convert.ToDouble(DiffType5)).ToString();
                            Color5 = "text-danger";
                        }
                    }      
                    
                    DiffTypeOther = reader["DiffTypeOther"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffTypeOther) < 0)
                        {
                            DiffTypeOther = (-Convert.ToDouble(DiffTypeOther)).ToString();
                            Color7 = "text-danger";
                        }
                    }  
                    
                    DiffSum = reader["DiffSum"].ToString();
                    if (!string.IsNullOrEmpty(DiffType1))
                    {
                        if (Convert.ToDouble(DiffSum) < 0)
                        {
                            DiffSum = (-Convert.ToDouble(DiffSum)).ToString();
                            Color6 = "text-danger";
                        }
                    }                    

                    //
                    diffTable.Add(new tableDiffOuts
                    {
                        DiffSum=DiffSum,
                        DiffType1=DiffType1,
                        DiffType2=DiffType2,
                        DiffType3=DiffType3,
                        DiffType4=DiffType4,
                        DiffType5=DiffType5,
                        Color1=Color1,
                        Color2=Color2,
                        Color3=Color3,
                        Color4=Color4,
                        Color5=Color5,
                        Color6=Color6,
                        Color7=Color7
                    });
                }
               
                conn.Close();
            }

            ViewBag.Date = Date.ToShortDateString();
            ViewBag.mainTable = mainTable;
            ViewBag.diffTable = diffTable;
            //

            return View(chart);
        }
        //
        public class tableOuts
        {
            public string Description { get; set; }
            public string Sum { get; set; }
            public string VehicleType1 { get; set; }
            public string VehicleType2 { get; set; }
            public string VehicleType3 { get; set; }
            public string VehicleType4 { get; set; }
            public string VehicleType5 { get; set; }
            public string VehicleTypeOther { get; set; }
        }
        public class tableDiffOuts
        {
            public string DiffSum { get; set; }
            public string DiffType1 { get; set; }
            public string DiffType2 { get; set; }
            public string DiffType3 { get; set; }
            public string DiffType4 { get; set; }
            public string DiffType5 { get; set; }
            public string DiffTypeOther { get; set; }
            public string Color1 { get; set; }
            public string Color2 { get; set; }
            public string Color3 { get; set; }
            public string Color4 { get; set; }
            public string Color5 { get; set; }
            public string Color6 { get; set; }
            public string Color7 { get; set; }
        }
        //
        public class existData
        {
            public string Sum { get; set; }
            public string VehicleType1 { get; set; }
            public string VehicleType2 { get; set; }
            public string VehicleType3 { get; set; }
            public string VehicleType4 { get; set; }
            public string VehicleType5 { get; set; }
            public string VehicleTypeOther { get; set; }
        }
        //
        public class divisionPlan
        {
            public string Sum { get; set; }
            public string DivisionType1 { get; set; }
            public string DivisionType2 { get; set; }
            public string DivisionType3 { get; set; }
            public string DivisionType4 { get; set; }
            public string DivisionType5 { get; set; }
            public string DivisionTypeOther { get; set; }
        }
        //
       
        public ActionResult IRNGVData()
        {
            var dataValue = db.tbl_IRNGV.Take(1000);
            return View(dataValue.ToList());
        }
        //
        [HttpPost]
        public JsonResult ImportIRNGVData(HttpPostedFileBase FileUpload)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            string query = "TRUNCATE TABLE tbl_IRNGV";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            List<string> data = new List<string>();

            #region"اطلاعات سامانه اتحادیه"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود و بروزرسانی موجودی انبار از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [excel$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "excel";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var List = from a in excelFile.Worksheet<IRNGV>(sheetName) select a;
                    int dataCount = 0;
                    foreach (var a in List)
                    {
                        try
                        {
                            dataCount += 1;
                            IRNGV values = new IRNGV();
                            values.Acceptance = a.Acceptance;
                            values.Plate = a.Plate;
                            values.ChassisNumber = a.ChassisNumber;
                            values.City = a.City;
                            values.Column1 = a.Column1;
                            values.Constractor = a.Constractor;
                            values.ConstructionYear = a.ConstructionYear;
                            values.CreateDate = DateTime.Now;
                            values.Creator = User.Identity.Name;
                            values.Cylinder = a.Cylinder;
                            values.DateofCertification = a.DateofCertification;
                            values.EngineNumber = a.EngineNumber;
                            values.InspectionCertificateNumber = a.InspectionCertificateNumber;
                            values.InspectionCompany = a.InspectionCompany;
                            values.InspectorName = a.InspectorName;
                            values.Insurance = a.Insurance;
                            values.InsuranceNumber = a.InsuranceNumber;
                            values.NationalCode = a.NationalCode;
                            values.Plate = a.Plate;
                            values.Regulator = a.Regulator;
                            values.State = a.State;
                            values.Valve = a.Valve;
                            values.VehicleType = a.VehicleType;
                            values.Workshop = a.Workshop;
                            values.WorkshopID = a.WorkshopID;
                            values.FillingValve = a.FillingValve;
                            values.CutoffValve = a.CutoffValve;

                            db.tbl_IRNGV.Add(values);
                            db.SaveChanges();
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    //if ((System.IO.File.Exists(pathToExcelFile)))
                    //{
                    //    System.IO.File.Delete(pathToExcelFile);
                    //}
                    string Message = "تعداد " + " " + dataCount + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                    ViewBag.Message = Message;
                    RedirectToAction("UploadExcel");
                    return Json(Message, JsonRequestBehavior.AllowGet);
                    //return View();
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        public ActionResult GCRData()
        {
            var dataValue = db.tbl_GCR;
            return View(dataValue.Take(1000).ToList());
        }
        //
        [HttpPost]
        public JsonResult ImportGCRData(HttpPostedFileBase FileUpload)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            string query = "TRUNCATE TABLE tbl_GCR";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            List<string> data = new List<string>();

            #region"اطلاعات سامانه پخش"
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/UploadedFiles/ImportFiles/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathToExcelFile + ";Extended Properties=Excel 12.0;");
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    //#region"ورود و بروزرسانی موجودی انبار از طریق فایل اکسل- شیت 1"
                    var adapter = new OleDbDataAdapter("SELECT * FROM [excel$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "excel";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var List = from a in excelFile.Worksheet<GCR>(sheetName) select a;
                    int dataCount = 0;
                    foreach (var a in List)
                    {
                        try
                        {
                            dataCount += 1;
                            GCR values = new GCR();
                            values.IssueTracking = a.IssueTracking;
                            values.DateofTurn = a.DateofTurn;
                            values.Status = a.Status;
                            values.OwnerFullName = a.OwnerFullName;
                            values.NationalCode = a.NationalCode;
                            values.OwnerMobile = a.OwnerMobile;
                            values.Plate = a.Plate;
                            values.NajiPlate = a.NajiPlate;
                            values.TypeofUsed = a.TypeofUsed;
                            values.VehicleType = a.VehicleType;
                            values.Model = a.Model;
                            values.Contractor = a.Contractor;
                            values.Workshop = a.Workshop;
                            values.State = a.State;
                            values.City = a.City;
                            values.WorkshopAddress = a.WorkshopAddress;
                            values.WorkshopPhone = a.WorkshopPhone;
                            values.ConversionDate = a.ConversionDate;
                            values.ConversionID = a.ConversionID;
                            values.CylinderBulk = a.CylinderBulk;
                            values.CylinderSerial = a.CylinderSerial;
                            values.ValveSerial = a.ValveSerial;
                            values.RegulatorSerial = a.RegulatorSerial;
                            values.ConversionCertificateNumber = a.ConversionCertificateNumber;
                            values.HealthCertificateID = a.HealthCertificateID;
                            values.EngineNumber = a.EngineNumber;
                            values.panNumber = a.panNumber;
                            values.ChassisNumber = a.ChassisNumber;
                            values.PersonalPassenger = a.PersonalPassenger;
                            values.CreateDate = DateTime.Now;
                            values.Creator = User.Identity.Name;
                            values.ConstructionYear = a.ConstructionYear;

                            db.tbl_GCR.Add(values);
                            db.SaveChanges();
                        }
                        //
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    //if ((System.IO.File.Exists(pathToExcelFile)))
                    //{
                    //    System.IO.File.Delete(pathToExcelFile);
                    //}
                    string Message = "تعداد " + " " + dataCount + " ردیف با موفقیت در بانک اطلاعاتی ذخیره شد ";
                    ViewBag.Message = Message;
                    RedirectToAction("UploadExcel");
                    return Json(Message, JsonRequestBehavior.AllowGet);
                    //return View();
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    //data.Add("<li>Only Excel file format is allowed</li>");  
                    data.Add("<li>تنها مجاز به بارگذاری اطلاعات مطابق قالب استاندارد می باشید</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>لطفا فایل اکسل مورد نظر را انتخاب کنید</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }
        //
        public ActionResult GCRlist()
        {
            return View();
        }
        //

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
