using cngfapco.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cngfapco.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private ContextDB db = new ContextDB();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        // GET: Search in fapco,irngv and ... vehicle registered diff data
        public ActionResult SearchInDiffInfo()
        {
            ViewBag.existItem = false;
            ViewBag.Table1 = "vw_VehicleRegistrations";
            ViewBag.Table2 = "vw_IRNGV";
            ViewBag.Table3 = "";
            ViewBag.Condition = "<>";

            return View();
        }

        [HttpPost]
        public ActionResult SearchInDiffInfo(string[] Table,string Table1, string Table2, string condition, string column)
        {            
            for(int i=0; i < Table.Count(); i++)
            {
                if(i==0)
                    Table1 = Table[i];
                else
                    Table2 = Table[i];
            }
            //
            string tbl1 = Table1.Replace("vw_", "");
            string tbl2 = Table2.Replace("vw_", "");
            float count = 0;
            ViewBag.existItem = false;
            //
            ViewBag.existItem = false;
            ViewBag.Table1 = Table1;
            ViewBag.Table2 = Table2;
            ViewBag.Condition = condition;
            ViewBag.Column = column;
            ViewBag.tbl1 = tbl1;
            ViewBag.tbl2 = tbl2;
            //
            string tbl1_NationalCode = "";
            string tbl1_VehicleType = "";
            string tbl1_ChassisNumber = "";
            string tbl1_Workshops = "";
            string tbl1_EngineNumber = "";
            string tbl1_ConstructionYear = "";
            string tbl1_HealthCertificate = "";
            string tbl1_Plate = "";

            string tbl2_NationalCode = "";
            string tbl2_VehicleType = "";
            string tbl2_ChassisNumber = "";
            string tbl2_Workshops = "";
            string tbl2_EngineNumber = "";
            string tbl2_ConstructionYear = "";
            string tbl2_HealthCertificate = "";
            string tbl2_Plate = "";
            //
            List<VehicleRegistrations> vehicleregistration = new List<VehicleRegistrations>();
            //

            if (!string.IsNullOrEmpty(condition))
            {
                SqlDataReader reader;
                var connStr = ConfigurationManager.ConnectionStrings["ContextDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("[dbo].[sp_SearchInDiffInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@condition", SqlDbType.VarChar).Value = condition;
                    cmd.Parameters.Add("@column", SqlDbType.VarChar).Value = column;
                    cmd.Parameters.Add("@Table1", SqlDbType.VarChar).Value = Table1;
                    cmd.Parameters.Add("@Table2", SqlDbType.VarChar).Value = Table2;

                    conn.Open();
                    reader = cmd.ExecuteReader();                   
                    while (reader.Read())
                    {
                        //ID= reader["ID"].ToString();
                        tbl1_Workshops = reader[tbl1 + "_Workshop"].ToString();
                        tbl2_Workshops = reader[tbl2 + "_Workshop"].ToString();
                        tbl2_NationalCode = reader[tbl2+ "_NationalCode"].ToString();
                        tbl1_NationalCode = reader[tbl1 + "_NationalCode"].ToString();
                        tbl1_VehicleType = reader[tbl1 + "_VehicleType"].ToString();
                        tbl2_VehicleType = reader[tbl2 + "_VehicleType"].ToString();
                        tbl1_ChassisNumber = reader[tbl1 + "_ChassisNumber"].ToString();
                        tbl2_ChassisNumber = reader[tbl2 + "_ChassisNumber"].ToString();
                        tbl1_EngineNumber = reader[tbl1 + "_EngineNumber"].ToString();
                        tbl2_EngineNumber = reader[tbl2 + "_EngineNumber"].ToString();

                        tbl1_ConstructionYear = reader[tbl1 + "_ConstructionYear"].ToString();
                        tbl2_ConstructionYear = reader[tbl2 + "_ConstructionYear"].ToString();
                        tbl1_HealthCertificate = reader[tbl1 + "_HealthCertificate"].ToString();
                        tbl2_HealthCertificate = reader[tbl2 + "_HealthCertificate"].ToString();
                        tbl1_Plate = reader[tbl1 + "_Plate2"].ToString();
                        tbl2_Plate = reader[tbl2 + "_Plate2"].ToString();
                        count += 1;

                        vehicleregistration.Add(new VehicleRegistrations
                        {
                            //ID=ID,
                            tbl1_ChassisNumber=tbl1_ChassisNumber,
                            tbl1_ConstructionYear=tbl1_ConstructionYear,
                            tbl1_EngineNumber=tbl1_EngineNumber,
                            tbl1_HealthCertificate=tbl1_HealthCertificate,
                            tbl1_NationalCode=tbl1_NationalCode,
                            tbl1_Plate=tbl1_Plate,
                            tbl1_VehicleType=tbl1_VehicleType,
                            tbl1_Workshops=tbl1_Workshops,
                            tbl2_ChassisNumber=tbl2_ChassisNumber,
                            tbl2_ConstructionYear=tbl2_ConstructionYear,
                            tbl2_EngineNumber=tbl2_EngineNumber,
                            tbl2_HealthCertificate=tbl2_HealthCertificate,
                            tbl2_NationalCode=tbl2_NationalCode,
                            tbl2_Plate=tbl2_Plate,
                            tbl2_VehicleType=tbl2_VehicleType,
                            tbl2_Workshops=tbl2_Workshops
                        });
                    }

                    conn.Close();
                }
                //
                ViewBag.count = count.ToString("#,##") + " " + " ردیف ";
                ViewBag.existItem = true;
                ViewBag.vehicleregistration = vehicleregistration;
            }
            //
            return View();
        }
        //
        public class VehicleRegistrations
        {
            public string ID { get; set; }
            public string tbl1_Workshops { get; set; }
            public string tbl2_Workshops { get; set; }
            public string tbl2_NationalCode { get; set; }
            public string tbl1_NationalCode { get; set; }
            public string tbl1_VehicleType { get; set; }
            public string tbl2_VehicleType { get; set; }
            public string tbl2_ChassisNumber { get; set; }
            public string tbl1_ChassisNumber { get; set; }
            public string tbl1_EngineNumber { get; set; }
            public string tbl2_EngineNumber { get; set; }
            public string tbl1_ConstructionYear { get; set; }
            public string tbl2_ConstructionYear { get; set; }
            public string tbl1_HealthCertificate { get; set; }
            public string tbl2_HealthCertificate { get; set; }
            public string tbl1_Plate { get; set; }
            public string tbl2_Plate { get; set; }
        }
        // GET: Search in Cylinder,Valve,Kit, ... banks
        public ActionResult SearchInBank()
        {
            ViewBag.existItem = false;
            ViewBag.Type = "Cylinder";
            ViewBag.Location = "Bank";

            return View();
        }

        [HttpPost]
        public ActionResult SearchInBank(string type, string serialNumber, string location)
        {
            var cylinderConstractor = db.tbl_TankConstractors.ToList();
            ViewBag.serialNumber = serialNumber;
            ViewBag.existItem = false;
            ViewBag.Type = type;
            ViewBag.Location = location;

            List<BankTank> tableOuts = new List<BankTank>();
            List<BankTankValve> valvetableOuts = new List<BankTankValve>();
            List<BankTankValve> kittableOuts = new List<BankTankValve>();
            List<BankTankValve> cutoffvalvetableOuts = new List<BankTankValve>();
            List<BankTankValve> fillingvalvetableOuts = new List<BankTankValve>();

            serialNumber = serialNumber.Replace(" ", "");

            if (!string.IsNullOrEmpty(type))
            {
                //-------------------برای قسمت مخزن ها --------------------------------------
                if (type.Equals("Cylinder"))
                {
                    if (location.Contains("Bank"))
                    {
                        var listofTanks = db.tbl_BankTanks.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofTanks.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofTanks)
                        {                            
                            tableOuts.Add(new BankTank
                            {
                                bulk = item.bulk,
                                constractor = item.constractor,
                                CreateDate = item.CreateDate,
                                Creator = item.Creator,
                                diameter = item.diameter,
                                expireDate = item.expireDate,
                                gregorianEMonth = item.gregorianEMonth,
                                gregorianEYear = item.gregorianEYear,
                                gregorianPMonth = item.gregorianPMonth,
                                gregorianPYear = item.gregorianPYear,
                                ID = item.ID,
                                length = item.length,
                                model = item.model,
                                pressure = item.pressure,
                                productDate = item.productDate,
                                rezve = item.rezve,
                                serialNumber = item.serialNumber,
                                status = item.status,
                                type = item.type,
                                workshop = item.workshop
                            });
                        }
                        //
                        ViewBag.tableOuts = tableOuts;
                        return View();
                    }
                    else
                    {
                        var listofTanks = db.tbl_Tanks.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofTanks.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofTanks)
                        {                            
                            if (!string.IsNullOrEmpty(item.workshop))
                            {
                                tableOuts.Add(new BankTank
                                {
                                    bulk = item.bulk,
                                    constractor = item.constractor,//Helper.Helpers.GetTankConstractor(Convert.ToInt32(item.constractor)).Constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    diameter = item.diameter,
                                    expireDate = item.expireDate,
                                    gregorianEMonth = item.gregorianEMonth,
                                    gregorianEYear = item.gregorianEYear,
                                    gregorianPMonth = item.gregorianPMonth,
                                    gregorianPYear = item.gregorianPYear,
                                    ID = item.ID,
                                    length = item.length,
                                    model = item.model,
                                    pressure = item.pressure,
                                    productDate = item.productDate,
                                    rezve = item.rezve,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title

                                });
                            }
                            else
                            {
                                tableOuts.Add(new BankTank
                                {
                                    bulk = item.bulk,
                                    constractor = item.constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    diameter = item.diameter,
                                    expireDate = item.expireDate,
                                    gregorianEMonth = item.gregorianEMonth,
                                    gregorianEYear = item.gregorianEYear,
                                    gregorianPMonth = item.gregorianPMonth,
                                    gregorianPYear = item.gregorianPYear,
                                    ID = item.ID,
                                    length = item.length,
                                    model = item.model,
                                    pressure = item.pressure,
                                    productDate = item.productDate,
                                    rezve = item.rezve,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = ""

                                });
                            }
                        }
                        //
                        ViewBag.tableOuts = tableOuts;
                        return View();
                    }
                }

                //-------------------برای قسمت شیر مخزن ها --------------------------------------
                if (type.Equals("Valve"))
                {
                    if (location.Contains("Bank"))
                    {
                        var listofValves = db.tbl_BankTankValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical == true)
                                {
                                    constractor = db.tbl_ValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().Valve;
                                    constractorID = db.tbl_ValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("البرز"))
                                        constractor = "البرز یدک";
                                    try
                                    {
                                        constractorID = db.tbl_ValveConstractors.Where(c => c.Valve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch {}
                                }
                            }

                            valvetableOuts.Add(new BankTankValve
                            {
                                constractor = constractor,
                                CreateDate = item.CreateDate,
                                Creator = item.Creator,                              
                                ID = item.ID,
                                model = item.model,
                                productDate = item.productDate,
                                rezve = item.rezve,
                                serialNumber = item.serialNumber,
                                status = item.status,
                                type = item.type,
                                workshop = item.workshop
                            });
                        }
                        //
                        ViewBag.tableOuts = valvetableOuts;
                        return View();
                    }
                    else
                    {
                        var listofValves = db.tbl_TankValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical == true)
                                {
                                    constractor = db.tbl_ValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().Valve;
                                    constractorID = db.tbl_ValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("البرز"))
                                        constractor = "البرز یدک";
                                    constractorID = db.tbl_ValveConstractors.Where(c => c.Valve.Equals(constractor)).SingleOrDefault().ID;
                                }
                            }

                            if (!string.IsNullOrEmpty(item.workshop))
                            {
                                valvetableOuts.Add(new BankTankValve
                                {
                                    constractor =constractor, //Helper.Helpers.GetValveConstractor(constractor).Valve,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = item.model,
                                    productDate = item.productDate,
                                    rezve = item.rezve,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                                });
                            }
                            else
                            {
                                valvetableOuts.Add(new BankTankValve
                                {
                                    constractor = constractor,//Helper.Helpers.GetValveConstractor(constractor).Valve,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = item.model,
                                    productDate = item.productDate,
                                    rezve = item.rezve,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = ""
                                });
                            }

                        }
                        //
                        ViewBag.tableOuts = valvetableOuts;
                        return View();
                    }
                }

                //-------------------برای قسمت کیت ها --------------------------------------
                if (type.Equals("Kit"))
                {
                    if (location.Contains("Bank"))
                    {
                        var listofKits = db.tbl_BankKits.Where(k => k.serialNumber.ToString().Replace(" ","").Contains(serialNumber)).ToList();
                        if (listofKits.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofKits)
                        {
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical == true)
                                {
                                    constractor = db.tbl_RegulatorConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().Regulator;
                                    constractorID = db.tbl_RegulatorConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن (EF)";
                                    else if (item.constractor.Contains("شهاب"))
                                        constractor = "شهاب گاز سوز (SG)";
                                    else if (item.constractor.Contains("هوماک"))
                                        constractor = "فن آوران - هوماک";
                                    try
                                    {
                                        constractorID = db.tbl_ValveConstractors.Where(c => c.Valve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }
                                                     
                            if (item.workshop == null)
                            {
                                kittableOuts.Add(new BankTankValve
                                {
                                    //constractor = Helper.Helpers.GetKitConstractor(constractor).Regulator,
                                    constractor=constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = item.model,
                                    productDate = item.productDate,
                                    rezve = item.generation,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = ""
                                });
                            }
                            else
                            {
                                kittableOuts.Add(new BankTankValve
                                {
                                   // constractor = Helper.Helpers.GetKitConstractor(constractor).Regulator,
                                   constractor=constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = item.model,
                                    productDate = item.productDate,
                                    rezve = item.generation,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = item.workshop
                                });
                            }
                           
                        }
                        //
                        ViewBag.tableOuts = kittableOuts;
                        return View();
                    }
                    else
                    {
                        var listofKits = db.tbl_Kits.Where(k => k.serialNumber.ToString().Replace(" ", "").Contains(serialNumber)).ToList();
                        if (listofKits.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofKits)
                        {
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical == true)
                                {
                                    constractor = db.tbl_RegulatorConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().Regulator;
                                    constractorID = db.tbl_RegulatorConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن (EF)";
                                    else if (item.constractor.Contains("شهاب"))
                                        constractor = "شهاب گاز سوز (SG)";
                                    else if (item.constractor.Contains("هوماک"))
                                        constractor = "فن آوران - هوماک";
                                    try
                                    {
                                        constractorID = db.tbl_ValveConstractors.Where(c => c.Valve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }
                            if (item.workshop == null)
                            {
                                kittableOuts.Add(new BankTankValve
                                {
                                    //constractor = Helper.Helpers.GetKitConstractor(constractor).Regulator,
                                    constractor=constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = item.model,
                                    productDate = item.productDate,
                                    rezve = item.generation,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    type = item.type,
                                    workshop = ""
                                });
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(item.workshop))
                                {
                                    kittableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetKitConstractor(constractor).Regulator,
                                        constractor=constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = item.model,
                                        productDate = item.productDate,
                                        rezve = item.generation,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        type = item.type,
                                        workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                                    });
                                }
                                else
                                {
                                    kittableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetKitConstractor(constractor).Regulator,
                                        constractor=constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = item.model,
                                        productDate = item.productDate,
                                        rezve = item.generation,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        type = item.type,
                                        workshop = ""
                                    });
                                }
                            }

                        }
                        //
                        ViewBag.tableOuts = kittableOuts;
                        return View();
                    }
                }

                //-------------------برای قسمت شیر قطع کن ها --------------------------------------
                if (type.Equals("CutoffValve"))
                {
                    if (location.Contains("Bank"))
                    {
                        var listofValves = db.tbl_BankCutofValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            string constractor = "";
                            int? constractorID = null;

                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                try
                                {
                                    if (Convert.ToInt32(item.constractor) > 0)
                                    {
                                        constractor = db.tbl_CutofValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().CutofValve;
                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                    }
                                    else
                                    {
                                        if (item.constractor.Contains("الکتروفن"))
                                            constractor = "الکتروفن";
                                        else if (item.constractor.Contains("البرز"))
                                            constractor = "البرز یدک";

                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.CutofValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                }
                                catch
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن";
                                    else if (item.constractor.Contains("البرز"))
                                        constractor = "البرز یدک";
                                    try
                                    {
                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.CutofValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }
                            try
                            {
                                cutoffvalvetableOuts.Add(new BankTankValve
                                {
                                    //constractor = Helper.Helpers.GetCutoffValveConstractor(constractor).CutofValve,
                                    constractor = constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = Helper.Helpers.GetCutoffValveConstractor(constractorID).Code,
                                    productDate = item.productDate,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    workshop = item.workshop
                                });

                            }
                            catch
                            {
                                cutoffvalvetableOuts.Add(new BankTankValve
                                {
                                    constractor = "یافت نشد",
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = "یافت نشد",
                                    productDate = item.productDate,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    workshop = "یافت تشد"
                                });

                            }
                        }
                        //
                        ViewBag.tableOuts = cutoffvalvetableOuts;
                        return View();
                    }
                    else
                    {
                        var listofValves = db.tbl_CutofValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            //int constractor = 1;
                            string constractor = "";
                            int? constractorID = null;

                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                try
                                {
                                    if (Convert.ToInt32(item.constractor) > 0)
                                    {
                                        constractor = db.tbl_CutofValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().CutofValve;
                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                    }
                                    else
                                    {
                                        if (item.constractor.Contains("الکتروفن"))
                                            constractor = "الکتروفن";
                                        else if (item.constractor.Contains("البرز"))
                                            constractor = "البرز یدک";

                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.CutofValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                }
                                catch
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن";
                                    else if (item.constractor.Contains("البرز"))
                                        constractor = "البرز یدک";
                                    try
                                    {
                                        constractorID = db.tbl_CutofValveConstractors.Where(c => c.CutofValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }                            

                            if (!string.IsNullOrEmpty(item.workshop))
                            {
                                try
                                {
                                    cutoffvalvetableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetCutoffValveConstractor(constractor).CutofValve,
                                        constractor = constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = Helper.Helpers.GetCutoffValveConstractor(constractorID).Code,
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                                    });
                                }
                                catch
                                {
                                    cutoffvalvetableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetCutoffValveConstractor(constractor).CutofValve,
                                        constractor = "یافت نشد",
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = "یافت نشد",
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                                    });
                                }
                            }
                            else
                            {
                                try
                                {
                                    cutoffvalvetableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetCutoffValveConstractor(constractor).CutofValve,
                                        constractor = constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = Helper.Helpers.GetCutoffValveConstractor(constractorID).Code,
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = ""
                                    });
                                }
                                catch
                                {
                                    cutoffvalvetableOuts.Add(new BankTankValve
                                    {
                                        constractor = "یافت نشد",
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = "یافت نشد",
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = "یافت نشد"
                                    });
                                }
                            }

                        }
                        //
                        ViewBag.tableOuts = cutoffvalvetableOuts;
                        return View();
                    }
                }

                //-------------------برای قسمت شیر پر کن ها --------------------------------------
                if (type.Equals("FillingValve"))
                {
                    if (location.Contains("Bank"))
                    {
                        var listofValves = db.tbl_BankFillingValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            //int constractor = 1;
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical==true)
                                {
                                    constractor = db.tbl_FillingValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().FillingValve;
                                    constractorID = db.tbl_FillingValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن";
                                    else if (item.constractor.Contains("آسیا محور"))
                                        constractor = "آسیا محور ساز";
                                    else if (item.constractor.Contains("تکاب") || item.constractor.Contains("کالای امید بهسازان"))
                                        constractor = "تامین کالای امید بهسازان (تکاب)";
                                    try
                                    {
                                        constractorID = db.tbl_FillingValveConstractors.Where(c => c.FillingValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }

                            try
                            {
                                fillingvalvetableOuts.Add(new BankTankValve
                                {
                                    //constractor = Helper.Helpers.GetFillingValveConstractor(constractor).FillingValve,
                                    constractor = constractor,
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = Helper.Helpers.GetFillingValveConstractor(constractorID).Code,
                                    productDate = item.productDate,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    workshop = item.workshop
                                });
                            }
                            catch
                            {
                                fillingvalvetableOuts.Add(new BankTankValve
                                {
                                    //constractor = Helper.Helpers.GetFillingValveConstractor(constractor).FillingValve,
                                    constractor = "یافت نشد",
                                    CreateDate = item.CreateDate,
                                    Creator = item.Creator,
                                    ID = item.ID,
                                    model = "یافت نشد",
                                    productDate = item.productDate,
                                    serialNumber = item.serialNumber,
                                    status = item.status,
                                    workshop = item.workshop
                                });
                            }
                            
                        }
                        //
                        ViewBag.tableOuts = fillingvalvetableOuts;
                        return View();
                    }
                    else
                    {
                        var listofValves = db.tbl_FillingValves.Where(b => b.serialNumber.Contains(serialNumber)).ToList();
                        if (listofValves.Count > 0)
                            ViewBag.existItem = true;

                        foreach (var item in listofValves)
                        {
                            //int constractor = 1;
                            string constractor = "";
                            int? constractorID = null;
                            // ...
                            int myInt;
                            bool isNumerical = int.TryParse(item.constractor, out myInt);
                            //
                            if (!string.IsNullOrEmpty(item.constractor))
                            {
                                if (isNumerical==true)
                                {
                                    constractor = db.tbl_FillingValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().FillingValve;
                                    constractorID = db.tbl_FillingValveConstractors.Where(c => c.ID.ToString().Equals(item.constractor)).SingleOrDefault().ID;
                                }
                                else
                                {
                                    if (item.constractor.Contains("الکتروفن"))
                                        constractor = "الکتروفن";
                                    else if (item.constractor.Contains("آسیا محور"))
                                        constractor = "آسیا محور ساز";
                                    else if (item.constractor.Contains("تکاب") || item.constractor.Contains("کالای امید بهسازان"))
                                        constractor = "تامین کالای امید بهسازان (تکاب)";
                                    try
                                    {
                                        constractorID = db.tbl_FillingValveConstractors.Where(c => c.FillingValve.Equals(constractor)).SingleOrDefault().ID;
                                    }
                                    catch { }
                                }
                            }

                            if (!string.IsNullOrEmpty(item.workshop))
                            {
                                try
                                {
                                    fillingvalvetableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetFillingValveConstractor(constractor).FillingValve,
                                        constractor = constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = Helper.Helpers.GetFillingValveConstractor(constractorID).Code,
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                                    });
                                }
                                catch
                                {
                                    fillingvalvetableOuts.Add(new BankTankValve
                                    {
                                        constractor = "یافت نشد",
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = "یافت نشد",
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = "یافت نشد"
                                    });
                                }
                                
                            }
                            else
                            {
                                try
                                {
                                    fillingvalvetableOuts.Add(new BankTankValve
                                    {
                                        //constractor = Helper.Helpers.GetFillingValveConstractor(constractor).FillingValve,
                                        constractor = constractor,
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = Helper.Helpers.GetFillingValveConstractor(constractorID).Code,
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = ""
                                    });
                                }
                                catch
                                {
                                    fillingvalvetableOuts.Add(new BankTankValve
                                    {
                                        constractor = "یافت نشد",
                                        CreateDate = item.CreateDate,
                                        Creator = item.Creator,
                                        ID = item.ID,
                                        model = "یافت نشد",
                                        productDate = item.productDate,
                                        serialNumber = item.serialNumber,
                                        status = item.status,
                                        workshop = "یافت نشد"
                                    });
                                }
                                
                            }

                        }
                        //
                        ViewBag.tableOuts = fillingvalvetableOuts;
                        return View();
                    }
                }
            }
            //
            return View();
        }

        // GET: Search in Registration Vehicle
        public ActionResult SearchInVehicle()
        {
            ViewBag.existItem = false;
            ViewBag.Type = "Cylinder";
            ViewBag.Location = "Registered";

            return View();
        }

        [HttpPost]
        public ActionResult SearchInVehicle(string Condition, string type, string location)
        {
            ViewBag.condition = Condition;
            ViewBag.existItem = false;
            ViewBag.Type = type;
            ViewBag.Location = location;

            List<VehicleRegistration> tableOuts = new List<VehicleRegistration>();
            List<VehicleTank> tanktableOuts = new List<VehicleTank>();

            if (location.Contains("Registered"))
            {
                var vehicleList = db.tbl_VehicleRegistrations
                .Where(v => v.OwnerFamily.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                            v.OwnerName.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                              v.AlphaPlate.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                               v.IranNumberPlate.Contains(Condition) ||
                                v.LeftNumberPlate.Contains(Condition) ||
                                 v.RightNumberPlate.Contains(Condition) ||
                                  v.ChassisNumber.Replace("/", "").Contains(Condition.Replace("/", "")) ||
                                   v.EngineNumber.Replace("/", "").Contains(Condition.Replace("/", "")) ||
                                    v.NationalCode.Replace("-", "").Contains(Condition.Replace("-", "")) ||
                                     v.VehicleCard.Replace("-", "").Contains(Condition.Replace("-", "")) ||
                                      v.VIN.Contains(Condition) ||
                                     v.Workshop.Title.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                                    v.ConstructionYear.Contains(Condition) ||
                                   v.MobileNumber.Contains(Condition.Replace("-", "")) ||
                                  v.FuelCard.Contains(Condition) ||
                                 v.License.Contains(Condition) ||
                                v.RefuelingLable.Contains(Condition) ||
                               v.SerialKey.Contains(Condition) ||
                              v.SerialKit.Contains(Condition) ||
                             v.SerialRefuelingValve.Contains(Condition) ||
                            v.SerialSparkPreview.Contains(Condition) ||
                           v.System.Contains(Condition) ||
                          v.TrackingCode.Contains(Condition) ||
                        v.VehicleType.Type.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی"))
                ).ToList();

                if (!string.IsNullOrEmpty(Condition))
                {
                    foreach (var item in vehicleList)
                    {
                        tableOuts.Add(new VehicleRegistration
                        {
                            Address = item.Address,
                            VehicleType = item.VehicleType,
                            TrackingCode = item.TrackingCode,
                            System = item.System,
                            SerialSparkPreview = item.SerialSparkPreview,
                            AlphaPlate = item.AlphaPlate,
                            ChassisNumber = item.ChassisNumber,
                            Checked = item.Checked,
                            CheckedDate = item.CheckedDate,
                            Checker = item.Checker,
                            ConstructionYear = item.ConstructionYear,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            CreatorIPAddress = item.CreatorIPAddress,
                            Description = item.Description,
                            EditDate = item.EditDate,
                            Editor = item.Editor,
                            EditorIPAddress = item.EditorIPAddress,
                            EngineNumber = item.EngineNumber,
                            FuelCard = item.FuelCard,
                            HealthCertificate = item.HealthCertificate,
                            ID = item.ID,
                            InstallationStatus = item.InstallationStatus,
                            InvoiceCode = item.InvoiceCode,
                            IranNumberPlate = item.IranNumberPlate,
                            LeftNumberPlate = item.LeftNumberPlate,
                            License = item.License,
                            MobileNumber = item.MobileNumber,
                            NationalCode = item.NationalCode,
                            OwnerFamily = item.OwnerFamily + " " + item.OwnerName,
                            RefuelingLable = item.RefuelingLable,
                            RightNumberPlate = item.RightNumberPlate,
                            SerialKey = item.SerialKey,
                            SerialKit = item.SerialKit,
                            SerialRefuelingValve = item.SerialRefuelingValve,
                            Status = item.Status,
                            TypeofUse = item.TypeofUse,
                            TypeofUseID = item.TypeofUseID,
                            VehicleTypeID = item.VehicleTypeID,
                            VIN = item.VIN,
                            Workshop = item.Workshop,
                            WorkshopID = item.WorkshopID,
                            RegisterStatus=item.RegisterStatus
                        });
                    }
                }
            }

            else
            {
                var vehicletankList = db.tbl_VehicleTanks
               .Where(v => v.Serial.Replace("/","").Contains(Condition.Replace("/", "")) ||
                            v.SerialTankValve.Replace("/", "").Contains(Condition.Replace("/", "")) ||
                             v.TankConstractor.Constractor.Contains(Condition) ||
                              v.TypeTankValve.Contains(Condition) ||
                               //v.Genaration.Contains(Condition) ||
                                v.ValveConstractor.Valve.Contains(Condition) 
                      )
               .ToList();
                if (!string.IsNullOrEmpty(Condition))
                {
                    foreach (var item in vehicletankList)
                    {
                        tanktableOuts.Add(new VehicleTank
                        {
                           CreateDate=item.CreateDate,
                           VehicleRegistrationID=item.VehicleRegistrationID,
                           Volume=Convert.ToDouble(Helper.Helpers.GetBulk(Convert.ToInt32(item.Volume)).Type),
                           VehicleRegistration=item.VehicleRegistration,
                           Creator=item.Creator,
                           ExpirationDate=item.ExpirationDate,
                           ID=item.ID,
                           ProductDate=item.ProductDate,
                           Serial=item.Serial,
                           SerialTankValve=item.SerialTankValve,
                           TankConstractor=item.TankConstractor,
                           TankConstractorID=item.TankConstractorID,
                           TypeTankValve=item.TypeTankValve,
                           ValveConstractor=item.ValveConstractor,
                           ValveConstractorID=item.ValveConstractorID
                           //Genaration=item.Genaration
                        });
                    }
                }
            }
            //
            if (tableOuts.Count > 0)
                ViewBag.existItem = true;
            ViewBag.tableOuts = tableOuts;            
            //               
            if (tanktableOuts.Count > 0)
            {
                ViewBag.existItem = true;
                ViewBag.tanktableOuts = tanktableOuts;
                ViewBag.cylinder = tanktableOuts.FirstOrDefault().Serial;
                ViewBag.valve = tanktableOuts.FirstOrDefault().SerialTankValve;
            }

            //
            return View();
        }
        //
        public ActionResult RelatedSearchInBank(string type, string serialCylinder, string serialValve, string location)
        {
            ViewBag.existItem = false;
            ViewBag.Type = type;
            ViewBag.Location = location;

            List<BankTank> tableOuts = new List<BankTank>();
            List<BankTankValve> valvetableOuts = new List<BankTankValve>();

            //-------------------برای قسمت مخزن ها --------------------------------------

            if (!string.IsNullOrEmpty(serialCylinder))
            {
                var listofTanks = db.tbl_Tanks.Where(b => b.serialNumber.Contains(serialCylinder)).ToList();
                if (listofTanks.Count > 0)
                    ViewBag.existItem = true;

                foreach (var item in listofTanks)
                {
                    int constractor = 2;
                    if (item.constractor.Contains("آسیا"))
                        constractor = 2;
                    else
                        constractor = 1;

                    if (!string.IsNullOrEmpty(item.workshop))
                    {
                        tableOuts.Add(new BankTank
                        {
                            bulk = item.bulk,
                            constractor = Helper.Helpers.GetTankConstractor(constractor).Constractor,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            diameter = item.diameter,
                            expireDate = item.expireDate,
                            gregorianEMonth = item.gregorianEMonth,
                            gregorianEYear = item.gregorianEYear,
                            gregorianPMonth = item.gregorianPMonth,
                            gregorianPYear = item.gregorianPYear,
                            ID = item.ID,
                            length = item.length,
                            model = item.model,
                            pressure = item.pressure,
                            productDate = item.productDate,
                            rezve = item.rezve,
                            serialNumber = item.serialNumber,
                            status = item.status,
                            type = item.type,
                            workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title

                        });
                    }
                    else
                    {
                        tableOuts.Add(new BankTank
                        {
                            bulk = item.bulk,
                            constractor = Helper.Helpers.GetTankConstractor(constractor).Constractor,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            diameter = item.diameter,
                            expireDate = item.expireDate,
                            gregorianEMonth = item.gregorianEMonth,
                            gregorianEYear = item.gregorianEYear,
                            gregorianPMonth = item.gregorianPMonth,
                            gregorianPYear = item.gregorianPYear,
                            ID = item.ID,
                            length = item.length,
                            model = item.model,
                            pressure = item.pressure,
                            productDate = item.productDate,
                            rezve = item.rezve,
                            serialNumber = item.serialNumber,
                            status = item.status,
                            type = item.type,
                            workshop = ""

                        });
                    }
                }
                //
                ViewBag.tableOuts = tableOuts;
                //return PartialView();                
            }

            //-------------------برای قسمت شیر مخزن ها --------------------------------------

            if (!string.IsNullOrEmpty(serialValve))
            {
                var listofValves = db.tbl_TankValves.Where(b => b.serialNumber.Contains(serialValve)).ToList();
                if (listofValves.Count > 0)
                    ViewBag.existItem = true;

                foreach (var item in listofValves)
                {
                    int constractor = 1;
                    if (item.constractor.Contains("البرز"))
                        constractor = 1;
                    else
                        constractor = 1;
                    if (!string.IsNullOrEmpty(item.workshop))
                    {
                        valvetableOuts.Add(new BankTankValve
                        {
                            constractor = Helper.Helpers.GetValveConstractor(constractor).Valve,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            ID = item.ID,
                            model = item.model,
                            productDate = item.productDate,
                            rezve = item.rezve,
                            serialNumber = item.serialNumber,
                            status = item.status,
                            type = item.type,
                            workshop = Helper.Helpers.GetWorkshops(Convert.ToInt32(item.workshop)).Title
                        });
                    }
                    else
                    {
                        valvetableOuts.Add(new BankTankValve
                        {
                            constractor = Helper.Helpers.GetValveConstractor(constractor).Valve,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            ID = item.ID,
                            model = item.model,
                            productDate = item.productDate,
                            rezve = item.rezve,
                            serialNumber = item.serialNumber,
                            status = item.status,
                            type = item.type,
                            workshop = ""
                        });
                    }

                }
                //
                ViewBag.valvetableOuts = valvetableOuts;
                //return PartialView();                
            }
            //
            return PartialView();
        }

        // GET: Search in Message
        public ActionResult SearchInMessage()
        {
            ViewBag.existItem = false;
           // ViewBag.Type = "";

            return View();
        }

        [HttpPost]
        public ActionResult SearchInMessage(string type, string Condition)
        {
            ViewBag.Condition = Condition;
            ViewBag.existItem = false;
            ViewBag.Type = type;

            List<Message> tableOuts = new List<Message>();
           
            if (!string.IsNullOrEmpty(Condition))
            {
                var listofMessage = db.tbl_Messages.Where(b => b.Description.Contains(Condition) || b.Subject.Contains(Condition)).ToList();
                if (listofMessage.Count > 0)
                    ViewBag.existItem = true;

                foreach (var item in listofMessage)
                {
                    tableOuts.Add(new Message
                    {
                        Subject=item.Subject,
                        Description=item.Description,
                        Attachment=item.Attachment,
                        Childs=item.Childs,
                        ID=item.ID,
                        LetterNumber=item.LetterNumber,
                        MessageID=item.MessageID,
                        Messages=item.Messages,
                        Priority=item.Priority,
                        ReadDate=item.ReadDate,
                        ReadStatus=item.ReadStatus,
                        ReciverID=item.ReciverID,
                        Sender=item.Sender,
                        SenderDate=item.SenderDate,
                        SenderID=item.SenderID,
                        Type=item.Type,
                        Workshop=item.Workshop,
                        WorkshopID=item.WorkshopID
                    });
                }
                //
                ViewBag.tableOuts = tableOuts;
                return View();
            }
            //
            return View();
        }
        // GET: Search in Registration Vehicle
        public ActionResult SearchInParts()
        {
            ViewBag.existItem = false;
            ViewBag.Type = "Cylinder";
            ViewBag.Location = "Bank";

            return View();
        }

        [HttpPost]
        public ActionResult SearchInParts(string Condition, string type, string location,string MaterialName)
        {
            location = "Bank";
            ViewBag.condition = Condition;
            ViewBag.existItem = false;
            ViewBag.Type = type;
            ViewBag.Location = location;
            ViewBag.MaterialName = MaterialName;

            List<VehicleRegistration> tableOuts = new List<VehicleRegistration>();
            List<VehicleTank> tanktableOuts = new List<VehicleTank>();

            if (location.Contains("Registered"))
            {
                var vehicleList = db.tbl_VehicleRegistrations
                .Where(v => v.OwnerFamily.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                            v.OwnerName.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                              v.AlphaPlate.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                               v.IranNumberPlate.Contains(Condition) ||
                                v.LeftNumberPlate.Contains(Condition) ||
                                 v.RightNumberPlate.Contains(Condition) ||
                                  v.ChassisNumber.Replace("/", "").Contains(Condition.Replace("/", "")) ||
                                   v.EngineNumber.Replace("/", "").Contains(Condition.Replace("/", "")) ||
                                    v.NationalCode.Replace("-", "").Contains(Condition.Replace("-", "")) ||
                                     v.VehicleCard.Replace("-", "").Contains(Condition.Replace("-", "")) ||
                                      v.VIN.Contains(Condition) ||
                                     v.Workshop.Title.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی")) ||
                                    v.ConstructionYear.Contains(Condition) ||
                                   v.MobileNumber.Contains(Condition.Replace("-", "")) ||
                                  v.FuelCard.Contains(Condition) ||
                                 v.License.Contains(Condition) ||
                                v.RefuelingLable.Contains(Condition) ||
                               v.SerialKey.Contains(Condition) ||
                              v.SerialKit.Contains(Condition) ||
                             v.SerialRefuelingValve.Contains(Condition) ||
                            v.SerialSparkPreview.Contains(Condition) ||
                           v.System.Contains(Condition) ||
                          v.TrackingCode.Contains(Condition) ||
                        v.VehicleType.Type.Replace("ي", "ی").Contains(Condition.Replace("ي", "ی"))
                ).ToList();

                if (!string.IsNullOrEmpty(Condition))
                {
                    foreach (var item in vehicleList)
                    {
                        tableOuts.Add(new VehicleRegistration
                        {
                            Address = item.Address,
                            VehicleType = item.VehicleType,
                            TrackingCode = item.TrackingCode,
                            System = item.System,
                            SerialSparkPreview = item.SerialSparkPreview,
                            AlphaPlate = item.AlphaPlate,
                            ChassisNumber = item.ChassisNumber,
                            Checked = item.Checked,
                            CheckedDate = item.CheckedDate,
                            Checker = item.Checker,
                            ConstructionYear = item.ConstructionYear,
                            CreateDate = item.CreateDate,
                            Creator = item.Creator,
                            CreatorIPAddress = item.CreatorIPAddress,
                            Description = item.Description,
                            EditDate = item.EditDate,
                            Editor = item.Editor,
                            EditorIPAddress = item.EditorIPAddress,
                            EngineNumber = item.EngineNumber,
                            FuelCard = item.FuelCard,
                            HealthCertificate = item.HealthCertificate,
                            ID = item.ID,
                            InstallationStatus = item.InstallationStatus,
                            InvoiceCode = item.InvoiceCode,
                            IranNumberPlate = item.IranNumberPlate,
                            LeftNumberPlate = item.LeftNumberPlate,
                            License = item.License,
                            MobileNumber = item.MobileNumber,
                            NationalCode = item.NationalCode,
                            OwnerFamily = item.OwnerFamily + " " + item.OwnerName,
                            RefuelingLable = item.RefuelingLable,
                            RightNumberPlate = item.RightNumberPlate,
                            SerialKey = item.SerialKey,
                            SerialKit = item.SerialKit,
                            SerialRefuelingValve = item.SerialRefuelingValve,
                            Status = item.Status,
                            TypeofUse = item.TypeofUse,
                            TypeofUseID = item.TypeofUseID,
                            VehicleTypeID = item.VehicleTypeID,
                            VIN = item.VIN,
                            Workshop = item.Workshop,
                            WorkshopID = item.WorkshopID
                        });
                    }
                }
            }

            else
            {
                if (MaterialName.Equals("مخزن"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.Serial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("شیر مخزن"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.SerialTankValve.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("رگلاتور"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.RegulatorSerial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("شیر پر کن"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.FillingValveSerial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("شیر قطع کن"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.CutofValveSerial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("ریل سوخت"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.FuelRelaySerial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

                if (MaterialName.Equals("Gas ECU"))
                {
                    var vehicletankList = db.tbl_VehicleTanks
                   .Where(v => v.GasECUSerial.Replace("/", "").Contains(Condition.Replace("/", "")))
                   .ToList();
                    ViewBag.tanktableOuts = vehicletankList;
                }

            }
            //
            if (tableOuts.Count > 0)
                ViewBag.existItem = true;
            ViewBag.tableOuts = tableOuts;

            return View();
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